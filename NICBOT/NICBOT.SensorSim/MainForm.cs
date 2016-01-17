namespace NICBOT.SensorSim
{
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

   public partial class MainForm : Form
   {
      #region Definition

      private const string RegistryCompanyName = "ULC Robotics";
      private const string RegistryApplicationName = "NICBOT Sensor Simulator";

      #endregion

      #region Fields

      private bool thicknessActive;
      private TcpListener thicknessListener;
      private TcpClient thicknessConnection;
      private AsyncCallback thicknessAcceptCallback;
      private AsyncCallback thicknessRxCallback;
      private byte[] thicknessRxBuffer;
      private byte[] thicknessTxBuffer;
      private StringBuilder thicknessCmdBuffer;
      private DateTime thicknessTimeLimit;
      private bool thicknessNewCmd;
      private string thicknessCmd;
      private bool thicknessRspPending;
      private DateTime thicknessRspTimeLimit;
      private double thicknessRspDelay;
      private double thicknessReading;

      private bool stressActive;
      private TcpListener stressListener;
      private TcpClient stressConnection;
      private AsyncCallback stressAcceptCallback;
      private AsyncCallback stressRxCallback;
      private byte[] stressRxBuffer;
      private byte[] stressTxBuffer;
      private StringBuilder stressCmdBuffer;
      private DateTime stressTimeLimit;
      private bool stressNewCmd;
      private string stressCmd;
      private bool stressRspPending;
      private DateTime stressRspTimeLimit;
      private double stressRspDelay;
      private double stressReading;

      private bool clientConnected;
      private bool clientConnectPending;
      private bool clientConnectComplete;
      private TcpClient clientConnection;
      private byte[] clientReceiveBuffer;
      private string clientRsp;
      private AsyncCallback clientConnectCallback;
      private AsyncCallback clientReceiveCallback;

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

         keyValue = appKey.GetValue("ThicknessAddress");
         this.ThicknessAddressTextBox.Text = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";

         keyValue = appKey.GetValue("ThicknessPort");
         this.ThicknessPortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "5000";

         keyValue = appKey.GetValue("ThicknessDelay");
         this.ThicknessDelayTextBox.Text = (null != keyValue) ? keyValue.ToString() : "250";

         keyValue = appKey.GetValue("ThicknessReading");
         this.ThicknessReadingTextBox.Text = (null != keyValue) ? keyValue.ToString() : "1.23";


         keyValue = appKey.GetValue("StressAddress");
         this.StressAddressTextBox.Text = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";

         keyValue = appKey.GetValue("StressPort");
         this.StressPortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "5001";

         keyValue = appKey.GetValue("StressDelay");
         this.StressDelayTextBox.Text = (null != keyValue) ? keyValue.ToString() : "250";

         keyValue = appKey.GetValue("StressReading");
         this.StressReadingTextBox.Text = (null != keyValue) ? keyValue.ToString() : "0.23";


         keyValue = appKey.GetValue("ClientAddress");
         this.ClientAddressTextBox.Text = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";

         keyValue = appKey.GetValue("ClientPort");
         this.ClientPortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "5000";

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

         appKey.SetValue("ThicknessAddress", this.ThicknessAddressTextBox.Text);
         appKey.SetValue("ThicknessPort", this.ThicknessPortTextBox.Text);
         appKey.SetValue("ThicknessDelay", this.ThicknessDelayTextBox.Text);
         appKey.SetValue("ThicknessReading", this.ThicknessReadingTextBox.Text);

         appKey.SetValue("StressAddress", this.StressAddressTextBox.Text);
         appKey.SetValue("StressPort", this.StressPortTextBox.Text);
         appKey.SetValue("StressDelay", this.StressDelayTextBox.Text);
         appKey.SetValue("StressReading", this.StressReadingTextBox.Text);

         appKey.SetValue("ClientAddress", this.ClientAddressTextBox.Text);
         appKey.SetValue("ClientPort", this.ClientPortTextBox.Text);

         #endregion
      }

      #endregion

      #region Helper Functions

      private void CloseThicknessConnection()
      {
         if (null != this.thicknessConnection)
         {
            this.thicknessConnection.Close();
            this.thicknessConnection = null;
         }
      }

      private void ProcessThicknessCommand(string command)
      {
         if (null != command)
         {
            string[] paramaters = command.Split(new char[] { ',' });

            if (9 == paramaters.Length)
            {
               this.thicknessCmd = command;
               this.thicknessNewCmd = true;
               this.thicknessRspTimeLimit = DateTime.Now.AddMilliseconds(this.thicknessRspDelay);
               this.thicknessRspPending = true;
            }
         }
      }

      private void CloseStressConnection()
      {
         if (null != this.stressConnection)
         {
            this.stressConnection.Close();
            this.stressConnection = null;
         }
      }

      private void ProcessStressCommand(string command)
      {
         if (null != command)
         {
            string[] paramaters = command.Split(new char[] { ',' });

            if (9 == paramaters.Length)
            {
               this.stressCmd = command;
               this.stressNewCmd = true;
               this.stressRspTimeLimit = DateTime.Now.AddMilliseconds(this.stressRspDelay);
               this.stressRspPending = true;
            }
         }
      }

      private void CloseClientConnection()
      {
         if (null != this.clientConnection)
         {
            this.clientConnection.Close();
            this.clientConnection = null;
         }
      }

      #endregion

      #region Delegates

      private void ThicknessAcceptCallback(IAsyncResult result)
      {
         try
         {
            if (false != this.thicknessActive)
            {            
               this.thicknessConnection = this.thicknessListener.EndAcceptTcpClient(result);
               this.thicknessTimeLimit = DateTime.Now.AddSeconds(60);
               this.thicknessConnection.Client.BeginReceive(this.thicknessRxBuffer, 0, this.thicknessRxBuffer.Length, SocketFlags.None, this.thicknessRxCallback, this);
            }
         }
         catch { }
      }

      private void ThicknessReceiveCallback(IAsyncResult result)
      {
         try
         {
            int byteCount = this.thicknessConnection.Client.EndReceive(result);

            if (0 == byteCount)
            {
               this.CloseThicknessConnection();

               if (null != this.thicknessListener)
               {
                  this.thicknessListener.BeginAcceptTcpClient(this.thicknessAcceptCallback, this);
               }
            }
            else
            {
               this.thicknessTimeLimit = DateTime.Now.AddSeconds(60);

               for (int i = 0; i < byteCount; i++)
               {
                  char ch = (char)this.thicknessRxBuffer[i];

                  if ((ch >= ' ') && (ch <= '~'))
                  {
                     this.thicknessCmdBuffer.Append(ch);
                  }
                  else if ('\r' == ch)
                  {
                     this.ProcessThicknessCommand(this.thicknessCmdBuffer.ToString());
                     this.thicknessCmdBuffer.Clear();
                  }
               }

               this.thicknessConnection.Client.BeginReceive(this.thicknessRxBuffer, 0, this.thicknessRxBuffer.Length, SocketFlags.None, this.thicknessRxCallback, this);
            }
         }
         catch
         {
         }
      }

      private void StressAcceptCallback(IAsyncResult result)
      {
         try
         {
            if (false != this.stressActive)
            {
               this.stressConnection = this.stressListener.EndAcceptTcpClient(result);
               this.stressTimeLimit = DateTime.Now.AddSeconds(60);
               this.stressConnection.Client.BeginReceive(this.stressRxBuffer, 0, this.stressRxBuffer.Length, SocketFlags.None, this.stressRxCallback, this);
            }
         }
         catch { }
      }

      private void StressReceiveCallback(IAsyncResult result)
      {
         try
         {
            int byteCount = this.stressConnection.Client.EndReceive(result);

            if (0 == byteCount)
            {
               this.CloseStressConnection();

               if (null != this.stressListener)
               {
                  this.stressListener.BeginAcceptTcpClient(this.stressAcceptCallback, this);
               }
            }
            else
            {
               this.stressTimeLimit = DateTime.Now.AddSeconds(60);

               for (int i = 0; i < byteCount; i++)
               {
                  char ch = (char)this.stressRxBuffer[i];

                  if ((ch >= ' ') && (ch <= '~'))
                  {
                     this.stressCmdBuffer.Append(ch);
                  }
                  else if ('\r' == ch)
                  {
                     this.ProcessStressCommand(this.stressCmdBuffer.ToString());
                     this.stressCmdBuffer.Clear();
                  }
               }

               this.stressConnection.Client.BeginReceive(this.stressRxBuffer, 0, this.stressRxBuffer.Length, SocketFlags.None, this.stressRxCallback, this);
            }
         }
         catch
         {
         }
      }

      private void ClientConnectCallback(IAsyncResult result)
      {
         try
         {
            this.clientConnection.Client.EndConnect(result);

            if (false != this.clientConnection.Connected)
            {
               this.clientConnection.Client.BeginReceive(this.clientReceiveBuffer, 0, this.clientReceiveBuffer.Length, SocketFlags.None, this.clientReceiveCallback, this);
            }
         }
         catch
         {
         }

         this.clientConnectComplete = true;
      }

      private void ClientReceiveCallback(IAsyncResult result)
      {
         try
         {
            int byteCount = this.clientConnection.Client.EndReceive(result);

            if (0 == byteCount)
            {
               this.CloseClientConnection();
            }
            else
            {
               this.clientRsp = Encoding.UTF8.GetString(this.clientReceiveBuffer, 0, byteCount);
               this.clientConnection.Client.BeginReceive(this.clientReceiveBuffer, 0, this.clientReceiveBuffer.Length, SocketFlags.None, this.clientReceiveCallback, this);
            }
         }
         catch
         {
         }
      }

      #endregion

      #region Thickness Events

      private void ThicknessActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.thicknessActive)
         {
            IPAddress address = null;
            int port = 0;

            if ((IPAddress.TryParse(this.ThicknessAddressTextBox.Text, out address) != false) &&
                (int.TryParse(this.ThicknessPortTextBox.Text, out port) != false))
            {
               try
               {
                  this.thicknessListener = new TcpListener(address, port);
                  this.thicknessListener.Start(0);
                  this.thicknessListener.BeginAcceptTcpClient(this.thicknessAcceptCallback, this);

                  this.ThicknessAddressTextBox.Enabled = false;
                  this.ThicknessPortTextBox.Enabled = false;

                  this.thicknessNewCmd = false;
                  this.ThicknessCmdTextBox.Text = "";
                  this.ThicknessActivityButton.Text = "Stop";
                  this.thicknessActive = true;

                  this.StatusLabel.Text = "thickness sensor listening";
               }
               catch (Exception ex)
               {
                  this.StatusLabel.Text = "exception on thickness listen: " + ex.Message;
               }
            }
            else
            {
               this.StatusLabel.Text = "invalid entry";
            }
         }
         else
         {
            this.thicknessListener.Stop();
            this.thicknessListener = null;

            this.CloseThicknessConnection();

            this.ThicknessAddressTextBox.Enabled = true;
            this.ThicknessPortTextBox.Enabled = true;

            this.ThicknessActivityButton.Text = "Start";

            this.StatusLabel.Text = "thickness sensor stopped";
            this.thicknessActive = false;
         }
      }

      private void ThicknessDelayTextBox_TextChanged(object sender, EventArgs e)
      {
         double value = 0;

         if (double.TryParse(this.ThicknessDelayTextBox.Text, out value) != false)
         {
            this.thicknessRspDelay = value;
         }
      }

      private void ThicknessReadingTextBox_TextChanged(object sender, EventArgs e)
      {
         double value = 0;

         if (double.TryParse(this.ThicknessReadingTextBox.Text, out value) != false)
         {
            this.thicknessReading = value;
         }
      }

      #endregion

      #region Stress Events

      private void StressActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.stressActive)
         {
            IPAddress address = null;
            int port = 0;

            if ((IPAddress.TryParse(this.StressAddressTextBox.Text, out address) != false) &&
                (int.TryParse(this.StressPortTextBox.Text, out port) != false))
            {
               try
               {
                  this.stressListener = new TcpListener(address, port);
                  this.stressListener.Start(0);
                  this.stressListener.BeginAcceptTcpClient(this.stressAcceptCallback, this);

                  this.StressAddressTextBox.Enabled = false;
                  this.StressPortTextBox.Enabled = false;

                  this.stressNewCmd = false;
                  this.StressCmdTextBox.Text = "";
                  this.StressActivityButton.Text = "Stop";
                  this.stressActive = true;

                  this.StatusLabel.Text = "stress sensor listening";
               }
               catch (Exception ex)
               {
                  this.StatusLabel.Text = "exception on stress listen: " + ex.Message;
               }
            }
            else
            {
               this.StatusLabel.Text = "invalid entry";
            }
         }
         else
         {
            this.stressListener.Stop();
            this.stressListener = null;

            this.CloseStressConnection();

            this.StressAddressTextBox.Enabled = true;
            this.StressPortTextBox.Enabled = true;

            this.StressActivityButton.Text = "Start";

            this.StatusLabel.Text = "stress sensor stopped";
            this.stressActive = false;
         }
      }

      private void StressDelayTextBox_TextChanged(object sender, EventArgs e)
      {
         double value = 0;

         if (double.TryParse(this.StressDelayTextBox.Text, out value) != false)
         {
            this.stressRspDelay = value;
         }
      }

      private void StressReadingTextBox_TextChanged(object sender, EventArgs e)
      {
         double value = 0;

         if (double.TryParse(this.StressReadingTextBox.Text, out value) != false)
         {
            this.stressReading = value;
         }
      }

      #endregion

      #region Location Client Events

      private void ClientActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.clientConnected)
         {
            if (false == this.clientConnectPending)
            {
               IPAddress ipAddress = null;
               int port = 0;

               if ((IPAddress.TryParse(this.ClientAddressTextBox.Text, out ipAddress) != false) &&
                   (int.TryParse(this.ClientPortTextBox.Text, out port) != false))
               {
                  this.ClientAddressTextBox.Enabled = false;
                  this.ClientPortTextBox.Enabled = false;

                  this.ClientActivityButton.Text = "Pending";
                  this.clientConnectPending = true;
                  this.clientConnectComplete = false;

                  this.clientConnection = new TcpClient();
                  this.clientConnection.BeginConnect(ipAddress, port, this.clientConnectCallback, this);
                  this.StatusLabel.Text = "connecting...";
               }
               else
               {
                  this.StatusLabel.Text = "invalid parameters";
               }
            }
         }
         else
         {
            this.CloseClientConnection();
            this.StatusLabel.Text = "disconnected";
            this.clientConnected = false;
            this.ClientAddressTextBox.Enabled = true;
            this.ClientPortTextBox.Enabled = true;
            this.ClientActivityButton.Text = "Connect";
         }
      }

      private void ClientSendButton_Click(object sender, EventArgs e)
      {
         if (null != this.clientConnection)
         {
            string commandString = this.CmdTextBox.Text + "\r";
            byte[] commandBuffer = Encoding.UTF8.GetBytes(commandString);
            this.clientConnection.Client.Send(commandBuffer, commandBuffer.Length, SocketFlags.None);
         }
      }

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         if (null != this.thicknessConnection)
         {
            this.ThicknessStatusLabel.Text = "connected";

            if (DateTime.Now > this.thicknessTimeLimit)
            {
               this.CloseThicknessConnection();
               this.thicknessListener.BeginAcceptTcpClient(this.thicknessAcceptCallback, this);
            }
            else
            {
               if (false != this.thicknessNewCmd)
               {
                  this.thicknessNewCmd = false;
                  this.ThicknessCmdTextBox.Text = this.thicknessCmd;
               }

               if ((false != this.thicknessRspPending) &&
                   (DateTime.Now > this.thicknessRspTimeLimit))
               {
                  this.thicknessRspPending = false;
                  string rspString = this.thicknessCmd + "," + this.thicknessReading.ToString("N3") + "\r";
                  byte[] rspBuffer = Encoding.UTF8.GetBytes(rspString);

                  try
                  {
                     this.thicknessConnection.Client.Send(rspBuffer, rspBuffer.Length, SocketFlags.None);
                  }
                  catch
                  {
                     this.CloseThicknessConnection();
                  }
               }
            }
         }
         else if (null != this.thicknessListener)
         {
            this.ThicknessStatusLabel.Text = "listening";
         }
         else
         {
            this.ThicknessStatusLabel.Text = "off";
         }


         if (null != this.stressConnection)
         {
            this.StressStatusLabel.Text = "connected";

            if (DateTime.Now > this.stressTimeLimit)
            {
               this.CloseStressConnection();
               this.stressListener.BeginAcceptTcpClient(this.stressAcceptCallback, this);
            }
            else
            {
               if (false != this.stressNewCmd)
               {
                  this.stressNewCmd = false;
                  this.StressCmdTextBox.Text = this.stressCmd;
               }

               if ((false != this.stressRspPending) &&
                   (DateTime.Now > this.stressRspTimeLimit))
               {
                  this.stressRspPending = false;
                  string rspString = this.stressCmd + "," + this.stressReading.ToString("N3") + "\r";
                  byte[] rspBuffer = Encoding.UTF8.GetBytes(rspString);

                  try
                  {
                     this.stressConnection.Client.Send(rspBuffer, rspBuffer.Length, SocketFlags.None);
                  }
                  catch
                  {
                     this.CloseStressConnection();
                  }
               }
            }
         }
         else if (null != this.stressListener)
         {
            this.StressStatusLabel.Text = "listening";
         }
         else
         {
            this.StressStatusLabel.Text = "off";
         }

         #region Location Client

         if ((false != this.clientConnectPending) &&
             (false != this.clientConnectComplete))
         {
            this.clientConnectPending = false;
            this.clientConnectComplete = false;

            if (false != this.clientConnection.Client.Connected)
            {
               this.clientConnected = true;
               this.ClientActivityButton.Text = "Disconnect";
               this.StatusLabel.Text = "connected";
            }
            else
            {
               this.ClientAddressTextBox.Enabled = true;
               this.ClientPortTextBox.Enabled = true;
               this.ClientActivityButton.Text = "Connect";
               this.StatusLabel.Text = "unable to connected";
               this.CloseClientConnection();
            }
         }

         if ((false != this.clientConnected) &&
             (null == this.clientConnection))
         {
            this.clientConnected = false;
            this.ClientAddressTextBox.Enabled = true;
            this.ClientPortTextBox.Enabled = true;
            this.ClientActivityButton.Text = "Connect";
            this.StatusLabel.Text = "disconnected";
         }

         if (null != this.clientRsp)
         {
            this.RspTextBox.Text = this.clientRsp;
            this.clientRsp = null;
         }

         #endregion
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

         this.thicknessAcceptCallback = new AsyncCallback(this.ThicknessAcceptCallback);
         this.thicknessRxCallback = new AsyncCallback(this.ThicknessReceiveCallback);
         this.thicknessRxBuffer = new byte[1500];
         this.thicknessTxBuffer = new byte[1500];
         this.thicknessCmdBuffer = new StringBuilder();


         this.stressAcceptCallback = new AsyncCallback(this.StressAcceptCallback);
         this.stressRxCallback = new AsyncCallback(this.StressReceiveCallback);
         this.stressRxBuffer = new byte[1500];
         this.stressTxBuffer = new byte[1500];
         this.stressCmdBuffer = new StringBuilder();

         this.thicknessActive = false;
         this.ThicknessActivityButton.Text = "Start";

         this.stressActive = false;
         this.StressActivityButton.Text = "Start";

         this.StatusLabel.Text = "";
         
         this.clientReceiveBuffer = new byte[1524];
         this.clientConnectCallback = new AsyncCallback(this.ClientConnectCallback);
         this.clientReceiveCallback = new AsyncCallback(this.ClientReceiveCallback);
         this.ClientActivityButton.Text = "Connect";

         this.UpdateTimer.Enabled = true;
      }

      #endregion      

   }
}
