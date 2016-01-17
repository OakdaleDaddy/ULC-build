namespace NICBOT.GUI
{
   using System;
   using System.Text;
   using System.Net;
   using System.Net.Sockets;
   using System.Threading;

   using NICBOT.Utilities;

   public class ThicknessSensor
   {
      #region Definition

      private delegate void ProcessHandler();

      #endregion

      #region Fields

      private static ThicknessSensor instance = null;

      private bool active;

      private bool execute;
      private Thread thread;

      private string hostAddress;
      private int hostPort;

      private AsyncCallback connectionCallback;
      private AsyncCallback receiveCallback;
      private TcpClient connection;
      private byte[] receiveBuffer;
      private StringBuilder responseBuffer;

      private string faultReason;
      private double reading;

      private bool connected;
      private bool connectFailure;
      private bool trigger;
      private bool requesting;
      private bool readingReady;
      
      private DateTime connectionTestTimeLimit;      
      private DateTime connectionResponseTimeLimit;

      private double triggerLatitude;
      private double triggerLongitude;
      private DateTime triggerDateTime;
      private string triggerDirection;
      private double triggerDisplacement;
      private double triggerRadialLocation;
          
      #endregion

      #region Properties

      public static ThicknessSensor Instance
      {
         get
         {
            if (null == instance)
            {
               instance = new ThicknessSensor();
               instance.Initialize();
            }

            return instance;
         }
      }

      private ProcessHandler Process { set; get; }

      public string FaultReason { get { return (this.faultReason); } }

      #endregion

      #region Helper Functions

      private string GetDirectionString(Directions direction)
      {
         string result = "";

         if (Directions.north == direction)
         {
            result = "N";
         }
         else if (Directions.east == direction)
         {
            result = "E";
         }
         else if (Directions.south == direction)
         {
            result = "S";
         }
         else if (Directions.west == direction)
         {
            result = "W";
         }

         return (result);
      }

      private void CloseConnection()
      {
         if (null != this.connection)
         {
            this.connection.Close();
            this.connection = null;
         }
      }

      private void CompleteReading(double reading)
      {
         this.requesting = false;
         this.reading = reading;
         this.readingReady = true;

         this.CloseConnection();
      }

      private void ProcessResponse(string response)
      {
         double result = double.NaN;

         if (null != response)
         {
            string[] paramaters = response.Split(new char[] { ',' });

            if (10 == paramaters.Length)
            {
               double.TryParse(paramaters[9], out result);
            }
         }

         this.faultReason = null;
         this.CompleteReading(result);
      }

      #endregion

      #region Delegates

      private void ConnectCallback(IAsyncResult result)
      {
         try
         {
            this.connection.Client.EndConnect(result);

            if (false != this.connection.Connected)
            {
               this.connection.Client.BeginReceive(this.receiveBuffer, 0, this.receiveBuffer.Length, SocketFlags.None, this.receiveCallback, this);
               this.connected = true;
            }
         }
         catch
         {
         }

         this.connectFailure = !this.connected;
      }

      private void ReceiveCallback(IAsyncResult result)
      {
         try
         {
            int byteCount = this.connection.Client.EndReceive(result);

            if (0 == byteCount)
            {
               this.CloseConnection();
            }
            else
            {
               for (int i = 0; i < byteCount; i++)
               {
                  char ch = (char)this.receiveBuffer[i];

                  if ((ch >= ' ') && (ch <= '~') && (this.responseBuffer.Length < 256))
                  {
                     this.responseBuffer.Append(ch);
                  }
                  else if ('\r' == ch)
                  {
                     this.ProcessResponse(this.responseBuffer.ToString());
                     this.responseBuffer.Clear();
                  }
               }

               this.connection.Client.BeginReceive(this.receiveBuffer, 0, this.receiveBuffer.Length, SocketFlags.None, this.receiveCallback, this);
            }
         }
         catch
         {
         }
      }

      #endregion

      #region Process Functions

      private void ProcessStart()
      {
         this.faultReason = null;
         this.requesting = false;
         this.readingReady = false;

         this.connectionTestTimeLimit = DateTime.Now;
         this.Process = this.ProcessIdle;
      }

      private void ProcessIdle()
      {
         if (DateTime.Now > this.connectionTestTimeLimit)
         {
            this.Process = this.ProcessTest;
         }
         else if (false != this.trigger)
         {
            this.requesting = true;
            this.trigger = false;
            this.Process = this.ProcessRead;
         }
      }

      private void ProcessTest()
      {
         try
         {
            this.connected = false;
            this.connectFailure = false;

            this.connection = new TcpClient();
            this.connection.BeginConnect(this.hostAddress, this.hostPort, this.connectionCallback, this);

            this.Process = this.ProcessWaitTestConnection;
         }
         catch
         {
            this.faultReason = "offline";
            this.connectionTestTimeLimit = DateTime.Now.AddSeconds(5);

            this.Process = this.ProcessIdle;
         }
      }

      private void ProcessWaitTestConnection()
      {
         bool testComplete = false;

         if (false != this.connected)
         {
            this.faultReason = null;
            testComplete = true;
         }
         else if (false != this.connectFailure)
         {
            this.faultReason = "offline";
            testComplete = true;
         }

         if (false != testComplete)
         {
            this.CloseConnection();
            this.connectionTestTimeLimit = DateTime.Now.AddSeconds(5);

            this.Process = this.ProcessIdle;
         }
      }

      private void ProcessRead()
      {
         try
         {
            this.connected = false;
            this.connectFailure = false;
            this.responseBuffer.Clear();

            this.connection = new TcpClient();
            this.connection.BeginConnect(this.hostAddress, this.hostPort, this.connectionCallback, this);

            this.Process = this.ProcessWaitReadConnection;
         }
         catch
         {
            this.CompleteReading(double.NaN);
            this.faultReason = "offline";

            this.Process = this.ProcessIdle;
         }
      }

      private void ProcessWaitReadConnection()
      {
         bool connectionComplete = false;

         if (false != this.connected)
         {
            try
            {
               /*
                  REQUEST	CMD1	,	LAT	,	LON	,	DAT	,	DIR	,	DIS	,	CHN	,	RAD	,	TIM		
                  REPLY		CMD1	,	LAT	,	LON	,	DAT	,	DIR	,	DIS	,	CHN	,	RAD	,	TIM	,	FOM

                  LAT=±##.########
                  LON=±###.########
                  DAT=####-##-##, i.e. YYYY-MM-DD
                  DIR=#, i.e. N=NORTH, S=SOUTH, E=EAST, W=WEST
                  DIS=#####, i.e. 123 cm
                  CHN=#, 1 or 2
                  RAD=###.##, i.e. 0.00 to 359.99
                  TIM=##:##:##, i.e. HH:MM:SS
                  FOM=##.###, i.e. 1.234 mm, 1.234 psi
            
                  CMD1,+10.1,+10.1,2015-08-25,N,123,1,180.00,12:00:01 <CR>
              
                  See http://boulter.com/gps/distance/?from=40.8616666+-73.0000&to=40.8616666+-73.0004&units=m             
                  Round coordinates to 4 places places data within 50 feet.             
                */

               char requestLatitudeSign = (this.triggerLatitude > 0) ? '+' : '-';
               double requestLatitude = Math.Abs(this.triggerLatitude);
               char requestLongitudeSign = (this.triggerLongitude > 0) ? '+' : '-';
               double requestLongitude = Math.Abs(this.triggerLongitude);

               string commandString = string.Format("CMD1,{0}{1:0.0000},{2}{3:0.0000},{4:D4}-{5:D2}-{6:D2},{7},{8},{9},{10},{11:D2}:{12:D2}:{13:D2}\r",
                                                    requestLatitudeSign,
                                                    requestLatitude,
                                                    requestLongitudeSign,
                                                    requestLongitude,
                                                    this.triggerDateTime.Year,
                                                    this.triggerDateTime.Month,
                                                    this.triggerDateTime.Day,
                                                    this.triggerDirection,
                                                    this.triggerDisplacement,
                                                    0,
                                                    this.triggerRadialLocation,
                                                    this.triggerDateTime.Hour,
                                                    this.triggerDateTime.Minute,
                                                    this.triggerDateTime.Second);

               byte[] commandBuffer = Encoding.UTF8.GetBytes(commandString);
               this.connection.Client.Send(commandBuffer, commandBuffer.Length, SocketFlags.None);

               this.connectionResponseTimeLimit = DateTime.Now.AddSeconds(15);

               this.Process = this.ProcessWaitReading;
            }
            catch
            {
               connectionComplete = true;
            }
         }
         else if (false != this.connectFailure)
         {
            connectionComplete = true;
         }

         if (false != connectionComplete)
         {
            this.faultReason = "offline";
            this.CompleteReading(double.NaN);

            this.Process = this.ProcessIdle;
         }
      }

      private void ProcessWaitReading()
      {
         if (null != this.connection)
         {
            if (DateTime.Now > this.connectionResponseTimeLimit)
            {
               this.faultReason = "timeout";
               this.CompleteReading(double.NaN);

               this.Process = this.ProcessIdle;
            }
         }
         else
         {
            this.Process = this.ProcessIdle;
         }
      }

      private void ThreadProcess()
      {
         this.Process = this.ProcessStart;

         for (; execute; )
         {
            this.Process();
            Thread.Sleep(50);
         }

         this.CloseConnection();
      }

      #endregion

      #region Constructor

      private ThicknessSensor()
      {
         this.connectionCallback = new AsyncCallback(this.ConnectCallback);
         this.receiveCallback = new AsyncCallback(this.ReceiveCallback);
         this.receiveBuffer = new byte[1500];
         this.responseBuffer = new StringBuilder();
      }

      #endregion

      #region Access Methods

      public void Initialize()
      {
         this.active = false;
         this.faultReason = null;
      }

      public void Start(string address, int port)
      {
         if ((false == this.active) &&
             (0 != port))
         {
            this.hostAddress = address;
            this.hostPort = port;

            this.thread = new Thread(this.ThreadProcess);
            this.thread.IsBackground = true;
            this.thread.Name = "Thickness Sensor";

            this.execute = true;
            this.thread.Start();

            this.active = true;
         }
      }

      public void Stop()
      {
         if (false != this.active)
         {
            this.execute = false;
            this.thread.Join(3000);

            this.thread = null;

            this.active = false;
            this.faultReason = "stopped";
         }
      }

      public void TriggerReading(double latitude, double longitude, DateTime dateTime, Directions direction, double displacement, double radialLocation)
      {
         if (this.Pending() == false)
         {
            this.triggerLatitude = latitude;
            this.triggerLongitude = longitude;
            this.triggerDateTime = dateTime;
            this.triggerDirection = this.GetDirectionString(direction);
            this.triggerDisplacement = displacement;
            this.triggerRadialLocation = radialLocation;

            this.readingReady = false;
            this.trigger = true;
         }
      }

      public bool Pending()
      {
         return (this.requesting || this.trigger);
      }

      public double GetReading()
      {
         double result = double.NaN;

         if ((null == this.faultReason) &&
             (false != this.readingReady))
         {
            result = this.reading;
         }

         return (result);
      }

      #endregion
   }
}