
namespace NICBOT.SensorClient
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
      private const string RegistryApplicationName = "NICBOT Sensor Client";

      #endregion

      #region Fields

      private bool clientConnected;
      private bool clientConnectPending;
      private bool clientConnectComplete;
      private TcpClient clientConnection;
      private byte[] clientReceiveBuffer;
      private string clientRsp;
      private AsyncCallback clientConnectCallback;
      private AsyncCallback clientReceiveCallback;

      private bool locationServerActive;
      private bool locationServerListening;
      private TcpListener locationServerListener;
      private TcpClient locationServerConnection;
      private AsyncCallback locationServerAcceptCallback;
      private AsyncCallback locationServerReceiveCallback;
      private byte[] locationServerReceiveBuffer;
      private byte[] locationServerTransmitBuffer;
      private StringBuilder locationServerCmdBuffer;
      private DateTime locationServerTimeLimit;
      private bool locationServerNewConnection;
      private bool locationServerNewCmd;
      private string locationServerCmd;
      
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

         keyValue = appKey.GetValue("ClientAddress");
         this.ClientAddressTextBox.Text = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";

         keyValue = appKey.GetValue("ClientPort");
         this.ClientPortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "5000";

         keyValue = appKey.GetValue("LocationServerAddress");
         this.LocationServerIpTextBox.Text = (null != keyValue) ? keyValue.ToString() : "127.0.0.1";

         keyValue = appKey.GetValue("LocationServerPort");
         this.LocationServerPortTextBox.Text = (null != keyValue) ? keyValue.ToString() : "5000";

         keyValue = appKey.GetValue("LocationServerResponse");
         this.LocationServerRspTextBox.Text = (null != keyValue) ? keyValue.ToString() : "CMD1,+10.1,+10.1,2015-08-25,N,123,1,180.00,12:00:01";
                          
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

         appKey.SetValue("ClientAddress", this.ClientAddressTextBox.Text);
         appKey.SetValue("ClientPort", this.ClientPortTextBox.Text);

         appKey.SetValue("LocationServerAddress", this.LocationServerIpTextBox.Text);
         appKey.SetValue("LocationServerPort", this.LocationServerPortTextBox.Text);
         appKey.SetValue("LocationServerResponse", this.LocationServerRspTextBox.Text);

         #endregion
      }

      #endregion

      #region Helper Functions

      private void CloseClientConnection()
      {
         if (null != this.clientConnection)
         {
            this.clientConnection.Close();
            this.clientConnection = null;
         }
      }

      private void CloseLocationServerConnection()
      {
         if (null != this.locationServerConnection)
         {
            this.locationServerConnection.Close();
            this.locationServerConnection = null;
         }
      }

      private void ProcessLocationServerCommand(string cmd)
      {
         this.locationServerCmd = cmd;
         this.locationServerNewCmd = true;
      }

      #endregion

      #region Delegates

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

      private void LocationServerAcceptCallback(IAsyncResult result)
      {
         try
         {
            if (false != this.locationServerActive)
            {
               this.locationServerConnection = this.locationServerListener.EndAcceptTcpClient(result);
               this.locationServerListening = false;
               this.locationServerNewConnection = true;
               this.locationServerTimeLimit = DateTime.Now.AddSeconds(60);
               this.locationServerConnection.Client.BeginReceive(this.locationServerReceiveBuffer, 0, this.locationServerReceiveBuffer.Length, SocketFlags.None, this.locationServerReceiveCallback, this);
            }
         }
         catch { }
      }

      private void LocationServerReceiveCallback(IAsyncResult result)
      {
         try
         {
            int byteCount = this.locationServerConnection.Client.EndReceive(result);

            if (0 == byteCount)
            {
               this.CloseLocationServerConnection();

               if (null != this.locationServerListener)
               {
                  this.locationServerListener.BeginAcceptTcpClient(this.locationServerAcceptCallback, this);
               }
            }
            else
            {
               this.locationServerTimeLimit = DateTime.Now.AddSeconds(60);

               for (int i = 0; i < byteCount; i++)
               {
                  char ch = (char)this.locationServerReceiveBuffer[i];

                  if ((ch >= ' ') && (ch <= '~'))
                  {
                     this.locationServerCmdBuffer.Append(ch);
                  }
                  else if ('\r' == ch)
                  {
                     this.ProcessLocationServerCommand(this.locationServerCmdBuffer.ToString());
                     this.locationServerCmdBuffer.Clear();
                  }
               }

               this.locationServerConnection.Client.BeginReceive(this.locationServerReceiveBuffer, 0, this.locationServerReceiveBuffer.Length, SocketFlags.None, this.locationServerReceiveCallback, this);
            }
         }
         catch
         {
         }
      }

      #endregion

      #region User Events

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

      #region Location Server Events

      private void ServerActivityButton_Click(object sender, EventArgs e)
      {
         if (false == this.locationServerActive)
         {
            IPAddress address = null;
            int port = 0;

            if ((IPAddress.TryParse(this.LocationServerIpTextBox.Text, out address) != false) &&
                (int.TryParse(this.LocationServerPortTextBox.Text, out port) != false))
            {
               try
               {
                  this.locationServerListener = new TcpListener(address, port);
                  this.locationServerListener.Start(0);

                  this.LocationServerIpTextBox.Enabled = false;
                  this.LocationServerPortTextBox.Enabled = false;

                  this.locationServerNewConnection = false;
                  this.locationServerNewCmd = false;
                  this.LocationServerCmdTextBox.Text = "";

                  this.LocationServerActivityButton.Text = "Stop";
                  this.locationServerActive = true;
                  this.locationServerListening = false;
               }
               catch (Exception ex)
               {
                  this.StatusLabel.Text = "exception on location server listen: " + ex.Message;
               }
            }
            else
            {
               this.StatusLabel.Text = "invalid entry";
            }
         }
         else
         {
            this.locationServerListener.Stop();
            this.locationServerListener = null;

            this.CloseLocationServerConnection();

            this.LocationServerIpTextBox.Enabled = true;
            this.LocationServerPortTextBox.Enabled = true;

            this.LocationServerActivityButton.Text = "Start";

            this.StatusLabel.Text = "location server stopped";
            this.locationServerActive = false;
         }
      }

      #endregion

      #region Form Events

      private void MainForm_Shown(object sender, EventArgs e)
      {
         this.LoadRegistry();
      }

      private void TickTimer_Tick(object sender, EventArgs e)
      {
         DateTime now = DateTime.Now;
         string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}.{3}", now.Hour, now.Minute, now.Second, (now.Millisecond / 100));
         this.TimeLabel.Text = timeText;

         if ((false != this.clientConnectPending) &&
             (false != this.clientConnectComplete))
         {
            this.clientConnectPending = false;
            this.clientConnectComplete = false;

            if (false != this.clientConnection.Client.Connected)
            {
               this.clientConnected = true;
               this.ClientActivityButton.Text = "Disconnect";
               this.StatusLabel.Text = "client connected";
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
            this.StatusLabel.Text = "client disconnected";
         }

         if (null != this.clientRsp)
         {
            this.RspTextBox.Text = this.clientRsp;
            this.clientRsp = null;
         }

         if (null != this.locationServerConnection)
         {
            if (false != this.locationServerNewConnection)
            {
               this.locationServerNewConnection = false;
               this.StatusLabel.Text = "location client connected";
            }

            if (DateTime.Now > this.locationServerTimeLimit)
            {
               this.CloseLocationServerConnection();
               this.StatusLabel.Text = "location client disconnected";
               this.locationServerListener.BeginAcceptTcpClient(this.locationServerAcceptCallback, this);
            }
            else
            {
               if (false != this.locationServerNewCmd)
               {
                  this.locationServerNewCmd = false;
                  this.LocationServerCmdTextBox.Text = this.locationServerCmd;

                  if ("LOCATION?" == this.locationServerCmd)
                  {
                     string rspString = this.LocationServerRspTextBox.Text + "\r";
                     byte[] rspBuffer = Encoding.UTF8.GetBytes(rspString);

                     try
                     {
                        this.locationServerConnection.Client.Send(rspBuffer, rspBuffer.Length, SocketFlags.None);
                        this.StatusLabel.Text = "location provided";
                     }
                     catch
                     {
                        this.CloseLocationServerConnection();
                        this.StatusLabel.Text = "location client disconnected";
                     }
                  }
               }
            }
         }

         if ((null == this.locationServerConnection) &&
             (null != this.locationServerListener) &&
             (false == this.locationServerListening))
         {
            try
            {
               this.locationServerListener.BeginAcceptTcpClient(this.locationServerAcceptCallback, this);
               this.locationServerListening = true;
               this.StatusLabel.Text = "location server listening";
            }
            catch { }
         }
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

         this.clientReceiveBuffer = new byte[1524];
         this.clientConnectCallback = new AsyncCallback(this.ClientConnectCallback);
         this.clientReceiveCallback = new AsyncCallback(this.ClientReceiveCallback);
         this.ClientActivityButton.Text = "Connect";

         this.locationServerAcceptCallback = new AsyncCallback(this.LocationServerAcceptCallback);
         this.locationServerReceiveCallback = new AsyncCallback(this.LocationServerReceiveCallback);
         this.locationServerReceiveBuffer = new byte[1500];
         this.locationServerTransmitBuffer = new byte[1500];
         this.locationServerCmdBuffer = new StringBuilder();
         this.LocationServerActivityButton.Text = "Start";
         
         this.StatusLabel.Text = "";
         this.TickTimer.Enabled = true;
      }

      #endregion

   }
}
