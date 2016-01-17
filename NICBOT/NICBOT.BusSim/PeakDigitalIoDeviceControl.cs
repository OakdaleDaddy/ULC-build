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
   public partial class PeakDigitalIoDeviceControl : DeviceControl
   {
      #region Fields

      private bool active;

      private int nodeId;
      private int outputTimeoutPeriod;
      private int reportRate;
      private int d0PowerUpLevel;
      private int d1PowerUpLevel;
      private int d2PowerUpLevel;
      private int d3PowerUpLevel;
      private int d0TimeoutLevel;
      private int d1TimeoutLevel;
      private int d2TimeoutLevel;
      private int d3TimeoutLevel;

      private DateTime outputTimeLimit;
      private bool outputTimeout;
      private DateTime reportTimeLimit;
      private bool immediateReportNeeded;

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

      private void Report()
      {
         int cobId = this.GetCobId(COBTypes.TPDO1, this.nodeId);

         byte[] rspFrame = new byte[1];

         byte reportValue = 0;
         reportValue |= (byte)((false != DIn0LevelCheckBox.Checked) ? 0x01 : 0);
         reportValue |= (byte)((false != DIn1LevelCheckBox.Checked) ? 0x02 : 0);
         reportValue |= (byte)((false != DIn2LevelCheckBox.Checked) ? 0x04 : 0);
         reportValue |= (byte)((false != DIn3LevelCheckBox.Checked) ? 0x08 : 0);
         reportValue |= (byte)((false != DIn4LevelCheckBox.Checked) ? 0x10 : 0);
         reportValue |= (byte)((false != DIn5LevelCheckBox.Checked) ? 0x20 : 0);
         reportValue |= (byte)((false != DIn6LevelCheckBox.Checked) ? 0x40 : 0);
         reportValue |= (byte)((false != DIn7LevelCheckBox.Checked) ? 0x80 : 0);

         rspFrame[0] = (byte)reportValue;
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
         this.immediateReportNeeded = false;

         this.DOut0TextBox.Text = this.d0PowerUpLevel.ToString();
         this.DOut1TextBox.Text = this.d1PowerUpLevel.ToString();
         this.DOut2TextBox.Text = this.d2PowerUpLevel.ToString();
         this.DOut3TextBox.Text = this.d3PowerUpLevel.ToString();
      }

      #endregion

      #region Form Events

      private void DOut0TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.DIn0FollowCheckBox.Checked)
         {
            this.DIn0LevelCheckBox.Checked = this.GetActive(this.DOut0TextBox.Text);
         }
      }

      private void DOut1TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.DIn1FollowCheckBox.Checked)
         {
            this.DIn1LevelCheckBox.Checked = this.GetActive(this.DOut1TextBox.Text);
         }
      }

      private void DOut2TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.DIn2FollowCheckBox.Checked)
         {
            this.DIn2LevelCheckBox.Checked = this.GetActive(this.DOut2TextBox.Text);
         }
      }

      private void DOut3TextBox_TextChanged(object sender, EventArgs e)
      {
         if (false != this.DIn3FollowCheckBox.Checked)
         {
            this.DIn3LevelCheckBox.Checked = this.GetActive(this.DOut3TextBox.Text);
         }
      }

      private void DIn0FollowCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.DIn0LevelCheckBox.Enabled = !this.DIn0FollowCheckBox.Checked;

         if (false != this.DIn0FollowCheckBox.Checked)
         {
            this.DIn0LevelCheckBox.Checked = this.GetActive(this.DOut0TextBox.Text); 
         }
      }

      private void DIn1FollowCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.DIn1LevelCheckBox.Enabled = !this.DIn1FollowCheckBox.Checked;

         if (false != this.DIn2FollowCheckBox.Checked)
         {
            this.DIn1LevelCheckBox.Checked = this.GetActive(this.DOut1TextBox.Text);
         }
      }

      private void DIn2FollowCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.DIn2LevelCheckBox.Enabled = !this.DIn2FollowCheckBox.Checked;

         if (false != this.DIn2FollowCheckBox.Checked)
         {
            this.DIn2LevelCheckBox.Checked = this.GetActive(this.DOut2TextBox.Text);
         }
      }

      private void DIn3FollowCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.DIn3LevelCheckBox.Enabled = !this.DIn3FollowCheckBox.Checked;

         if (false != this.DIn3FollowCheckBox.Checked)
         {
            this.DIn3LevelCheckBox.Checked = this.GetActive(this.DOut3TextBox.Text);
         }
      }

      private void DIn0LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      private void DIn1LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      private void DIn2LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      private void DIn3LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      private void DIn4LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      private void DIn5LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      private void DIn6LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      private void DIn7LevelCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         this.immediateReportNeeded = true;
      }

      #endregion

      #region Constructor

      public PeakDigitalIoDeviceControl()
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

         keyValue = appKey.GetValue(deviceTag + "D0PowerUpLevel");
         this.DOut0PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "D1PowerUpLevel");
         this.DOut1PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "D2PowerUpLevel");
         this.DOut2PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "D3PowerUpLevel");
         this.DOut3PowerUpLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "D0TimeoutLevel");
         this.DOut0TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "D1TimeoutLevel");
         this.DOut1TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "D2TimeoutLevel");
         this.DOut2TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "D3TimeoutLevel");
         this.DOut3TimeoutLevelTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "DIn0Follows");
         this.DIn0FollowCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn1Follows");
         this.DIn1FollowCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn2Follows");
         this.DIn2FollowCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn3Follows");
         this.DIn3FollowCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn0Level");
         this.DIn0LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn1Level");
         this.DIn1LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn2Level");
         this.DIn2LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn3Level");
         this.DIn3LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn4Level");
         this.DIn4LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn5Level");
         this.DIn5LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn6Level");
         this.DIn6LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "DIn7Level");
         this.DIn7LevelCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "NodeId", this.NodeIdTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());

         appKey.SetValue(deviceTag + "Timeout", this.TimeoutTextBox.Text);
         appKey.SetValue(deviceTag + "Rate", this.RateTextBox.Text);

         appKey.SetValue(deviceTag + "D0PowerUpLevel", this.DOut0PowerUpLevelTextBox.Text);
         appKey.SetValue(deviceTag + "D1PowerUpLevel", this.DOut1PowerUpLevelTextBox.Text);
         appKey.SetValue(deviceTag + "D2PowerUpLevel", this.DOut2PowerUpLevelTextBox.Text);
         appKey.SetValue(deviceTag + "D3PowerUpLevel", this.DOut3PowerUpLevelTextBox.Text);

         appKey.SetValue(deviceTag + "D0TimeoutLevel", this.DOut0TimeoutLevelTextBox.Text);
         appKey.SetValue(deviceTag + "D1TimeoutLevel", this.DOut1TimeoutLevelTextBox.Text);
         appKey.SetValue(deviceTag + "D2TimeoutLevel", this.DOut2TimeoutLevelTextBox.Text);
         appKey.SetValue(deviceTag + "D3TimeoutLevel", this.DOut3TimeoutLevelTextBox.Text);

         appKey.SetValue(deviceTag + "DIn0Follows", this.DIn0FollowCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn1Follows", this.DIn1FollowCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn2Follows", this.DIn2FollowCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn3Follows", this.DIn3FollowCheckBox.Checked ? "1" : "0");

         appKey.SetValue(deviceTag + "DIn0Level", this.DIn0LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn1Level", this.DIn1LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn2Level", this.DIn2LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn3Level", this.DIn3LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn4Level", this.DIn4LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn5Level", this.DIn5LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn6Level", this.DIn6LevelCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "DIn7Level", this.DIn7LevelCheckBox.Checked ? "1" : "0");
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

         else if ("D0PowerUpLevel" == name)
         {
            this.DOut0PowerUpLevelTextBox.Text = reader.Value;
         }
         else if ("D1PowerUpLevel" == name)
         {
            this.DOut1PowerUpLevelTextBox.Text = reader.Value;
         }
         else if ("D2PowerUpLevel" == name)
         {
            this.DOut2PowerUpLevelTextBox.Text = reader.Value;
         }
         else if ("D3PowerUpLevel" == name)
         {
            this.DOut3PowerUpLevelTextBox.Text = reader.Value;
         }

         else if ("D0TimeoutLevel" == name)
         {
            this.DOut0TimeoutLevelTextBox.Text = reader.Value;
         }
         else if ("D1TimeoutLevel" == name)
         {
            this.DOut1TimeoutLevelTextBox.Text = reader.Value;
         }
         else if ("D2TimeoutLevel" == name)
         {
            this.DOut2TimeoutLevelTextBox.Text = reader.Value;
         }
         else if ("D3TimeoutLevel" == name)
         {
            this.DOut3TimeoutLevelTextBox.Text = reader.Value;
         }

         else if ("DIn0Follows" == name)
         {
            this.DIn0FollowCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn1Follows" == name)
         {
            this.DIn1FollowCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn2Follows" == name)
         {
            this.DIn2FollowCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn3Follows" == name)
         {
            this.DIn3FollowCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }

         else if ("DIn0Level" == name)
         {
            this.DIn0LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn1Level" == name)
         {
            this.DIn1LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn2Level" == name)
         {
            this.DIn2LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn3Level" == name)
         {
            this.DIn3LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn4Level" == name)
         {
            this.DIn4LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn5Level" == name)
         {
            this.DIn5LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn6Level" == name)
         {
            this.DIn6LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("DIn7Level" == name)
         {
            this.DIn7LevelCheckBox.Checked = ("0" != reader.Value) ? true : false;
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

         writer.WriteElementString("D0PowerUpLevel", this.DOut0PowerUpLevelTextBox.Text);
         writer.WriteElementString("D1PowerUpLevel", this.DOut1PowerUpLevelTextBox.Text);
         writer.WriteElementString("D2PowerUpLevel", this.DOut2PowerUpLevelTextBox.Text);
         writer.WriteElementString("D3PowerUpLevel", this.DOut3PowerUpLevelTextBox.Text);

         writer.WriteElementString("D0TimeoutLevel", this.DOut0TimeoutLevelTextBox.Text);
         writer.WriteElementString("D1TimeoutLevel", this.DOut1TimeoutLevelTextBox.Text);
         writer.WriteElementString("D2TimeoutLevel", this.DOut2TimeoutLevelTextBox.Text);
         writer.WriteElementString("D3TimeoutLevel", this.DOut3TimeoutLevelTextBox.Text);

         writer.WriteElementString("DIn0Follows", (false != this.DIn0FollowCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn1Follows", (false != this.DIn1FollowCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn2Follows", (false != this.DIn2FollowCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn3Follows", (false != this.DIn3FollowCheckBox.Checked) ? "1" : "0");

         writer.WriteElementString("DIn0Level", (false != this.DIn0LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn1Level", (false != this.DIn1LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn2Level", (false != this.DIn2LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn3Level", (false != this.DIn3LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn4Level", (false != this.DIn4LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn5Level", (false != this.DIn5LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn6Level", (false != this.DIn6LevelCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("DIn7Level", (false != this.DIn7LevelCheckBox.Checked) ? "1" : "0");
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
         this.DOut0PowerUpLevelTextBox.Enabled = false;
         this.DOut1PowerUpLevelTextBox.Enabled = false;
         this.DOut2PowerUpLevelTextBox.Enabled = false;
         this.DOut3PowerUpLevelTextBox.Enabled = false;
         this.DOut0TimeoutLevelTextBox.Enabled = false;
         this.DOut1TimeoutLevelTextBox.Enabled = false;
         this.DOut2TimeoutLevelTextBox.Enabled = false;
         this.DOut3TimeoutLevelTextBox.Enabled = false;

         this.nodeId = 0;
         this.outputTimeoutPeriod = 0;
         this.reportRate = 0;
         this.d0PowerUpLevel = 0;
         this.d1PowerUpLevel = 0;
         this.d2PowerUpLevel = 0;
         this.d3PowerUpLevel = 0;
         this.d0TimeoutLevel = 0;
         this.d1TimeoutLevel = 0;
         this.d2TimeoutLevel = 0;
         this.d3TimeoutLevel = 0;

         if ((int.TryParse(this.NodeIdTextBox.Text, out nodeId) != false) &&
             (int.TryParse(this.TimeoutTextBox.Text, out outputTimeoutPeriod) != false) &&
             (int.TryParse(this.RateTextBox.Text, out reportRate) != false) &&
             (int.TryParse(this.DOut0PowerUpLevelTextBox.Text, out d0PowerUpLevel) != false) &&
             (int.TryParse(this.DOut1PowerUpLevelTextBox.Text, out d1PowerUpLevel) != false) &&
             (int.TryParse(this.DOut2PowerUpLevelTextBox.Text, out d2PowerUpLevel) != false) &&
             (int.TryParse(this.DOut3PowerUpLevelTextBox.Text, out d3PowerUpLevel) != false) &&
             (int.TryParse(this.DOut0TimeoutLevelTextBox.Text, out d0TimeoutLevel) != false) &&
             (int.TryParse(this.DOut1TimeoutLevelTextBox.Text, out d1TimeoutLevel) != false) &&
             (int.TryParse(this.DOut2TimeoutLevelTextBox.Text, out d2TimeoutLevel) != false) &&
             (int.TryParse(this.DOut3TimeoutLevelTextBox.Text, out d3TimeoutLevel) != false))            
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
         this.DOut0PowerUpLevelTextBox.Enabled = true;
         this.DOut1PowerUpLevelTextBox.Enabled = true;
         this.DOut2PowerUpLevelTextBox.Enabled = true;
         this.DOut3PowerUpLevelTextBox.Enabled = true;
         this.DOut0TimeoutLevelTextBox.Enabled = true;
         this.DOut1TimeoutLevelTextBox.Enabled = true;
         this.DOut2TimeoutLevelTextBox.Enabled = true;
         this.DOut3TimeoutLevelTextBox.Enabled = true;

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
                  this.DOut0TextBox.Text = ((msg[0] & 0x01) != 0) ? "1" : "0";
                  this.DOut1TextBox.Text = ((msg[0] & 0x02) != 0) ? "1" : "0";
                  this.DOut2TextBox.Text = ((msg[0] & 0x04) != 0) ? "1" : "0";
                  this.DOut3TextBox.Text = ((msg[0] & 0x08) != 0) ? "1" : "0";

                  this.outputTimeLimit = DateTime.Now.AddMilliseconds(this.outputTimeoutPeriod);
                  this.outputTimeout = false;
               }
            }

         }
      }

      public override void UpdateDevice()
      {
         DateTime now = DateTime.Now;

         if (false != this.active)
         {
            if (false == this.outputTimeout)
            {
               if (now > this.outputTimeLimit)
               {
                  this.outputTimeout = true;
                  this.DOut0TextBox.Text = this.d0TimeoutLevel.ToString();
                  this.DOut1TextBox.Text = this.d1TimeoutLevel.ToString();
                  this.DOut2TextBox.Text = this.d2TimeoutLevel.ToString();
                  this.DOut3TextBox.Text = this.d3TimeoutLevel.ToString();
               }
            }

            if (now > this.reportTimeLimit)
            {
               this.reportTimeLimit = this.reportTimeLimit.AddMilliseconds(this.reportRate);
               this.Report();
            }

            if (false != immediateReportNeeded)
            {
               this.immediateReportNeeded = false;
               this.Report();
            }
         }
      }

      #endregion
   }
}
