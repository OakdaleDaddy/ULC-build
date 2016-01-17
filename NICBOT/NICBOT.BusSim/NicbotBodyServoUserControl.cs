using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NICBOT.BusSim
{
   public partial class NicbotBodyServoUserControl : UserControl
   {
      #region Fields

      private UInt32 acceleration;
      private UInt32 velocity;
      private Int32 position;
      private Int32 target;
      private Int32 actual;

      private byte status;

      private bool moveRelative;
      private bool moveAbsolute;
      private bool stop;
      private bool increasing;

      private double workingVelocity;
      private double workingPosition;

      #endregion

      #region Helper Functions

      public void EvaluateMovement(ref bool control)
      {
         bool activate = false;

         this.moveAbsolute = false;
         this.moveRelative = false;
         this.stop = false;

         if (this.actual < this.target)
         {
            this.increasing = true;
            activate = true;
         }
         else if (this.actual > this.target)
         {
            this.increasing = false;
            activate = true;
         }

         if (false != activate)
         {
            control = true;
            this.status = 0x40;
         }
      }

      #endregion

      #region Properties

      public string DisplayName
      {
         set { this.NameLabel.Text = value; }
         get { return this.NameLabel.Text; }
      }

      #endregion

      #region User Events

      private void ErrorCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         if (false != this.ErrorCheckBox.Checked)
         {
            this.status = 0x64;
         }
      }

      #endregion

      #region Constructor

      public NicbotBodyServoUserControl()
      {
         this.InitializeComponent();
      }

      #endregion

      #region Access Methods

      public void Reset()
      {
         this.acceleration = 0;
         this.velocity = 0;
         this.position = 0;
         this.target = 0;
         this.actual = -1;

         this.AccelerationTextBox.Text = this.acceleration.ToString();
         this.VelocityTextBox.Text = this.velocity.ToString();
         this.PositionTextBox.Text = this.position.ToString();
         this.TargetTextBox.Text = this.target.ToString();
         this.ActualTextBox.Text = this.actual.ToString();

         this.status = 0x44;
         this.moveAbsolute = false;
         this.moveRelative = false;
         this.stop = false;

         this.workingVelocity = 0;
         this.workingPosition = -1;
      }

      public void SetAcceleration(UInt32 acceleration)
      {
         this.acceleration = acceleration;
         this.AccelerationTextBox.Text = this.acceleration.ToString();
      }

      public void SetVelocity(UInt32 velocity)
      {
         this.velocity = velocity;
         this.VelocityTextBox.Text = this.velocity.ToString();
      }

      public void SetPosition(Int32 position)
      {
         this.position = position;
         this.PositionTextBox.Text = this.position.ToString();
      }

      public void MoveRelative()
      {
         if ((this.status & 0x04) != 0)
         {
            this.workingPosition = this.actual;
            this.target = this.actual + this.position;
            this.TargetTextBox.Text = this.target.ToString();
            this.EvaluateMovement(ref this.moveRelative);
         }
      }

      public void MoveAbsolute()
      {
         if ((this.status & 0x04) != 0)
         {
            this.workingPosition = this.actual;
            this.target = this.position;
            this.TargetTextBox.Text = this.target.ToString();
            this.EvaluateMovement(ref this.moveAbsolute);
         }
      }

      public void MoveAbsoluteWhileMoving()
      {
         this.workingPosition = this.actual;
         this.target = this.position;
         this.TargetTextBox.Text = this.target.ToString();
         this.EvaluateMovement(ref this.moveAbsolute);
      }

      public void StopWithDeceleration()
      {
         this.moveAbsolute = false;
         this.moveRelative = false;
         this.stop = true;
      }

      public void SetOrigin()
      {
         if ((this.status & 0x04) != 0)
         {
            this.position = 0;
            this.PositionTextBox.Text = this.position.ToString();

            this.actual = 0;
            this.ActualTextBox.Text = this.actual.ToString();
         }
      }

      public void Update(double elapsedTime)
      {
         if ((this.status & 0x20) == 0)
         {
            if (false != stop)
            {
               this.workingVelocity = 0;
               this.stop = false;
               this.status = 0x44;
            }
            else if ((false != this.moveAbsolute) || (this.moveRelative))
            {
               double acceleration = (this.acceleration * 10)  * elapsedTime;
               this.workingVelocity = this.workingVelocity + acceleration;

               if (this.workingVelocity > this.velocity)
               {
                  this.workingVelocity = this.velocity;
               }

               double positionChange = this.workingVelocity * elapsedTime;

               if (false != increasing)
               {
                  this.workingPosition += positionChange;
                  this.actual = (Int32)this.workingPosition;

                  if (this.actual >= this.target)
                  {
                     this.workingVelocity = 0;
                     this.workingPosition = this.target;                     
                     this.actual = this.target;
                     this.moveAbsolute = false;
                     this.moveRelative = false;
                     this.status = 0x44;
                  }
               }
               else
               {
                  this.workingPosition -= positionChange;
                  this.actual = (Int32)this.workingPosition;

                  if (this.actual <= this.target)
                  {
                     this.workingVelocity = 0;
                     this.workingPosition = this.target;
                     this.actual = this.target;
                     this.moveAbsolute = false;
                     this.moveRelative = false;
                     this.status = 0x44;
                  }
               }
            }

            this.ActualTextBox.Text = this.actual.ToString();
         }
      }

      public byte GetStatus()
      {
         return (this.status);
      }

      public Int32 GetActualPosition()
      {
         return (this.actual);
      }

      #endregion

   }
}
