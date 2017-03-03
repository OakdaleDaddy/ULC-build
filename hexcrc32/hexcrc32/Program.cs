
namespace hexcrc32
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Security.Cryptography;
   using System.Text;
   using System.Threading.Tasks;

   class Program
   {
      #region Definition

      private enum OutputTypes
      {
         ascii,
         binary,
      }

      #endregion

      #region Helper Function

      static private string GetOutputFilePath(string fullPath, OutputTypes outputType)
      {
         string result = null;
         string filePath = null;
         string fileName = null;
         string fileExtension = null;

         int fileNameIndex = fullPath.LastIndexOf('\\');
         int fileNameLength = 0;
         int fileExtensionIndex = fullPath.LastIndexOf('.');
         int fileExtensionLength = 0;

         if (fileNameIndex >= 0)
         {
            filePath = fullPath.Substring(0, fileNameIndex);

            if (fileExtensionIndex >= 0)
            {
               if (fileExtensionIndex > fileNameIndex)
               {
                  // \n.e or \n.
                  fileNameLength = fileExtensionIndex - fileNameIndex;
                  fileName = fullPath.Substring(fileNameIndex, fileNameLength);

                  fileExtensionIndex++;
                  fileExtensionLength = fullPath.Length - fileExtensionIndex;

                  if (fileExtensionLength >= 0)
                  {
                     fileExtension = fullPath.Substring(fileExtensionIndex, fileExtensionLength);
                  }
               }
               else
               { 
                  // d.d\n
                  fileNameLength = fullPath.Length - fileNameIndex;
                  fileName = fullPath.Substring(fileNameIndex, fileNameLength);
               }
            }
            else
            {
               // \\n
               fileNameLength = fullPath.Length - fileNameIndex;
               fileName = fullPath.Substring(fileNameIndex, fileNameLength);
            }
         }
         else
         {
            if (fileExtensionIndex >= 0)
            {
               // n.e or n.
               fileNameLength = fileExtensionIndex;
               fileName = fullPath.Substring(0, fileNameLength);

               fileExtensionIndex++;
               fileExtensionLength = fullPath.Length - fileExtensionIndex;

               if (fileExtensionLength >= 0)
               {
                  fileExtension = fullPath.Substring(fileExtensionIndex, fileExtensionLength);
               }
            }
            else
            {
               // n
               fileName = fullPath;
            }
         }

         if (null != filePath)
         {
            result = filePath;
         }

         if (null != fileName)
         {
            if (null == result)
            {
               result = "";
            }

            result += fileName + "_crc";
         }

         if (OutputTypes.binary == outputType)
         {
            if (null == result)
            {
               result = "";
            }

            result += ".bin";
         }
         else
         {
            if (null != fileExtension)
            {
               if (null == result)
               {
                  result = "";
               }

               result += "." + fileExtension;
            }
         }

         return (result);
      }

      static private bool ProcessLine(int lineNumber, string line, UInt32 beginMemory, UInt32 endMemory, byte[] memoryArray, ref UInt32 baseAddress, OutputTypes outputType, FileStream outputStream)
      {
         //Console.WriteLine(line);
         bool endFile = false;

         bool lengthValid = false;
         bool addressValid = false;
         bool typeValid = false;
         bool dataValid = false;
         bool checkValid = false;
         bool validLine = false;

         byte length = 0;
         byte addressHi = 0;
         byte addressLo = 0;
         UInt16 address = 0;
         byte type = 0;
         byte[] dataArray = new byte[256];
         byte check = 0;

         bool ignoreLine = false;

         if (null != line)
         {
            if (line.Length >= 3)
            {
               lengthValid = byte.TryParse(line.Substring(1, 2), System.Globalization.NumberStyles.HexNumber, null, out length);

               if (false != lengthValid)
               {
                  int expectedLineLength = 11 + (2 * length); // semicolon(1), length(2), address(4), type(2), check(2), data(length)

                  if (expectedLineLength == line.Length)
                  {
                     addressValid = byte.TryParse(line.Substring(3, 2), System.Globalization.NumberStyles.HexNumber, null, out addressHi);
                     addressValid = addressValid && byte.TryParse(line.Substring(5, 2), System.Globalization.NumberStyles.HexNumber, null, out addressLo);
                     address = (UInt16)((addressHi << 8) | addressLo);

                     typeValid = byte.TryParse(line.Substring(7, 2), System.Globalization.NumberStyles.HexNumber, null, out type);

                     dataValid = true;
                     int dataIndex = 9;

                     for (int i = 0; i < length; i++)
                     {
                        byte data = 0;
                        dataValid = dataValid && byte.TryParse(line.Substring(dataIndex, 2), System.Globalization.NumberStyles.HexNumber, null, out data);
                        dataArray[i] = data;
                        dataIndex += 2;
                     }

                     checkValid = byte.TryParse(line.Substring(expectedLineLength - 2, 2), System.Globalization.NumberStyles.HexNumber, null, out check);
                  }

                  if ((false != addressValid) &&
                      (false != typeValid) &&
                      (false != dataValid) &&
                      (false != checkValid))
                  {
                     byte calculatedCheck = (byte)(length + addressHi + addressLo + type + check);

                     for (int i = 0; i < length; i++)
                     {
                        calculatedCheck += dataArray[i];
                     }

                     checkValid = (0 == calculatedCheck) ? true : false;
                  }

                  if (false != checkValid)
                  {
                     if (0 == type)
                     {
                        // data

                        int ignoreCount = 0;

                        for (int i = 0; i < length; i++)
                        {
                           byte ch = dataArray[i];
                           UInt32 byteAddress = (UInt32)(baseAddress + address + i);

                           if ((byteAddress >= beginMemory) && (byteAddress <= endMemory))
                           {
                              if (null != memoryArray)
                              {
                                 UInt32 memoryArrayIndex = byteAddress - beginMemory;

                                 if (memoryArrayIndex < memoryArray.Length)
                                 {
                                    memoryArray[memoryArrayIndex] = ch;
                                 }
                                 else
                                 {
                                    Console.WriteLine("error: line {0}, address 0x{1:X8} is out of range", lineNumber, baseAddress);
                                    endFile = true;
                                 }
                              }
                           }
                           else
                           {
                              Console.WriteLine("warning: line {0}, address 0x{1:X8}, value 0x{2:X2}, is outside memory range", lineNumber, baseAddress, ch);
                              ignoreCount++;
                           }
                        }

                        if (ignoreCount == length)
                        {
                           ignoreLine = true;
                        }

                        validLine = true;
                     }
                     else if (1 == type)
                     {
                        // end of file

                        if (0 == length)
                        {
                           endFile = true;
                           ignoreLine = true;
                           validLine = true;
                        }
                     }
                     else if (2 == type)
                     {
                        // extended segment address

                        if (2 == length)
                        {
                           baseAddress = (UInt32)((dataArray[0] << 20) | (dataArray[1] << 12));
                           validLine = true;
                        }
                     }
                     else if (3 == type)
                     {
                        // start segment address

                        if (4 == length)
                        {
                           baseAddress = (UInt32)((dataArray[0] << 24) | (dataArray[1] << 16) | (dataArray[2] << 8) | dataArray[3]);
                           validLine = true;
                        }
                     }
                     else if (4 == type)
                     {
                        // extended linear address

                        if (2 == length)
                        {
                           baseAddress = (UInt32)((dataArray[0] << 24) | (dataArray[1] << 16));
                           validLine = true;
                        }
                     }
                     else if (5 == type)
                     {
                        // start linear address

                        if (4 == length)
                        {
                           baseAddress = (UInt32)((dataArray[0] << 24) | (dataArray[1] << 16) | (dataArray[2] << 8) | dataArray[3]);
                           validLine = true;
                        }
                     }
                  }
               }
            }

            if (false != validLine)
            {
               if (false == ignoreLine)
               {
                  if (OutputTypes.ascii == outputType)
                  {
                     byte[] outputArray = Encoding.UTF8.GetBytes(line + "\r\n");
                     outputStream.Write(outputArray, 0, outputArray.Length);
                  }
                  else if (OutputTypes.binary == outputType)
                  {
                     outputStream.WriteByte(length);
                     outputStream.WriteByte(addressHi);
                     outputStream.WriteByte(addressLo);
                     outputStream.WriteByte(type);
                     outputStream.Write(dataArray, 0, length);
                     outputStream.WriteByte(check);
                  }
               }
            }
            else
            {
               Console.WriteLine("error: line {0} is invalid", lineNumber);
               endFile = true;
            }
         }

         return (endFile);
      }

      static private void WriteCrc(byte[] crcBytes, UInt32 crcLocation, OutputTypes outputType, FileStream outputStream)
      {
         UInt16 upperWord = (UInt16)((crcLocation >> 16) & 0xFFFF);
         UInt16 lowerWord = (UInt16)(crcLocation & 0xFFFF);

         byte upperWordMsb = (byte)((upperWord >> 8) & 0xFF);
         byte upperWordLsb = (byte)(upperWord & 0xFF);
         byte s4Check = (byte)(0x100 - (0x02 + 0x04 + upperWordMsb + upperWordLsb));

         byte lowerWordMsb = (byte)((lowerWord >> 8) & 0xFF);
         byte lowerWordLsb = (byte)(lowerWord & 0xFF);
         byte s0Check = (byte)(0x100 - (0x04 + lowerWordMsb + lowerWordLsb + crcBytes[0] + crcBytes[1] + crcBytes[2] + crcBytes[3]));

         if (OutputTypes.ascii == outputType)
         {
            string s4Line = string.Format(":02000004{0:X2}{1:X2}{2:X2}", upperWordMsb, upperWordLsb, s4Check);
            byte[] s4OutputArray = Encoding.UTF8.GetBytes(s4Line + "\r\n");
            outputStream.Write(s4OutputArray, 0, s4OutputArray.Length);

            string s0Line = string.Format(":04{0:X2}{1:X2}00{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}", lowerWordMsb, lowerWordLsb, crcBytes[0], crcBytes[1], crcBytes[2], crcBytes[3], s0Check);
            byte[] s0OutputArray = Encoding.UTF8.GetBytes(s0Line + "\r\n");
            outputStream.Write(s0OutputArray, 0, s0OutputArray.Length);
         }
         else if (OutputTypes.binary == outputType)
         {
            outputStream.WriteByte(0x02);
            outputStream.WriteByte(0x00);
            outputStream.WriteByte(0x00);
            outputStream.WriteByte(0x04);
            outputStream.WriteByte(upperWordMsb);
            outputStream.WriteByte(upperWordLsb);
            outputStream.WriteByte(s4Check);

            outputStream.WriteByte(0x04);
            outputStream.WriteByte(lowerWordMsb);
            outputStream.WriteByte(lowerWordLsb);
            outputStream.WriteByte(0x00);
            outputStream.WriteByte(crcBytes[0]);
            outputStream.WriteByte(crcBytes[1]);
            outputStream.WriteByte(crcBytes[2]);
            outputStream.WriteByte(crcBytes[3]);
            outputStream.WriteByte(s0Check);
         }
      }

      static private void WriteS1(OutputTypes outputType, FileStream outputStream)
      {
         if (OutputTypes.ascii == outputType)
         {
            string s1Line = string.Format(":00000001FF");
            byte[] s1OutputArray = Encoding.UTF8.GetBytes(s1Line + "\r\n");
            outputStream.Write(s1OutputArray, 0, s1OutputArray.Length);
         }
         else if (OutputTypes.binary == outputType)
         {
            outputStream.WriteByte(0x00);
            outputStream.WriteByte(0x00);
            outputStream.WriteByte(0x00);
            outputStream.WriteByte(0x01);
            outputStream.WriteByte(0xFF);
         }
      }

      #endregion

      #region Process Main

      static void Main(string[] args)
      {
         OutputTypes outputType = OutputTypes.ascii;
         UInt32 beginMemory = 0x00000000;
         UInt32 endMemory = 0xFFFFFFFB;
         UInt32 crcLocation = 0xFFFFFFFC;
         UInt32 crcPolynomial = 0x04C11DB7;
         UInt32 crcInitial = 0xFFFFFFFF;
         bool generateCrc = false;
         string inputFilePath = ""; ;
         string outputFilePath = "";

         try
         {
            Console.Clear();
         }
         catch { }

         Console.WriteLine("Intel Hex CRC32 Creator and Converter");
         Console.WriteLine("Version 1.1");
         Console.WriteLine("Copyright (c)2017 ULC Robotics");

         if (args.Length < 1)
         {
            Console.WriteLine("");
            Console.WriteLine("Syntax:");
            Console.WriteLine("   hexcrc32 <Intel Hex File> <options>");
            Console.WriteLine("");
            Console.WriteLine("Options...");
            Console.WriteLine("");
            Console.WriteLine("    -bn:  Ignore address lower than n in Intel Hex-file");
            Console.WriteLine("    -en:  Ignore address higher than n in Intel Hex-file");
            Console.WriteLine("    -cn:  Address where to put the checksum.");
            Console.WriteLine("    -pn:  Set CRC32 polynomial (default is 0x04C11DB7L)");
            Console.WriteLine("    -in:  Set CRC32 initial value (default is 0xFFFFFFFF)");
            Console.WriteLine("    -x:   Output is a binary file (default is ASCII)");
            Console.WriteLine("");
            Console.WriteLine("  Undefined space is filled with 0xFF");
            Console.WriteLine("  n is an hexadecimal value, up to eight digits");
         }
         else
         {
            for (int i = 1; i < args.Length; i++)
            {
               bool successfulParse = false;
               string option = args[i];

               if (option.Length >= 2)
               {
                  string command = args[i].Substring(0, 2);

                  if ("-b" == command)
                  {
                     if (option.Length > 2)
                     {
                        string nString = option.Substring(2);
                        UInt32 n = 0;

                        if (UInt32.TryParse(nString, System.Globalization.NumberStyles.HexNumber, null, out n) != false)
                        {
                           beginMemory = n;
                           successfulParse = true;
                        }
                     }
                  }
                  else if ("-e" == command)
                  {
                     if (option.Length > 2)
                     {
                        string nString = option.Substring(2);
                        UInt32 n = 0;

                        if (UInt32.TryParse(nString, System.Globalization.NumberStyles.HexNumber, null, out n) != false)
                        {
                           endMemory = n;
                           successfulParse = true;
                        }
                     }
                  }
                  else if ("-c" == command)
                  {
                     if (option.Length > 2)
                     {
                        string nString = option.Substring(2);
                        UInt32 n = 0;

                        if (UInt32.TryParse(nString, System.Globalization.NumberStyles.HexNumber, null, out n) != false)
                        {
                           crcLocation = n;
                           generateCrc = true;
                           successfulParse = true;
                        }
                     }
                  }
                  else if ("-p" == command)
                  {
                     if (option.Length > 2)
                     {
                        string nString = option.Substring(2);
                        UInt32 n = 0;

                        if (UInt32.TryParse(nString, System.Globalization.NumberStyles.HexNumber, null, out n) != false)
                        {
                           crcPolynomial = n;
                           successfulParse = true;
                        }
                     }
                  }
                  else if ("-i" == command)
                  {
                     if (option.Length > 2)
                     {
                        string nString = option.Substring(2);
                        UInt32 n = 0;

                        if (UInt32.TryParse(nString, System.Globalization.NumberStyles.HexNumber, null, out n) != false)
                        {
                           crcInitial = n;
                           successfulParse = true;
                        }
                     }
                  }
                  else if ("-x" == command)
                  {
                     outputType = OutputTypes.binary;
                     successfulParse = true;
                  }
               }

               if (false == successfulParse)
               {
                  Console.WriteLine("warning: unknown option: {0}", option);
               }
            }

            inputFilePath = args[0];
            outputFilePath = GetOutputFilePath(inputFilePath, outputType);

            if (null != outputFilePath)
            {
               if (File.Exists(inputFilePath) != false)
               {
                  StreamReader inputStream = new StreamReader(inputFilePath);
                  FileStream outputStream = new FileStream(outputFilePath, FileMode.Create);

                  if ((null != inputStream) && (null != outputStream))
                  {
                     Console.WriteLine("Address range is 0x{0:X8} to 0x{1:X8}", beginMemory, endMemory);
                     byte[] memoryArray = null;
                     UInt32 baseAddress = 0;
                     int lineNumber = 1;

                     if (false != generateCrc)
                     {
                        Console.WriteLine("CRC location is 0x{0:X8}", crcLocation);

                        int memoryArraySize = (int)(endMemory - beginMemory);
                        memoryArray = new byte[memoryArraySize];

                        for (int i = 0; i < memoryArray.Length; i++)
                        {
                           memoryArray[i] = 0xFF;
                        }
                     }

                     for (; ; )
                     {
                        string line = inputStream.ReadLine();

                        if (null != line)
                        {
                           bool endFile = ProcessLine(lineNumber, line, beginMemory, endMemory, memoryArray, ref baseAddress, outputType, outputStream);
                           lineNumber++;

                           if (false != endFile)
                           {
                              break;
                           }
                        }
                        else
                        {
                           break;
                        }
                     }

                     if (false != generateCrc)
                     {
                        CrcEngine crcEngine = new CrcEngine();

                        crcEngine.Width = 32;
                        crcEngine.Polynomial = crcPolynomial;
                        crcEngine.Initial = crcInitial;
                        crcEngine.ReflectIn = false;
                        crcEngine.ReflectOut = false;
                        crcEngine.Xor = 0x0;

                        crcEngine.Initialize();

                        for (int i = 0; i < memoryArray.Length; i += 4)
                        {
                           for (int j = 3; j >= 0; j--)
                           {
                              int ch = 0xFF;
                              int index = i + j;

                              if (index < memoryArray.Length)
                              {
                                 ch = memoryArray[index];
                              }

                              crcEngine.Next(ch);
                           }

                           UInt32 x = crcEngine.Final();
                        }

                        UInt32 crcValue = crcEngine.Final();
                        byte[] crcBytes = BitConverter.GetBytes(crcValue);
                        Console.WriteLine("CRC32 is 0x{0:X8}", crcValue);

                        WriteCrc(crcBytes, crcLocation, outputType, outputStream); 
                     }

                     WriteS1(outputType, outputStream); 

                     if (OutputTypes.ascii == outputType)
                     {
                        Console.WriteLine("Intel Hex file {0} written.", outputFilePath);
                     }
                     else if (OutputTypes.binary == outputType)
                     {
                        Console.WriteLine("Binary file {0} written.", outputFilePath);
                     }

                     inputStream.Close();
                     outputStream.Close();
                  }
                  else if (null != inputStream)
                  {
                     inputStream.Close();
                     Console.WriteLine("error: unable to open output file {0}", outputFilePath);
                  }
                  else if (null != outputStream)
                  {
                     outputStream.Close();
                     Console.WriteLine("error: unable to open input file {0}", inputFilePath);
                  }
                  else
                  {
                     Console.WriteLine("error: unable to open files {0} and {1}", inputFilePath, outputFilePath);
                  }
               }
               else
               {
                  Console.WriteLine("error: input file {0} not found", inputFilePath);
               }
            }
            else
            {
               Console.WriteLine("error: invalid input: {0}", inputFilePath);
            }
         }
      }

      #endregion
   }
}
