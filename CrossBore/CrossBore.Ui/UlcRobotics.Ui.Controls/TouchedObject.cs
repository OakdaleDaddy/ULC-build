
namespace UlcRobotics.Ui.Controls
{
   using System;

   public class TouchedObject
   {
      #region Fields

      private static TouchedObject instance = null;

      private object touchedObject;
      private bool touchDownProcessed;

      #endregion

      #region Helper Functions

      private void Initialize()
      {
         this.touchedObject = null;
         this.touchDownProcessed = false;
      }

      #endregion

      #region Properties

      public static TouchedObject Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new TouchedObject();
               instance.Initialize();
            }

            return instance;
         }
      }
      
      #endregion

      #region Constructor

      private TouchedObject()
      {
      }

      #endregion

      #region Access Methods

      public void TouchDown(object touchedObject)
      {
         this.touchedObject = touchedObject;
         this.touchDownProcessed = false;
      }

      public bool ButtonDownExpected(object touchedObject)
      {
         bool result = false;

         if (null == this.touchedObject)
         {
            result = true;
         }
         else if (touchedObject == this.touchedObject)
         {
            if (false == this.touchDownProcessed)
            {
               this.touchDownProcessed = true;
               result = true;
            }
         }

         return (result);
      }

      public bool ButtonUpExpected(object touchedObject)
      {
         bool result = false;

         if (null == this.touchedObject)
         {
            result = true;
         }
         else if (touchedObject == this.touchedObject)
         {
            result = true;
         }

         this.touchedObject = null;

         return (result);
      }

      #endregion
   }
}