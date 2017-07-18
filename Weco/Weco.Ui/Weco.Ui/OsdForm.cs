namespace Weco.Ui
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public partial class OsdForm : Form
   {
      #region Fields

      private bool mouseDown;
      private Point mouseDownPoint;

      private bool shifted;
      private bool capsLocked;
      private bool changePending;

      private Thread beepThread;
      private bool executeBeepThread;
      private bool doDeep;

      #endregion

      #region Properties

      #endregion

      #region Helper Functions

      private void ProcessBeep()
      {
         for (; executeBeepThread; )
         {
            if (false != this.doDeep)
            {
               this.doDeep = false;
               Console.Beep();
            }

            Thread.Sleep(10);
         }
      }

      private string GetLineText(int line)
      {
         string result = "";

         if ((line >= 1) && (line <= 8))
         {
            int index = line - 1;

            if (index < this.OsdRichTextBox.Lines.Length)
            {
               result = this.OsdRichTextBox.Lines[index];
            }
         }

         return (result);
      }

      private void ClearScreen()
      {
         this.OsdRichTextBox.Text = "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n";
         this.OsdRichTextBox.SelectionStart = 0;
      }

      private bool GetShift(bool considerCapsLock)
      {
         bool result = false;

         if ((false != considerCapsLock) && (false != this.capsLocked))
         {
            result = !result;
         }

         if (false != this.shifted)
         {
            result = !result;
         }

         return (result);
      }

      private void UpdateKeys()
      {
         this.doDeep = true;

         if (false != this.capsLocked)
         {
            this.CapsLockKeyButton.BackColor = Color.Black;
            this.CapsLockKeyButton.ForeColor = Color.White;
         }
         else
         {
            this.CapsLockKeyButton.BackColor = Color.FromArgb(240, 240, 240);
            this.CapsLockKeyButton.ForeColor = Color.Black;
         }

         if (this.GetShift(false) != false)
         {
            this.OneKeyButton.Text = "!";
            this.TwoKeyButton.Text = "@";
            this.ThreeKeyButton.Text = "#";
            this.FourKeyButton.Text = "$";
            this.FiveKeyButton.Text = "%";
            this.SixKeyButton.Text = "^";
            this.SevenKeyButton.Text = "&";
            this.EightKeyButton.Text = "*";
            this.NineKeyButton.Text = "(";
            this.ZeroKeyButton.Text = ")";
            this.DashKeyButton.Text = "_";
            this.EqualKeyButton.Text = "+";

            this.LeftBracketKeyButton.Text = "{";
            this.RightBracketKeyButton.Text = "}";
            this.BackSlashKeyButton.Text = "|";
         
            this.SemaColonKeyButton.Text = ":";
            this.SingleQuoteKeyButton.Text = "\"";

            this.CommaKeyButton.Text = "<";
            this.PeriodKeyButton.Text = ">";
            this.ForwardSlashKeyButton.Text = "?";
         }
         else
         {
            this.OneKeyButton.Text = "1";
            this.TwoKeyButton.Text = "2";
            this.ThreeKeyButton.Text = "3";
            this.FourKeyButton.Text = "4";
            this.FiveKeyButton.Text = "5";
            this.SixKeyButton.Text = "6";
            this.SevenKeyButton.Text = "7";
            this.EightKeyButton.Text = "8";
            this.NineKeyButton.Text = "9";
            this.ZeroKeyButton.Text = "0";
            this.DashKeyButton.Text = "-";
            this.EqualKeyButton.Text = "=";

            this.LeftBracketKeyButton.Text = "[";
            this.RightBracketKeyButton.Text = "]";
            this.BackSlashKeyButton.Text = "\\";

            this.SemaColonKeyButton.Text = ";";
            this.SingleQuoteKeyButton.Text = "'";

            this.CommaKeyButton.Text = ",";
            this.PeriodKeyButton.Text = ".";
            this.ForwardSlashKeyButton.Text = "/";
         }

         if (this.GetShift(true) != false)
         {
            this.QKeyButton.Text = "Q";
            this.WKeyButton.Text = "W";
            this.EkeyButton.Text = "E";
            this.RKeyButton.Text = "R";
            this.TKeyButton.Text = "T";
            this.YKeyButton.Text = "Y";
            this.UKeyButton.Text = "U";
            this.IKeyButton.Text = "I";
            this.OKeyButton.Text = "O";
            this.PKeyButton.Text = "P";

            this.AKeyButton.Text = "A";
            this.SKeyButton.Text = "S";
            this.DKeyButton.Text = "D";
            this.FKeyButton.Text = "F";
            this.GKeyButton.Text = "G";
            this.HKeyButton.Text = "H";
            this.JKeyButton.Text = "J";
            this.KKeyButton.Text = "K";
            this.LKeyButton.Text = "L";
            
            this.ZKeyButton.Text = "Z";
            this.XKeyButton.Text = "X";
            this.CKeyButton.Text = "C";
            this.VkeyButton.Text = "V";
            this.BKeyButton.Text = "B";
            this.NKeyButton.Text = "N";
            this.MKeyButton.Text = "M";
         }
         else
         {
            this.QKeyButton.Text = "q";
            this.WKeyButton.Text = "w";
            this.EkeyButton.Text = "e";
            this.RKeyButton.Text = "r";
            this.TKeyButton.Text = "t";
            this.YKeyButton.Text = "y";
            this.UKeyButton.Text = "u";
            this.IKeyButton.Text = "i";
            this.OKeyButton.Text = "o";
            this.PKeyButton.Text = "p";

            this.AKeyButton.Text = "a";
            this.SKeyButton.Text = "s";
            this.DKeyButton.Text = "d";
            this.FKeyButton.Text = "f";
            this.GKeyButton.Text = "g";
            this.HKeyButton.Text = "h";
            this.JKeyButton.Text = "j";
            this.KKeyButton.Text = "k";
            this.LKeyButton.Text = "l";

            this.ZKeyButton.Text = "z";
            this.XKeyButton.Text = "x";
            this.CKeyButton.Text = "c";
            this.VkeyButton.Text = "v";
            this.BKeyButton.Text = "b";
            this.NKeyButton.Text = "n";
            this.MKeyButton.Text = "m";
         }
      }

      private void InsertCharacter(string ch, bool deSelectShift = true)
      {
         int chLength = ch.Length;
         int limit = 30 - chLength;
         bool changed = false;

         if ("\n" == ch)
         {
            bool insertNewLine = false;
            bool setNextLine = false;
            int nextLineIndex = 0;

            if (this.OsdRichTextBox.Lines.Length > 0)
            {
               int currentLineIndex = this.OsdRichTextBox.GetLineFromCharIndex(this.OsdRichTextBox.SelectionStart);
               string currentLineText = this.OsdRichTextBox.Lines[currentLineIndex];
               int startOfCurrentLine = this.OsdRichTextBox.GetFirstCharIndexOfCurrentLine();
               int offsetIntoLine = this.OsdRichTextBox.SelectionStart - startOfCurrentLine;

               if ((offsetIntoLine != currentLineText.Length) ||
                   (8 == this.OsdRichTextBox.Lines.Length))
               {
                  nextLineIndex = currentLineIndex + 1;
                  setNextLine = true;
               }
               else if (currentLineText.Contains('\n') == false)
               {
                  insertNewLine = true;
               }
            }
            else
            {
               insertNewLine = true;
            }

            if (false != insertNewLine)
            {
               changed = true;
               this.OsdRichTextBox.Select(this.OsdRichTextBox.SelectionStart, 1);
               this.OsdRichTextBox.SelectedText = ch;
            }
            else if (false != setNextLine)
            {
               if (nextLineIndex < this.OsdRichTextBox.Lines.Length)
               {
                  changed = true;
                  int nextLineLength = this.OsdRichTextBox.Lines[nextLineIndex].Length;
                  int startOfNextLine = this.OsdRichTextBox.GetFirstCharIndexFromLine(nextLineIndex);
                  this.OsdRichTextBox.SelectionStart = startOfNextLine + nextLineLength;
               }
            }
         }
         else
         {
            int currentLineIndex = this.OsdRichTextBox.GetLineFromCharIndex(this.OsdRichTextBox.SelectionStart);

            if (currentLineIndex < this.OsdRichTextBox.Lines.Length)
            {
               int startOfCurrentLine = this.OsdRichTextBox.GetFirstCharIndexOfCurrentLine();
               int offsetIntoLine = this.OsdRichTextBox.SelectionStart - startOfCurrentLine;

               if (offsetIntoLine < limit)
               {
                  this.OsdRichTextBox.Select(this.OsdRichTextBox.SelectionStart, chLength);

                  if ("\n" != this.OsdRichTextBox.SelectedText)
                  {
                     changed = true;
                     this.OsdRichTextBox.SelectedText = ch;
                  }
                  else
                  {
                     changed = true;
                     this.OsdRichTextBox.SelectedText = ch + "\n";

                     if (offsetIntoLine < limit-1)
                     {
                        this.OsdRichTextBox.SelectionStart--;
                     }
                  }
               }
            }
            else
            {
               this.OsdRichTextBox.Select(0, chLength);
               this.OsdRichTextBox.SelectedText = ch;
            }
         }

         if (false != changed)
         {
            this.doDeep = true;

            if (false == this.changePending)
            {
               this.changePending = true;
               this.ClearScreenButton.Text = "UNDO";
               this.WriteTextButton.BackColor = Color.FromArgb(255, 255, 192);
            }
         }
         
         if ((false != deSelectShift) && (false != this.shifted))
         {
            this.shifted = false;
            this.UpdateKeys();
         }

         this.OsdRichTextBox.Focus();
      }

      #endregion

      #region User Events Event

      private void WriteTextButton_Click(object sender, EventArgs e)
      {
         if (false == ParameterAccessor.Instance.Osd.ShowDescription)
         {
            ParameterAccessor.Instance.Osd.ShowDescription = true;
            VideoStampOsd.Instance.SetDescriptionVisible(ParameterAccessor.Instance.Osd.ShowDescription);
            this.DescriptionToggleButton.BackColor = Color.Lime;
         }

         ParameterAccessor.Instance.Osd.Line1 = this.GetLineText(1);
         ParameterAccessor.Instance.Osd.Line2 = this.GetLineText(2);
         ParameterAccessor.Instance.Osd.Line3 = this.GetLineText(3);
         ParameterAccessor.Instance.Osd.Line4 = this.GetLineText(4);
         ParameterAccessor.Instance.Osd.Line5 = this.GetLineText(5);
         ParameterAccessor.Instance.Osd.Line6 = this.GetLineText(6);
         ParameterAccessor.Instance.Osd.Line7 = this.GetLineText(7);
         ParameterAccessor.Instance.Osd.Line8 = this.GetLineText(8);

         VideoStampOsd.Instance.SetDescriptionText(1, ParameterAccessor.Instance.Osd.Line1);
         VideoStampOsd.Instance.SetDescriptionText(2, ParameterAccessor.Instance.Osd.Line2);
         VideoStampOsd.Instance.SetDescriptionText(3, ParameterAccessor.Instance.Osd.Line3);
         VideoStampOsd.Instance.SetDescriptionText(4, ParameterAccessor.Instance.Osd.Line4);
         VideoStampOsd.Instance.SetDescriptionText(5, ParameterAccessor.Instance.Osd.Line5);
         VideoStampOsd.Instance.SetDescriptionText(6, ParameterAccessor.Instance.Osd.Line6);
         VideoStampOsd.Instance.SetDescriptionText(7, ParameterAccessor.Instance.Osd.Line7);
         VideoStampOsd.Instance.SetDescriptionText(8, ParameterAccessor.Instance.Osd.Line8);

         this.changePending = false;
         this.ClearScreenButton.Text = "CLEAR SCREEN";
         this.WriteTextButton.BackColor = Color.FromArgb(240, 240, 240);
         
         this.OsdRichTextBox.Focus();
      }

      private void ClearScreenButton_Click(object sender, EventArgs e)
      {
         if (false == this.changePending)
         {
            this.ClearScreen();

            this.changePending = true;
            this.ClearScreenButton.Text = "UNDO";
            this.WriteTextButton.BackColor = Color.FromArgb(255, 255, 192);

            this.OsdRichTextBox.Focus();
         }
         else
         {
            this.changePending = false;
            this.ClearScreenButton.Text = "CLEAR SCREEN";
            this.WriteTextButton.BackColor = Color.FromArgb(240, 240, 240);

            this.OsdRichTextBox.Text = ParameterAccessor.Instance.Osd.Line1 + "\n" +
                                       ParameterAccessor.Instance.Osd.Line2 + "\n" +
                                       ParameterAccessor.Instance.Osd.Line3 + "\n" +
                                       ParameterAccessor.Instance.Osd.Line4 + "\n" +
                                       ParameterAccessor.Instance.Osd.Line5 + "\n" +
                                       ParameterAccessor.Instance.Osd.Line6 + "\n" +
                                       ParameterAccessor.Instance.Osd.Line7 + "\n" +
                                       ParameterAccessor.Instance.Osd.Line8;
            this.OsdRichTextBox.SelectionStart = ParameterAccessor.Instance.Osd.Line1.Length;
            this.OsdRichTextBox.Focus();
         }
      }

      private void TimeToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !ParameterAccessor.Instance.Osd.ShowTime;
         ParameterAccessor.Instance.Osd.ShowTime = selection;
         this.TimeToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowTime) ? Color.Lime : Color.FromArgb(64, 64, 64);
         VideoStampOsd.Instance.SetTimeVisible(ParameterAccessor.Instance.Osd.ShowTime);
      }

      private void DistanceToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !ParameterAccessor.Instance.Osd.ShowDistance;
         ParameterAccessor.Instance.Osd.ShowDistance = selection;
         this.DistanceToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowDistance) ? Color.Lime : Color.FromArgb(64, 64, 64);
         VideoStampOsd.Instance.SetDistanceVisible(ParameterAccessor.Instance.Osd.ShowDistance);
      }

      private void DateToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !ParameterAccessor.Instance.Osd.ShowDate;
         ParameterAccessor.Instance.Osd.ShowDate = selection;
         this.DateToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowDate) ? Color.Lime : Color.FromArgb(64, 64, 64);
         VideoStampOsd.Instance.SetDateVisible(ParameterAccessor.Instance.Osd.ShowDate);
      }

      private void CameraIdToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !ParameterAccessor.Instance.Osd.ShowCameraId;
         ParameterAccessor.Instance.Osd.ShowCameraId = selection;
         this.CameraIdToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowCameraId) ? Color.Lime : Color.FromArgb(64, 64, 64);
         VideoStampOsd.Instance.SetCameraIdVisible(ParameterAccessor.Instance.Osd.ShowCameraId);
      }

      private void DescriptionToggleButton_Click(object sender, EventArgs e)
      {
         bool selection = !ParameterAccessor.Instance.Osd.ShowDescription;
         ParameterAccessor.Instance.Osd.ShowDescription = selection;
         this.DescriptionToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowDescription) ? Color.Lime : Color.FromArgb(64, 64, 64);
         VideoStampOsd.Instance.SetDescriptionVisible(ParameterAccessor.Instance.Osd.ShowDescription);
      }

      private void DeleteKeyButton_Click(object sender, EventArgs e)
      {
         int currentLineIndex = this.OsdRichTextBox.GetLineFromCharIndex(this.OsdRichTextBox.SelectionStart);

         if (currentLineIndex < this.OsdRichTextBox.Lines.Length)
         {
            int lineLength = this.OsdRichTextBox.Lines[currentLineIndex].Length;
            int startOfCurrentLine = this.OsdRichTextBox.GetFirstCharIndexOfCurrentLine();
            int offsetIntoLine = this.OsdRichTextBox.SelectionStart - startOfCurrentLine;

            if (offsetIntoLine < lineLength)
            {
               if ("\n" != this.OsdRichTextBox.Lines[currentLineIndex])
               {
                  this.doDeep = true;

                  int indexToClear = this.OsdRichTextBox.SelectionStart;
                  this.OsdRichTextBox.Text = this.OsdRichTextBox.Text.Remove(indexToClear, 1);
                  this.OsdRichTextBox.SelectionStart = indexToClear;

                  if (false == this.changePending)
                  {
                     this.changePending = true;
                     this.ClearScreenButton.Text = "UNDO";
                     this.WriteTextButton.BackColor = Color.FromArgb(255, 255, 192);
                  }
               }
            }
         }

         this.OsdRichTextBox.Focus();
      }

      private void OneKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "!" : "1";
         this.InsertCharacter(ch);
      }

      private void TwoKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "@" : "2";
         this.InsertCharacter(ch);
      }

      private void ThreeKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "#" : "3";
         this.InsertCharacter(ch);
      }

      private void FourKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "$" : "4";
         this.InsertCharacter(ch);
      }

      private void FiveKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "%" : "5";
         this.InsertCharacter(ch);
      }

      private void SixKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "^" : "6";
         this.InsertCharacter(ch);
      }

      private void SevenKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "&" : "7";
         this.InsertCharacter(ch);
      }

      private void EightKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "*" : "8";
         this.InsertCharacter(ch);
      }

      private void NineKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "(" : "9";
         this.InsertCharacter(ch);
      }

      private void ZeroKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? ")" : "0";
         this.InsertCharacter(ch);
      }

      private void DashKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "_" : "-";
         this.InsertCharacter(ch);
      }

      private void EqualKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "+" : "=";
         this.InsertCharacter(ch);
      }

      private void BackspaceKeyButton_Click(object sender, EventArgs e)
      {
         int currentLineIndex = this.OsdRichTextBox.GetLineFromCharIndex(this.OsdRichTextBox.SelectionStart);

         if (currentLineIndex < this.OsdRichTextBox.Lines.Length)
         {
            int startOfCurrentLine = this.OsdRichTextBox.GetFirstCharIndexOfCurrentLine();

            if (this.OsdRichTextBox.SelectionStart > startOfCurrentLine)
            {
               this.doDeep = true;

               int indexToClear = this.OsdRichTextBox.SelectionStart - 1;
               this.OsdRichTextBox.Text = this.OsdRichTextBox.Text.Remove(indexToClear, 1);
               this.OsdRichTextBox.SelectionStart = indexToClear;

               if (false == this.changePending)
               {
                  this.changePending = true;
                  this.ClearScreenButton.Text = "UNDO";
                  this.WriteTextButton.BackColor = Color.FromArgb(255, 255, 192);
               }
            }
         }
      
         this.OsdRichTextBox.Focus();
      }

      private void TabKeyButton_Click(object sender, EventArgs e)
      {
         string ch = "   ";
         this.InsertCharacter(ch, false);
      }

      private void QKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "Q" : "q";
         this.InsertCharacter(ch);
      }

      private void WKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "W" : "w";
         this.InsertCharacter(ch);
      }

      private void EkeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "E" : "e";
         this.InsertCharacter(ch);
      }

      private void RKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "R" : "r";
         this.InsertCharacter(ch);
      }

      private void TKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "T" : "t";
         this.InsertCharacter(ch);
      }

      private void YKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "Y" : "y";
         this.InsertCharacter(ch);
      }

      private void UKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "U" : "u";
         this.InsertCharacter(ch);
      }

      private void IKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "I" : "i";
         this.InsertCharacter(ch);
      }

      private void OKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "O" : "o";
         this.InsertCharacter(ch);
      }

      private void PKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "P" : "p";
         this.InsertCharacter(ch);
      }

      private void LeftBracketKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "{" : "[";
         this.InsertCharacter(ch);
      }

      private void RightBracketKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "}" : "]";
         this.InsertCharacter(ch);
      }

      private void BackSlashKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "|" : "\\";
         this.InsertCharacter(ch);
      }

      private void CapsLockKeyButton_Click(object sender, EventArgs e)
      {
         this.capsLocked = !this.capsLocked;
         this.UpdateKeys();
         this.OsdRichTextBox.Focus();
      }

      private void AKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "A" : "a";
         this.InsertCharacter(ch);
      }

      private void SKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "S" : "s";
         this.InsertCharacter(ch);
      }

      private void DKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "D" : "d";
         this.InsertCharacter(ch);
      }

      private void FKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "F" : "f";
         this.InsertCharacter(ch);
      }

      private void GKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "G" : "g";
         this.InsertCharacter(ch);
      }

      private void HKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "H" : "h";
         this.InsertCharacter(ch);
      }

      private void JKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "J" : "j";
         this.InsertCharacter(ch);
      }

      private void KKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "K" : "k";
         this.InsertCharacter(ch);
      }

      private void LKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "L" : "l";
         this.InsertCharacter(ch);
      }

      private void SemaColonKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? ":" : ";";
         this.InsertCharacter(ch);
      }

      private void SingleQuoteKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "\"" : "'";
         this.InsertCharacter(ch);
      }

      private void EnterKeyButton_Click(object sender, EventArgs e)
      {
         this.InsertCharacter("\n", false);
      }

      private void LeftShiftKeyButton_Click(object sender, EventArgs e)
      {
         this.shifted = !this.shifted;
         this.UpdateKeys();
         this.OsdRichTextBox.Focus();
      }

      private void ZKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "Z" : "z";
         this.InsertCharacter(ch);
      }

      private void XKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "X" : "x";
         this.InsertCharacter(ch);
      }

      private void CKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "C" : "c";
         this.InsertCharacter(ch);
      }

      private void VkeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "V" : "v";
         this.InsertCharacter(ch);
      }

      private void BKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "B" : "b";
         this.InsertCharacter(ch);
      }

      private void NKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "N" : "n";
         this.InsertCharacter(ch);
      }

      private void MKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(true) != false) ? "M" : "m";
         this.InsertCharacter(ch);
      }

      private void CommaKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "<" : ",";
         this.InsertCharacter(ch);
      }

      private void PeriodKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? ">" : ".";
         this.InsertCharacter(ch);
      }

      private void ForwardSlashKeyButton_Click(object sender, EventArgs e)
      {
         string ch = (this.GetShift(false) != false) ? "?" : "/";
         this.InsertCharacter(ch);
      }

      private void RightShiftKeyButton_Click(object sender, EventArgs e)
      {
         this.shifted = !this.shifted;
         this.UpdateKeys();
         this.OsdRichTextBox.Focus();
      }

      private void UpArrowKeyButton_Click(object sender, EventArgs e)
      {
         int currentLine = this.OsdRichTextBox.GetLineFromCharIndex(this.OsdRichTextBox.SelectionStart);
         int startOfCurrentLine = this.OsdRichTextBox.GetFirstCharIndexOfCurrentLine();
         int offsetIntoLine = this.OsdRichTextBox.SelectionStart - startOfCurrentLine;

         if (currentLine > 0)
         {
            this.doDeep = true;

            int startOfPreviousLine = this.OsdRichTextBox.GetFirstCharIndexFromLine(currentLine - 1);
            int previousLineLength = startOfCurrentLine - startOfPreviousLine;

            if (previousLineLength >= offsetIntoLine)
            {
               this.OsdRichTextBox.SelectionStart = startOfPreviousLine + offsetIntoLine;
            }
            else
            {
               this.OsdRichTextBox.SelectionStart = startOfCurrentLine - 1;
            }
         }

         this.OsdRichTextBox.Focus();
      }

      private void ExitKeyButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void SpaceKeyButton_Click(object sender, EventArgs e)
      {
         this.InsertCharacter(" ");
      }

      private void LeftArrowKeyButton_Click(object sender, EventArgs e)
      {
         if (this.OsdRichTextBox.SelectionStart > 0)
         {
            this.doDeep = true;
            this.OsdRichTextBox.SelectionStart--;
         }

         this.OsdRichTextBox.Focus();
      }

      private void RightArrowKeyButton_Click(object sender, EventArgs e)
      {
         if (this.OsdRichTextBox.SelectionStart < this.OsdRichTextBox.Text.Length)
         {
            this.doDeep = true;
            this.OsdRichTextBox.SelectionStart++;
         }

         this.OsdRichTextBox.Focus();
      }

      private void DownKeyButton_Click(object sender, EventArgs e)
      {
         int currentLine = this.OsdRichTextBox.GetLineFromCharIndex(this.OsdRichTextBox.SelectionStart);
         int startOfCurrentLine = this.OsdRichTextBox.GetFirstCharIndexOfCurrentLine();
         int offsetIntoLine = this.OsdRichTextBox.SelectionStart - startOfCurrentLine;

         if (currentLine < (this.OsdRichTextBox.Lines.Length-1))
         {
            this.doDeep = true;

            int startOfNextLine = this.OsdRichTextBox.GetFirstCharIndexFromLine(currentLine + 1);
            int nextLineLength = this.OsdRichTextBox.Lines[currentLine + 1].Length;

            if (nextLineLength >= offsetIntoLine)
            {
               this.OsdRichTextBox.SelectionStart = startOfNextLine + offsetIntoLine;
            }
            else
            {
               this.OsdRichTextBox.SelectionStart = startOfNextLine + nextLineLength;
            }
         }

         this.OsdRichTextBox.Focus();
      }

      #endregion

      #region Form Events

      private void OsdForm_Shown(object sender, EventArgs e)
      {
         this.shifted = false;
         this.capsLocked = false;
         this.UpdateKeys();

         this.WriteTextButton.BackColor = Color.FromArgb(240, 240, 240);

         this.DateToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowDate) ? Color.Lime : Color.FromArgb(64, 64, 64);
         this.TimeToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowTime) ? Color.Lime : Color.FromArgb(64, 64, 64);
         this.CameraIdToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowCameraId) ? Color.Lime : Color.FromArgb(64, 64, 64);
         this.DescriptionToggleButton.BackColor = (false != ParameterAccessor.Instance.Osd.ShowDescription) ? Color.Lime : Color.FromArgb(64, 64, 64);
         
         this.OsdRichTextBox.Text = ParameterAccessor.Instance.Osd.Line1 + "\n" +
                                    ParameterAccessor.Instance.Osd.Line2 + "\n" +
                                    ParameterAccessor.Instance.Osd.Line3 + "\n" +
                                    ParameterAccessor.Instance.Osd.Line4 + "\n" +
                                    ParameterAccessor.Instance.Osd.Line5 + "\n" +
                                    ParameterAccessor.Instance.Osd.Line6 + "\n" +
                                    ParameterAccessor.Instance.Osd.Line7 + "\n" +
                                    ParameterAccessor.Instance.Osd.Line8;
         this.OsdRichTextBox.SelectionStart = ParameterAccessor.Instance.Osd.Line1.Length;
         this.OsdRichTextBox.Focus();

         this.executeBeepThread = true;
         this.doDeep = false;
         this.beepThread = new Thread(this.ProcessBeep);
         this.beepThread.IsBackground = true;
         this.beepThread.Start();
      }

      private void LeftPanel_MouseDown(object sender, MouseEventArgs e)
      {
         this.mouseDownPoint = e.Location;
         this.mouseDown = true;
      }

      private void LeftPanel_MouseUp(object sender, MouseEventArgs e)
      {
         this.mouseDown = false;
      }

      private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
      {
         if (false != this.mouseDown)
         {
            this.Top += (e.Y - mouseDownPoint.Y);
            this.Left += (e.X - mouseDownPoint.X);
         }
      }

      private void OsdForm_FormClosed(object sender, FormClosedEventArgs e)
      {
         this.executeBeepThread = false;
         this.beepThread.Join(3000);
      }

      #endregion

      #region Constructor

      public OsdForm()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
