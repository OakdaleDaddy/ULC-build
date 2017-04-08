
namespace AssemblyVersion
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;

   class Program
   {

      static void GetVersionInfo(byte[] buffer, int count, ref int a, ref int b, ref int c, ref int d)
      {
         // [assembly: AssemblyFileVersion("1.0.0.0")]
         int i;
         int startQuote = -1;
         int firstDot = -1;
         int secondDot = -1;
         int thirdDot = -1;
         int endQuote = -1;

         for (i = 0; i < count; i++)
         {
            if ('\"' == buffer[i])
            {
               startQuote = i;
               i++;
               break;                  
            }
         }

         for (; i < count; i++)
         {
            if ('.' == buffer[i])
            {
               firstDot = i;
               i++;
               break;
            }
         }

         for (; i < count; i++)
         {
            if ('.' == buffer[i])
            {
               secondDot = i;
               i++;
               break;
            }
         }

         for (; i < count; i++)
         {
            if ('.' == buffer[i])
            {
               thirdDot = i;
               i++;
               break;
            }
         }

         for (; i < count; i++)
         {
            if ('\"' == buffer[i])
            {
               endQuote = i;
               i++;
               break;
            }
         }

         if ((startQuote > 0) &&
             (firstDot > 0) &&
             (secondDot > 0) &&
             (thirdDot > 0) &&
             (endQuote > 0))
         {
            string aString = Encoding.UTF8.GetString(buffer, startQuote + 1, firstDot - startQuote - 1);
            string bString = Encoding.UTF8.GetString(buffer, firstDot + 1, secondDot - firstDot - 1);
            string cString = Encoding.UTF8.GetString(buffer, secondDot + 1, thirdDot - secondDot - 1);
            string dString = Encoding.UTF8.GetString(buffer, thirdDot + 1, endQuote - thirdDot - 1); 

            int ta = 0;
            int tb = 0;
            int tc = 0;
            int td = 0;

            if ((int.TryParse(aString, out ta) != false) &&
                (int.TryParse(bString, out tb) != false) &&
                (int.TryParse(cString, out tc) != false) &&
                (int.TryParse(dString, out td) != false))
            {
               a = ta;
               b = tb;
               c = tc;
               d = td;
            }
         }
      }

      static void Main(string[] args)
      {
         if (args.Length < 1)
         {
            Console.Write("Usage: assemblyversion <path to AssemblyInfo.cs>\n");
         }
         else
         {
            string filePath = args[0];

            if (File.Exists(filePath) == false)
            {
               string message = string.Format("File {0} does not exist.\n", filePath);
               Console.Write(message);
            }
            else
            {
               int lastDotIndex = -1;

               for (int i = 0; i < filePath.Length; i++)
               {
                  if ('.' == filePath[i])
                  {
                     lastDotIndex = i;
                  }
               }

               string newFilePath = "";

               if (-1 == lastDotIndex)
               {
                  newFilePath = filePath + "_";
               }
               else
               {
                  newFilePath = filePath.Substring(0, lastDotIndex);
                  newFilePath += "_";
                  newFilePath += filePath.Substring(lastDotIndex, filePath.Length - lastDotIndex);
               }

               FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
               FileStream newStream = File.Create(newFilePath);

               if (null == fileStream)
               {
                  string message = string.Format("Unable to open {0}\n", filePath);
                  Console.Write(message);
               }
               else
               {
                  DateTime now = DateTime.Now;
                  byte[] readBuffer = new byte[1024];
                  byte[] lineBuffer = new byte[1024];
                  int lineBufferCount = 0;

                  for (; ; )
                  {
                     int readCount = fileStream.Read(readBuffer, 0, readBuffer.Length);

                     if (0 == readCount)
                     {
                        break;
                     }
                     else
                     {
                        for (int i = 0; i < readCount; i++)
                        {
                           byte ch = readBuffer[i];

                           if (lineBufferCount < lineBuffer.Length)
                           {
                              lineBuffer[lineBufferCount++] = ch;
                           }

                           if ('\n' == ch)
                           {
                              string line = Encoding.UTF8.GetString(lineBuffer, 0, lineBufferCount);

                              if (('/' != line[0]) &&
                                  (line.Contains("[assembly: AssemblyVersion") != false))
                              {
                                 // [assembly: AssemblyVersion("1.0.0.0")]

                                 int a = 0;
                                 int b = 0;
                                 int c = 0;
                                 int d = 0;

                                 GetVersionInfo(lineBuffer, lineBufferCount, ref a, ref b, ref c, ref d);

                                 if ((a == now.Year) && (b == now.Month) && (c == now.Day))
                                 {
                                    d++;
                                 }
                                 else
                                 {
                                    a = now.Year;
                                    b = now.Month;
                                    c = now.Day;
                                    d = 1;
                                 }

                                 string version = string.Format("[assembly: AssemblyVersion(\"{0:D4}.{1:D2}.{2:D2}.{3:D2}\")]\r\n", a, b, c, d);
                                 byte[] versionData = Encoding.UTF8.GetBytes(version);

                                 newStream.Write(versionData, 0, versionData.Length);

                                 string message = string.Format("AssemblyVersion updated.\n");
                                 Console.Write(message);
                              }
                              else if (('/' != line[0]) &&
                                       (line.Contains("[assembly: AssemblyFileVersion") != false))
                              {
                                 // [assembly: AssemblyFileVersion("1.0.0.0")]

                                 int a = 0;
                                 int b = 0;
                                 int c = 0;
                                 int d = 0;

                                 GetVersionInfo(lineBuffer, lineBufferCount, ref a, ref b, ref c, ref d);

                                 if ((a == now.Year) && (b == now.Month) && (c == now.Day))
                                 {
                                    d++;
                                 }
                                 else
                                 {
                                    a = now.Year;
                                    b = now.Month;
                                    c = now.Day;
                                    d = 1;
                                 }

                                 string version = string.Format("[assembly: AssemblyFileVersion(\"{0:D4}.{1:D2}.{2:D2}.{3:D2}\")]\r\n", a, b, c, d);
                                 byte[] versionData = Encoding.UTF8.GetBytes(version);

                                 newStream.Write(versionData, 0, versionData.Length);

                                 string message = string.Format("AssemblyFileVersion updated.\n");
                                 Console.Write(message);
                              }
                              else
                              {
                                 newStream.Write(lineBuffer, 0, lineBufferCount);
                              }

                              lineBufferCount = 0;
                           }
                        }

                     }
                  }

                  fileStream.Close();
                  newStream.Close();

                  File.Copy(newFilePath, filePath, true);
                  File.Delete(newFilePath);
               }
            }
         }
      }
   }
}
