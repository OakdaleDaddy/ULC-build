
namespace E4.DeviceTest
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Drawing;
   using System.Linq;
   using System.Net;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   using E4.PCANLight;

   public partial class BusParametersForm : Form
   {
      #region Fields

      private BusParameters busParameters;

      #endregion

      #region User Events

      private void OkButton_Click(object sender, EventArgs e)
      {
         if (this.busParameters is PciBusParameters)
         {
            int bitRate;

            if (int.TryParse(this.BaudComboBox.Text, out bitRate) != false)
            {
               PciBusParameters pciBusParameters = (PciBusParameters)this.busParameters;
               pciBusParameters.BitRate = bitRate;
               this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
               MessageBox.Show("Entry invalid.", "Invalid Parameters", MessageBoxButtons.OK);
            }
         }
         else if (this.busParameters is UsbBusParameters)
         {
            int bitRate;

            if (int.TryParse(this.BaudComboBox.Text, out bitRate) != false)
            {
               UsbBusParameters usbBusParameters = (UsbBusParameters)this.busParameters;
               usbBusParameters.BitRate = bitRate;
               this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
               MessageBox.Show("Entry invalid.", "Invalid Parameters", MessageBoxButtons.OK);
            }
         }
         else if (this.busParameters is IpGatewayBusParameters)
         {
            IPAddress transmitAddress = null;
            int transmitPort = 0;
            IPAddress receiveAddress = null;
            int receivePort = 0;
            bool valid = true;

            if (false != valid)
            {
               if (IPAddress.TryParse(this.TransmitAddressTextBox.Text, out transmitAddress) == false)
               {
                  valid = false;
                  MessageBox.Show("Invalid transmit address.", "Invalid Parameters", MessageBoxButtons.OK);
               }
           }

            if (false != valid)
            {
               if (int.TryParse(this.TransmitPortTextBox.Text, out transmitPort) == false)
               {
                  valid = false;
                  MessageBox.Show("Invalid transmit port.", "Invalid Parameters", MessageBoxButtons.OK);
               }
            }

            if (false != valid)
            {
               if (IPAddress.TryParse(this.ReceiveAddressTextBox.Text, out receiveAddress) == false)
               {
                  valid = false;
                  MessageBox.Show("Invalid receive address.", "Invalid Parameters", MessageBoxButtons.OK);
               }
            }

            if (false != valid)
            {
               if (int.TryParse(this.ReceivePortTextBox.Text, out receivePort) == false)
               {
                  valid = false;
                  MessageBox.Show("Invalid receive port.", "Invalid Parameters", MessageBoxButtons.OK);
               }
            }

            if (false != valid)
            {
               IpGatewayBusParameters ipGatewayBusParameters = (IpGatewayBusParameters)this.busParameters;
               ipGatewayBusParameters.TransmitEndPoint = new IPEndPoint(transmitAddress, transmitPort);
               ipGatewayBusParameters.ReceiveEndPoint = new IPEndPoint(receiveAddress, receivePort);
               this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
         }
         else
         {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
         }
      }

      private void CancelAButton_Click(object sender, EventArgs e)
      {
         this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      }

      #endregion

      #region Constructor

      public BusParametersForm(BusParameters busParameters)
      {
         this.InitializeComponent();

         this.busParameters = busParameters;
         this.Text = this.busParameters.BusInterface.ToString() + " Parameters";

         this.BaudComboBox.Items.Clear();
         this.BaudComboBox.Items.Add("10000");
         this.BaudComboBox.Items.Add("20000");
         this.BaudComboBox.Items.Add("50000");
         this.BaudComboBox.Items.Add("100000");
         this.BaudComboBox.Items.Add("125000");
         this.BaudComboBox.Items.Add("250000");
         this.BaudComboBox.Items.Add("500000");
         this.BaudComboBox.Items.Add("1000000");
         this.BaudComboBox.SelectedIndex = 5;

         if (this.busParameters is PciBusParameters)
         {
            this.IpGatewayParameterPanel.Visible = false;
            this.RateParameterPanel.Left = ((this.Width - this.RateParameterPanel.Width) / 2);

            PciBusParameters pciBusParameters = (PciBusParameters)this.busParameters;
            this.BaudComboBox.Text = pciBusParameters.BitRate.ToString();
         }
         else if (this.busParameters is UsbBusParameters)
         {
            this.IpGatewayParameterPanel.Visible = false;
            this.RateParameterPanel.Left = ((this.Width - this.RateParameterPanel.Width) / 2);

            UsbBusParameters usbBusParameters = (UsbBusParameters)this.busParameters;
            this.BaudComboBox.Text = usbBusParameters.BitRate.ToString();
         }
         else if (this.busParameters is IpGatewayBusParameters)
         {
            this.RateParameterPanel.Visible = false;
            this.IpGatewayParameterPanel.Left = ((this.Width - this.IpGatewayParameterPanel.Width) / 2);

            IpGatewayBusParameters ipGatewayBusParameters = (IpGatewayBusParameters)this.busParameters;

            if (null != ipGatewayBusParameters.TransmitEndPoint)
            {
               this.TransmitAddressTextBox.Text = ipGatewayBusParameters.TransmitEndPoint.Address.ToString();
               this.TransmitPortTextBox.Text = ipGatewayBusParameters.TransmitEndPoint.Port.ToString();
            }

            if (null != ipGatewayBusParameters.ReceiveEndPoint)
            {
               this.ReceiveAddressTextBox.Text = ipGatewayBusParameters.ReceiveEndPoint.Address.ToString();
               this.ReceivePortTextBox.Text = ipGatewayBusParameters.ReceiveEndPoint.Port.ToString();
            }
         }
         else
         {
            this.Text = "Unknown Bus Parameters";
         }
      }

      #endregion

   }
}
