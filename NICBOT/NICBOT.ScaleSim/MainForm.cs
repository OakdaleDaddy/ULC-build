namespace NICBOT.ScaleSim
{
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

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "NICBOT Scale Simulator";

      #endregion

      #region Fields

      private bool active;
      private int mode;
      private int unit;

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

         keyValue = appKey.GetValue("Port");
         this.PortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1";

         keyValue = appKey.GetValue("BaudRate");
         this.BaudRateComboBox.SelectedIndex = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 2) : 2;

         keyValue = appKey.GetValue("Mode");
         int mode = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;
         this.StreamModeRadioButton.Checked = (0 == mode);
         this.CommandModeRadioButton.Checked = (1 == mode);

         keyValue = appKey.GetValue("Unit");
         int unit = (null != keyValue) ? (int.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0;
         this.PoundRadioButton.Checked = (0 == unit);
         this.OunceRadioButton.Checked = (1 == unit);
         this.KilogramRadioButton.Checked = (2 == unit);

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
         appKey.SetValue("BaudRate", this.BaudRateComboBox.SelectedIndex.ToString());
         appKey.SetValue("Mode", this.mode.ToString());
         appKey.SetValue("Unit", this.unit.ToString());

         #endregion
      }

      #endregion

      #region User Events

      private void StreamModeRadioButton_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.StreamModeRadioButton.Checked)
         {
            this.mode = 0;
         }
      }

      private void CommandModeRadioButton_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.CommandModeRadioButton.Checked)
         {
            this.mode = 1;
         }
      }

      private void PoundRadioButton_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.PoundRadioButton.Checked)
         {
            this.unit = 0;
            this.ScaleTrackBar.Minimum = -750;
            this.ScaleTrackBar.Maximum = 15000;
            this.ScaleTrackBar.Value = 0;
            this.MaximumScaleLabel.Text = "150.00";
            this.MinimumScaleLabel.Text = "-7.50";
            this.ScaleTrackBar_ValueChanged(sender, e);
         }
      }

      private void OunceRadioButton_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.OunceRadioButton.Checked)
         {
            this.unit = 1;
            this.ScaleTrackBar.Minimum = -1200;
            this.ScaleTrackBar.Maximum = 24000;
            this.ScaleTrackBar.Value = 0;
            this.MaximumScaleLabel.Text = "2400.0";
            this.MinimumScaleLabel.Text = "-120.0";
            this.ScaleTrackBar_ValueChanged(sender, e);
         }
      }

      private void KilogramRadioButton_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.KilogramRadioButton.Checked)
         {
            this.unit = 2;
            this.ScaleTrackBar.Minimum = -300;
            this.ScaleTrackBar.Maximum = 6000;
            this.ScaleTrackBar.Value = 0;
            this.MaximumScaleLabel.Text = "60.00";
            this.MinimumScaleLabel.Text = "-3.00";
            this.ScaleTrackBar_ValueChanged(sender, e);
         }
      }

      private void ActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.active)
         {
            int port = 0;
            int baudrate = 0;
            string result = null;

            if ((int.TryParse(this.PortTextBox.Text, out port) != false) &&
                (int.TryParse(this.BaudRateComboBox.Text, out baudrate) != false))
            {
               FgScale.Instance.Start(port, baudrate, this.mode, this.unit, ref result);

               if (null == result)
               {
                  this.StatusLabel.Text = "Scale started.";
                  this.ActivityButton.Text = "Stop";
                  this.active = true;

                  this.StreamModeRadioButton.Enabled = false;
                  this.CommandModeRadioButton.Enabled = false;
                  this.PoundRadioButton.Enabled = false;
                  this.OunceRadioButton.Enabled = false;
                  this.KilogramRadioButton.Enabled = false;

                  this.PortTextBox.Enabled = false;
                  this.BaudRateComboBox.Enabled = false;
               }
               else
               {
                  this.StatusLabel.Text = result;
               }
            }
            else
            {
               this.StatusLabel.Text = "Invalid entry.";
            }
         }
         else
         {
            FgScale.Instance.Stop();

            this.StatusLabel.Text = "Scale stopped.";
            this.ActivityButton.Text = "Start";
            this.active = false;

            this.StreamModeRadioButton.Enabled = true;
            this.CommandModeRadioButton.Enabled = true;
            this.PoundRadioButton.Enabled = true;
            this.OunceRadioButton.Enabled = true;
            this.KilogramRadioButton.Enabled = true;

            this.PortTextBox.Enabled = true;
            this.BaudRateComboBox.Enabled = true;
         }
      }

      private void ScaleTrackBar_ValueChanged(object sender, EventArgs e)
      {
         double value = this.ScaleTrackBar.Value;

         if (1 == this.unit)
         {
            value /= 10;
            this.ActualReadingTextBox.Text = value.ToString("N1");
            FgScale.Instance.SetReading(value);
         }
         else
         {
            value /= 100;
            this.ActualReadingTextBox.Text = value.ToString("N2");
            FgScale.Instance.SetReading(value);
         }

         this.OutOfRangeCheckBox.Checked = false;
      }

      private void SetReadingButton_Click(object sender, EventArgs e)
      {
         double unitValue = 0;
         int scaleValue = 0;

         if (double.TryParse(this.ScaleSetPointTextBox.Text, out unitValue) != false)
         {

            if (1 == this.unit)
            {
               scaleValue = (int)(unitValue * 10);
            }
            else
            {
               scaleValue = (int)(unitValue * 100);
            }

            if ((scaleValue <= this.ScaleTrackBar.Maximum) &&
                (scaleValue >= this.ScaleTrackBar.Minimum))
            {
               this.ScaleTrackBar.Value = scaleValue;
            }
            else
            {
               this.OutOfRangeCheckBox.Checked = true;
            }
         }
      }

      private void OutOfRangeCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         FgScale.Instance.SetOutOfRange(this.OutOfRangeCheckBox.Checked);
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

         this.StreamModeRadioButton.Checked = true;
         this.PoundRadioButton.Checked = true;

         this.BaudRateComboBox.Items.Clear();
         this.BaudRateComboBox.Items.Add("2400");
         this.BaudRateComboBox.Items.Add("4800");
         this.BaudRateComboBox.Items.Add("9600");
         this.BaudRateComboBox.SelectedIndex = 2;

         this.ActivityButton.Text = "Start";
         this.active = false;

         this.StatusLabel.Text = "";
      }

      #endregion
      
   }
}
