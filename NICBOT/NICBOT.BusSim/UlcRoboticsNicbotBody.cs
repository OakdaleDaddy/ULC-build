
namespace NICBOT.BusSim
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Data;
   using System.Linq;
   using System.Text;
   using System.Windows.Forms;
   using System.Xml;

   using Microsoft.Win32;

   public partial class UlcRoboticsNicbotBody : DeviceControl
   {
      #region Definition

      private enum ToolLocations
      {
         front,
         rear,
      }
   
      #endregion

      #region Fields

      private Random randomValue;

      private UInt32 deviceType;
      private UInt32 errorStatus;
      private string deviceName;
      private string version;

      private byte nvBaudRateCode;
      private byte nvNodeId;
      private byte nvMode;

      private bool active;
      private bool resetActive;
      private byte baudRateCode;
      private byte nodeId;
      private byte sdoNodeId;
      
      private UInt32 sdoConsumerHeartbeat;
      private byte consumerHeartbeatNode;
      private UInt16 consumerHeartbeatTime;
      private bool consumerHeartbeatActive;
      private DateTime consumerHeartbeatTimeLimit;
      
      private UInt16 producerHeartbeatTime;

      private TPDOMapping[] tpdoMapping;
      private DateTime heartbeatLimit;

      private byte sdoMode;
      private byte mode;

      private byte videoSelectA;
      private byte videoSelectB;
      private UInt16 solenoidControl;

      private Int16 frontDrillSpeed;
      private Int16 frontDrillIndex;
      private Int16 rearDrillSpeed;
      private Int16 rearDrillIndex;
      private UInt16 sensorIndex;

      private UInt32 drillServoProportionalControlConstant;
      private UInt32 drillServoIntegralControlConstant;
      private UInt32 drillServoDerivativeControlConstant;
      private UInt32 drillServoAcceleration;
      private UInt32 drillServoHomingVelocity;
      private UInt32 drillServoHomingBackoffCount;
      private UInt32 drillServoTravelVelocity;
      private UInt32 drillServoErrorLimit;
      private UInt32 drillServoPulsesPerUnit;

      private UInt16 drillIndexLimit;

      private byte autoDrillControl;
      private UInt16 autoDrillSearchSpeed;
      private UInt16 autoDrillTravelSpeed;
      private UInt16 autoDrillRotationSpeed;
      private UInt16 autoDrillCuttingSpeed;
      private UInt16 autoDrillCuttingDepth;
      private UInt16 autoDrillPeckCuttingIncrement;
      private UInt16 autoDrillPeckRetractionDistance;
      private UInt16 autoDrillPeckRetractionPosition;

      private Int16 actualFrontDrillSpeed;
      private Int16 actualFrontDrillIndex;
      private Int16 actualRearDrillSpeed;
      private Int16 actualRearDrillIndex;
      private double actualSensorIndex;

      private UInt16 controlWord;
      //private ToolLocations controlLocation;
      //private bool controlFrontLaserOn;
      //private bool controlRearLaserOn;
      private bool controlLocateOrigin;
      private bool controlAutoCut;
      //private bool controlPauseAuto;
      
      private UInt16 statusWord;
      //private bool statusAutoDrillOriginFound;
      //private bool statusAutoDrillCutComplete;
      //private bool statusAutoDrillOriginHunting;
      //private bool statusAutoDrillCutActive;
      //private bool statusAutoDrillCutPaused;

      //private UInt16 frontDrillOrigin;
     // private UInt16 rearDrillOrigin;

      private NicbotBodyDrillContext frontDrillContext;
      private NicbotBodyDrillContext rearDrillContext;

      private DateTime speedUpdateTime;
      private DateTime indexUpdateTime;
      private DateTime indexLastUpdateTime;

      private UInt16 accelerometerX;
      private UInt16 accelerometerY;
      private UInt16 accelerometerZ;

      private UInt16 processedControlWord;
      
      #endregion

      #region Helper Functions

      private void SendDebug(UInt32 a, UInt32 b)
      {
         int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
         byte[] debugMsg = new byte[8];

         byte[] aData = BitConverter.GetBytes(a);
         byte[] bData = BitConverter.GetBytes(b);

         for (int i = 0; i < 4; i++)
         {
            debugMsg[i] = aData[i];
         }

         for (int i = 0; i < 4; i++)
         {
            debugMsg[i+4] = bData[i];
         }

         this.Transmit(cobId, debugMsg);
      }

      private byte GetServoStatus(int axis)
      {
         byte result = 0;

         if (0 == axis)
         {
            result = this.Servo0UserControl.GetStatus();
         }
         else if (1 == axis)
         {
            result = this.Servo1UserControl.GetStatus();
         }

         return (result);
      }

      private void SetServoAcceleration(int axis, UInt32 acceleration)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.SetAcceleration(acceleration);
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.SetAcceleration(acceleration);
         }
      }

      private void SetServoVelocity(int axis, UInt32 velocity)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.SetVelocity(velocity);
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.SetVelocity(velocity);
         }
      }

      private void SetServoPosition(int axis, Int32 position)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.SetPosition(position);
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.SetPosition(position);
         }
      }

      private void MoveServoRelative(int axis)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.MoveRelative();
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.MoveRelative();
         }
      }

      private void MoveServoAbsolute(int axis)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.MoveAbsolute();
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.MoveAbsolute();
         }
      }

      private void MoveServoAbsoluteWhileMoving(int axis)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.MoveAbsoluteWhileMoving();
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.MoveAbsoluteWhileMoving();
         }
      }

      private void StopServo(int axis)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.StopWithDeceleration();
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.StopWithDeceleration();
         }
      }

      private void SetServoOrigin(int axis)
      {
         if (0 == axis)
         {
            this.Servo0UserControl.SetOrigin();
         }
         else if (1 == axis)
         {
            this.Servo1UserControl.SetOrigin();
         }
      }

      private void UpdateCameraControl()
      {
         UInt16 cameraControl = 0;

         if (0 != this.VideoSelectA)
         {
            cameraControl |= (UInt16)(1 << (this.VideoSelectA - 1));
         }

         if (0 != this.videoSelectB)
         {
            cameraControl |= (UInt16)(1 << (this.videoSelectB - 1));
         }

         this.CameraControlTextBox.Text = string.Format("{0:X4}", cameraControl);

         this.Camera1CameraControl.CameraOn = ((cameraControl & 0x0001) != 0) ? true : false;
         this.Camera2CameraControl.CameraOn = ((cameraControl & 0x0002) != 0) ? true : false;
         this.Camera3CameraControl.CameraOn = ((cameraControl & 0x0004) != 0) ? true : false;
         this.Camera4CameraControl.CameraOn = ((cameraControl & 0x0008) != 0) ? true : false;
         this.Camera5CameraControl.CameraOn = ((cameraControl & 0x0010) != 0) ? true : false;
         this.Camera6CameraControl.CameraOn = ((cameraControl & 0x0020) != 0) ? true : false;
         this.Camera7CameraControl.CameraOn = ((cameraControl & 0x0040) != 0) ? true : false;
         this.Camera8CameraControl.CameraOn = ((cameraControl & 0x0080) != 0) ? true : false;
         this.Camera9CameraControl.CameraOn = ((cameraControl & 0x0100) != 0) ? true : false;
         this.Camera10CameraControl.CameraOn = ((cameraControl & 0x0200) != 0) ? true : false;
         this.Camera11CameraControl.CameraOn = ((cameraControl & 0x0400) != 0) ? true : false;
         this.Camera12CameraControl.CameraOn = ((cameraControl & 0x0800) != 0) ? true : false;
      }

      private NicBotCameraControl GetCameraObject(int cameraId)
      {
         NicBotCameraControl result = this.Camera1CameraControl;

         if (1 == cameraId) { result = this.Camera1CameraControl; }
         else if (2 == cameraId) { result = this.Camera2CameraControl; }
         else if (3 == cameraId) { result = this.Camera3CameraControl; }
         else if (4 == cameraId) { result = this.Camera4CameraControl; }
         else if (5 == cameraId) { result = this.Camera5CameraControl; }
         else if (6 == cameraId) { result = this.Camera6CameraControl; }
         else if (7 == cameraId) { result = this.Camera7CameraControl; }
         else if (8 == cameraId) { result = this.Camera8CameraControl; }
         else if (9 == cameraId) { result = this.Camera9CameraControl; }
         else if (10 == cameraId) { result = this.Camera10CameraControl; }
         else if (11 == cameraId) { result = this.Camera11CameraControl; }
         else if (12 == cameraId) { result = this.Camera12CameraControl; }

         return (result);
      }

      private void SetCameraLightLevel(int cameraId, byte level)
      {
         NicBotCameraControl control = this.GetCameraObject(cameraId);
         control.LightLevel = level;
      }

      private byte GetCameraLightLevel(int cameraId)
      {
         NicBotCameraControl control = this.GetCameraObject(cameraId);
         byte level = control.LightLevel;
         return (level);
      }

      private void ResetControlStatus()
      {
         //this.controlFrontLaserOn = false;
         //this.controlRearLaserOn = false;
         this.controlLocateOrigin = false;
         this.controlAutoCut = false;
         //this.controlPauseAuto = false;

         //this.statusAutoDrillOriginFound = false;
         //this.statusAutoDrillCutComplete = false;
         //this.statusAutoDrillOriginHunting = false;
         //this.statusAutoDrillCutActive = false;
         //this.statusAutoDrillCutPaused = false;
      }

      private void UpdateSpeed(ref Int16 last, Int16 requested)
      {
         int percent = 100;// this.randomValue.Next(98, 103);
         requested = (Int16)(((double)requested) * ((double)percent / 100));
         Int16 actual = requested;

         if (last > requested)
         {
            double difference = last - requested;
            double step = difference / 5;
            step = (step < 1.0) ? difference : step;
            actual = (Int16)(last - step);
         }
         else if (last < requested)
         {
            double difference = requested - last;
            double step = difference / 5;
            step = (step < 1.0) ? difference : step;
            actual = (Int16)(last + step);
         }

         last = actual;
      }

      private void UpdateIndex(ref double last, UInt16 requested, UInt16 speed, double elapsedSeconds)
      {
         double delta = ((double)speed / 600) * elapsedSeconds;

         if (last > requested)
         {
            last -= delta;

            if (last < requested)
            {
               last = requested;
            }
         }
         else if (last < requested)
         {
            last += delta;

            if (last > requested)
            {
               last = requested;
            }
         }
      }

      private void UpdateActualDrillPosition()
      {
         Int32 position;

         if (0 != this.drillServoPulsesPerUnit)
         {
            position = this.Servo1UserControl.GetActualPosition();
            position *= -100;
            position /= (Int32)this.drillServoPulsesPerUnit;
            position &= 0xFFFF;

            if (position != this.ActualFrontDrillIndex)
            {
               this.ActualFrontDrillIndex = (Int16)position;
            }


            position = this.Servo0UserControl.GetActualPosition();
            position *= -100;
            position /= (Int32)this.drillServoPulsesPerUnit;
            position &= 0xFFFF;

            if (position != actualRearDrillIndex)
            {
               this.ActualRearDrillIndex = (Int16)position;
            }
         }
      }

      private void UpdateDrill(NicbotBodyDrillContext drillContext, double elapsedSeconds)
      {
         if ((drillContext.control & 0x01) != 0)
         {
            drillContext.OnLaser(true);
         }
         else
         {
            drillContext.OnLaser(false);
         }

         if (NicbotBodyDrillStates.idle == drillContext.state)
         {
            if ((drillContext.control & 0x04) != 0)
            {
               drillContext.control &= 0xFB;

               this.SetServoAcceleration(drillContext.axis, this.drillServoAcceleration);
               this.SetServoVelocity(drillContext.axis, this.drillServoHomingVelocity);
               this.SetServoPosition(drillContext.axis, 20000000);
               this.MoveServoRelative(drillContext.axis);

               this.SendDebug(0xBC, 1);
               drillContext.state = NicbotBodyDrillStates.homeRetractToLimit;
            }
            else if (drillContext.manualSetPoint != drillContext.processedSetPoint)
            {
               Int32 positionRequest;

               positionRequest = -drillContext.manualSetPoint;
               positionRequest *= (Int32)drillServoPulsesPerUnit;
               positionRequest /= 100;
               positionRequest--;

               this.SetServoAcceleration(drillContext.axis, this.drillServoAcceleration);
               this.SetServoVelocity(drillContext.axis, this.drillServoTravelVelocity);
               this.SetServoPosition(drillContext.axis, positionRequest);
               this.MoveServoAbsolute(drillContext.axis);

               drillContext.processedSetPoint = drillContext.manualSetPoint;

               this.SendDebug(0xBC, 10);
               drillContext.state = NicbotBodyDrillStates.manual;
            }
         }
         else
         {
            byte servoStatus;

            servoStatus = this.GetServoStatus(drillContext.axis);

            if ((servoStatus & 0x20) != 0)
            {
               drillContext.state = NicbotBodyDrillStates.faulted;
               //this.devicefasetDeviceFault(servoExcessivePositionFaultCode);
            }
            else if ((drillContext.control & 0x02) != 0)
            {
               drillContext.control &= 0xFD;
               this.StopServo(drillContext.axis);

               this.SendDebug(0xBC, 12);
               drillContext.state = NicbotBodyDrillStates.stop;
            }
            else
            {
               if (NicbotBodyDrillStates.manual == drillContext.state)
               {
                  // todo check limits to stop
                  if ((servoStatus & 0x04) != 0)
                  {
                     this.SendDebug(0xBC, 11);
                     drillContext.state = NicbotBodyDrillStates.idle;
                  }
                  else if (drillContext.manualSetPoint != drillContext.processedSetPoint)
                  {
                     Int32 positionRequest;

                     positionRequest = -drillContext.manualSetPoint;
                     positionRequest *= (Int32)drillServoPulsesPerUnit;
                     positionRequest /= 100;
                     positionRequest--;

                     this.SendDebug(0xCD, (UInt32)positionRequest);
                     this.SetServoPosition(drillContext.axis, positionRequest);
                     this.MoveServoAbsoluteWhileMoving(drillContext.axis);

                     drillContext.processedSetPoint = drillContext.manualSetPoint;
                  }
               }
               else if ((NicbotBodyDrillStates.stop == drillContext.state))
               {
                  if ((servoStatus & 0x04) != 0)
                  {
                     this.SendDebug(0xBC, 13);
                     drillContext.state = NicbotBodyDrillStates.idle;
                  }
               }
               else if ((NicbotBodyDrillStates.homeRetractToLimit == drillContext.state))
               {
                  if (((this.StatusWord & drillContext.retractMask) != 0))
                  {
                     this.StopServo(drillContext.axis);
                     this.SendDebug(0xBC, 2);
                     drillContext.state = NicbotBodyDrillStates.homeStopFromRetract;
                  }
               }
               else if ((NicbotBodyDrillStates.homeStopFromRetract == drillContext.state))
               {
                  if ((servoStatus & 0x04) != 0)
                  {
                     this.SetServoPosition(drillContext.axis, -4000000);
                     this.MoveServoRelative(drillContext.axis);

                     this.SendDebug(0xBC, 3);
                     drillContext.state = NicbotBodyDrillStates.homeExtendToNotLimit;
                  }
               }
               else if ((NicbotBodyDrillStates.homeExtendToNotLimit == drillContext.state))
               {
                  if (((this.StatusWord & drillContext.retractMask) == 0))
                  {
                     this.StopServo(drillContext.axis);
                     this.SendDebug(0xBC, 4);
                     drillContext.state = NicbotBodyDrillStates.homeStopFromExtend;
                  }
               }
               else if ((NicbotBodyDrillStates.homeStopFromExtend == drillContext.state))
               {
                  if ((servoStatus & 0x04) != 0)
                  {
                     Int32 positionRequest = (Int32)this.drillServoHomingBackoffCount;
                     positionRequest *= -1;
                     this.SetServoPosition(drillContext.axis, positionRequest);
                     this.MoveServoRelative(drillContext.axis);

                     this.SendDebug(0xBC, 5);
                     drillContext.state = NicbotBodyDrillStates.homeBackoff;
                  }
               }
               else if ((NicbotBodyDrillStates.homeBackoff == drillContext.state))
               {
                  if ((servoStatus & 0x04) != 0)
                  {
                     this.SetServoOrigin(drillContext.axis);
                     drillContext.processedSetPoint = 0;
                     drillContext.manualSetPoint = 0;

                     this.SendDebug(0xBC, 6);
                     drillContext.state = NicbotBodyDrillStates.idle;
                  }
               }
            }
         }

      }

