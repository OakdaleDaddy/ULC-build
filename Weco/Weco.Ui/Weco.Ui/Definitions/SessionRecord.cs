
namespace Weco.Ui
{
   using System;
   using System.Collections.Generic;
   using System.IO;

   using Weco.Utilities;

   public class SessionRecord
   {
      #region Fields

      private static SessionRecord instance = null;

      private DateTime startTime;
      private List<SessionData> dataList;

      #endregion

      #region Helper Functions

      private void Add(SessionData data)
      {
         this.dataList.Add(data);
      }

      #endregion

      #region Properties

      public static SessionRecord Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new SessionRecord();
               instance.Initialize();
            }

            return instance;
         }
      }

      public string StoragePath { set; get; }

      #endregion

      #region Constructor

      private void Initialize()
      {
         this.dataList = new List<SessionData>();
      }

      private SessionRecord()
      {
      }

      #endregion

      #region Access Methods

      public void Reset()
      {
         this.dataList.Clear();
      }

      public void Start(double laserFront, double laserRear, double targetFront, double targetRear, double latitude, double longitude)
      {
         this.startTime = DateTime.Now;
         this.dataList.Clear();
         this.Add(new SessionStartData(laserFront, laserRear, targetFront, targetRear, latitude, longitude));
         Tracer.WriteHigh(TraceGroup.SESSION, null, "session start {0:0} {1:0} {2:0} {3:0} {4:0.000000} {5:0.000000}", laserFront, laserRear, targetFront, targetRear, latitude, longitude);
      }

      public void Complete(double laserFront, double laserRear, double targetFront, double targetRear)
      {
         this.Add(new SessionCompleteData(laserFront, laserRear, targetFront, targetRear));
         Tracer.WriteHigh(TraceGroup.SESSION, null, "session complete {0:0} {1:0} {2:0} {3:0}", laserFront, laserRear, targetFront, targetRear);

         try
         {
            if (Directory.Exists(this.StoragePath) == false)
            {
               Directory.CreateDirectory(this.StoragePath);
            }

            DateTime dt = this.startTime;
            string filePath = string.Format(@"{0}\session_{1:D4}{2:D2}{3:D2}_{4:D2}{5:D2}{6:D2}.csv", this.StoragePath, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

            Tracer.WriteHigh(TraceGroup.SESSION, null, "storing data to {0}", filePath);
            StreamWriter file = new System.IO.StreamWriter(filePath);

            if (null != file)
            {
               for (int i = 0; i < this.dataList.Count; i++)
               {
                  string csvString = this.dataList[i].GetCsvData();
                  file.WriteLine(csvString);
               }

               file.Close();
            }
         }
         catch { }
      }

      public void RecordMovement(double laserFront, double laserRear, double targetFront, double targetRear)
      {
         this.Add(new SessionMovementData(laserFront, laserRear, targetFront, targetRear));
         Tracer.WriteHigh(TraceGroup.SESSION, null, "session movement {0:0} {1:0} {2:0} {3:0}", laserFront, laserRear, targetFront, targetRear);
      }

      public void RecordMeasurement(double distance, SessionMeasurementData.Types type)
      {
         this.Add(new SessionMeasurementData(distance, type));
         Tracer.WriteHigh(TraceGroup.SESSION, null, "session measurement {0:0.00} {1}", distance, type.ToString());
      }

      public int GetRecordCount()
      {
         int result = this.dataList.Count;
         return (result);
      }

      public string GetTimeString(int index)
      {
         string result = "";

         if (index < this.dataList.Count)
         {
            DateTime dt = this.dataList[index].DataTime;
            result = string.Format("{0:D2}:{1:D2}:{2:D2}", dt.Hour, dt.Minute, dt.Second);
         }

         return (result);
      }

      public string GetDescriptionString(int index)
      {
         string result = "";

         if (index < this.dataList.Count)
         {
            result = this.dataList[index].GetDescription();
         }

         return (result);
      }

      #endregion

   }

}
