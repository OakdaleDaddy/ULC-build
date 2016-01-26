using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

namespace NICBOT.OSD
{
   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "NICBOT OSD Control";

      #endregion

      #region Fields

      private bool active;

      #endregion

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

      private void ValidateWindowLocation(ref int top, ref int left, int defaultValueTop, int defaultValueLeft)
      {
         int topResult = defaultValueTop;
         int leftResult = defaultValueLeft;

         for (int i = 0; i < Screen.AllScreens.Length; i++)
         {
            int minimumLeft = Screen.AllScreens[i].Bounds.X;
            int maximumLeft = minimumLeft + Screen.AllScreens[i].Bounds.Width;
            int minimumTop = Screen.AllScreens[i].Bounds.Y;
            int maximumTop = minimumTop + Screen.AllScreens[i].Bounds.Height;

            if ((top >= minimumTop) && (top <= maximumTop) &&
                (left >= minimumLeft) && (left <= maximumLeft))
            {
               topResult = top;
               leftResult = left;

               break;
            }
         }

         top = topResult;
         left = leftResult;
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
         int defaultValueTop = (SystemInformation.PrimaryMonitorSize.Height - this.Height) / 2;
         int top = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         keyValue = appKey.GetValue("Left");
         int defaultValueLeft = (SystemInformation.PrimaryMonitorSize.Width - this.Width) / 2;
         int left = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : defaultValue) : defaultValue;

         this.ValidateWindowLocation(ref top, ref left, defaultValueTop, defaultValueLeft);
         this.Top = top;
         this.Left = left;

         keyValue = appKey.GetValue("WindowState");
         this.WindowState = ((null != keyValue) && Enum.IsDefined(this.WindowState.GetType(), keyValue)) ? (FormWindowState)Enum.Parse(this.WindowState.GetType(), keyValue.ToString()) : FormWindowState.Normal;

         #endregion

         #region Session Information

         keyValue = appKey.GetValue("Port");
         this.PortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1";

         keyValue = appKey.GetValue("BaudRate");
         this.BaudTextBox.Text = (null != keyValue) ? keyValue.ToString() : "9600";

         keyValue = appKey.GetValue("HorizontalOffset");
         this.HorizontalOffsetTextBox.Text = (null != keyValue) ? keyValue.ToString() : "0";

         keyValue = appKey.GetValue("VerticleOffset");
         this.VerticalOffsetTextBox.Text = (null != keyValue) ? keyValue.ToString() : "0";

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

         appKey.SetValue("Port", this.PortTextBox.Text);
         appKey.SetValue("BaudRate", this.BaudTextBox.Text);
         appKey.SetValue("HorizontalOffset", this.HorizontalOffsetTextBox.Text);
         appKey.SetValue("VerticleOffset", this.VerticalOffsetTextBox.Text);

