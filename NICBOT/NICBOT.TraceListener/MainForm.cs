using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

namespace NICBOT.TraceListener
{
   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "NICBOT Trace Listener";

      #endregion

      private bool active;
      private UdpClient listener;

      #region Registry Functions

      private RegistryKey GetAppKey()
      {
         RegistryKey softwareKey;
         RegistryKey ownerKey;
         RegistryKey appKey;

         softwareKey = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);

         if (null == softwareKey)
         {
            softwareKey = Registry.CurrentUser.CreateSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);
         }

         ownerKey = softwareKey.OpenSubKey(RegistryCompanyName, RegistryKeyPermissionCheck.ReadWriteSubTree);

         if (null == ownerKey)
         {
            ownerKey = softwareKey.CreateSubKey(RegistryCompanyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
         }

         appKey = ownerKey.OpenSubKey(RegistryApplicationName, RegistryKeyPermissionCheck.ReadWriteSubTree);

         if (null == appKey)
         {
            appKey = ownerKey.CreateSubKey(RegistryApplicationName, RegistryKeyPermissionCheck.ReadWriteSubTree);
         }

         return (appKey);
      }

      private void LoadRegistry()
      {
         RegistryKey appKey;
         object keyValue;
         int defaultValue;
         int parsedValue;

         appKey = this.GetAppKey();

         #region Window Location

         keyValue = appKey.GetValue("Height");
         defaultValue = this.MinimumSize.Height;
         this.Height = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Width");
         defaultValue = this.MinimumSize.Width;
         this.Width = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Top");
         defaultValue = (SystemInformation.PrimaryMonitorSize.Height - this.Height) / 2;
         this.Top = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Left");
         defaultValue = (SystemInformation.PrimaryMonitorSize.Width - this.Width) / 2;
         this.Left = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("WindowState");
         this.WindowState = ((null != keyValue) && Enum.IsDefined(this.WindowState.GetType(), keyValue)) ? (FormWindowState)Enum.Parse(this.WindowState.GetType(), keyValue.ToString()) : FormWindowState.Normal;

         #endregion

         #region Session Information

         keyValue = appKey.GetValue("LogFontName");
         string fontName = (null != keyValue) ? keyValue.ToString() : "Microsoft Sans Serif";
         
         keyValue = appKey.GetValue("LogFontSize");
         string fontSize = (null != keyValue) ? keyValue.ToString() : "8.25";

         float fontSizeValue = 8.25f;
         float tempFontSizeValue = 8.25f;
         if (float.TryParse(fontSize, out tempFontSizeValue) != false)
         {
            fontSizeValue = tempFontSizeValue;
         }

         this.LogRichTextBox.Font = new Font(fontName, fontSizeValue); 

         keyValue = appKey.GetValue("Port");
         this.PortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "10000";

         keyValue = appKey.GetValue("AutoScroll");
         this.ScrollToCursorCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "1")) ? true : false;

         #endregion
      }

      private void SaveRegistry()
      {
         RegistryKey appKey;

         appKey = this.GetAppKey();

         #region Window Location

         if (this.WindowState != FormWindowState.Minimized)
         {
            appKey.SetValue("Height", this.Height);
            appKey.SetValue("Width", this.Width);
            appKey.SetValue("Top", this.Top);
            appKey.SetValue("Left", this.Left);
         }

         appKey.SetValue("WindowState", this.WindowState);

         #endregion

         #region Session Information

         appKey.SetValue("LogFontName", this.LogRichTextBox.Font.Name);
         appKey.SetValue("LogFontSize", this.LogRichTextBox.Font.Size);
         appKey.SetValue("Port", this.PortTextBox.Text);
         appKey.SetValue("AutoScroll", this.ScrollToCursorCheckBox.Checked ? "1" : "0");

         #endregion
      }

      #endregion

      #region User Events

      private void LogRichTextBox_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == System.Windows.Forms.MouseButtons.Right)
         {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = this.LogRichTextBox.Font;

            DialogResult result = fontDialog.ShowDialog(this);

            if (DialogResult.OK == result)
            {
               this.LogRichTextBox.Font = fontDialog.Font;
            }
         }
      }

      private void ActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.active)
         {
            int port = 0;

            if (int.TryParse(this.PortTextBox.Text, out port) != false)
            {
               this.listener = new UdpClient(port);

               this.active = true;
               this.ActivityButton.Text = "Stop";

               this.UpdateTimer.Enabled = true;
            }
         }
         else
         {
            this.listener.Close();
            this.listener = null;

            this.active = false;
            this.ActivityButton.Text = "Start";

            this.UpdateTimer.Enabled = false;
         }
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         if (null != this.listener)
         {
            bool appended = false;

            while (0 != this.listener.Available)
            {
               IPEndPoint remoteEndpoint = default(IPEndPoint);
               byte[] datagram = this.listener.Receive(ref remoteEndpoint);

               if ((null != datagram) && (datagram.Length > 4))
               {
                  int length = (datagram[0] << 8) | datagram[1];
                  int sequenceId = (datagram[2] << 8) | datagram[3];
                  int messageLength = datagram.Length - 4;
                  string message = Encoding.UTF8.GetString(datagram, 4, messageLength);

                  string logString = string.Format("{0:D5} {1}", sequenceId, message);
                  this.LogRichTextBox.AppendText(logString + "\n");

                  appended = true;
               }
            }

            if ((false != appended) && (false != this.ScrollToCursorCheckBox.Checked))
            {
               this.LogRichTextBox.ScrollToCaret();
            }
         }
      }

      private void ClearButton_Click(object sender, EventArgs e)
      {
         this.LogRichTextBox.Clear();
      }

      private void CloseButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();
      }

      private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
      {
         this.SaveRegistry();
      }

      #endregion

      #region Constructor

      public MainForm()
      {
         this.InitializeComponent();
         this.ActivityButton.Text = "Start";
      }

      #endregion

   }
}
