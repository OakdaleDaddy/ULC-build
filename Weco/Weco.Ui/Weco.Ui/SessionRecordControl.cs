
namespace Weco.Ui
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Data;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public partial class SessionRecordControl : UserControl
   {
      #region Definition

      private const int LineCount = 9;

      #endregion

      #region Fields

      private int topIndex;
      private bool topIndexSet;
      private int recordCount;

      private Label[] timeLabels;
      private Label[] descriptionLabels;

      #endregion

      #region Helper Functions

      private void DisplayRecordData(int displayRecordCount)
      {
         int displayTopIndex = 0;
         int displayLineCount = displayRecordCount;

         if (this.topIndex < 0)
         {
            this.topIndex = 0;
         }

         if (displayRecordCount > LineCount)
         {
            displayLineCount = LineCount;

            if (false == this.topIndexSet)
            {
               displayTopIndex = displayRecordCount - LineCount;
            }
            else
            {
               displayTopIndex = this.topIndex;
            }
         }

         this.UpArrowLabel.Visible = (0 != displayTopIndex) ? true : false;
         this.DownArrowLabel.Visible = ((displayRecordCount > LineCount) && ((displayTopIndex + LineCount) != displayRecordCount)) ? true : false;

         int displayIndexLimit = displayTopIndex + displayLineCount;
         for (int i = 0; i < LineCount; i++)
         {
            if (displayTopIndex == displayIndexLimit)
            {
               break;
            }

            this.timeLabels[i].Text = SessionRecord.Instance.GetTimeString(displayTopIndex);
            this.descriptionLabels[i].Text = SessionRecord.Instance.GetDescriptionString(displayTopIndex);

            displayTopIndex++;
         }
      }

      #endregion

      #region User Events

      private void UpButton_Click(object sender, EventArgs e)
      {
         if (false == this.topIndexSet)
         {
            if (this.recordCount > LineCount)
            {
               this.topIndex = this.recordCount - LineCount - 1;
               this.topIndexSet = true;
               this.DisplayRecordData(this.recordCount);
            }
         }
         else
         {
            if (this.topIndex > 0)
            {
               this.topIndex--;
               this.DisplayRecordData(this.recordCount);
            }
         }
      }

      private void DownButton_Click(object sender, EventArgs e)
      {
         if (false != this.topIndexSet)
         {
            int topIndexLimit = this.recordCount - LineCount;

            this.topIndex++;
            this.DisplayRecordData(this.recordCount);

            if (this.topIndex == topIndexLimit)
            {
               this.topIndexSet = false;
            }
         }
      }

      #endregion

      #region Constructor

      public SessionRecordControl()
      {
         this.InitializeComponent();

         this.timeLabels = new Label[LineCount];
         this.timeLabels[0] = this.RecordTime01Label;
         this.timeLabels[1] = this.RecordTime02Label;
         this.timeLabels[2] = this.RecordTime03Label;
         this.timeLabels[3] = this.RecordTime04Label;
         this.timeLabels[4] = this.RecordTime05Label;
         this.timeLabels[5] = this.RecordTime06Label;
         this.timeLabels[6] = this.RecordTime07Label;
         this.timeLabels[7] = this.RecordTime08Label;
         this.timeLabels[8] = this.RecordTime09Label;

         this.descriptionLabels = new Label[LineCount];
         this.descriptionLabels[0] = this.RecordActivity01Label;
         this.descriptionLabels[1] = this.RecordActivity02Label;
         this.descriptionLabels[2] = this.RecordActivity03Label;
         this.descriptionLabels[3] = this.RecordActivity04Label;
         this.descriptionLabels[4] = this.RecordActivity05Label;
         this.descriptionLabels[5] = this.RecordActivity06Label;
         this.descriptionLabels[6] = this.RecordActivity07Label;
         this.descriptionLabels[7] = this.RecordActivity08Label;
         this.descriptionLabels[8] = this.RecordActivity09Label;

         this.Reset();
      }

      #endregion

      #region Access Methods

      public void Reset()
      {
         this.topIndex = -1;
         this.topIndexSet = false;
         this.recordCount = -1;

         for (int i=0;i<LineCount;i++)
         {
            this.timeLabels[i].Text = "";
            this.descriptionLabels[i].Text = "";
         }

         this.UpArrowLabel.Visible = false;
         this.DownArrowLabel.Visible = false;
      }

      public void UpdateRecordData()
      {
         int displayRecordCount = SessionRecord.Instance.GetRecordCount();

         if (this.recordCount != displayRecordCount)
         {
            this.DisplayRecordData(displayRecordCount);
            this.recordCount = displayRecordCount;
         }
      }

      #endregion

   }
}