         #endregion
      }

      #endregion

      #region Helper Functions

      private void LockControls()
      {
         this.BaudTextBox.Enabled = false;
         this.PortTextBox.Enabled = false;

         this.UpButton.Enabled = true;
         this.DownButton.Enabled = true;
         this.LeftButton.Enabled = true;
         this.RightButton.Enabled = true;
         this.HorizontalOffsetTextBox.Enabled = true;
         this.VerticalOffsetTextBox.Enabled = true;
         this.SetButton.Enabled = true;
      }

      private void UnlockControls()
      {
         this.BaudTextBox.Enabled = true;
         this.PortTextBox.Enabled = true;

         this.UpButton.Enabled = false;
         this.DownButton.Enabled = false;
         this.LeftButton.Enabled = false;
         this.RightButton.Enabled = false;
         this.HorizontalOffsetTextBox.Enabled = false;
         this.VerticalOffsetTextBox.Enabled = false;
         this.SetButton.Enabled = false;
      }

      #endregion

      #region User Events

      private void ActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.active)
         {
            int port = 0;
            int baudrate = 0;

            if ((int.TryParse(this.PortTextBox.Text, out port) != false) &&
                (int.TryParse(this.BaudTextBox.Text, out baudrate) != false))
            {
               VideoStampOsd.Instance.Start(port, baudrate);
               string faultReason = VideoStampOsd.Instance.FaultReason;

               if (null == faultReason)
               {
                  this.StatusLabel.Text = "OSD interface started.";
                  this.ActivityButton.Text = "Stop";
                  this.active = true;
                  this.LockControls();
               }
               else
               {
                  this.StatusLabel.Text = string.Format("Unable to start: '{0}'.", faultReason);
               }
            }
            else
            {
               this.StatusLabel.Text = "Invalid entry.";
            }
         }
         else
         {
            VideoStampOsd.Instance.Stop();

            this.StatusLabel.Text = "OSD interface stopped.";
            this.ActivityButton.Text = "Start";
            this.active = false;
            this.UnlockControls();
         }
      }

      private void SetButton_Click(object sender, EventArgs e)
      {
         int horizontalOffset = 0;
         int verticalOffset = 0;

         if ((int.TryParse(this.HorizontalOffsetTextBox.Text, out horizontalOffset) != false) &&
             (int.TryParse(this.VerticalOffsetTextBox.Text, out verticalOffset) != false))
         {
            VideoStampOsd.Instance.SetVideoChannel(0); // all channels
            VideoStampOsd.Instance.SetScreenHorizontalPositionOffset(horizontalOffset);
            VideoStampOsd.Instance.SetScreenVerticalPositionOffset(verticalOffset);

            this.StatusLabel.Text = "OSD offset values set.";
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void UpButton_Click(object sender, EventArgs e)
      {
         int verticalOffset = 0;

         if (int.TryParse(this.VerticalOffsetTextBox.Text, out verticalOffset) != false)
         {
            int adjustedVerticalOffset = verticalOffset;
            
            VideoStampOsd.Instance.SetVideoChannel(0); // all channels
            VideoStampOsd.Instance.IncreaseVeriticalOffset(ref adjustedVerticalOffset);

            if (adjustedVerticalOffset != verticalOffset)
            {
               this.VerticalOffsetTextBox.Text = adjustedVerticalOffset.ToString();
               this.StatusLabel.Text = "OSD vertical offset increased.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void DownButton_Click(object sender, EventArgs e)
      {
         int verticalOffset = 0;

         if (int.TryParse(this.VerticalOffsetTextBox.Text, out verticalOffset) != false)
         {
            int adjustedVerticalOffset = verticalOffset;

            VideoStampOsd.Instance.SetVideoChannel(0); // all channels
            VideoStampOsd.Instance.DecreaseVeriticalOffset(ref adjustedVerticalOffset);

            if (adjustedVerticalOffset != verticalOffset)
            {
               this.VerticalOffsetTextBox.Text = adjustedVerticalOffset.ToString();
               this.StatusLabel.Text = "OSD vertical offset decreased.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void LeftButton_Click(object sender, EventArgs e)
      {
         int horizontalOffset = 0;

         if (int.TryParse(this.HorizontalOffsetTextBox.Text, out horizontalOffset) != false)
         {
            int adjustedHorizontalOffset = horizontalOffset;

            VideoStampOsd.Instance.SetVideoChannel(0); // all channels
            VideoStampOsd.Instance.DecreaseHorizontalOffset(ref adjustedHorizontalOffset);

            if (adjustedHorizontalOffset != horizontalOffset)
            {
               this.HorizontalOffsetTextBox.Text = adjustedHorizontalOffset.ToString();
               this.StatusLabel.Text = "OSD horizontal offset decreased.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      private void RightButton_Click(object sender, EventArgs e)
      {
         int horizontalOffset = 0;

         if (int.TryParse(this.HorizontalOffsetTextBox.Text, out horizontalOffset) != false)
         {
            int adjustedHorizontalOffset = horizontalOffset;

            VideoStampOsd.Instance.SetVideoChannel(0); // all channels
            VideoStampOsd.Instance.IncreaseHorizontalOffset(ref adjustedHorizontalOffset);

            if (adjustedHorizontalOffset != horizontalOffset)
            {
               this.HorizontalOffsetTextBox.Text = adjustedHorizontalOffset.ToString();
               this.StatusLabel.Text = "OSD horizontal offset increased.";
            }
         }
         else
         {
            this.StatusLabel.Text = "Invalid entry.";
         }
      }

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();

         this.StatusLabel.Text = "";
         this.ActivityButton.Text = "Start";
         this.active = false;

         this.UnlockControls();
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
      }

      #endregion

   }
}
