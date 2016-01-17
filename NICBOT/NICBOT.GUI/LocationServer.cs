namespace NICBOT.GUI
{
   using System;
   using System.Text;
   using System.Net;
   using System.Net.Sockets;
   using System.Threading;

   using NICBOT.Utilities;

   public class LocationServer
   {
      #region Definition

      public delegate bool LocationHandler(ref double latitude, ref double longitude, ref DateTime dateTime, ref Directions direction, ref double displacement, ref double radialLocation);
      public delegate void ThicknessReadingHandler(double thickness);
      public delegate void StressReadingHandler(double stress);

      private delegate void ProcessHandler();

      #endregion

      #region Fields

      private static LocationServer instance = null;

      private bool active;

      private bool execute;
      private Thread thread;

      private string hostAddress;
      private int hostPort;

      private string faultReason;
      
      private bool serverListening;
      private TcpListener listener;
      private TcpClient connection;
      private AsyncCallback acceptCallback;
      private AsyncCallback receiveCallback;
      private byte[] receiveBuffer;
      private StringBuilder commandBuffer;
      
      private DateTime faultTimeLimit;
      private DateTime connectionTimeLimit;
      
      #endregion

      #region Properties

      public static LocationServer Instance
      {
         get
         {
            if (null == instance)
            {
               instance = new LocationServer();
               instance.Initialize();
            }

            return instance;
         }
      }

      private ProcessHandler Process { set; get; }

      public LocationHandler OnLocationRequest { set; get; }
      public ThicknessReadingHandler OnThicknessReading { set; get; }
      public StressReadingHandler OnStressReading { set; get; }

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

      private bool GetLocationData(ref double latitude, ref double longitude, ref DateTime dateTime, ref Directions direction, ref double displacement, ref double radialLocation)
      {
         bool result = false;

         if (null != this.OnLocationRequest)
         {
            result = this.OnLocationRequest(ref latitude, ref longitude, ref dateTime, ref direction, ref displacement, ref radialLocation);
         }

         return (result);
      }

      private void ReportThickness(double thickness)
      {
         if (null != this.OnThicknessReading)
         {
            this.OnThicknessReading(thickness);
         }
      }

      private void ReportStress(double stress)
      {
         if (null != this.OnStressReading)
         {
            this.OnStressReading(stress);
         }
      }

      private void StopListener()
      {
         if (null != this.listener)
         {
            this.listener.Stop();
            this.listener = null;
         }
      }

      private void CloseConnection()
      {
         if (null != this.connection)
         {
            this.connection.Close();
            this.connection = null;
         }
      }
      
      private void ProcessCommand(string command)
      {
         if (null != command)
         {
            string[] paramaters = command.Split(new char[] { ',' });

            if (paramaters.Length > 0)
            {
               if ("LOCATION?" == paramaters[0])
               {
                  /*
                     REQUEST  LOCATION? <CR>
                     REPLY    LOCATION , LAT	,	LON	,	DAT	,	DIR	,	DIS	,	RAD	,	TIM		

                     LAT=±##.####
                     LON=±###.###
                     DAT=####-##-##, i.e. YYYY-MM-DD
                     DIR=#, i.e. N=NORTH, S=SOUTH, E=EAST, W=WEST
                     DIS=#####, i.e. 123 cm
                     RAD=###.##, i.e. 0.00 to 359.99
                     TIM=##:##:##, i.e. HH:MM:SS
            
                     CMD1,+10.1,+10.1,2015-08-25,N,123,180.00,12:00:01 <CR>
              
                     See http://boulter.com/gps/distance/?from=40.8616666+-73.0000&to=40.8616666+-73.0004&units=m             
                     Round coordinates to 4 places places data within 50 feet.             
                  */
                  
                  double latitude = 0;
                  double longitude = 0;
                  DateTime dateTime = default(DateTime);
                  Directions direction = Directions.unknown;
                  double displacement = 0;
                  double radialLocation = 0;
                  
                  bool validData = this.GetLocationData(ref latitude, ref longitude, ref dateTime, ref direction, ref displacement, ref radialLocation);
                  string replyString = "LOCATION,";

                  if (double.IsNaN(latitude) == false) 
                  {
                     char replyLatitudeSign = (latitude > 0) ? '+' : '-';
                     double replyLatitude = Math.Abs(latitude);

                     replyString += string.Format("{0}{1:0.0000},", replyLatitudeSign, replyLatitude);
                  }
                  else
                  {
                     replyString += ",";
                  }

                  if (double.IsNaN(longitude) == false) 
                  {
                     char replyLongitudeSign = (longitude > 0) ? '+' : '-';
                     double replyLongitude = Math.Abs(longitude);

                     replyString += string.Format("{0}{1:0.0000},", replyLongitudeSign, replyLongitude);
                  }
                  else
                  {
                     replyString += ",";
                  }

                  replyString += string.Format("{0:D4}-{1:D2}-{2:D2},", dateTime.Year, dateTime.Month, dateTime.Day);

                  if (Directions.unknown != direction)
                  {
                     string directionString = this.GetDirectionString(direction);
                     replyString += directionString + ",";
                  }
                  else
                  {
                     replyString += ",";
                  }

                  replyString += string.Format("{0},{1},{2:D2}:{3:D2}:{4:D2}\r", displacement, radialLocation, dateTime.Hour, dateTime.Minute, dateTime.Second);
 
                  try
                  {
                     byte[] replyBuffer = Encoding.UTF8.GetBytes(replyString);
                     this.connection.Client.Send(replyBuffer, replyBuffer.Length, SocketFlags.None);
                  }
                  catch
                  {
                  }

                  this.connectionTimeLimit = DateTime.Now.AddSeconds(60);
               }
               else if ("THICKNESS" == paramaters[0])
               {
                  if (paramaters.Length > 1)
                  {
                     double thickness = 0;

                     if (double.TryParse(paramaters[1], out thickness) != false) 
                     {
                        this.ReportThickness(thickness);
                     }
                  }
               }
               else if ("STRESS" == paramaters[0])
               {
                  if (paramaters.Length > 1)
                  {
                     double stress = 0;

                     if (double.TryParse(paramaters[1], out stress) != false) 
                     {
                        this.ReportStress(stress);
                     }
                  }
               }
            }
         }
      }

      #endregion

      #region Delegates

      private void AcceptCallback(IAsyncResult result)
      {
         try
         {
            if (false != this.active)
            {
               this.connection = this.listener.EndAcceptTcpClient(result);
               this.serverListening = false;
               this.connectionTimeLimit = DateTime.Now.AddSeconds(60);
               this.commandBuffer.Clear();
               this.connection.Client.BeginReceive(this.receiveBuffer, 0, this.receiveBuffer.Length, SocketFlags.None, this.receiveCallback, this);
            }
         }
         catch 
         { 
         }
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
               this.connectionTimeLimit = DateTime.Now.AddSeconds(60);

               for (int i = 0; i < byteCount; i++)
               {
                  char ch = (char)this.receiveBuffer[i];

                  if ((ch >= ' ') && (ch <= '~'))
                  {
                     this.commandBuffer.Append(ch);
                  }
                  else if ('\r' == ch)
                  {
                     this.ProcessCommand(this.commandBuffer.ToString());
                     this.commandBuffer.Clear();
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

      #region Constructor

      private LocationServer()
      {
         this.acceptCallback = new AsyncCallback(this.AcceptCallback);
         this.receiveCallback = new AsyncCallback(this.ReceiveCallback);
         this.receiveBuffer = new byte[1500];
         this.commandBuffer = new StringBuilder();
      }

      #endregion

      #region Process Functions

      private void ProcessStart()
      {
         this.faultReason = null;

         try
         {
            IPAddress address = IPAddress.Parse(this.hostAddress);
            this.listener = new TcpListener(address, this.hostPort);
            this.listener.Start(0);
            this.listener.BeginAcceptTcpClient(this.acceptCallback, this);
            this.serverListening = true;
         }
         catch
         {
         }

         if ((false != this.serverListening) &&
             (null == this.connection))
         {
            this.faultReason = null;
            this.Process = this.ProcessListening;
         }
         else
         {
            this.StopListener();
            this.faultReason = "offline";
            this.faultTimeLimit = DateTime.Now.AddSeconds(60);
            this.Process = this.ProcessFaulted;
         }
      }

      private void ProcessListening()
      {
         if (false == this.serverListening)
         {
            this.listener.BeginAcceptTcpClient(this.acceptCallback, this);
            this.serverListening = true;
         }

         if (null != this.connection)
         {
            this.Process = this.ProcessConnected;
         }
      }

      private void ProcessConnected()
      {
         bool connected = true;

         if (null == this.connection)
         {
            connected = false;
         }
         else if (DateTime.Now > this.connectionTimeLimit)
         {
            this.CloseConnection();
            connected = false;
         }

         if (false == connected)
         {
            this.listener.BeginAcceptTcpClient(this.acceptCallback, this);
            this.Process = this.ProcessListening;
         }
      }

      private void ProcessFaulted()
      {
         if (DateTime.Now > this.faultTimeLimit)
         {
            this.Process = this.ProcessStart;
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
         this.StopListener();
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
         if (false == this.active)
         {
            this.hostAddress = address;
            this.hostPort = port;

            this.thread = new Thread(this.ThreadProcess);
            this.thread.IsBackground = true;
            this.thread.Name = "Location Server";

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

      #endregion 
   }
}
