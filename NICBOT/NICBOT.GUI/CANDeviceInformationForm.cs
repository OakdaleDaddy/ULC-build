﻿
namespace NICBOT.GUI
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

   using NICBOT.CAN;

   public partial class CANDeviceInformationForm : Form
   {
      #region Properties

      public string Title { set; get; }
      public CAN.Device Device { set; get; }

      #endregion

      #region Helper Functions

      private void SetLogSelect(Button button, bool selected)
      {
         if (false != selected)
         {
            button.ForeColor = Color.White;
         }
         else
         {
            button.ForeColor = Color.Black;
         }
      }

      #endregion

      #region User Events

      private void SdoButton_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceSDO = !this.Device.TraceSDO;
            this.SetLogSelect(this.SdoButton, this.Device.TraceSDO);
         }
      }

      private void HbButton_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceHB = !this.Device.TraceHB;
            this.SetLogSelect(this.HbButton, this.Device.TraceHB);
         }
      }

      private void Rpdo1Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO1 = !this.Device.TraceRPDO1;
            this.SetLogSelect(this.Rpdo1Button, this.Device.TraceRPDO1);
         }
      }

      private void Tpdo1Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO1 = !this.Device.TraceTPDO1;
            this.SetLogSelect(this.Tpdo1Button, this.Device.TraceTPDO1);
         }
      }

      private void Rpdo2Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO2 = !this.Device.TraceRPDO2;
            this.SetLogSelect(this.Rpdo2Button, this.Device.TraceRPDO2);
         }
      }

      private void Tpdo2Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO2 = !this.Device.TraceTPDO2;
            this.SetLogSelect(this.Tpdo2Button, this.Device.TraceTPDO2);
         }
      }

      private void Rpdo3Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO3 = !this.Device.TraceRPDO3;
            this.SetLogSelect(this.Rpdo3Button, this.Device.TraceRPDO3);
         }
      }

      private void Tpdo3Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO3 = !this.Device.TraceTPDO3;
            this.SetLogSelect(this.Tpdo3Button, this.Device.TraceTPDO3);
         }
      }

      private void Rpdo4Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceRPDO4 = !this.Device.TraceRPDO4;
            this.SetLogSelect(this.Rpdo4Button, this.Device.TraceRPDO4);
         }
      }

      private void Tpdo4Button_Click(object sender, EventArgs e)
      {
         if (null != this.Device)
         {
            this.Device.TraceTPDO4 = !this.Device.TraceTPDO4;
            this.SetLogSelect(this.Tpdo4Button, this.Device.TraceTPDO4);
         }
      }

      private void RestartButton_Click(object sender, EventArgs e)
      {
         this.Device.Reset();
         this.Device.Configure();
         this.Device.Start();
      }

      private void BackButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      #endregion

      #region Form Events

      private void CANDeviceInformationForm_Shown(object sender, EventArgs e)
      {
         this.TitleLabel.Text = this.Title;

         if (null != this.Device)
         {
            this.DeviceNameValuePanel.ValueText = this.Device.DeviceName;
            this.DeviceVersionValuePanel.ValueText = this.Device.DeviceVersion;
            this.DeviceTypeValuePanel.ValueText = string.Format("{0:X8}", this.Device.DeviceType);
            this.LoggingNameValuePanel.ValueText = this.Device.Name;
            this.NodeIdValuePanel.ValueText = this.Device.NodeId.ToString();

            this.SetLogSelect(this.SdoButton, this.Device.TraceSDO);
            this.SetLogSelect(this.HbButton, this.Device.TraceHB);
            this.SetLogSelect(this.Rpdo1Button, this.Device.TraceRPDO1);
            this.SetLogSelect(this.Rpdo2Button, this.Device.TraceRPDO2);
            this.SetLogSelect(this.Rpdo3Button, this.Device.TraceRPDO3);
            this.SetLogSelect(this.Rpdo4Button, this.Device.TraceRPDO4);
            this.SetLogSelect(this.Tpdo1Button, this.Device.TraceTPDO1);
            this.SetLogSelect(this.Tpdo2Button, this.Device.TraceTPDO2);
            this.SetLogSelect(this.Tpdo3Button, this.Device.TraceTPDO3);
            this.SetLogSelect(this.Tpdo4Button, this.Device.TraceTPDO4);
         }
      }

      #endregion

      #region Constructor

      public CANDeviceInformationForm()
      {
         this.InitializeComponent();
      }

      #endregion

   }
}
