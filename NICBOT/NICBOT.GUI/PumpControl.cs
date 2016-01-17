namespace NICBOT.GUI
{
   using System;

   using NICBOT.Utilities;

   public class PumpControl
   {
      #region Definition

      public enum States
      {
         stopped,
         running,
         pausedWaitStop,
         paused,
         setManual,
         manualSpeed,
         manualPressure,
         manualRelief,
         manualStop,
         manualWaitStop,
      }

      private enum AutoSteps
      {
         start,
         extend,
         waitExtend,
         pumpStart,
         waitSetPoint,
         pumpStop,
         waitpumpStop,
         relieve,
         waitRelieve,
         retract,
         waitRetract,
         complete,
      }

      #endregion

      #region Fields

      private static PumpControl front = null;
      private static PumpControl rear = null;

      private ToolLocations location;
      private PumpParameters parameters;
      private States state;

      private bool autoActivated;
      private bool autoPaused;
      private AutoSteps autoStep;
      private DateTime autoTimeLimit;

      private bool manualControl;
      private bool manualPressureRelief;
      private bool manualPressureReliefStopping;
      private PumpModes manualMode;
      private PumpDirections manualDirection;
      private double manualSetPoint;
      
      private double pressureReading; // feedback from 0 to 10 volt input
      private double measuredVolume; // accumulated based on pump speed and time
      private double speedReading; // feedback from 0 to 10 volt input
      private DateTime lastVolumeUpdate;

      private PumpModes mode;
      private double setPoint;
      private bool on; // true sets digital output 1, false clears digital output 1
      private PumpDirections direction;
      private double speedSetting;

      private PumpAutoStates autoState;

      #endregion

      #region Properties

      public static PumpControl Front
      {
         get
         {
            if (null == front)
            {
               front = new PumpControl(ToolLocations.front);
               front.Initialize();
            }

            return front;
         }
      }

      public static PumpControl Rear
      {
         get
         {
            if (null == rear)
            {
               rear = new PumpControl(ToolLocations.rear);
               rear.Initialize();
            }

            return rear;
         }
      }

      #endregion

      #region Constructor

      private PumpControl(ToolLocations location)
      {
         this.location = location;
         this.parameters = (ToolLocations.front == this.location) ? ParameterAccessor.Instance.FrontPump : ParameterAccessor.Instance.RearPump;
      }

      #endregion

      #region Access Methods
      
      public void Initialize()
      {
         this.state = States.stopped;

         this.autoActivated = false;
         this.autoPaused = false;
         this.autoStep = AutoSteps.complete;

         this.manualControl = false;
         this.manualPressureRelief = false;
         this.manualPressureReliefStopping = false;
         this.manualMode = PumpModes.speed;
         this.manualDirection = PumpDirections.forward;
         this.manualSetPoint = 0;

         this.mode = PumpModes.speed;
         this.setPoint = 0;
         this.on = false;
         this.direction = PumpDirections.forward;
         this.speedSetting = 0;

         this.autoState = PumpAutoStates.off;
      }

      public double GetVolumePerSecond()
      {
         double volumePerRotation = 1000 / this.parameters.FlowConstant.OperationalValue;
         double volumePerMinute = volumePerRotation * this.speedReading;
         double volumePerSecond = volumePerMinute / 60;

         return (volumePerSecond);
      }

      public void Update(double pressureReading, double speedReading)
      {
         States processedState;

         this.pressureReading = pressureReading;
         this.speedReading = speedReading;

         DateTime now = DateTime.Now;
         TimeSpan ts = now - this.lastVolumeUpdate;
         this.lastVolumeUpdate = now;
         
         bool nozzleExteded = NicBotComm.Instance.GetNozzleExtended(this.location);

         if (false != nozzleExteded)
         {
            double volumePerSecond = this.GetVolumePerSecond();
            double addedVolume = ts.TotalSeconds * volumePerSecond;
            this.measuredVolume += addedVolume;
         }

         do 
         {
            processedState = this.state;

            switch (this.state)
            {
               #region case States.stopped:
               case States.stopped:
               {
                  if (false != this.autoActivated)
                  {
                     this.mode = (false != this.parameters.PressureAutoFill) ? PumpModes.pressure : PumpModes.volume;
                     this.setPoint = (false != this.parameters.PressureAutoFill) ? this.parameters.AutoFillPressure.OperationalValue : this.parameters.AutoFillVolume.OperationalValue;
                     this.autoState = PumpAutoStates.running;
                     this.autoStep = AutoSteps.start;
                     this.state = States.running;
                  }
                  else if ((false != manualControl) || (false != this.manualPressureRelief))
                  {
                     this.state = States.setManual;
                  }
                  else
                  {
                     this.mode = (false != this.parameters.PressureAutoFill) ? PumpModes.pressure : PumpModes.volume;
                     this.setPoint = (false != this.parameters.PressureAutoFill) ? this.parameters.AutoFillPressure.OperationalValue : this.parameters.AutoFillVolume.OperationalValue;
                  }

                  break;
               }
               #endregion
               #region case States.running:
               case States.running:
               {
                  if (false != this.autoPaused)
                  {
                     this.on = false;
                     this.speedSetting = 0;
                     this.state = States.pausedWaitStop;
                     this.autoState = PumpAutoStates.paused;
                  }
                  else if (false != this.autoActivated)
                  {
                     AutoSteps processedAutoStep;

                     #region Automatic Process
                     do
                     {
                        processedAutoStep = this.autoStep;

                        switch (this.autoStep)
                        {
                           case AutoSteps.start:
                           {
                              this.autoStep = AutoSteps.extend;
                              break;
                           }
                           case AutoSteps.extend:
                           {
                              if (false == nozzleExteded)
                              {
                                 NicBotComm.Instance.SetNozzleExtend(this.location, true);
                                 this.autoTimeLimit = DateTime.Now.AddMilliseconds(500);
                                 this.autoStep = AutoSteps.waitExtend;
                              }
                              else
                              {
                                 this.autoStep = AutoSteps.pumpStart;
                              }

                              break;
                           }
                           case AutoSteps.waitExtend:
                           {
                              if (DateTime.Now > this.autoTimeLimit)
                              {
                                 this.autoStep = AutoSteps.pumpStart;
                              } 
                              
                              break;
                           }
                           case AutoSteps.pumpStart:
                           {
                              this.on = true;
                              this.direction = PumpDirections.forward;
                              this.speedSetting = this.parameters.ForwardSpeed.OperationalValue;
                              this.autoStep = AutoSteps.waitSetPoint;

                              break;
                           }
                           case AutoSteps.waitSetPoint:
                           {
                              if (PumpModes.volume == this.mode)
                              {
                                 // speed = percentage of difference
                                 double volumePerSecond = this.GetVolumePerSecond();
                                 double remainingVolume = this.setPoint - this.measuredVolume;
                                 double secondsRemaining = remainingVolume / volumePerSecond;
                                 double adjustedSetupPoint = this.setPoint - (volumePerSecond / 3);

                                 if (secondsRemaining < 0.7)
                                 {
                                    this.speedSetting = this.parameters.ForwardSpeed.OperationalValue * (0.05);
                                 }
                                 else if (secondsRemaining < 2)
                                 {
                                    this.speedSetting = this.parameters.ForwardSpeed.OperationalValue * (0.5);
                                 }

                                 if (this.measuredVolume >= adjustedSetupPoint)
                                 {
                                    Tracer.WriteMedium(TraceGroup.PUMP, "", "volume good, stop {0}", this.measuredVolume);
                                    this.autoStep = AutoSteps.pumpStop;
                                 }                                
                              }
                              else if (PumpModes.pressure == this.mode)
                              {
                                 if (this.pressureReading >= this.parameters.AutoFillPressure.OperationalValue)
                                 {
                                    this.autoStep = AutoSteps.pumpStop;
                                 }
                              }

                              break;
                           }
                           case AutoSteps.pumpStop:
                           {
                              this.on = false;
                              this.speedSetting = 0;
                              this.autoStep = AutoSteps.waitpumpStop;

                              break;
                           }
                           case AutoSteps.waitpumpStop:
                           {
                              if (0 == this.speedReading)
                              {
                                 this.autoStep = AutoSteps.relieve;
                              }

                              break;
                           }
                           case AutoSteps.relieve:
                           {
                              double tolerance = 0.5;
                              double upperLimit = this.parameters.RelievedPressure.OperationalValue + tolerance;

                              if ((false != this.parameters.AutoPressureRelief) && (this.pressureReading > upperLimit))
                              {
                                 this.mode = PumpModes.pressure;
                                 this.setPoint = this.parameters.RelievedPressure.OperationalValue;

                                 this.on = true;
                                 this.direction = PumpDirections.reverse;
                                 this.speedSetting = this.parameters.ReverseSpeed.OperationalValue;
                                 this.autoStep = AutoSteps.waitRelieve;
                              }
                              else
                              {
                                 this.autoStep = AutoSteps.retract;
                              }
                              
                              break;
                           }
                           case AutoSteps.waitRelieve:
                           {
                              double tolerance = 0.5;
                              double upperLimit = this.parameters.RelievedPressure.OperationalValue + tolerance;

                              if (this.pressureReading <= upperLimit)
                              {
                                 this.on = false;
                                 this.direction = PumpDirections.forward;
                                 this.speedSetting = 0;
                                 this.autoStep = AutoSteps.retract;
                              }

                              break;
                           }
                           case AutoSteps.retract:
                           {
                              if (false != this.parameters.AutoNozzleRetraction)
                              {
                                 NicBotComm.Instance.SetNozzleExtend(ToolLocations.front, false);
                                 NicBotComm.Instance.SetNozzleExtend(ToolLocations.rear, false);
                                 this.autoTimeLimit = DateTime.Now.AddMilliseconds(500);
                                 this.autoStep = AutoSteps.waitRetract;
                              }
                              else
                              {
                                 this.autoStep = AutoSteps.complete;
                              } 
                              
                              break;
                           }
                           case AutoSteps.waitRetract:
                           {
                              if (DateTime.Now > this.autoTimeLimit)
                              {
                                 this.autoStep = AutoSteps.complete;
                              } 
                              
                              break;
                           }
                           case AutoSteps.complete:
                           {
                              this.autoActivated = false;
                              break;
                           }
                        }

                        if (processedAutoStep != this.autoStep)
                        {
                           Tracer.WriteMedium(TraceGroup.PUMP, null, "pump auto step {0}", this.autoStep.ToString());
                        }
                     }
                     while (processedAutoStep != this.autoStep);
                     #endregion
                  }
                  else
                  {
                     this.on = false;
                     this.speedSetting = 0;
                     this.autoState = PumpAutoStates.off;
                     this.state = States.stopped;
                  }

                  break;
               }
               #endregion
               #region case States.pausedWaitStop:
               case States.pausedWaitStop:
               {
                  if (0 == this.speedReading)
                  {
                     this.state = States.paused;
                  }
                  
                  break;
               }
               #endregion
               #region case States.paused:
               case States.paused:
               {
                  if (false == this.autoActivated)
                  {
                     this.autoState = PumpAutoStates.off;
                     this.state = States.stopped;
                  }
                  else if (false == autoPaused)
                  {
                     this.autoState = (false != this.autoActivated) ? PumpAutoStates.running : PumpAutoStates.off;
                     this.state = States.stopped;
                  }
                  else if ((false != manualControl) || (false != this.manualPressureRelief))
                  {
                     this.state = States.setManual;
                  }

                  break;
               }
               #endregion
               #region case States.setManual:
               case States.setManual:
               {
                  if ((false == this.manualControl) && (false == this.manualPressureRelief))
                  {
                     this.autoState = (false != this.autoActivated) ? PumpAutoStates.running : PumpAutoStates.off;
                     this.state = States.stopped;
                  }
                  else if (false != this.manualPressureRelief)
                  {
                     this.mode = PumpModes.pressure;
                     this.setPoint = this.parameters.RelievedPressure.OperationalValue;
                     this.on = true;
                     this.state = States.manualRelief;

                     Tracer.WriteMedium(TraceGroup.PUMP, "", "pressure relief");
                  }
                  else
                  {
                     this.mode = this.manualMode;
                     this.on = true;

                     if (PumpModes.speed == this.mode)
                     {
                        this.setPoint = this.manualSetPoint;
                        this.direction = this.manualDirection;
                        this.speedSetting = this.setPoint;
                        this.state = States.manualSpeed;
                     }
                     else if (PumpModes.pressure == this.mode)
                     {
                        this.setPoint = this.manualSetPoint;

                        if (this.pressureReading > this.setPoint)
                        {
                           this.manualDirection = PumpDirections.reverse;
                        }
                        else
                        {
                           this.manualDirection = PumpDirections.forward;
                        }

                        this.state = States.manualPressure;
                     }

                     Tracer.WriteMedium(TraceGroup.PUMP, "", "manual mode {0}", this.mode.ToString());
                  }

                  break;
               }
               #endregion
               #region case States.manualSpeed:
               case States.manualSpeed:
               {
                  if ((false == this.manualControl) || (false != this.manualPressureRelief) || (this.manualMode != this.mode) || (this.direction != this.manualDirection))
                  {
                     this.setPoint = 0;
                     this.state = States.manualStop;
                  }
                  else
                  {
                     this.setPoint = this.manualSetPoint;
                     this.speedSetting = this.setPoint;
                  }

                  break;
               }
               #endregion
               #region case States.manualPressure:
               case States.manualPressure:
               {
                  if ((false == this.manualControl) || (false != this.manualPressureRelief) || (this.manualMode != this.mode))
                  {
                     this.state = States.manualStop;
                  }
                  else
                  {
                     if (this.setPoint != this.manualSetPoint)
                     {
                        if (this.pressureReading > this.manualSetPoint)
                        {
                           if (this.setPoint > this.manualSetPoint)
                           {
                              this.manualDirection = PumpDirections.reverse;
                           }
                           else
                           {
                              this.manualDirection = PumpDirections.forward;
                           }
                        }
                        else
                        {
                           if (this.setPoint < this.manualSetPoint)
                           {
                              this.manualDirection = PumpDirections.forward;
                           }
                           else
                           {
                              this.manualDirection = PumpDirections.reverse;
                           }
                        }

                        this.setPoint = this.manualSetPoint;
                     }

                     PumpDirections neededDirection = PumpDirections.forward;
                     double neededSpeed = 0;

                     if (PumpDirections.forward == this.manualDirection)
                     {
                        double tolarence = 0.5;
                        double limit = this.setPoint - tolarence;
                        neededDirection = PumpDirections.forward;

                        if (this.pressureReading < limit)
                        {
                           neededSpeed = this.parameters.ForwardSpeed.OperationalValue;
                        }
                     }
                     else if (PumpDirections.reverse == this.manualDirection)
                     {
                        double tolarence = 0.5;
                        double limit = this.setPoint + tolarence;
                        neededDirection = PumpDirections.reverse;

                        if (this.pressureReading > limit)
                        {
                           neededSpeed = this.parameters.ReverseSpeed.OperationalValue;
                        }
                     }

                     if ((neededDirection != this.direction) && (0 != this.speedReading))
                     {
                        Tracer.WriteMedium(TraceGroup.PUMP, "", "manual pressure direction to {0}", neededDirection.ToString());
                        this.state = States.manualStop;
                     }
                     else
                     {
                        this.direction = neededDirection;
                        this.speedSetting = neededSpeed;
                     }
                  }

                  break;
               }
               #endregion
               #region case States.manualRelief:
               case States.manualRelief:
               {
                  double neededSpeed = 0;
                  double tolarence = 0.5;
                  double upperLimit = this.setPoint + tolarence;

                  if (this.pressureReading > upperLimit)
                  {
                     neededSpeed = this.parameters.ReverseSpeed.OperationalValue;
                  }

                  if (0 == neededSpeed)
                  {
                     this.manualPressureRelief = false;
                     this.manualPressureReliefStopping = true;
                     this.state = States.manualStop;
                  }
                  else
                  {
                     this.direction = PumpDirections.reverse;
                     this.speedSetting = neededSpeed;
                  }

                  break;
               }
               #endregion
               #region case States.manualStop:
               case States.manualStop:
               {
                  this.on = false;
                  this.speedSetting = 0;
                  this.state = States.manualWaitStop;
                  break;
               }
               #endregion
               #region case States.manualWaitStop:
               case States.manualWaitStop:
               {
                  if (0 == this.speedReading)
                  {
                     if (false != this.manualPressureReliefStopping)
                     {
                        if (PumpModes.speed == this.manualMode)
                        {
                           this.manualSetPoint = 0;
                        }
                        else if (PumpModes.pressure == this.manualMode)
                        {
                           this.manualSetPoint = this.pressureReading;
                        }

                        this.manualPressureReliefStopping = false;
                        Tracer.WriteMedium(TraceGroup.PUMP, "", "relief complete");
                     }

                     Tracer.WriteMedium(TraceGroup.PUMP, "", "manual stop complete");
                     this.state = States.setManual;
                  }

                  break;
               }
               #endregion
            }
         }
         while (processedState != this.state);
      }

      public void ResetVolume()
      {
         this.measuredVolume = 0;
      }

      public void StartAuto()
      {
         this.autoPaused = false;
         this.autoActivated = true;
      }

      public void PauseAuto()
      {
         this.autoPaused = true;
      }

      public void ResumeAuto()
      {
         this.Stop();
         this.autoPaused = false;
      }

      public void StopAuto()
      {
         this.autoActivated = false;
      }

      public void SetDirection(PumpDirections direction)
      {
         this.manualDirection = direction;
      }

      public void SetSpeed(double speed)
      {
         lock (this)
         {
            this.manualMode = PumpModes.speed;
            this.manualSetPoint = speed;
         }
      }

      public void SetPressure(double pressure)
      {
         lock (this)
         {
            this.manualMode = PumpModes.pressure;
            this.manualSetPoint = pressure;
         }
      }

      public void Start()
      {
         this.manualControl = true;
      }

      public void Stop()
      {
         this.manualControl = false;
      }

      public void RelievePressure()
      {
         Tracer.WriteMedium(TraceGroup.PUMP, "", "relieve pressure");
         this.manualPressureRelief = true;
      }

      public PumpAutoStates GetAutoState()
      {
         return (this.autoState);
      }

      public PumpModes GetMode()
      {
         return (this.mode);
      }
      
      public double GetSetPoint()
      {
         return (this.setPoint);
      }

      public double GetMeasuredVolume()
      {
         return (this.measuredVolume);
      }

      public bool GetOnState()
      {
         return (this.on);
      }

      public PumpDirections GetDirection()
      {
         return (this.direction);
      }
      
      public double GetSpeedSetting()
      {
         return (this.speedSetting);
      }

      public bool GetActivity()
      {
         bool activity = (this.state != States.stopped) && (this.state != States.paused);
         return (activity);
      }

      #endregion

   }
}