#if false
      private enum AutoDrillStates
      {
         start,
         findOrigin,
         cutting,
         retracting,
         paused
      }

      private AutoDrillStates autoDrillState;
#endif

      private void UpdateTools(UInt16 drillOrigin, ref UInt16 drillIndexSetPoint, ref double actualDrillIndex, double elapsedSeconds)
      {
#if false
         // set front laser
         if (false != this.controlFrontLaserOn)
         {
            this.FrontLaserOnOffLabel.Text = "on";
            this.FrontLaserOnOffLabel.BackColor = Color.Red;
         }
         else
         {
            this.FrontLaserOnOffLabel.Text = "off";
            this.FrontLaserOnOffLabel.BackColor = Color.DarkSlateGray;
         }

         // set rear laser
         if (false != this.controlRearLaserOn)
         {
            this.RearLaserOnOffLabel.Text = "on";
            this.RearLaserOnOffLabel.BackColor = Color.Red;
         }
         else
         {
            this.RearLaserOnOffLabel.Text = "off";
            this.RearLaserOnOffLabel.BackColor = Color.DarkSlateGray;
         }
#endif

#if false
         // process drill mode
         if ((false != this.controlLocateOrigin) && (false != this.statusAutoDrillOriginHunting) && (false == this.statusAutoDrillCutPaused))
         {
            if (false == this.controlPauseAuto)
            {
               UInt16 actualDrillIndexValue = (UInt16)actualDrillIndex;

               if (this.drillIndexLimit == actualDrillIndexValue)
               {
                  this.statusAutoDrillOriginHunting = false;
               }
               else if (drillOrigin == actualDrillIndexValue)
               {
                  drillIndexSetPoint = actualDrillIndexValue;
                  this.statusAutoDrillOriginFound = true;
                  this.statusAutoDrillOriginHunting = false;
               }
               else
               {
                  double drillIndex = actualDrillIndex;
                  this.UpdateIndex(ref drillIndex, this.drillIndexLimit, this.AutoDrillSearchSpeed, elapsedSeconds);
                  actualDrillIndex = drillIndex;
               }
            }
            else
            {
               drillIndexSetPoint = (UInt16)actualDrillIndex;
               this.statusAutoDrillCutPaused = true;
            }
         }
         else if ((false != this.controlAutoCut) && (false == this.statusAutoDrillCutPaused))
         {
            #region Auto Drill

            AutoDrillStates currentState;
            
            do
            {
               currentState = this.autoDrillState;

               switch(this.autoDrillState)
               {
                  case AutoDrillStates.start:
                  {
                     if ((this.AutoDrillControl & 0x10) != 0)
                     {
                        this.controlLocateOrigin = true;
                        this.statusAutoDrillOriginHunting = true;
                        this.autoDrillState = AutoDrillStates.findOrigin;
                     }
                     else
                     {
                        this.autoDrillState = AutoDrillStates.cutting;
                     }

                     break;
                  }
                  case AutoDrillStates.findOrigin:
                  {
                     if (false == this.statusAutoDrillOriginHunting)
                     {
                        this.autoDrillState = AutoDrillStates.cutting;
                     }

                     break;
                  }
                  case AutoDrillStates.cutting:
                  {
                     if (false == this.controlPauseAuto)
                     {
                     }
                     else
                     {
                        drillIndexSetPoint = (UInt16)actualDrillIndex;
                        this.statusAutoDrillCutPaused = true;
                     }

                     break;
                  }
               }
            }
            while (currentState != this.autoDrillState);

            #endregion
         }
         else if ((false != this.statusAutoDrillCutPaused) || ((false == this.controlAutoCut) && (false == this.controlLocateOrigin)))
         {
            double drillIndex = actualDrillIndex;
            this.UpdateIndex(ref drillIndex, drillIndexSetPoint, this.AutoDrillTravelSpeed, elapsedSeconds);
            actualDrillIndex = drillIndex;

            if (false == this.controlPauseAuto)
            {
               this.statusAutoDrillCutPaused = false;
            }
         }
#endif
      }

      private void CheckTpdoMappings(UInt16 pdoIndex, byte pdoSubIndex)
      {
         for (int i = 0; i < 4; i++)
         {
            if (this.tpdoMapping[i].Contains(pdoIndex, pdoSubIndex) != false)
            {
               this.tpdoMapping[i].Activate();
            }
         }
      }

      private UInt16 GetStatus()
      {
         UInt16 result = 0;

         if (0 == this.Mode)
         {
#if false
            if (ToolLocations.front == this.controlLocation)
            {
               if (0 == this.ActualFrontDrillIndex)
               {
                  result |= 0x01;
               }

               if (this.drillIndexLimit == this.ActualFrontDrillIndex)
               {
                  result |= 0x02;
               }
            }
            else
            {
               if (0 == this.ActualRearDrillIndex)
               {
                  result |= 0x01;
               }

               if (this.drillIndexLimit == this.ActualRearDrillIndex)
               {
                  result |= 0x02;
               }
            }

            if (false != this.statusAutoDrillOriginFound)
            {
               result |= 0x04;
            }

            if (false != this.statusAutoDrillCutComplete)
            {
               result |= 0x08;
            }

            if (false != this.statusAutoDrillOriginHunting)
            {
               result |= 0x10;
            }

            if (false != this.statusAutoDrillCutActive)
            {
               result |= 0x20;
            }

            if (false != this.statusAutoDrillCutPaused)
            {
               result |= 0x40;
            }

            if (ToolLocations.rear == this.controlLocation)
            {
               result |= 0x80;
            }

#endif
            result |= (UInt16)((false != this.RearDrillRetractLimitCheckBox.Checked) ? 0x0100 : 0);
            result |= (UInt16)((false != this.RearDrillExtendLimitCheckBox.Checked) ? 0x0200 : 0);
            result |= (UInt16)((false != this.FrontDrillRetractLimitCheckBox.Checked) ? 0x0400 : 0);
            result |= (UInt16)((false != this.FrontDrillExtendLimitCheckBox.Checked) ? 0x0800 : 0);
         }
         else if (1 == this.Mode)
         {
         }

         return (result);
      }
      
      #endregion

      #region Properties

      private byte Mode
      {
         set
         {
            this.mode = value;
            this.DeviceModeLabel.Text = (1 == value) ? "inspect" : "repair";
         }

         get
         {
            return (this.mode);
         }
      }

      private byte VideoSelectA
      {
         set
         {
            this.videoSelectA = value;
            this.VideoSelectATextBox.Text = string.Format("{0:X2}", value);
            this.UpdateCameraControl();
         }

         get
         {
            return (this.videoSelectA);
         }
      }

      private byte VideoSelectB
      {
         set
         {
            this.videoSelectB = value;
            this.VideoSelectBTextBox.Text = string.Format("{0:X2}", value);
            this.UpdateCameraControl();
         }

         get
         {
            return (this.videoSelectB);
         }
      }

      private UInt16 SolenoidControl
      {
         set
         {
            this.solenoidControl = value;
            this.SolenoidControlTextBox.Text = string.Format("{0:X4}", value);

            if (0 == this.Mode)
            {
               this.Solenoid1SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0004) != 0) ? true : false;
               this.Solenoid2SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x4000) != 0) ? true : false;
               this.Solenoid3SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0008) != 0) ? true : false;
               this.Solenoid4SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x8000) != 0) ? true : false;
               this.Solenoid5SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x2000) != 0) ? true : false;
               this.Solenoid6SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x1000) != 0) ? true : false;
               this.Solenoid7SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0040) != 0) ? true : false;
               this.Solenoid8SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0080) != 0) ? true : false;
               this.Solenoid9SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0001) != 0) ? true : false;
               this.Solenoid10SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0002) != 0) ? true : false;

               UInt16 wheelModeSelectValue = (UInt16)(value & 0x0030);

               if (0 != wheelModeSelectValue)
               {
               }
            }
            else if (1 == this.Mode)
            {
               this.Solenoid1SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0001) != 0) ? true : false;
               this.Solenoid2SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0002) != 0) ? true : false;
               this.Solenoid3SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0004) != 0) ? true : false;
               this.Solenoid4SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0008) != 0) ? true : false;
               this.Solenoid5SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x4000) != 0) ? true : false;
               this.Solenoid6SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x8000) != 0) ? true : false;
               this.Solenoid7SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x1000) != 0) ? true : false;
               this.Solenoid8SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x2000) != 0) ? true : false;
               this.Solenoid9SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0020) != 0) ? true : false;
               this.Solenoid10SolenoidControl.SolenoidOn = ((this.solenoidControl & 0x0010) != 0) ? true : false;

               UInt16 wheelModeSelectValue = (UInt16)(value & 0x00C0);

               if (0 != wheelModeSelectValue)
               {
               }
            }
         }

         get
         {
            return (this.solenoidControl);
         }
      }

      private Int16 FrontDrillSpeed
      {
         set
         {
            this.frontDrillSpeed = value;
            this.FrontDrillSpeedTextBox.Text = string.Format("{0}", value);
         }

         get
         {
            return (this.frontDrillSpeed);
         }
      }

      private Int16 FrontDrillIndex
      {
         set
         {
            this.frontDrillIndex = value;
            this.frontDrillContext.manualSetPoint = value;
            this.FrontDrillIndexTextBox.Text = string.Format("{0}", value);
         }

         get
         {
            return (this.frontDrillIndex);
         }
      }

      private Int16 RearDrillSpeed
      {
         set
         {
            this.rearDrillSpeed = value;
            this.RearDrillSpeedTextBox.Text = string.Format("{0}", value);
         }

         get
         {
            return (this.rearDrillSpeed);
         }
      }

      private Int16 RearDrillIndex
      {
         set
         {
            this.rearDrillIndex = value;
            this.rearDrillContext.manualSetPoint = value;
            this.RearDrillIndexTextBox.Text = string.Format("{0}", value);
         }

         get
         {
            return (this.rearDrillIndex);
         }
      }

      private UInt16 SensorIndex
      {
         set
         {
            this.sensorIndex = value;
            this.SensorIndexTextBox.Text = string.Format("{0}", value);
         }

         get
         {
            return (this.sensorIndex);
         }
      }

      private byte AutoDrillControl
      {
         set
         {
            if ((value != this.autoDrillControl) || (false != this.resetActive))
            {
               this.autoDrillControl = value;
               this.AutoDrillControlTextBox.Text = string.Format("{0:X2}", value);

               if ((this.autoDrillControl & 0x01) != 0)
               {
                  this.AutoDrillModeTextBox.Text = "peck";
               }
               else
               {
                  this.AutoDrillModeTextBox.Text = "continuous";
               }

               if ((this.autoDrillControl & 0x10) != 0)
               {
                  this.AutoDrillControlOriginTextBox.Text = "auto origin";
               }
               else
               {
                  this.AutoDrillControlOriginTextBox.Text = "";
               }

               if ((this.autoDrillControl & 0x20) != 0)
               {
                  this.AutoDrillRectractionModeTextBo.Text = "position";
               }
               else
               {
                  this.AutoDrillRectractionModeTextBo.Text = "distance";
               }
            }
         }

         get
         {
            return (this.autoDrillControl);
         }
      }

      private UInt16 AutoDrillSearchSpeed
      {
         set
         {
            if (value != this.autoDrillSearchSpeed)
            {
               this.autoDrillSearchSpeed = value;
               this.AutoDrillSearchSpeedTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillSearchSpeed);
         }
      }

      private UInt16 AutoDrillTravelSpeed
      {
         set
         {
            if (value != this.autoDrillTravelSpeed)
            {
               this.autoDrillTravelSpeed = value;
               this.AutoDrillTravelSpeedTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillTravelSpeed);
         }
      }

      private UInt16 AutoDrillRotationSpeed
      {
         set
         {
            if (value != this.autoDrillRotationSpeed)
            {
               this.autoDrillRotationSpeed = value;
               this.AutoDrillRotationSpeedTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillRotationSpeed);
         }
      }

      private UInt16 AutoDrillCuttingSpeed
      {
         set
         {
            if (value != this.autoDrillCuttingSpeed)
            {
               this.autoDrillCuttingSpeed = value;
               this.AutoDrillCuttingSpeedTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillCuttingSpeed);
         }
      }

      private UInt16 AutoDrillCuttingDepth
      {
         set
         {
            if (value != this.autoDrillCuttingDepth)
            {
               this.autoDrillCuttingDepth = value;
               this.AutoDrillCuttingDepthTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillCuttingDepth);
         }
      }

      private UInt16 AutoDrillPeckCuttingIncrement
      {
         set
         {
            if (value != this.autoDrillPeckCuttingIncrement)
            {
               this.autoDrillPeckCuttingIncrement = value;
               this.AutoDrillPeckCuttingIncrementTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillPeckCuttingIncrement);
         }
      }

      private UInt16 AutoDrillPeckRetractionDistance
      {
         set
         {
            if (value != this.autoDrillPeckRetractionDistance)
            {
               this.autoDrillPeckRetractionDistance = value;
               this.AutoDrillPeckRetractionDistanceTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillPeckRetractionDistance);
         }
      }

      private UInt16 AutoDrillPeckRetractionPosition
      {
         set
         {
            if (value != this.autoDrillPeckRetractionPosition)
            {
               this.autoDrillPeckRetractionPosition = value;
               this.AutoDrillPeckRetractionPositionTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.autoDrillPeckRetractionPosition);
         }
      }

      private Int16 ActualFrontDrillSpeed
      {
         set
         {
            if (value != this.actualFrontDrillSpeed)
            {
               this.actualFrontDrillSpeed = value;
               this.ActualFrontDrillSpeedTextBox.Text = string.Format("{0}", value);
               this.CheckTpdoMappings(0x2411, 0);
            }
         }

         get
         {
            return (this.actualFrontDrillSpeed);
         }
      }

      private Int16 ActualFrontDrillIndex
      {
         set
         {
            if (value != this.actualFrontDrillIndex)
            {
               this.actualFrontDrillIndex = value;
               this.ActualFrontDrillIndexTextBox.Text = string.Format("{0}", value);
               this.CheckTpdoMappings(0x2412, 0);
            }
         }

         get
         {
            return (this.actualFrontDrillIndex);
         }
      }

      private Int16 ActualRearDrillSpeed
      {
         set
         {
            if (value != this.actualRearDrillSpeed)
            {
               this.actualRearDrillSpeed = value;
               this.ActualRearDrillSpeedTextBox.Text = string.Format("{0}", value);
               this.CheckTpdoMappings(0x2413, 0);
            }
         }

         get
         {
            return (this.actualRearDrillSpeed);
         }
      }

      private Int16 ActualRearDrillIndex
      {
         set
         {
            if (value != this.actualRearDrillIndex)
            {
               this.actualRearDrillIndex = value;
               this.ActualRearDrillIndexTextBox.Text = string.Format("{0}", value);
               this.CheckTpdoMappings(0x2414, 0);
            }
         }

         get
         {
            return (this.actualRearDrillIndex);
         }
      }

      private double ActualSensorIndex
      {
         set
         {
            if (value != this.actualSensorIndex)
            {
               this.actualSensorIndex = value;
               this.ActualSensorIndexTextBox.Text = string.Format("{0:0.0}", value);
               this.CheckTpdoMappings(0x2415, 0);
            }
         }

         get
         {
            return (this.actualSensorIndex);
         }
      }

      private UInt16 AccelerometerX
      {
         set
         {
            if (value != this.accelerometerX)
            {
               this.accelerometerX = value;
               this.AccelerometerXTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.accelerometerX);
         }
      }

      private UInt16 AccelerometerY
      {
         set
         {
            if (value != this.accelerometerY)
            {
               this.accelerometerY = value;
               this.AccelerometerYTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.accelerometerY);
         }
      }

      private UInt16 AccelerometerZ
      {
         set
         {
            if (value != this.accelerometerZ)
            {
               this.accelerometerZ = value;
               this.AccelerometerZTextBox.Text = string.Format("{0}", value);
            }
         }

         get
         {
            return (this.accelerometerZ);
         }
      }

      private UInt16 ControlWord
      {
         set
         {
            if ((value != this.controlWord) || (false != this.resetActive))
            {
#if false
               if ((value & 0x0080) == 0)
               {
                  if (ToolLocations.front != this.controlLocation)
                  {
                     this.ResetControlStatus();
                     this.controlLocation = ToolLocations.front;
                  }
               }
               else
               {
                  if (ToolLocations.rear != this.controlLocation)
                  {
                     this.ResetControlStatus();
                     this.controlLocation = ToolLocations.rear;
                  }
               }

               this.controlFrontLaserOn = ((value & 0x0001) != 0) ? true : false;
               this.controlRearLaserOn = ((value & 0x0002) != 0) ? true : false;

#if false
               if ((false == this.controlLocateOrigin) && ((value & 0x02) != 0))
               {
                  if (ToolLocations.front == this.controlLocation)
                  {
                     this.frontDrillOrigin = 0;
                     UInt16.TryParse(this.FrontDrillIndexOriginTextBox.Text, out this.frontDrillOrigin);
                  }
                  else
                  {
                     this.rearDrillOrigin = 0;
                     UInt16.TryParse(this.RearDrillIndexOriginTextBox.Text, out this.rearDrillOrigin);
                  }

                  this.controlLocateOrigin = true;
                  this.statusAutoDrillOriginFound = false;
                  this.statusAutoDrillOriginHunting = true;
               }
               else if ((false != this.controlLocateOrigin) && ((value & 0x02) == 0))
               {
                  this.controlLocateOrigin = false;
                  this.statusAutoDrillOriginFound = false;
                  this.statusAutoDrillOriginHunting = false;
               }

               if ((false == this.controlAutoCut) && ((value & 0x04) != 0))
               {
                  this.controlAutoCut = true;
                  this.autoDrillState = AutoDrillStates.start;
               }

               if ((false != this.controlAutoCut) && ((value & 0x04) == 0))
               {
                  this.controlAutoCut = false;
               }

               if ((false == this.controlPauseAuto) && ((value & 0x08) != 0))
               {
                  this.controlPauseAuto = true;
               }

               if ((false != this.controlPauseAuto) && ((value & 0x08) == 0))
               {
                  this.controlPauseAuto = false;
               }
#endif
#endif
               this.controlWord = value;
               this.ControlWordTextBox.Text = string.Format("{0:X4}", value);
            }
         }

         get
         {
            return (this.controlWord);
         }
      }

      private UInt16 StatusWord
      {
         set
         {
            if ((value != this.statusWord) || (false != this.resetActive))
            {
               this.statusWord = value;
               this.StatusWordTextBox.Text = string.Format("{0:X4}", value);
               this.CheckTpdoMappings(0x2501, 0);
            }
         }

         get
         {
            return (this.statusWord);
         }
      }

      #endregion

      #region Delegates

      private bool TPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if (0 == this.Mode)
         {
            if ((index >= 0x2411) && (index <= 0x2414))
            {
               result = true;
            }
            else if ((0x2441 == index) ||
                     (0x2442 == index) ||
                     (0x2443 == index))
            {
               result = true;
            }
            else if (0x2501 == index)
            {
               result = true;
            }
         }
         else if (1 == this.Mode)
         {
            if (0x2415 == index)
            {
               result = true;
            }
            else if ((0x2441 == index) ||
                     (0x2442 == index) ||
                     (0x2443 == index))
            {
               result = true;
            }
            else if (0x2501 == index)
            {
               result = true;
            }
         }

         return (result);
      }

      private int TPdoSizeHandler(UInt16 index, byte subIndex)
      {
         byte result = 0;

         if (0 == this.Mode)
         {
            if ((index >= 0x2411) && (index <= 0x2414))
            {
               result = 2;
            }
            else if ((0x2441 == index) ||
                     (0x2442 == index) ||
                     (0x2443 == index))
            {
               result = 2;
            }
            else if (0x2501 == index)
            {
               result = 2;
            }
         }
         else if (1 == this.Mode)
         {
            if (0x2415 == index)
            {
               result = 2;
            }
            else if ((0x2441 == index) ||
                     (0x2442 == index) ||
                     (0x2443 == index))
            {
               result = 2;
            }
            else if (0x2501 == index)
            {
               result = 2;
            }
         }

         return (result);
      }

      private int TPdoDataHandler(UInt16 index, byte subIndex, byte[] buffer, int offset)
      {
         byte[] pdoData = new byte[8];
         UInt32 dataLength = 0;
         this.LoadDeviceData(index, subIndex, pdoData, ref dataLength);

         for (int i = 0; i < dataLength; i++)
         {
            buffer[offset + i] = pdoData[i];
         }

         return ((int)dataLength);
      }

      private void FrontLaserControl(bool on)
      {
         if (false != on)
         {
            this.FrontLaserOnOffLabel.Text = "on";
            this.FrontLaserOnOffLabel.BackColor = Color.Red;
         }
         else
         {
            this.FrontLaserOnOffLabel.Text = "off";
            this.FrontLaserOnOffLabel.BackColor = Color.DarkSlateGray;
         }
      }

      private void RearLaserControl(bool on)
      {
         if (false != on)
         {
            this.RearLaserOnOffLabel.Text = "on";
            this.RearLaserOnOffLabel.BackColor = Color.Red;
         }
         else
         {
            this.RearLaserOnOffLabel.Text = "off";
            this.RearLaserOnOffLabel.BackColor = Color.DarkSlateGray;
         }
      }

      #endregion

      #region Device Specific Functions

      protected void Reset(bool fromPowerUp)
      {
         this.resetActive = true;
         this.DeviceStateLabel.Text = "PRE-OP";

         this.baudRateCode = this.nvBaudRateCode;
         this.nodeId = this.nvNodeId;

         this.sdoNodeId = this.nvNodeId;
         this.sdoMode = this.nvMode; 

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Reset(i, this.nodeId);
         }

         this.sdoConsumerHeartbeat = 0;
         this.consumerHeartbeatNode = 0;
         this.consumerHeartbeatTime = 0;
         this.consumerHeartbeatActive = false;

         this.producerHeartbeatTime = 0;

         this.Camera1CameraControl.Running = false;
         this.Camera2CameraControl.Running = false;
         this.Camera3CameraControl.Running = false;
         this.Camera4CameraControl.Running = false;
         this.Camera5CameraControl.Running = false;
         this.Camera6CameraControl.Running = false;
         this.Camera7CameraControl.Running = false;
         this.Camera8CameraControl.Running = false;
         this.Camera9CameraControl.Running = false;
         this.Camera10CameraControl.Running = false;
         this.Camera11CameraControl.Running = false;
         this.Camera12CameraControl.Running = false;

         if (false != fromPowerUp)
         {
            this.Solenoid1SolenoidControl.Running = false;
            this.Solenoid2SolenoidControl.Running = false;
            this.Solenoid3SolenoidControl.Running = false;
            this.Solenoid4SolenoidControl.Running = false;
            this.Solenoid5SolenoidControl.Running = false;
            this.Solenoid6SolenoidControl.Running = false;
            this.Solenoid7SolenoidControl.Running = false;
            this.Solenoid8SolenoidControl.Running = false;
            this.Solenoid9SolenoidControl.Running = false;
            this.Solenoid10SolenoidControl.Running = false;
         }

         this.Mode = this.nvMode;

         if (0 == this.Mode)
         {
            this.Solenoid1SolenoidControl.Title = "FRONT DRILL COVER";
            this.Solenoid2SolenoidControl.Title = "FRONT NOZZLE";
            this.Solenoid3SolenoidControl.Title = "REAR DRILL COVER";
            this.Solenoid4SolenoidControl.Title = "REAR NOZZLE";

            this.FrontDrillSpeedLabel.Visible = true;
            this.FrontDrillSpeedTextBox.Visible = true;
            this.ActualFrontDrillSpeedTextBox.Visible = true;

            this.FrontDrillIndexLabel.Visible = true;
            this.FrontDrillIndexTextBox.Visible = true;
            this.ActualFrontDrillIndexTextBox.Visible = true;

            this.FrontLaserOnOffLabel.Visible = true;
            this.FrontDrillIndexOriginTextBox.Visible = true;

            this.RearDrillSpeedLabel.Visible = true;
            this.RearDrillSpeedTextBox.Visible = true;
            this.ActualRearDrillSpeedTextBox.Visible = true;

            this.RearDrillIndexLabel.Visible = true;
            this.RearDrillIndexTextBox.Visible = true;
            this.ActualRearDrillIndexTextBox.Visible = true;

            this.RearLaserOnOffLabel.Visible = true;
            this.RearDrillIndexOriginTextBox.Visible = true;

            this.SensorIndexLabel.Visible = false;
            this.SensorIndexTextBox.Visible = false;
            this.ActualSensorIndexTextBox.Visible = false;

            this.DrillAutoGroupBox.Visible = true;

            this.drillServoProportionalControlConstant = 0;
            this.drillServoIntegralControlConstant = 0;
            this.drillServoDerivativeControlConstant = 0;
            this.drillServoAcceleration = 2560;
            this.drillServoHomingVelocity = 1310720;
            this.drillServoHomingBackoffCount = 44274;
            this.drillServoTravelVelocity = 2621440;
            this.drillServoErrorLimit = 3000;
            this.drillServoPulsesPerUnit = 885472;

            this.Servo0UserControl.Visible = true;
            this.Servo1UserControl.Visible = true;

            this.Servo0UserControl.Reset();
            this.Servo1UserControl.Reset();

            this.frontDrillContext.state = NicbotBodyDrillStates.idle;
            this.frontDrillContext.manualSetPoint = 0;
            this.frontDrillContext.processedSetPoint = 0;

            this.rearDrillContext.state = NicbotBodyDrillStates.idle;
            this.rearDrillContext.manualSetPoint = 0;
            this.rearDrillContext.processedSetPoint = 0;
         }
         else if (1 == this.Mode)
         {
            this.Solenoid1SolenoidControl.Title = "SENSOR RETRACT";
            this.Solenoid2SolenoidControl.Title = "SENSOR EXTEND";
            this.Solenoid3SolenoidControl.Title = "SENSOR ARM STOW";
            this.Solenoid4SolenoidControl.Title = "SENSOR ARM DEPLOY";

            this.FrontDrillSpeedLabel.Visible = false;
            this.FrontDrillSpeedTextBox.Visible = false;
            this.ActualFrontDrillSpeedTextBox.Visible = false;

            this.FrontDrillIndexLabel.Visible = false;
            this.FrontDrillIndexTextBox.Visible = false;
            this.ActualFrontDrillIndexTextBox.Visible = false;

            this.FrontLaserOnOffLabel.Visible = false;
            this.FrontDrillIndexOriginTextBox.Visible = false;

            this.RearDrillSpeedLabel.Visible = false;
            this.RearDrillSpeedTextBox.Visible = false;
            this.ActualRearDrillSpeedTextBox.Visible = false;

            this.RearDrillIndexLabel.Visible = false;
            this.RearDrillIndexTextBox.Visible = false;
            this.ActualRearDrillIndexTextBox.Visible = false;

            this.RearLaserOnOffLabel.Visible = false;
            this.RearDrillIndexOriginTextBox.Visible = false;

            this.SensorIndexLabel.Visible = true;
            this.SensorIndexTextBox.Visible = true;
            this.ActualSensorIndexTextBox.Visible = true;

            this.DrillAutoGroupBox.Visible = false;

            this.Servo0UserControl.Visible = false;
            this.Servo1UserControl.Visible = false;
         }
         else
         {
            this.Solenoid1SolenoidControl.Title = "";
            this.Solenoid2SolenoidControl.Title = "";
            this.Solenoid3SolenoidControl.Title = "";
            this.Solenoid4SolenoidControl.Title = "";

            this.FrontDrillSpeedLabel.Visible = false;
            this.FrontDrillSpeedTextBox.Visible = false;
            this.ActualFrontDrillSpeedTextBox.Visible = false;

            this.FrontDrillIndexLabel.Visible = false;
            this.FrontDrillIndexTextBox.Visible = false;
            this.ActualFrontDrillIndexTextBox.Visible = false;

            this.FrontLaserOnOffLabel.Visible = false;
            this.FrontDrillIndexOriginTextBox.Visible = false;

            this.RearDrillSpeedLabel.Visible = false;
            this.RearDrillSpeedTextBox.Visible = false;
            this.ActualRearDrillSpeedTextBox.Visible = false;

            this.RearDrillIndexLabel.Visible = false;
            this.RearDrillIndexTextBox.Visible = false;
            this.ActualRearDrillIndexTextBox.Visible = false;

            this.RearLaserOnOffLabel.Visible = false;
            this.RearDrillIndexOriginTextBox.Visible = false;

            this.SensorIndexLabel.Visible = false;
            this.SensorIndexTextBox.Visible = false;
            this.ActualSensorIndexTextBox.Visible = false;

            this.DrillAutoGroupBox.Visible = false;

            this.Servo0UserControl.Visible = false;
            this.Servo1UserControl.Visible = false;
         }

         this.VideoSelectA = 0;
         this.VideoSelectB = 0;

         for (int i = 1; i <= 12; i++)
         {
            this.SetCameraLightLevel(i, 0);
         }

         if (false != fromPowerUp)
         {
            this.SolenoidControl = 0;
         }

         this.drillIndexLimit = 635;

         this.AutoDrillControl = 0;
         this.AutoDrillSearchSpeed = 600;
         this.AutoDrillTravelSpeed = 600;
         this.AutoDrillRotationSpeed = 60;
         this.AutoDrillCuttingSpeed = 600;
         this.AutoDrillCuttingDepth = 0;
         this.AutoDrillPeckCuttingIncrement = 10;
         this.AutoDrillPeckRetractionDistance = 50;
         this.AutoDrillPeckRetractionPosition = 0;

         this.FrontDrillSpeed = 0;
         this.FrontDrillIndex = 0;
         this.RearDrillSpeed = 0;
         this.RearDrillIndex = 0;
         this.SensorIndex = 0;

         this.ActualFrontDrillSpeed = 0;
         this.ActualFrontDrillIndex = 0;
         this.ActualRearDrillSpeed = 0;
         this.ActualRearDrillIndex = 0;
         this.ActualSensorIndex = 0;

         //this.statusAutoDrillOriginFound = false;
         //this.statusAutoDrillCutComplete = false;
         //this.statusAutoDrillOriginHunting = false;
         //this.statusAutoDrillCutActive = false;
         //this.statusAutoDrillCutPaused = false;

         this.AccelerometerX = 0;
         this.AccelerometerY = 0;
         this.AccelerometerZ = 0;

         this.ControlWord = 0;
         this.processedControlWord = 0;
         this.StatusWord = 0;

         this.NodeIdTextBox.Text = this.nodeId.ToString();

         if (false != this.active)
         {
            int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
            byte[] bootUpMsg = new byte[1];
            bootUpMsg[0] = 0;

            this.Transmit(cobId, bootUpMsg);
            base.Reset();
         }

         this.resetActive = false;
      }

      protected override void Start()
      {
         base.Start();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Start();
         }

         this.Camera1CameraControl.Running = true;
         this.Camera2CameraControl.Running = true;
         this.Camera3CameraControl.Running = true;
         this.Camera4CameraControl.Running = true;
         this.Camera5CameraControl.Running = true;
         this.Camera6CameraControl.Running = true;
         this.Camera7CameraControl.Running = true;
         this.Camera8CameraControl.Running = true;
         this.Camera9CameraControl.Running = true;
         this.Camera10CameraControl.Running = true;
         this.Camera11CameraControl.Running = true;
         this.Camera12CameraControl.Running = true;

         this.Solenoid1SolenoidControl.Running = true;
         this.Solenoid2SolenoidControl.Running = true;
         this.Solenoid3SolenoidControl.Running = true;
         this.Solenoid4SolenoidControl.Running = true;
         this.Solenoid5SolenoidControl.Running = true;
         this.Solenoid6SolenoidControl.Running = true;
         this.Solenoid7SolenoidControl.Running = true;
         this.Solenoid8SolenoidControl.Running = true;
         this.Solenoid9SolenoidControl.Running = true;
         this.Solenoid10SolenoidControl.Running = true;

         this.DeviceStateLabel.Text = "RUNNING";

         if (0 == this.Mode)
         {
            this.CheckTpdoMappings(0x2412, 0);
            this.CheckTpdoMappings(0x2414, 0);
         }

         if (1 == this.Mode)
         {
            this.CheckTpdoMappings(0x2415, 0);
         }

         this.frontDrillContext.control |= 0x04;
         this.rearDrillContext.control |= 0x04;

         DateTime now = DateTime.Now;
         this.heartbeatLimit = now.AddMilliseconds(this.producerHeartbeatTime);
         this.speedUpdateTime = now.AddMilliseconds(100);
         this.indexUpdateTime = now.AddMilliseconds(100);
         this.indexLastUpdateTime = now;
      }

      protected override void Stop()
      {
         base.Stop();

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i].Stop();
         }

         this.Camera1CameraControl.Running = false;
         this.Camera2CameraControl.Running = false;
         this.Camera3CameraControl.Running = false;
         this.Camera4CameraControl.Running = false;
         this.Camera5CameraControl.Running = false;
         this.Camera6CameraControl.Running = false;
         this.Camera7CameraControl.Running = false;
         this.Camera8CameraControl.Running = false;
         this.Camera9CameraControl.Running = false;
         this.Camera10CameraControl.Running = false;
         this.Camera11CameraControl.Running = false;
         this.Camera12CameraControl.Running = false;

         this.Solenoid1SolenoidControl.Running = false;
         this.Solenoid2SolenoidControl.Running = false;
         this.Solenoid3SolenoidControl.Running = false;
         this.Solenoid4SolenoidControl.Running = false;
         this.Solenoid5SolenoidControl.Running = false;
         this.Solenoid6SolenoidControl.Running = false;
         this.Solenoid7SolenoidControl.Running = false;
         this.Solenoid8SolenoidControl.Running = false;
         this.Solenoid9SolenoidControl.Running = false;
         this.Solenoid10SolenoidControl.Running = false;
         
         this.DeviceStateLabel.Text = "STOPPED";
      }

      protected override bool LoadDeviceData(UInt16 index, byte subIndex, byte[] buffer, ref UInt32 dataLength)
      {
         bool valid = false;

         if (0x1000 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.deviceType);
            valid = true;
         }
         else if (0x1001 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.errorStatus);
            valid = true;
         }
         else if (0x1008 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.deviceName);
            valid = true;
         }
         else if (0x100A == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.version);
            valid = true;
         }
         else if ((0x1016 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x1016 == index) && (1 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.sdoConsumerHeartbeat);
            valid = true;            
         }
         else if (0x1017 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.producerHeartbeatTime);
            valid = true;
         }
         else if ((0x1018 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x1018 == index) && (1 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (UInt32)0);
            valid = true;
         }
         else if ((0x1800 <= index) && (0x1803 >= index))
         {
            int offset = (index - 0x1800);
            this.tpdoMapping[offset].LoadParameterData(subIndex, buffer, ref dataLength);
            valid = true;
         }
         else if ((0x1A00 <= index) && (0x1A03 >= index))
         {
            int offset = (index - 0x1A00);
            this.tpdoMapping[offset].LoadMapData(subIndex, buffer, ref dataLength);
            valid = true;
         }
         else if (0x2100 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.baudRateCode);
            valid = true;
         }
         else if (0x2101 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.sdoNodeId);
            valid = true;
         }
         else if (0x2102 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.sdoMode);
            valid = true;
         }
         else if (0x2105 == index)
         {
            dataLength = this.MoveDeviceData(buffer, 0x65766173);
            valid = true;
         }
         else if ((0x2301 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if ((0x2301 == index) && (1 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.VideoSelectA);
            valid = true;
         }
         else if ((0x2301 == index) && (2 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.VideoSelectB);
            valid = true;
         }
         else if ((0x2303 == index) && (0 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)12);
            valid = true;
         }
         else if ((0x2303 == index) && (1 <= subIndex) && (12 >= subIndex))
         {
            int cameraId = subIndex;
            dataLength = this.MoveDeviceData(buffer, this.GetCameraLightLevel(cameraId));
            valid = true;
         }
         else if (0x2304 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.SolenoidControl);
            valid = true;
         }
         else if (0x2311 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.FrontDrillSpeed);
               valid = true;
            }
         }
         else if (0x2312 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.FrontDrillIndex);
               valid = true;
            }
         }
         else if (0x2313 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.RearDrillSpeed);
               valid = true;
            }
         }
         else if (0x2314 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.RearDrillIndex);
               valid = true;
            }
         }
         else if (0x2315 == index)
         {
            if (1 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.SensorIndex);
               valid = true;
            }
         }
         else if (0x2322 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillIndexLimit);
               valid = true;
            }
         }
         else if (0x2324 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillIndexLimit);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (0 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, (byte)9);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (1 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillControl);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (2 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillSearchSpeed);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (3 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillTravelSpeed);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (4 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillRotationSpeed);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (5 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillCuttingSpeed);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (6 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillCuttingDepth);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (7 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillPeckCuttingIncrement);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (8 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillPeckRetractionDistance);
               valid = true;
            }
         }
         else if ((0x2331 == index) && (9 == subIndex))
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.AutoDrillPeckRetractionPosition);
               valid = true;
            }
         }
         else if (0x233D == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoProportionalControlConstant);
               valid = true;
            }
         }
         else if (0x233E == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoIntegralControlConstant);
               valid = true;
            }
         }
         else if (0x233F == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoDerivativeControlConstant);
               valid = true;
            }
         }
         else if (0x2340 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoAcceleration);
               valid = true;
            }
         }
         else if (0x2341 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoHomingVelocity);
               valid = true;
            }
         }
         else if (0x2342 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoHomingBackoffCount);
               valid = true;
            }
         }
         else if (0x2343 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoTravelVelocity);
               valid = true;
            }
         }
         else if (0x2344 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoErrorLimit);
               valid = true;
            }
         }
         else if (0x2345 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.drillServoPulsesPerUnit);
               valid = true;
            }
         }
         else if (0x2346 == index)
         {
            if (0 == this.Mode)
            {
               buffer[0] = 0;// servo_get_status(0);
               dataLength = 1;
            }
         }
         else if (0x2347 == index)
         {
            if (0 == this.Mode)
            {
               buffer[0] = 0;// servo_get_status(1);
               dataLength = 1;
            }
         }
         else if (0x2411 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.actualFrontDrillSpeed);
               valid = true;
            }
         }
         else if (0x2412 == index)
         {
            if (0 == this.Mode)
            {
               UInt16 actualDrillIndexValue = (UInt16)this.ActualFrontDrillIndex;
               dataLength = this.MoveDeviceData(buffer, actualDrillIndexValue);
               valid = true;
            }
         }
         else if (0x2413 == index)
         {
            if (0 == this.Mode)
            {
               dataLength = this.MoveDeviceData(buffer, this.actualRearDrillSpeed);
               valid = true;
            }
         }
         else if (0x2414 == index)
         {
            if (0 == this.Mode)
            {
               UInt16 actualDrillIndexValue = (UInt16)this.ActualRearDrillIndex;
               dataLength = this.MoveDeviceData(buffer, actualDrillIndexValue);
               valid = true;
            }
         }
         else if (0x2415 == index)
         {
            if (1 == this.Mode)
            {
               UInt16 actualSensorIndexValue = (UInt16)this.ActualSensorIndex;
               dataLength = this.MoveDeviceData(buffer, actualSensorIndexValue);
               valid = true;
            }
         }
         else if (0x2441 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.AccelerometerX);
            valid = true;
         }
         else if (0x2442 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.AccelerometerY);
            valid = true;
         }
         else if (0x2443 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.AccelerometerZ);
            valid = true;
         }
         else if (0x2500 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.ControlWord);
            valid = true;
         }
         else if (0x2501 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.StatusWord);
            valid = true;
         }

         return (valid);
      }

      protected override bool EvaluateDeviceDataSize(UInt16 index, byte subIndex, UInt32 dataLength)
      {
         bool valid = false;

         return (valid);
      }

      protected override bool StoreDeviceData(UInt16 index, byte subIndex, byte[] buffer, int offset, UInt32 length)
      {
         bool valid = false;

         if ((0x1016 == index) && (1 == subIndex))
         {
            if (4 == length)
            {
               this.sdoConsumerHeartbeat = BitConverter.ToUInt32(buffer, offset);

               this.consumerHeartbeatNode = (byte)((this.sdoConsumerHeartbeat >> 16) & 0x7F);
               this.consumerHeartbeatTime = (UInt16)(this.sdoConsumerHeartbeat & 0xFFFF);
               this.consumerHeartbeatActive = false;

               valid = true;
            }
         }
         else if (0x1017 == index)
         {
            if (2 == length)
            {
               this.producerHeartbeatTime = BitConverter.ToUInt16(buffer, offset);
               this.heartbeatLimit = this.heartbeatLimit.AddMilliseconds(this.producerHeartbeatTime);
               valid = true;
            }
         }
         else if ((0x1800 <= index) && (0x1803 >= index))
         {
            int pdoMapOffset = (index - 0x1800);
            UInt32 value = this.ExtractUInt32(buffer, offset, length);
            valid = this.tpdoMapping[pdoMapOffset].StoreParameterData(subIndex, (int)length, value);
         }
         else if ((0x1A00 <= index) && (0x1A03 >= index))
         {
            int pdoMapOffset = (index - 0x1A00);
            UInt32 value = this.ExtractUInt32(buffer, offset, length);
            valid = this.tpdoMapping[pdoMapOffset].StoreMapData(subIndex, (int)length, value);
         }
         else if (0x2100 == index)
         {
            if (1 == length)
            {
               this.baudRateCode = buffer[offset];
               valid = true;
            }
         }
         else if (0x2101 == index)
         {
            if (1 == length)
            {
               this.sdoNodeId = buffer[offset];
               this.NodeIdTextBox.Text = this.nodeId.ToString();
               valid = true;
            }
         }
         else if (0x2102 == index)
         {
            if (1 == length)
            {
               this.sdoMode = buffer[offset];
               valid = true;
            }
         }
         else if (0x2105 == index)
         {
            UInt32 value = BitConverter.ToUInt32(buffer, offset);

            if (0x65766173 == value)
            {
               this.nvBaudRateCode = this.baudRateCode;
               this.nvNodeId = this.sdoNodeId;
               this.nvMode = this.sdoMode;

               valid = true;
            }
         }
         else if ((0x2301 == index) && (1 == subIndex))
         {
            if (1 == length)
            {
               byte value = buffer[offset];

               if (value <= 12)
               {
                  this.VideoSelectA = value;
                  valid = true;
               }
            }
         }
         else if ((0x2301 == index) && (2 == subIndex))
         {
            if (1 == length)
            {
               byte value = buffer[offset];

               if (value <= 12)
               {
                  this.VideoSelectB = value;
                  valid = true;
               }
            }
         }
         else if ((0x2303 == index) && (1 <= subIndex) && (12 >= subIndex))
         {
            int cameraId = subIndex;
            this.SetCameraLightLevel(cameraId, buffer[offset]);
            valid = true;
         }
         else if (0x2304 == index)
         {
            if (2 == length)
            {
               this.SolenoidControl = BitConverter.ToUInt16(buffer, offset);
               valid = true;
            }
         }
         else if (0x2311 == index)
         {
            if (0 == this.Mode)
            {
               if (2 == length)
               {
                  this.FrontDrillSpeed = BitConverter.ToInt16(buffer, offset);
                  valid = true;
               }
            }
         }
         else if (0x2312 == index)
         {
            if (0 == this.Mode)
            {
               if (2 == length)
               {
                  Int16 value = BitConverter.ToInt16(buffer, offset);

                  if (value <= this.drillIndexLimit)
                  {
                     this.FrontDrillIndex = value;
                     valid = true;
                  }
               }
            }
         }
         else if (0x2313 == index)
         {
            if (0 == this.Mode)
            {
               if (2 == length)
               {
                  this.RearDrillSpeed = BitConverter.ToInt16(buffer, offset);
                  valid = true;
               }
            }
         }
         else if (0x2314 == index)
         {
            if (0 == this.Mode)
            {
               if (2 == length)
               {
                  Int16 value = BitConverter.ToInt16(buffer, offset);

                  if (value <= this.drillIndexLimit)
                  {
                     this.RearDrillIndex = value;
                     valid = true;
                  }
               }
            }
         }
         else if (0x2315 == index)
         {
            if (1 == this.Mode)
            {
               if (2 == length)
               {
                  this.SensorIndex = BitConverter.ToUInt16(buffer, offset);
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (1 == subIndex))
         {
            if ((0 == this.Mode) &&
                (1 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               this.AutoDrillControl = buffer[offset];
               valid = true;
            }
         }
         else if ((0x2331 == index) && (2 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 searchSpeed = BitConverter.ToUInt16(buffer, offset);

               if ((searchSpeed > 0) && (searchSpeed <= (3 * this.drillIndexLimit)))
               {
                  this.AutoDrillSearchSpeed = searchSpeed;
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (3 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 travelSpeed = BitConverter.ToUInt16(buffer, offset);

               if ((travelSpeed > 0) && (travelSpeed <= (3 * this.drillIndexLimit)))
               {
                  this.AutoDrillTravelSpeed = travelSpeed;
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (4 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 rotationSpeed = BitConverter.ToUInt16(buffer, offset);

               if ((rotationSpeed > 0) && (rotationSpeed <= 500))
               {
                  this.AutoDrillRotationSpeed = rotationSpeed;
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (5 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 cuttingSpeed = BitConverter.ToUInt16(buffer, offset);

               if ((cuttingSpeed > 0) && (cuttingSpeed <= (3 * this.drillIndexLimit)))
               {
                  this.AutoDrillCuttingSpeed = cuttingSpeed;
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (6 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 cuttingDepth = BitConverter.ToUInt16(buffer, offset);

               if ((cuttingDepth > 0) && (cuttingDepth <= this.drillIndexLimit))
               {
                  this.AutoDrillCuttingDepth = cuttingDepth;
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (7 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 peckIncrement = BitConverter.ToUInt16(buffer, offset);

               if ((peckIncrement > 0) && (peckIncrement <= this.drillIndexLimit))
               {
                  this.AutoDrillPeckCuttingIncrement = peckIncrement;
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (8 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 retractionDistance = BitConverter.ToUInt16(buffer, offset);

               if ((retractionDistance > 0) && (retractionDistance <= this.drillIndexLimit))
               {
                  this.AutoDrillPeckRetractionDistance = retractionDistance;
                  valid = true;
               }
            }
         }
         else if ((0x2331 == index) && (9 == subIndex))
         {
            if ((0 == this.Mode) &&
                (2 == length) &&
                (false == this.controlLocateOrigin) &&
                (false == this.controlAutoCut))
            {
               UInt16 retractionPosition = BitConverter.ToUInt16(buffer, offset);

               if ((retractionPosition >= 0) && (retractionPosition <= this.drillIndexLimit))
               {
                  this.AutoDrillPeckRetractionPosition = retractionPosition;
                  valid = true;
               }
            }
         }
         else if (0x233D == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoProportionalControlConstant = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x233E == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoIntegralControlConstant = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x233F == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoDerivativeControlConstant = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x2340 == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoAcceleration = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x2341 == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoHomingVelocity = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x2342 == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoHomingBackoffCount = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x2343 == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoTravelVelocity = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x2344 == index)
         {
            if ((0 == this.Mode) && (2 == length))
            {
               this.drillServoErrorLimit = BitConverter.ToUInt16(buffer, offset);
               valid = true;
               // set servos
            }
         }
         else if (0x2345 == index)
         {
            if ((0 == this.Mode) && (4 == length))
            {
               this.drillServoPulsesPerUnit = BitConverter.ToUInt32(buffer, offset);
               valid = true;
            }
         }
         else if (0x2500 == index)
         {
            if (2 == length)
            {
               this.ControlWord = BitConverter.ToUInt16(buffer, offset);
               valid = true;
            }
         }

         return (valid);
      }

      #endregion

      #region Message Process Functions

      private void ProcessNetworkMessage(byte[] frame)
      {
         if (frame.Length >= 2)
         {
            if ((frame[1] == 0) || (frame[1] == this.nodeId))
            {
               if (0x81 == frame[0])
               {
                  this.Reset(false);
               }
               else if (0x01 == frame[0])
               {
                  this.Start();
               }
               else if (0x02 == frame[0])
               {
                  this.Stop();
               }
            }
         }
      }

      private void ProcessSdoMessage(byte[] frame)
      {
         int css = (int)((frame[0] >> 5) & 0x7);

         if (0 == css)
         {
            // download SDO segment

            this.ProcessDownload(this.nodeId, frame);
         }
         else if (1 == css)
         {
            // initiate SDO download

            this.InitiateDownload(this.nodeId, frame);
         }
         else if (2 == css)
         {
            // initiate SDO upload

            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];

            this.InitiateUpload(this.nodeId, index, subIndex);
         }
         else if (3 == css)
         {
            // upload SDO segment

            bool t = (((frame[0] >> 4) & 0x1) != 0) ? true : false;
            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];

            this.ProcessUpload(this.nodeId, t, index, subIndex);
         }
         else if (4 == css)
         {
            // abort

            UInt16 index = BitConverter.ToUInt16(frame, 1);
            byte subIndex = frame[3];
            UInt32 code = BitConverter.ToUInt32(frame, 4);

            this.AbortTransfer(this.nodeId, index, subIndex, code);
         }
      }

      private void ProcessPdo1Message(byte[] frame)
      {
      }

      private void ProcessPdo2Message(byte[] frame)
      {
      }

      private void ProcessPdo3Message(byte[] frame)
      {
      }

      private void ProcessPdo4Message(byte[] frame)
      {
      }

      #endregion

      #region User Events

      private void SetAccelerometerButton_Click(object sender, EventArgs e)
      {
         UInt16 accelerometerX = 0;
         UInt16 accelerometerY = 0;
         UInt16 accelerometerZ = 0;

         if (UInt16.TryParse(this.AccelerometerXSetpointTextBox.Text, out accelerometerX) != false)
         {
            this.AccelerometerX = accelerometerX;
         }

         if (UInt16.TryParse(this.AccelerometerYSetpointTextBox.Text, out accelerometerY) != false)
         {
            this.AccelerometerY = accelerometerY;
         }

         if (UInt16.TryParse(this.AccelerometerZSetpointTextBox.Text, out accelerometerZ) != false)
         {
            this.AccelerometerZ = accelerometerZ;
         }
      }

      #endregion

      #region Constructor

      public UlcRoboticsNicbotBody()
         : base()
      {
         this.InitializeComponent();
         
         this.randomValue = new Random();

         this.deviceType = 0x00003010;
         this.errorStatus = 0;
         this.deviceName = "NICBOT Body";
         this.version = "v1.00";

         this.nvBaudRateCode = 3;
         this.nvNodeId = 32;

         this.tpdoMapping = new TPDOMapping[4];

         for (int i = 0; i < 4; i++)
         {
            this.tpdoMapping[i] = new TPDOMapping();
            this.tpdoMapping[i].OnPdoMappable = new TPDOMapping.PdoMappableHandler(this.TPdoMappableHandler);
            this.tpdoMapping[i].OnPdoSize = new TPDOMapping.PdoSizeHandler(this.TPdoSizeHandler);
            this.tpdoMapping[i].OnPdoData = new TPDOMapping.PdoDataHandler(this.TPdoDataHandler);
         }

         this.NodeIdTextBox.Text = this.nvNodeId.ToString();

         this.frontDrillContext = new NicbotBodyDrillContext();
         this.frontDrillContext.OnLaser = new NicbotBodyDrillContext.LaserHandler(this.FrontLaserControl);
         this.frontDrillContext.retractMask = 0x0400;
         this.frontDrillContext.axis = 1;

         this.rearDrillContext = new NicbotBodyDrillContext();
         this.rearDrillContext.OnLaser = new NicbotBodyDrillContext.LaserHandler(this.RearLaserControl);
         this.rearDrillContext.retractMask = 0x0100;
         this.rearDrillContext.axis = 0;
      }

      #endregion

      #region Access Functions

      public override void LoadRegistry(RegistryKey appKey, string deviceTag)
      {
         object keyValue;
         byte parsedValue = 0;

         keyValue = appKey.GetValue(deviceTag + "Enabled");
         this.EnabledCheckBox.Checked = ("0" != (string)((null != keyValue) ? keyValue.ToString() : "0")) ? true : false;

         keyValue = appKey.GetValue(deviceTag + "Description");
         this.DescriptionTextBox.Text = (null != keyValue) ? keyValue.ToString() : "";

         keyValue = appKey.GetValue(deviceTag + "BusId");
         string busId = (null != keyValue) ? keyValue.ToString() : "";
         this.SetBusId(busId);

         keyValue = appKey.GetValue(deviceTag + "nvBaudRateCode");
         this.nvBaudRateCode = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 6) : 6);

         keyValue = appKey.GetValue(deviceTag + "nvNodeId");
         this.nvNodeId = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 31) : 31);
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();

         keyValue = appKey.GetValue(deviceTag + "nvMode");
         this.nvMode = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 0) : 0);
         this.Mode = this.nvMode;

         keyValue = appKey.GetValue(deviceTag + "FrontDrillIndexOrigin");
         this.FrontDrillIndexOriginTextBox.Text = (null != keyValue) ? keyValue.ToString() : "10";

         keyValue = appKey.GetValue(deviceTag + "RearDrillIndexOrigin");
         this.RearDrillIndexOriginTextBox.Text = (null != keyValue) ? keyValue.ToString() : "10";
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());
         appKey.SetValue(deviceTag + "nvBaudRateCode", this.nvBaudRateCode);
         appKey.SetValue(deviceTag + "nvNodeId", this.nvNodeId);
         appKey.SetValue(deviceTag + "nvMode", this.nvMode);
         appKey.SetValue(deviceTag + "FrontDrillIndexOrigin", this.FrontDrillIndexOriginTextBox.Text);
         appKey.SetValue(deviceTag + "RearDrillIndexOrigin", this.RearDrillIndexOriginTextBox.Text);
      }

      public override void Read(XmlReader reader)
      {
         string name = reader.Name;
         reader.Read();

         if ("Enabled" == name)
         {
            this.EnabledCheckBox.Checked = ("0" != reader.Value) ? true : false;
         }
         else if ("Description" == name)
         {
            this.DescriptionTextBox.Text = reader.Value;
         }
         else if ("BusId" == name)
         {
            this.SetBusId(reader.Value);
         }
         else if ("nvBaudRateCode" == name)
         {
            byte.TryParse(reader.Value, out this.nvBaudRateCode);
         }
         else if ("nvNodeId" == name)
         {
            byte.TryParse(reader.Value, out this.nvNodeId);
            this.NodeIdTextBox.Text = this.nvNodeId.ToString();
         }
         else if ("nvMode" == name)
         {
            byte.TryParse(reader.Value, out this.nvMode);
            this.Mode = this.nvMode;
         }
         else if ("FrontDrillIndexOrigin" == name)
         {
            this.FrontDrillIndexOriginTextBox.Text = reader.Value;
         }
         else if ("RearDrillIndexOrigin" == name)
         {
            this.RearDrillIndexOriginTextBox.Text = reader.Value;
         }
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("NodeId", this.NodeIdTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());
         writer.WriteElementString("nvBaudRateCode", this.nvBaudRateCode.ToString());
         writer.WriteElementString("nvNodeId", this.nvNodeId.ToString());
         writer.WriteElementString("nvMode", this.nvMode.ToString());
         writer.WriteElementString("FrontDrillIndexOrigin", this.FrontDrillIndexOriginTextBox.Text);
         writer.WriteElementString("RearDrillIndexOrigin", this.RearDrillIndexOriginTextBox.Text);
      }

      public override void SetBusId(string busId)
      {
         this.BusIdTextBox.Text = busId;
         base.SetBusId(busId);
      }

      public override void PowerUp()
      {
         this.EnabledCheckBox.Enabled = false;
         this.DescriptionTextBox.Enabled = false;
         this.NodeIdTextBox.Enabled = false;

         this.active = this.EnabledCheckBox.Checked;
         this.Reset(true);

         if (false == this.active)
         {
            this.DeviceStateLabel.Text = "DISABLED";
         }
      }

      public override void PowerDown()
      {
         this.active = false;

         this.EnabledCheckBox.Enabled = true;
         this.DescriptionTextBox.Enabled = true;
         this.NodeIdTextBox.Enabled = true;

         this.deviceState = DeviceStates.stopped;
         this.DeviceStateLabel.Text = "OFF";
      }

      public override void DeviceReceive(int cobId, byte[] frame)
      {
         if (false != this.active)
         {
            COBTypes frameType = (COBTypes)((cobId >> 7) & 0xF);
            int nodeId = (int)(cobId & 0x7F);

            if (COBTypes.NMT == frameType)
            {
               if ((0 == nodeId) || (nodeId == this.nodeId))
               {
                  this.ProcessNetworkMessage(frame);
               }
            }
            else if (COBTypes.RSDO == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.stopped != this.deviceState))
               {
                  this.ProcessSdoMessage(frame);
               }
            }
            else if (COBTypes.RPDO1 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo1Message(frame);
               }
            }
            else if (COBTypes.RPDO2 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo2Message(frame);
               }
            }
            else if (COBTypes.RPDO3 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo3Message(frame);
               }
            }
            else if (COBTypes.RPDO4 == frameType)
            {
               if ((nodeId == this.nodeId) && (DeviceStates.running == this.deviceState))
               {
                  this.ProcessPdo4Message(frame);
               }
            }
            else if (COBTypes.ERROR == frameType)
            {
               if ((0 != this.sdoConsumerHeartbeat) &&
                   (nodeId == this.consumerHeartbeatNode))
               {
                  this.consumerHeartbeatActive = true;
                  this.consumerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.consumerHeartbeatTime);
               }
            }
         }
      }

      public override void UpdateDevice()
      {
         DateTime now = DateTime.Now;

         if ((false != this.consumerHeartbeatActive) &&
             (now > this.consumerHeartbeatTimeLimit) &&
             (DeviceStates.preop != this.deviceState))
         {
            this.Reset(false);
         }

         if (0 != this.producerHeartbeatTime)
         {
            if (now > this.heartbeatLimit)
            {
               this.heartbeatLimit = this.heartbeatLimit.AddMilliseconds(this.producerHeartbeatTime);

               int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
               byte[] heartbeatMsg = new byte[1];

               if (DeviceStates.preop == this.deviceState)
               {
                  heartbeatMsg[0] = 0x7F;
               }
               else if (DeviceStates.running == this.deviceState)
               {
                  heartbeatMsg[0] = 0x05;
               }
               else
               {
                  heartbeatMsg[0] = 0x04;
               }

               this.Transmit(cobId, heartbeatMsg);
            }
         }

         this.UpdateActualDrillPosition();

         if (DeviceStates.running == this.deviceState)
         {
            for (int i = 0; i < 4; i++)
            {
               int cobId = 0;
               byte[] frame = null;
               this.tpdoMapping[i].Update(ref cobId, ref frame);

               if (null != frame)
               {
                  bool transmitted = this.Transmit(cobId, frame);

                  if (false != transmitted)
                  {
                     this.tpdoMapping[i].Transmitted();
                  }
               }
            }

            if (0 == this.Mode)
            {
               if (this.processedControlWord != this.ControlWord)
               {
                  this.processedControlWord = this.ControlWord;
                  this.frontDrillContext.control = (byte)(this.ControlWord & 0xFF);
                  this.rearDrillContext.control = (byte)((this.ControlWord >> 8) & 0xFF);
               }
            }

            if (now > this.speedUpdateTime)
            {
               this.speedUpdateTime = now.AddMilliseconds(100);

               if (0 == this.Mode)
               {
                  Int16 frontSpeed = this.ActualFrontDrillSpeed;
                  this.UpdateSpeed(ref frontSpeed, this.frontDrillSpeed);
                  this.ActualFrontDrillSpeed = frontSpeed;

                  Int16 rearSpeed = this.ActualRearDrillSpeed;
                  this.UpdateSpeed(ref rearSpeed, this.rearDrillSpeed);
                  this.ActualRearDrillSpeed = rearSpeed;
               }
            }

            if (now > this.indexUpdateTime)
            {
               TimeSpan ts = now - this.indexLastUpdateTime;
               this.indexLastUpdateTime = now;
               double elapsedSeconds = ts.TotalSeconds;
               this.indexUpdateTime = now.AddMilliseconds(100);

               if (0 == this.Mode)
               {
                  this.Servo0UserControl.Update(elapsedSeconds);
                  this.Servo1UserControl.Update(elapsedSeconds);

                  this.UpdateDrill(this.frontDrillContext, elapsedSeconds);
                  this.UpdateDrill(this.rearDrillContext, elapsedSeconds);


#if false
                  if (this.controlLocation == ToolLocations.front)
                  {
                     // process front tools

                     UInt16 drillIndexSetPoint = this.FrontDrillIndex;
                     double actualDrillIndex = this.ActualFrontDrillIndex;
                     this.UpdateTools(this.frontDrillOrigin, ref drillIndexSetPoint, ref actualDrillIndex, elapsedSeconds);
                     this.FrontDrillIndex = drillIndexSetPoint;
                     this.ActualFrontDrillIndex = actualDrillIndex;                     

                     double rearDrillIndex = this.ActualRearDrillIndex;
                     this.UpdateIndex(ref rearDrillIndex, 0, this.AutoDrillTravelSpeed, elapsedSeconds);
                     this.ActualRearDrillIndex = rearDrillIndex;
                  }

                  if (this.controlLocation == ToolLocations.rear)
                  {
                     // process front tools

                     UInt16 drillIndexSetPoint = this.RearDrillIndex;
                     double actualDrillIndex = this.ActualRearDrillIndex;
                     this.UpdateTools(this.rearDrillOrigin, ref drillIndexSetPoint, ref actualDrillIndex, elapsedSeconds);
                     this.RearDrillIndex = drillIndexSetPoint;
                     this.ActualRearDrillIndex = actualDrillIndex;

                     double frontDrillIndex = this.ActualFrontDrillIndex;
                     this.UpdateIndex(ref frontDrillIndex, 0, this.AutoDrillTravelSpeed, elapsedSeconds);
                     this.ActualFrontDrillIndex = frontDrillIndex;
                  }
#endif
               }

               if (1 == this.Mode)
               {
                  double sensorIndex = this.ActualSensorIndex;
                  this.UpdateIndex(ref sensorIndex, this.sensorIndex, this.AutoDrillTravelSpeed, elapsedSeconds);
                  this.ActualSensorIndex = sensorIndex;
               }
              
               this.StatusWord = this.GetStatus();
            }
         }
         else
         {
            if (now > this.speedUpdateTime)
            {
               this.speedUpdateTime = now.AddMilliseconds(100);

               if (0 == this.Mode)
               {
                  Int16 frontSpeed = this.ActualFrontDrillSpeed;
                  this.UpdateSpeed(ref frontSpeed, 0);
                  this.ActualFrontDrillSpeed = frontSpeed;

                  Int16 rearSpeed = this.ActualRearDrillSpeed;
                  this.UpdateSpeed(ref rearSpeed, 0);
                  this.ActualRearDrillSpeed = rearSpeed;
               }
            }

            this.FrontLaserOnOffLabel.Text = "off";
            this.FrontLaserOnOffLabel.BackColor = Color.DarkSlateGray;

            this.RearLaserOnOffLabel.Text = "off";
            this.RearLaserOnOffLabel.BackColor = Color.DarkSlateGray;
         }
      }

      #endregion

   }
}
