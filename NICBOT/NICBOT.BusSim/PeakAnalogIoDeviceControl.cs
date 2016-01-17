using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;

namespace NICBOT.BusSim
{
   public partial class PeakAnalogIoDeviceControl : DeviceControl
   {
      #region Fields

      private bool active;

      private int nodeId;
      private int outputTimeoutPeriod;
      private int reportRate;
      private int a0PowerUpLevel;
      private int a1PowerUpLevel;
      private int a2PowerUpLevel;
      private int a3PowerUpLevel;
      private int a0TimeoutLevel;
      private int a1TimeoutLevel;
      private int a2TimeoutLevel;
      private int a3TimeoutLevel;

      private DateTime outputTimeLimit;
      private bool outputTimeout;
      private DateTime reportTimeLimit;

      #endregion

      #region Helper Functions

      private bool GetActive(string value)
      {
         bool result = true;

         if ((null != value) && ("" != value) && ("0" != value))
         {
            result = false;
         }

         return (result);
      }

      private UInt16 GetValue(string valueString)
      {
         UInt16 result = 0;
         UInt16.TryParse(valueString, out result);

         if (result > 4095)
         {
            result = 4095;
         }

         return (result);
      }

      private void Report()
      {
         int cobId;
         byte[] rspFrame = new byte[8];

         cobId = this.GetCobId(COBTypes.TPDO1, this.nodeId);
         rspFrame[0] = (byte)((this.AIn0AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[1] = (byte)((this.AIn0AnalogInputControl.Value >> 8) & 0xFF);
         rspFrame[2] = (byte)((this.AIn1AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[3] = (byte)((this.AIn1AnalogInputControl.Value >> 8) & 0xFF);
         rspFrame[4] = (byte)((this.AIn2AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[5] = (byte)((this.AIn2AnalogInputControl.Value >> 8) & 0xFF);
         rspFrame[6] = (byte)((this.AIn3AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[7] = (byte)((this.AIn3AnalogInputControl.Value >> 8) & 0xFF);
         this.Transmit(cobId, rspFrame);

         cobId = this.GetCobId(COBTypes.TPDO2, this.nodeId);
         rspFrame[0] = (byte)((this.AIn4AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[1] = (byte)((this.AIn4AnalogInputControl.Value >> 8) & 0xFF);
         rspFrame[2] = (byte)((this.AIn5AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[3] = (byte)((this.AIn5AnalogInputControl.Value >> 8) & 0xFF);
         rspFrame[4] = (byte)((this.AIn6AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[5] = (byte)((this.AIn6AnalogInputControl.Value >> 8) & 0xFF);
         rspFrame[6] = (byte)((this.AIn7AnalogInputControl.Value >> 0) & 0xFF);
         rspFrame[7] = (byte)((this.AIn7AnalogInputControl.Value >> 8) & 0xFF);
         this.Transmit(cobId, rspFrame);
      }

      #endregion

      #region Device Specific Functions

      protected override void Reset()
      {
         this.deviceState = DeviceStates.running;
         this.DeviceStateLabel.Text = "RUNNING";

         DateTime now = DateTime.Now;
         this.outputTimeLimit = now.AddMilliseconds(this.outputTimeoutPeriod);
         this.outputTimeout = false;
         this.reportTimeLimit = now.AddMilliseconds(this.reportRate);

         this.AOut0TextBox.Text = this.a0PowerUpLevel.ToString();
         this.AOut1TextBox.Text = this.a1PowerUpLevel.ToString();
         this.AOut2TextBox.Text = this.a2PowerUpLevel.ToString();
         this.AOut3TextBox.Text = this.a3PowerUpLevel.ToString();
      }

      #endregion

      #region Form Events

      private void AOut0TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.AIn0AnalogInputControl.Follows)
         {
            this.AIn0AnalogInputControl.ValueText = this.AOut0TextBox.Text;
         }
      }

      private void AOut1TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.AIn1AnalogInputControl.Follows)
         {
            this.AIn1AnalogInputControl.ValueText = this.AOut1TextBox.Text;
         }
      }

      private void AOut2TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.AIn2AnalogInputControl.Follows)
         {
            this.AIn2AnalogInputControl.ValueText = this.AOut2TextBox.Text;
         }
      }

      private void AOut3TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.AIn3AnalogInputControl.Follows)
         {
            this.AIn3AnalogInputControl.ValueText = this.AOut3TextBox.Text;
         }
      }

      #endregion

      #region Constructor

      public PeakAnalogIoDeviceControl()
         : base()
      {
         this.InitializeComponent();
      }

      #endregion

      #region Access Functions

      public override void LoadRegistry(RegistryKey appKey, string deviceTag)
      {
         object keyValue;

         keyValue = appKey.GetValue(deviceTag + "Enabled");
         this.EnabledCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "Description");
         this.DescriptionTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "NodeId");
         this.NodeIdTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "BusId");
         string busId = (null != keyValue) ? keyValue.ToString() : "";
         this.SetBusId(busId);

         keyValue = appKey.GetValue(deviceTag + "Timeout");
         this.TimeoutTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "Rate");
         this.RateTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A0PowerUpLevel");
         this.AOut0PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A1PowerUpLevel");
         this.AOut1PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A2PowerUpLevel");
         this.AOut2PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A3PowerUpLevel");
         this.AOut3PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A0TimeoutLevel");
         this.AOut0TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A1TimeoutLevel");
         this.AOut1TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A2TimeoutLevel");
         this.AOut2TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "A3TimeoutLevel");
         this.AOut3TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn0Follows");
         this.AIn0AnalogInputControl.Follows = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "AIn1Follows");
         this.AIn1AnalogInputControl.Follows = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "AIn2Follows");
         this.AIn2AnalogInputControl.Follows = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "AIn3Follows");
         this.AIn3AnalogInputControl.Follows = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "AIn0Level");
         this.AIn0AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn1Level");
         this.AIn1AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn2Level");
         this.AIn2AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn3Level");
         this.AIn3AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn4Level");
         this.AIn4AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn5Level");
         this.AIn5AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn6Level");
         this.AIn6AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "AIn7Level");
         this.AIn7AnalogInputControl.ValueText = (null != keyValue) ? keyValue.ToString() : "";
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "NodeId", this.NodeIdTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());

         appKey.SetValue(deviceTag + "Timeout", this.TimeoutTextBox.Text);
         appKey.SetValue(deviceTag + "Rate", this.RateTextBox.Text);

         appKey.SetValue(deviceTag + "A0PowerUpLevel", this.AOut0PowerUpLevelTextBox.Text);
         appKey.SetValue(deviceTag + "A1PowerUpLevel", this.AOut1PowerUpLevelTextBox.Text);
         appKey.SetValue(deviceTag + "A2PowerUpLevel", this.AOut2PowerUpLevelTextBox.Text);
         appKey.SetValue(deviceTag + "A3PowerUpLevel", this.AOut3PowerUpLevelTextBox.Text);

         appKey.SetValue(deviceTag + "A0TimeoutLevel", this.AOut0TimeoutLevelTextBox.Text);
         appKey.SetValue(deviceTag + "A1TimeoutLevel", this.AOut1TimeoutLevelTextBox.Text);
         appKey.SetValue(deviceTag + "A2TimeoutLevel", this.AOut2TimeoutLevelTextBox.Text);
         appKey.SetValue(deviceTag + "A3TimeoutLevel", this.AOut3TimeoutLevelTextBox.Text);

         appKey.SetValue(deviceTag + "AIn0Follows", this.AIn0AnalogInputControl.Follows ? "1" : "0");
         appKey.SetValue(deviceTag + "AIn1Follows", this.AIn1AnalogInputControl.Follows ? "1" : "0");
         appKey.SetValue(deviceTag + "AIn2Follows", this.AIn2AnalogInputControl.Follows ? "1" : "0");
         appKey.SetValue(deviceTag + "AIn3Follows", this.AIn3AnalogInputControl.Follows ? "1" : "0");

         appKey.SetValue(deviceTag + "AIn0Level", this.AIn0AnalogInputControl.ValueText);
         appKey.SetValue(deviceTag + "AIn1Level", this.AIn1AnalogInputControl.ValueText);
         appKey.SetValue(deviceTag + "AIn2Level", this.AIn2AnalogInputControl.ValueText);
         appKey.SetValue(deviceTag + "AIn3Level", this.AIn3AnalogInputControl.ValueText);
         appKey.SetValue(deviceTag + "AIn4Level", this.AIn4AnalogInputControl.ValueText);
         appKey.SetValue(deviceTag + "AIn5Level", this.AIn5AnalogInputControl.ValueText);
         appKey.SetValue(deviceTag + "AIn6Level", this.AIn6AnalogInputControl.ValueText);
         appKey.SetValue(deviceTag + "AIn7Level", this.AIn7AnalogInputControl.ValueText);
      }

      public override void Read(XmlReader reader)
      {
         string name = reader.Name;
         reader.Read();

         if ("Enabled" == name)
         {
            this.EnabledCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("Description" == name)
         {
            this.DescriptionTextBox.Text = reader.Value;
         }
         else if ("NodeId" == name)
         {
            this.NodeIdTextBox.Text = reader.Value;
         }
         else if ("BusId" == name)
         {
            this.SetBusId(reader.Value);
         }

         else if ("Timeout" == name)
         {
            this.TimeoutTextBox.Text = reader.Value;
         }
         else if ("Rate" == name)
         {
            this.RateTextBox.Text = reader.Value;
         }

         else if ("A0PowerUpLevel" == name)
         {
            this.AOut0PowerUpLevelTextBox.Text = reader.Value;
         }
         else if ("A1PowerUpLevel" == name)
         {
            this.AOut1PowerUpLevelTextBox.Text = reader.Value;
         }
         else if ("A2PowerUpLevel" == name)
         {
            this.AOut2PowerUpLevelTextBox.Text = reader.Value;
         }
         else if ("A3PowerUpLevel" == name)
         {
            this.AOut3PowerUpLevelTextBox.Text = reader.Value;
         }

         else if ("A0TimeoutLevel" == name)
         {
            this.AOut0TimeoutLevelTextBox.Text = reader.Value;
         }
         else if ("A1TimeoutLevel" == name)
         {
            this.AOut1TimeoutLevelTextBox.Text = reader.Value;
         }
         else if ("A2TimeoutLevel" == name)
         {
            this.AOut2TimeoutLevelTextBox.Text = reader.Value;
         }
         else if ("A3TimeoutLevel" == name)
         {
            this.AOut3TimeoutLevelTextBox.Text = reader.Value;
         }

         else if ("AIn0Follows" == name)
         {
            this.AIn0AnalogInputControl.Follows = ("0" != reader.Value) ? true : false;
         }
         else if ("AIn1Follows" == name)
         {
            this.AIn1AnalogInputControl.Follows = ("0" != reader.Value) ? true : false;
         }
         else if ("AIn2Follows" == name)
         {
            this.AIn2AnalogInputControl.Follows = ("0" != reader.Value) ? true : false;
         }
         else if ("AIn3Follows" == name)
         {
            this.AIn3AnalogInputControl.Follows = ("0" != reader.Value) ? true : false;
         }

         else if ("AIn0Level" == name)
         {
            this.AIn0AnalogInputControl.ValueText = reader.Value;
         }
         else if ("AIn1Level" == name)
         {
            this.AIn1AnalogInputControl.ValueText = reader.Value;
         }
         else if ("AIn2Level" == name)
         {
            this.AIn2AnalogInputControl.ValueText = reader.Value;
         }
         else if ("AIn3Level" == name)
         {
            this.AIn3AnalogInputControl.ValueText = reader.Value;
         }
         else if ("AIn4Level" == name)
         {
            this.AIn4AnalogInputControl.ValueText = reader.Value;
         }
         else if ("AIn5Level" == name)
         {
            this.AIn5AnalogInputControl.ValueText = reader.Value;
         }
         else if ("AIn6Level" == name)
         {
            this.AIn6AnalogInputControl.ValueText = reader.Value;
         }
         else if ("AIn7Level" == name)
         {
            this.AIn7AnalogInputControl.ValueText = reader.Value;
         }       
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("NodeId", this.NodeIdTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());

         writer.WriteElementString("Timeout", this.TimeoutTextBox.Text);
         writer.WriteElementString("Rate", this.RateTextBox.Text);

         writer.WriteElementString("A0PowerUpLevel", this.AOut0PowerUpLevelTextBox.Text);
         writer.WriteElementString("A1PowerUpLevel", this.AOut1PowerUpLevelTextBox.Text);
         writer.WriteElementString("A2PowerUpLevel", this.AOut2PowerUpLevelTextBox.Text);
         writer.WriteElementString("A3PowerUpLevel", this.AOut3PowerUpLevelTextBox.Text);

         writer.WriteElementString("A0TimeoutLevel", this.AOut0TimeoutLevelTextBox.Text);
         writer.WriteElementString("A1TimeoutLevel", this.AOut1TimeoutLevelTextBox.Text);
         writer.WriteElementString("A2TimeoutLevel", this.AOut2TimeoutLevelTextBox.Text);
         writer.WriteElementString("A3TimeoutLevel", this.AOut3TimeoutLevelTextBox.Text);

         writer.WriteElementString("AIn0Follows", (false != this.AIn0AnalogInputControl.Follows) ? "1" : "0");
         writer.WriteElementString("AIn1Follows", (false != this.AIn1AnalogInputControl.Follows) ? "1" : "0");
         writer.WriteElementString("AIn2Follows", (false != this.AIn2AnalogInputControl.Follows) ? "1" : "0");
         writer.WriteElementString("AIn3Follows", (false != this.AIn3AnalogInputControl.Follows) ? "1" : "0");

         writer.WriteElementString("AIn0Level", this.AIn0AnalogInputControl.ValueText);
         writer.WriteElementString("AIn1Level", this.AIn1AnalogInputControl.ValueText);
         writer.WriteElementString("AIn2Level", this.AIn2AnalogInputControl.ValueText);
         writer.WriteElementString("AIn3Level", this.AIn3AnalogInputControl.ValueText);
         writer.WriteElementString("AIn4Level", this.AIn4AnalogInputControl.ValueText);
         writer.WriteElementString("AIn5Level", this.AIn5AnalogInputControl.ValueText);
         writer.WriteElementString("AIn6Level", this.AIn6AnalogInputControl.ValueText);
         writer.WriteElementString("AIn7Level", this.AIn7AnalogInputControl.ValueText);
      }

      public override void SetBusId(string busId)
      {
         this.BusIdTextBox.Text = busId;
         base.SetBusId(busId);
      }

      public override void PowerUp()
      {
         this.EnabledCheckBox.Enabled = false;
         this.DescriptionTextBox.Enabled = false;
         this.NodeIdTextBox.Enabled = false;
         this.TimeoutTextBox.Enabled = false;
         this.RateTextBox.Enabled = false;
         this.AOut0PowerUpLevelTextBox.Enabled = false;
         this.AOut1PowerUpLevelTextBox.Enabled = false;
         this.AOut2PowerUpLevelTextBox.Enabled = false;
         this.AOut3PowerUpLevelTextBox.Enabled = false;
         this.AOut0TimeoutLevelTextBox.Enabled = false;
         this.AOut1TimeoutLevelTextBox.Enabled = false;
         this.AOut2TimeoutLevelTextBox.Enabled = false;
         this.AOut3TimeoutLevelTextBox.Enabled = false;

         this.nodeId = 0;
         this.outputTimeoutPeriod = 0;
         this.reportRate = 0;
         this.a0PowerUpLevel = 0;
         this.a1PowerUpLevel = 0;
         this.a2PowerUpLevel = 0;
         this.a3PowerUpLevel = 0;
         this.a0TimeoutLevel = 0;
         this.a1TimeoutLevel = 0;
         this.a2TimeoutLevel = 0;
         this.a3TimeoutLevel = 0;

         if ((int.TryParse(this.NodeIdTextBox.Text, out nodeId) != false) &&
             (int.TryParse(this.TimeoutTextBox.Text, out outputTimeoutPeriod) != false) &&
             (int.TryParse(this.RateTextBox.Text, out reportRate) != false) &&
             (int.TryParse(this.AOut0PowerUpLevelTextBox.Text, out a0PowerUpLevel) != false) &&
             (int.TryParse(this.AOut1PowerUpLevelTextBox.Text, out a1PowerUpLevel) != false) &&
             (int.TryParse(this.AOut2PowerUpLevelTextBox.Text, out a2PowerUpLevel) != false) &&
             (int.TryParse(this.AOut3PowerUpLevelTextBox.Text, out a3PowerUpLevel) != false) &&
             (int.TryParse(this.AOut0TimeoutLevelTextBox.Text, out a0TimeoutLevel) != false) &&
             (int.TryParse(this.AOut1TimeoutLevelTextBox.Text, out a1TimeoutLevel) != false) &&
             (int.TryParse(this.AOut2TimeoutLevelTextBox.Text, out a2TimeoutLevel) != false) &&
             (int.TryParse(this.AOut3TimeoutLevelTextBox.Text, out a3TimeoutLevel) != false))
         {
            this.active = this.EnabledCheckBox.Checked;

            if (false != this.active)
            {
               this.Reset();
            }
            else
            {
               this.DeviceStateLabel.Text = "DISABLED";
            }
         }
         else
         {
            this.DeviceStateLabel.Text = "ERROR";
         }
      }

      public override void PowerDown()
      {
         this.active = false;

         this.EnabledCheckBox.Enabled = true;
         this.DescriptionTextBox.Enabled = true;
         this.NodeIdTextBox.Enabled = true;
         this.TimeoutTextBox.Enabled = true;
         this.RateTextBox.Enabled = true;
         this.AOut0PowerUpLevelTextBox.Enabled = true;
         this.AOut1PowerUpLevelTextBox.Enabled = true;
         this.AOut2PowerUpLevelTextBox.Enabled = true;
         this.AOut3PowerUpLevelTextBox.Enabled = true;
         this.AOut0TimeoutLevelTextBox.Enabled = true;
         this.AOut1TimeoutLevelTextBox.Enabled = true;
         this.AOut2TimeoutLevelTextBox.Enabled = true;
         this.AOut3TimeoutLevelTextBox.Enabled = true;

         this.deviceState = DeviceStates.stopped;
         this.DeviceStateLabel.Text = "OFF";
      }

      public override void DeviceReceive(int cobId, byte[] msg)
      {
         if (false != this.active)
         {
            COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
            int nodeId = (int)(cobId & 0x7F);

            if (COBTypes.RPDO1 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.AOut0TextBox.Text = BitConverter.ToUInt16(msg, 0).ToString();
                  this.AOut1TextBox.Text = BitConverter.ToUInt16(msg, 2).ToString();
                  this.AOut2TextBox.Text = BitConverter.ToUInt16(msg, 4).ToString();
                  this.AOut3TextBox.Text = BitConverter.ToUInt16(msg, 6).ToString();

                  this.outputTimeLimit = DateTime.Now.AddMilliseconds(this.outputTimeoutPeriod);
                  this.outputTimeout = false;
               }
            }

         }
      }

      public override void UpdateDevice()
      {
         if (false != this.active)
         {
            DateTime now = DateTime.Now;

            if (false == this.outputTimeout)
            {
               if (now > this.outputTimeLimit)
               {
                  this.outputTimeout = true;
                  this.AOut0TextBox.Text = this.a0TimeoutLevel.ToString();
                  this.AOut1TextBox.Text = this.a1TimeoutLevel.ToString();
                  this.AOut2TextBox.Text = this.a2TimeoutLevel.ToString();
                  this.AOut3TextBox.Text = this.a3TimeoutLevel.ToString();
               }
            }

            if (now > this.reportTimeLimit)
            {
               this.reportTimeLimit = this.reportTimeLimit.AddMilliseconds(this.reportRate);
               this.Report();
            }
         }
      }

      #endregion

   }
}
