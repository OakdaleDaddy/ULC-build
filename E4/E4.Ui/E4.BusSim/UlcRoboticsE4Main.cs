using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;

namespace E4.BusSim
{
   using System.Text;
   using System.Windows.Forms;
   using System.Xml;

   using Microsoft.Win32;

   public partial class UlcRoboticsE4Main : DeviceControl
   {
      #region Fields

      private UInt32 deviceType;
      private byte errorRegister;
      private UInt32 manufacturerStatusRegister;
      private UInt32 predefinedError1;
      private UInt32 predefinedError2;
      private UInt32 predefinedError3;
      private UInt32 predefinedError4;
      private UInt32 communicationCyclePeriod;
      private UInt32 synchronousWindowLength;
      private string manufacturerDeviceName;
      private string manufacturerHardwareVersion;
      private string manufacturerSoftwareVersion;
      private UInt16 guardTime;
      private byte lifeTimeFactor;
      private UInt16 emergencyInhibitTime;
      private UInt32 consumerHeartbeatTime;
      private UInt16 producerHeartbeatTime;
      
      private RPDOMapping[] rpdoMapping;
      private TPDOMapping[] tpdoMapping;

      private byte cameraSelect;
      private UInt32 cameraLedDefaultIntensity;
      private UInt32 cameraLedIntensity;
      private byte cameraLedChannelMask;

      private UInt32 mainBoardImuConfiguration;
      private UInt16 mainBoardImuAxisRemap;
      private byte mainBoardImuUnitSelection;
      private UInt16 mainBoardImuUpdateFrequency;
      private Int16 mainBoardImuRollValue;
      private Int16 mainBoardImuPitchValue;
      private Int16 mainBoardImuYawValue;
      private byte mainBoardImuTemperature;
      private byte mainBoardImuErrorTemperature;

      private UInt32 targetBoardImuConfiguration;
      private UInt16 targetBoardImuAxisRemap;
      private byte targetBoardImuUnitSelection;
      private UInt16 targetBoardImuUpdateFrequency;
      private Int16 targetBoardImuRollValue;
      private Int16 targetBoardImuPitchValue;
      private Int16 targetBoardImuYawValue;
      private byte targetBoardImuTemperature;
      private byte targetBoardImuErrorTemperature;

      private byte laserAimEnable;
      private byte laserTimeToMeasure;
      private byte laserMeasuredDistanceHighest;
      private UInt32 laserMeasuredDistance;
      private byte laserTemperature;
      private byte laserErrorTemperature;
      private byte laserControlByte;
      private byte laserStatusByte;

      private byte laserScannerPosition;
      private byte laserScannerTemperature;
      private byte laserScannerErrorTemperature;

      private byte mcuTemperature;
      private byte mcuErrorTemperature;
      private byte dcLinkVoltageByte;

      private byte outputs;

      private MainMotor[] motors;

      private byte workingConsumerHeartbeatNode;
      private UInt16 workingConsumerHeartbeatTime;
      private bool workingConsumerHeartbeatActive;
      private DateTime workingConsumerHeartbeatTimeLimit;

      private DateTime workingProducerHeartbeatTimeLimit;

      private bool laserMeasureActive;
      private bool laserMeasureExecuted;
      private byte activeLaserControlByte;
      private int activeLaserTimeToMeasure;
      private int laserReadingCount;
      private int laserReadingLimit;
      private UInt32[] laserReadings;
      private DateTime laserReadingTimeLimit;

      private byte nvNodeId;

      private bool active;
      private byte nodeId;

      #endregion

      #region Delegates
      
      private bool RPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if ((0x2301 == index) ||
             ((0x2302 == index) && (0x01 == subIndex)) ||
             ((0x2304 == index) && (0x01 == subIndex)) ||
             ((0x2310 == index) && (0x01 == subIndex)) ||
             ((0x2311 == index) && (0x02 == subIndex)) ||
             (0x2400 == index) ||
             (0x2401 == index) ||
             ((0x2403 == index) && (0x02 == subIndex)) ||
             (0x2404 == index) ||
             ((0x2411 == index) && (0x02 == subIndex)) ||
             ((0x2443 == index) && (0x02 == subIndex)) ||
             ((0x2446 == index) && (0x02 == subIndex)))
         {
            result = true;
         }

         if (false == result)
         {
            for (int i = 0; i < this.motors.Length; i++)
            {
               if (false != result)
               {
                  break;
               }

               result = this.motors[i].RPdoMappable(index, subIndex);
            }
         }

         return (result);
      }

      private int RPdoSizeHandler(UInt16 index, byte subIndex)
      {
         int result = 0;

         if ((0x2301 == index) ||
             ((0x2304 == index) && (0x01 == subIndex)) ||
             ((0x2310 == index) && (0x01 == subIndex)) ||
             ((0x2311 == index) && (0x02 == subIndex)) ||
             (0x2400 == index) ||
             (0x2401 == index) ||
             ((0x2403 == index) && (0x02 == subIndex)) ||
             (0x2404 == index) ||
             ((0x2411 == index) && (0x02 == subIndex)) ||
             ((0x2443 == index) && (0x02 == subIndex)) ||
             ((0x2446 == index) && (0x02 == subIndex)))
         {
            result = 1;
         }
         else if ((0x2302 == index) && (0x01 == subIndex))
         {
            result = 4;
         }

         if (0 == result)
         {
            for (int i = 0; i < this.motors.Length; i++)
            {
               if (0 != result)
               {
                  break;
               }

               result = this.motors[i].RPdoSize(index, subIndex);
            }
         }

         return (result);
      }

      private bool RPdoDataHandler(UInt16 index, byte subIndex, byte[] data, int offset, UInt32 length)
      {
         bool result = this.StoreDeviceData(index, subIndex, data, offset, length);
         return (result);
      }
      
      private bool TPdoMappableHandler(UInt16 index, byte subIndex)
      {
         bool result = false;

         if ((0x1001 == index) ||
             (0x1002 == index) ||
             (0x2301 == index) ||
             ((0x2302 == index) && (0x01 == subIndex)) ||
             ((0x2304 == index) && (0x01 == subIndex)) ||
             ((0x2310 == index) && (0x01 == subIndex)) ||
             ((0x2311 == index) && (0x01 == subIndex)) ||
             ((0x2311 == index) && (0x02 == subIndex)) ||
             (0x2000 == index) ||
             (0x2400 == index) ||
             (0x2401 == index) ||
             ((0x2402 == index) && (0x00 == subIndex)) ||
             ((0x2402 == index) && (0x01 == subIndex)) ||
             ((0x2403 == index) && (0x01 == subIndex)) ||
             ((0x2403 == index) && (0x02 == subIndex)) ||
             (0x2404 == index) ||
             (0x2405 == index) ||
             ((0x2410 == index) && (0x01 == subIndex)) ||
             ((0x2411 == index) && (0x01 == subIndex)) ||
             ((0x2411 == index) && (0x02 == subIndex)) ||
             ((0x2441 == index) && (0x01 == subIndex)) ||
             ((0x2441 == index) && (0x02 == subIndex)) ||
             ((0x2441 == index) && (0x03 == subIndex)) ||
             ((0x2443 == index) && (0x01 == subIndex)) ||
             ((0x2443 == index) && (0x02 == subIndex)) ||
             ((0x2445 == index) && (0x01 == subIndex)) ||
             ((0x2445 == index) && (0x02 == subIndex)) ||
             ((0x2445 == index) && (0x03 == subIndex)) ||
             ((0x2446 == index) && (0x01 == subIndex)) ||
             ((0x2446 == index) && (0x02 == subIndex)))
         {
            result = true;
         }

         if (false == result)
         {
            for (int i = 0; i < this.motors.Length; i++)
            {
               if (false != result)
               {
                  break;
               }

               result = this.motors[i].TPdoMappable(index, subIndex);
            }
         }

         return (result);
      }

      private int TPdoSizeHandler(UInt16 index, byte subIndex)
      {
         int result = 0;

         if ((0x1001 == index) ||
             (0x2301 == index) ||
             ((0x2304 == index) && (0x01 == subIndex)) ||
             ((0x2310 == index) && (0x01 == subIndex)) ||
             ((0x2311 == index) && (0x01 == subIndex)) ||
             ((0x2311 == index) && (0x02 == subIndex)) ||
             (0x2000 == index) ||
             (0x2400 == index) ||
             (0x2401 == index) ||
             ((0x2402 == index) && (0x00 == subIndex)) ||
             ((0x2403 == index) && (0x01 == subIndex)) ||
             ((0x2403 == index) && (0x02 == subIndex)) ||
             (0x2404 == index) ||
             (0x2405 == index) ||
             ((0x2410 == index) && (0x01 == subIndex)) ||
             ((0x2411 == index) && (0x01 == subIndex)) ||
             ((0x2411 == index) && (0x02 == subIndex)) ||
             ((0x2443 == index) && (0x01 == subIndex)) ||
             ((0x2443 == index) && (0x02 == subIndex)) ||
             ((0x2446 == index) && (0x01 == subIndex)) ||
             ((0x2446 == index) && (0x02 == subIndex)))
         {
            result = 1;
         }
         else if (((0x2441 == index) && (0x01 == subIndex)) ||
                  ((0x2441 == index) && (0x02 == subIndex)) ||
                  ((0x2441 == index) && (0x03 == subIndex)) ||
                  ((0x2445 == index) && (0x01 == subIndex)) ||
                  ((0x2445 == index) && (0x02 == subIndex)) ||
                  ((0x2445 == index) && (0x03 == subIndex)))
         {
            result = 2;
         }
         else if ((0x1002 == index) ||
                  ((0x2302 == index) && (0x01 == subIndex)) ||
                  ((0x2402 == index) && (0x01 == subIndex)))
         {
            result = 4;
         }

         if (0 == result)
         {
            for (int i = 0; i < this.motors.Length; i++)
            {
               if (0 != result)
               {
                  break;
               }

               result = this.motors[i].TPdoSize(index, subIndex);
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

      private void MotorTpdoCheck(UInt16 index, byte subIndex)
      {
         this.CheckTpdoMappings(index, subIndex);
      }

      #endregion

      #region Properties

      #region Communication 

      private byte ErrorRegister
      {
         set
         {
            this.SetValue(0x1001, 0x00, ErrorRegisterLabel, "Error Register", ref this.errorRegister, value, 2);
         }

         get
         {
            return(this.errorRegister);
         }
      }

      private UInt32 ManufacturerStatusRegister
      {
         set
         {
            this.SetValue(0x1002, 0x00, ManufacturerStatusRegisterLabel, "Manufacturer Status Register", ref this.manufacturerStatusRegister, value, 8);
         }

         get
         {
            return (this.manufacturerStatusRegister);
         }
      }

      private UInt32 PredefinedError1
      {
         set
         {
            this.SetValue(0x1003, 0x01, PredefinedErrorField1Label, "Pre-defined Error Field 1", ref this.predefinedError1, value, 8);
         }

         get
         {
            return (this.predefinedError1);
         }
      }

      private UInt32 PredefinedError2
      {
         set
         {
            this.SetValue(0x1003, 0x02, PredefinedErrorField2Label, "Pre-defined Error Field 2", ref this.predefinedError2, value, 8);
         }

         get
         {
            return (this.predefinedError2);
         }
      }

      private UInt32 PredefinedError3
      {
         set
         {
            this.SetValue(0x1003, 0x03, PredefinedErrorField3Label, "Pre-defined Error Field 3", ref this.predefinedError3, value, 8);
         }

         get
         {
            return (this.predefinedError3);
         }
      }

      private UInt32 PredefinedError4
      {
         set
         {
            this.SetValue(0x1003, 0x04, PredefinedErrorField4Label, "Pre-defined Error Field 4", ref this.predefinedError4, value, 8);
         }

         get
         {
            return (this.predefinedError4);
         }
      }

      private UInt32 CommunicationCyclePeriod
      {
         set
         {
            this.SetValue(0x1006, 0x00, CommunicationCyclePeriodLabel, "Communication Cycle Period", ref this.communicationCyclePeriod, value, 8);
         }

         get
         {
            return (this.communicationCyclePeriod);
         }
      }

      private UInt32 SynchronousWindowLength
      {
         set
         {
            this.SetValue(0x1007, 0x00, SynchronousWindowLengthLabel, "Synchronous Window Length", ref this.synchronousWindowLength, value, 8);
         }

         get
         {
            return (this.synchronousWindowLength);
         }
      }

      private UInt16 GuardTime
      {
         set
         {
            this.SetValue(0x100C, 0x00, GuardTimeLabel, "Guard Time", ref this.guardTime, value, 4);
         }

         get
         {
            return (this.guardTime);
         }
      }

      private byte LifeTimeFactor
      {
         set
         {
            this.SetValue(0x100D, 0x00, LifeTimeFactorLabel, "Life Time Factor", ref this.lifeTimeFactor, value, 2);
         }

         get
         {
            return (this.lifeTimeFactor);
         }
      }

      private UInt16 EmergencyInhibitTime
      {
         set
         {
            this.SetValue(0x1015, 0x00, EmergencyInhibitTimeLabel, "Emergency Inhibit Time", ref this.emergencyInhibitTime, value, 4);
         }

         get
         {
            return (this.emergencyInhibitTime);
         }
      }

      private UInt32 ConsumerHeartbeatTime
      {
         set
         {
            this.SetValue(0x1016, 0x00, ConsumerHeartbeatTimeLabel, "Consumer Heartbeat Time", ref this.consumerHeartbeatTime, value, 8);

            this.workingConsumerHeartbeatNode = (byte)((this.consumerHeartbeatTime >> 16) & 0x7F);
            this.workingConsumerHeartbeatTime = (UInt16)(this.consumerHeartbeatTime & 0xFFFF);
            this.workingConsumerHeartbeatActive = false;
         }

         get
         {
            return (this.consumerHeartbeatTime);
         }
      }

      private UInt16 ProducerHeartbeatTime
      {
         set
         {
            this.SetValue(0x1017, 0x00, ProducerHeartbeatTimeLabel, "Producer Heartbeat Time", ref this.producerHeartbeatTime, value, 4);

            this.workingProducerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.producerHeartbeatTime);
         }

         get
         {
            return (this.producerHeartbeatTime);
         }
      }

      #endregion

      #region Camera

      private byte CameraSelect
      {
         set
         {
            this.SetValue(0x2301, 0x00, CameraSelectLabel, "Camera Select", ref this.cameraSelect, value, 2);
         }

         get
         {
            return (this.cameraSelect);
         }
      }

      private UInt32 CameraLedDefaultIntensity
      {
         set
         {
            this.SetValue(0x2302, 0x01, CameraLedDefaultIntensityLabel, "Camera LED Default Intensity", ref this.cameraLedDefaultIntensity, value, 8);
         }

         get
         {
            return (this.cameraLedDefaultIntensity);
         }
      }

      private UInt32 CameraLedIntensity
      {
         set
         {
            this.SetValue(0x2303, 0x01, CameraLedIntensityLabel, "Camera LED Intensity", ref this.cameraLedIntensity, value, 8);
         }

         get
         {
            return (this.cameraLedIntensity);
         }
      }

      private byte CameraLedChannelMask
      {
         set
         {
            this.SetValue(0x2303, 0x01, CameraLedChannelMaskLabel, "Camera LED Channel Mask", ref this.cameraLedChannelMask, value, 2);
         }

         get
         {
            return (this.cameraLedChannelMask);
         }
      }

      #endregion

      #region IMU

      private UInt32 MainBoardImuConfiguration
      {
         set
         {
            this.SetValue(0x2440, 0x01, MainBoardImuConfigurationLabel, "Main Board IMU Configuration", ref this.mainBoardImuConfiguration, value, 8);
         }

         get
         {
            return (this.mainBoardImuConfiguration);
         }
      }

      private UInt16 MainBoardImuAxisRemap
      {
         set
         {
            this.SetValue(0x2440, 0x02, MainBoardImuAxisRemapLabel, "Main Board IMU Axis Remap", ref this.mainBoardImuAxisRemap, value, 4);
         }

         get
         {
            return (this.mainBoardImuAxisRemap);
         }
      }

      private byte MainBoardImuUnitSelection
      {
         set
         {
            this.SetValue(0x2440, 0x03, MainBoardImuUnitSelectionLabel, "Main Board IMU Unit Selection", ref this.mainBoardImuUnitSelection, value, 2);
         }

         get
         {
            return (this.mainBoardImuUnitSelection);
         }
      }

      private UInt16 MainBoardImuUpdateFrequency
      {
         set
         {
            this.SetValue(0x2440, 0x04, MainBoardImuUpdateFrequencyLabel, "Main Board IMU Update Frequency", ref this.mainBoardImuUpdateFrequency, value, 4);
         }

         get
         {
            return (this.mainBoardImuUpdateFrequency);
         }
      }

      private Int16 MainBoardImuRollValue
      {
         set
         {
            this.SetValue(0x2441, 0x01, MainBoardImuRollValueLabel, "Main Board IMU Roll Value", ref this.mainBoardImuRollValue, value, 4);
         }

         get
         {
            return (this.mainBoardImuRollValue);
         }
      }

      private Int16 MainBoardImuPitchValue
      {
         set
         {
            this.SetValue(0x2441, 0x02, MainBoardImuPitchValueLabel, "Main Board IMU Pitch Value", ref this.mainBoardImuPitchValue, value, 4);
         }

         get
         {
            return (this.mainBoardImuPitchValue);
         }
      }

      private Int16 MainBoardImuYawValue
      {
         set
         {
            this.SetValue(0x2441, 0x03, MainBoardImuYawValueLabel, "Main Board IMU Yaw Value", ref this.mainBoardImuYawValue, value, 4);
         }

         get
         {
            return (this.mainBoardImuYawValue);
         }
      }

      private byte MainBoardImuTemperature
      {
         set
         {
            this.SetValue(0x2446, 0x01, MainBoardImuTemperatureLabel, "Main Board IMU Temperature", ref this.mainBoardImuTemperature, value, 2);
         }

         get
         {
            return (this.mainBoardImuTemperature);
         }
      }

      private byte MainBoardImuErrorTemperature
      {
         set
         {
            this.SetValue(0x2443, 0x02, MainBoardImuTemperatureLabel, "Main Board IMU Error Temperature", ref this.mainBoardImuErrorTemperature, value, 2);
         }

         get
         {
            return (this.mainBoardImuErrorTemperature);
         }
      }

      private UInt32 TargetBoardImuConfiguration
      {
         set
         {
            this.SetValue(0x2444, 0x01, TargetBoardImuConfigurationLabel, "Target Board IMU Configuration", ref this.targetBoardImuConfiguration, value, 8);
         }

         get
         {
            return (this.targetBoardImuConfiguration);
         }
      }

      private UInt16 TargetBoardImuAxisRemap
      {
         set
         {
            this.SetValue(0x2444, 0x02, TargetBoardImuAxisRemapLabel, "Target Board IMU Axis Remap", ref this.targetBoardImuAxisRemap, value, 4);
         }

         get
         {
            return (this.targetBoardImuAxisRemap);
         }
      }

      private byte TargetBoardImuUnitSelection
      {
         set
         {
            this.SetValue(0x2444, 0x03, TargetBoardImuUnitSelectionLabel, "Target Board IMU Unit Selection", ref this.targetBoardImuUnitSelection, value, 2);
         }

         get
         {
            return (this.targetBoardImuUnitSelection);
         }
      }

      private UInt16 TargetBoardImuUpdateFrequency
      {
         set
         {
            this.SetValue(0x2444, 0x04, TargetBoardImuUpdateFrequencyLabel, "Target Board IMU Update Frequency", ref this.targetBoardImuUpdateFrequency, value, 4);
         }

         get
         {
            return (this.targetBoardImuUpdateFrequency);
         }
      }

      private Int16 TargetBoardImuRollValue
      {
         set
         {
            this.SetValue(0x2445, 0x01, TargetBoardImuRollValueLabel, "Target Board IMU Roll Value", ref this.targetBoardImuRollValue, value, 4);
         }

         get
         {
            return (this.targetBoardImuRollValue);
         }
      }

      private Int16 TargetBoardImuPitchValue
      {
         set
         {
            this.SetValue(0x2445, 0x02, TargetBoardImuPitchValueLabel, "Target Board IMU Pitch Value", ref this.targetBoardImuPitchValue, value, 4);
         }

         get
         {
            return (this.targetBoardImuPitchValue);
         }
      }

      private Int16 TargetBoardImuYawValue
      {
         set
         {
            this.SetValue(0x2445, 0x03, TargetBoardImuYawValueLabel, "Target Board IMU Yaw Value", ref this.targetBoardImuYawValue, value, 4);
         }

         get
         {
            return (this.targetBoardImuYawValue);
         }
      }

      private byte TargetBoardImuTemperature
      {
         set
         {
            this.SetValue(0x2446, 0x01, TargetBoardImuTemperatureLabel, "Target Board IMU Temperature", ref this.targetBoardImuTemperature, value, 2);
         }

         get
         {
            return (this.targetBoardImuTemperature);
         }
      }

      private byte TargetBoardImuErrorTemperature
      {
         set
         {
            this.SetValue(0x2446, 0x02, TargetBoardImuErrorTemperatureLabel, "Target Board IMU Error Temperature", ref this.targetBoardImuErrorTemperature, value, 2);
         }

         get
         {
            return (this.targetBoardImuErrorTemperature);
         }
      }

      #endregion

      #region Laser Finder

      private byte LaserAimEnable
      {
         set
         {
            this.SetValue(0x2400, 0x00, LaserAimEnableLabel, "Laser Aim Enable", ref this.laserAimEnable, value, 2);
         }

         get
         {
            return (this.laserAimEnable);
         }
      }

      private byte LaserTimeToMeasure
      {
         set
         {
            this.SetValue(0x2401, 0x00, LaserTimeToMeasureLabel, "Laser Time To Measure", ref this.laserTimeToMeasure, value, 2);
         }

         get
         {
            return (this.laserTimeToMeasure);
         }
      }

      private byte LaserMeasuredDistanceHighest
      {
         set
         {
            this.SetValue(0x2402, 0x00, LaserMeasuredDistanceHighestLabel, "Laser Measured Distance Highest", ref this.laserMeasuredDistanceHighest, value, 2);
         }

         get
         {
            return (this.laserMeasuredDistanceHighest);
         }
      }

      private UInt32 LaserMeasuredDistance
      {
         set
         {
            this.SetValue(0x2402, 0x01, LaserMeasuredDistanceLabel, "Laser Measured Distance (mm)", ref this.laserMeasuredDistance, value, 8);
         }

         get
         {
            return (this.laserMeasuredDistance);
         }
      }

      private byte LaserTemperature
      {
         set
         {
            this.SetValue(0x2403, 0x01, LaserTemperatureLabel, "Laser Temperature", ref this.laserTemperature, value, 2);
         }

         get
         {
            return (this.laserTemperature);
         }
      }

      private byte LaserErrorTemperature
      {
         set
         {
            this.SetValue(0x2403, 0x02, LaserErrorTemperatureLabel, "Laser Error Temperature", ref this.laserErrorTemperature, value, 2);
         }

         get
         {
            return (this.laserErrorTemperature);
         }
      }

      private byte LaserControlByte
      {
         set
         {
            this.SetValue(0x2404, 0x00, LaserControlByteLabel, "Laser Control Byte", ref this.laserControlByte, value, 2);
         }

         get
         {
            return (this.laserControlByte);
         }
      }

      private byte LaserStatusByte
      {
         set
         {
            this.SetValue(0x2405, 0x00, LaserStatusByteLabel, "Laser Status Byte", ref this.laserStatusByte, value, 2);
         }

         get
         {
            return (this.laserStatusByte);
         }
      }

      #endregion

      #region Laser Scanner 

      private byte LaserScannerPosition
      {
         set
         {
            this.SetValue(0x2410, 0x01, LaserScannerPositionLabel, "Laser Scanner Position", ref this.laserScannerPosition, value, 2);
         }

         get
         {
            return (this.laserScannerPosition);
         }
      }

      private byte LaserScannerTemperature
      {
         set
         {
            this.SetValue(0x2411, 0x01, LaserScannerTemperatureLabel, "Laser Scanner Temperature", ref this.laserScannerTemperature, value, 2);
         }

         get
         {
            return (this.laserScannerTemperature);
         }
      }

      private byte LaserScannerErrorTemperature
      {
         set
         {
            this.SetValue(0x2411, 0x02, LaserScannerErrorTemperatureLabel, "Laser Scanner Error Temperature", ref this.laserScannerErrorTemperature, value, 2);
         }

         get
         {
            return (this.laserScannerErrorTemperature);
         }
      }

      #endregion

      #region MCU

      private byte Outputs
      {
         set
         {
            this.SetValue(0x2310, 0x01, OutputsLabel, "Outputs", ref this.outputs, value, 2);
         }

         get
         {
            return (this.outputs);
         }
      }

      private byte McuTemperature
      {
         set
         {
            this.SetValue(0x2311, 0x01, McuTemperatureLabel, "MCU Temperature", ref this.mcuTemperature, value, 2);
         }

         get
         {
            return (this.mcuTemperature);
         }
      }

      private byte McuErrorTemperature
      {
         set
         {
            this.SetValue(0x2311, 0x02, McuErrorTemperatureLabel, "MCU Error Temperature", ref this.mcuErrorTemperature, value, 2);
         }

         get
         {
            return (this.mcuErrorTemperature);
         }
      }

      private byte DcLinkVoltageByte
      {
         set
         {
            this.SetValue(0x2000, 0x00, DcLinkVoltageByteLabel, "DC Link Voltage Byte", ref this.dcLinkVoltageByte, value, 2);
         }

         get
         {
            return (this.dcLinkVoltageByte);
         }
      }

      #endregion

      #endregion

      #region Helper Functions

      private void SetLabelValue(UInt16 index, byte subIndex, Label label, string description, int value, int precision)
      {
         if (null != label)
         {
            string valueFormatString = "{" + string.Format("0:X{0}", precision) + "}";
            string valueString = string.Format(valueFormatString, value);

            string indexString = string.Format("0x{0:X4}", index);
            string subIndexString = (0 == subIndex) ? " " : string.Format("-{0:X2} ", subIndex);

            label.Text = indexString + subIndexString + description + "..." + valueString;
         }
      }

      private void CheckTpdoMappings(UInt16 index, byte subIndex)
      {
         if (null != this.tpdoMapping)
         {
            for (int i = 0; i < this.tpdoMapping.Length; i++)
            {
               if (this.tpdoMapping[i].Contains(index, subIndex) != false)
               {
                  this.tpdoMapping[i].Activate();
               }
            }
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref byte existingValue, byte assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref Int16 existingValue, Int16 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref UInt16 existingValue, UInt16 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref Int32 existingValue, Int32 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private void SetValue(UInt16 index, byte subIndex, Label label, string description, ref UInt32 existingValue, UInt32 assignedValue, int precision)
      {
         this.SetLabelValue(index, subIndex, label, description, (int)assignedValue, precision);

         if (existingValue != assignedValue)
         {
            existingValue = assignedValue;
            this.CheckTpdoMappings(index, subIndex);
         }
      }

      private UInt32 GetLaserMeasuredDistance()
      {
         UInt32 result = 0;

         if (UInt32.TryParse(this.LaserMeasuredDistanceTextBox.Text, out laserMeasuredDistance) != false)
         {
            result = (UInt32)laserMeasuredDistance;
         }

         return (result);
      }

      private void ResetLaserMeasurements()
      {
         this.laserMeasureActive = false;
         this.laserMeasureExecuted = false;
         this.LaserMeasuredDistanceHighest = 1;
         this.LaserMeasuredDistance = 0;

         for (int i = 0; i < this.laserReadings.Length; i++)
         {
            this.laserReadings[i] = 0;
         }

         this.LaserStatusByte = 0x00;
      }

      #endregion

      #region Device Specific Functions

      protected void Reset(bool fromPowerUp)
      {
         if (false != this.active)
         {
            this.DeviceStateLabel.Text = "PRE-OP";

            this.nodeId = this.nvNodeId;
            this.NodeIdTextBox.Text = this.nodeId.ToString();

            if (false != fromPowerUp)
            {
               this.ErrorRegister = 0;
               this.ManufacturerStatusRegister = 0;
               this.PredefinedError1 = 0;
               this.PredefinedError2 = 0;
               this.PredefinedError3 = 0;
               this.PredefinedError4 = 0;

               this.MainBoardImuRollValue = 0;
               this.MainBoardImuPitchValue = 0;
               this.MainBoardImuYawValue = 0;
               this.MainBoardImuTemperature = 0;
               
               this.TargetBoardImuRollValue = 0;
               this.TargetBoardImuPitchValue = 0;
               this.TargetBoardImuYawValue = 0;
               this.TargetBoardImuTemperature = 0;
               
               this.LaserTemperature = 0;
               this.LaserScannerPosition = 0;
               this.LaserScannerTemperature = 0;
               
               this.Outputs = 0;
               this.McuTemperature = 23;
            }

            this.CommunicationCyclePeriod = 0;
            this.SynchronousWindowLength = 0;
            this.GuardTime = 0;
            this.LifeTimeFactor = 0;
            this.EmergencyInhibitTime = 0;
            this.ConsumerHeartbeatTime = 0;
            this.ProducerHeartbeatTime = 0;

            for (int i = 0; i < this.rpdoMapping.Length; i++)
            {
               this.rpdoMapping[i].Reset(i, this.nodeId);
            }

            for (int i = 0; i < this.tpdoMapping.Length; i++)
            {
               this.tpdoMapping[i].Reset(i, this.nodeId);
            }

            this.CameraSelect = 0;
            this.CameraLedDefaultIntensity = 858993459;
            this.CameraLedIntensity = 3355443;
            this.CameraLedChannelMask = 0;

            this.MainBoardImuConfiguration = 0;
            this.MainBoardImuAxisRemap = 0x24;
            this.MainBoardImuUnitSelection = 0;
            this.MainBoardImuUpdateFrequency = 10;
            this.MainBoardImuErrorTemperature = 1;

            this.TargetBoardImuConfiguration = 0;
            this.TargetBoardImuAxisRemap = 0x24;
            this.TargetBoardImuUnitSelection = 0;
            this.TargetBoardImuUpdateFrequency = 10;
            this.TargetBoardImuErrorTemperature = 1;

            this.LaserAimEnable = 0;
            this.LaserTimeToMeasure = 6;
            this.LaserMeasuredDistanceHighest = 1;
            this.LaserMeasuredDistance = 0;
            this.LaserErrorTemperature = 85;
            this.LaserControlByte = 0;
            this.LaserStatusByte = 0;
            this.LaserScannerErrorTemperature = 1;

            this.McuErrorTemperature = 0;
            this.DcLinkVoltageByte = 0;

            for (int i = 0; i < this.motors.Length; i++)
            {
               this.motors[i].Reset(fromPowerUp);
            }

            int cobId = this.GetCobId(COBTypes.ERROR, this.nodeId);
            byte[] bootUpMsg = new byte[1];
            bootUpMsg[0] = 0;

            this.laserMeasureActive = false;

            this.Transmit(cobId, bootUpMsg);
            base.Reset();
         }
      }

      protected override void Start()
      {
         if (false != this.active)
         {
            base.Start();

            for (int i = 0; i < this.rpdoMapping.Length; i++)
            {
               this.rpdoMapping[i].Start();
            }

            for (int i = 0; i < this.tpdoMapping.Length; i++)
            {
               this.tpdoMapping[i].Start();
            }

            for (int i = 0; i < this.motors.Length; i++)
            {
               this.motors[i].Start();
            }

            this.DeviceStateLabel.Text = "RUNNING";

            DateTime now = DateTime.Now;
            this.workingProducerHeartbeatTimeLimit = now.AddMilliseconds(this.producerHeartbeatTime);
         }
      }

      protected override void Stop()
      {
         if (false != this.active)
         {
            base.Stop();

            for (int i = 0; i < this.rpdoMapping.Length; i++)
            {
               this.rpdoMapping[i].Stop();
            }

            for (int i = 0; i < this.tpdoMapping.Length; i++)
            {
               this.tpdoMapping[i].Stop();
            }

            for (int i = 0; i < this.motors.Length; i++)
            {
               this.motors[i].Stop();
            }

            this.DeviceStateLabel.Text = "STOPPED";
         }
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
            dataLength = this.MoveDeviceData(buffer, this.errorRegister);
            valid = true;
         }
         else if (0x1002 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.manufacturerStatusRegister);
            valid = true;
         }
         else if ((0x1003 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)5);
            valid = true;
         }
         else if ((0x1003 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.PredefinedError1);
            valid = true;
         }
         else if ((0x1003 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.PredefinedError2);
            valid = true;
         }
         else if ((0x1003 == index) && (0x03 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.PredefinedError3);
            valid = true;
         }
         else if ((0x1003 == index) && (0x04 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.PredefinedError4);
            valid = true;
         }
         else if (0x1005 == index)
         {
            UInt32 syncCobId = 0x00000080;
            dataLength = this.MoveDeviceData(buffer, syncCobId);
            valid = true;
         }
         else if (0x1006 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.CommunicationCyclePeriod);
            valid = true;
         }
         else if (0x1007 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.SynchronousWindowLength);
            valid = true;
         }
         else if (0x1008 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.manufacturerDeviceName);
            valid = true;
         }
         else if (0x1009 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.manufacturerHardwareVersion);
            valid = true;
         }
         else if (0x100A == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.manufacturerSoftwareVersion);
            valid = true;
         }
         else if (0x100C == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.GuardTime);
            valid = true;
         }
         else if (0x100D == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.LifeTimeFactor);
            valid = true;
         }            
         else if (0x1012 == index)
         {
            UInt32 timestampCobId = 0x00000100;
            dataLength = this.MoveDeviceData(buffer, timestampCobId);
            valid = true;
         }
         else if (0x1013 == index)
         {
            UInt32 highResolutionTimestamp = 0;
            dataLength = this.MoveDeviceData(buffer, highResolutionTimestamp);
            valid = true;
         }
         else if (0x1014 == index)
         {
            UInt32 emergencyCobId = (UInt32)this.nodeId + 0x00000080;
            dataLength = this.MoveDeviceData(buffer, emergencyCobId);
            valid = true;
         }
         else if (0x1015 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.EmergencyInhibitTime);
            valid = true;
         }            
         else if ((0x1016 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x1016 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.ConsumerHeartbeatTime);
            valid = true;
         }
         else if (0x1017 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.ProducerHeartbeatTime);
            valid = true;
         }
         else if ((0x1018 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x1018 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (UInt32)0);
            valid = true;
         }
         else if ((0x1400 <= index) && (0x1600 > index))
         {
            int pdoMapOffset = (index - 0x1400);

            if (pdoMapOffset < this.rpdoMapping.Length)
            {
               this.rpdoMapping[pdoMapOffset].LoadParameterData(subIndex, buffer, ref dataLength);
               valid = true;
            }
         }
         else if ((0x1600 <= index) && (0x1800 > index))
         {
            int pdoMapOffset = (index - 0x1600);

            if (pdoMapOffset < this.rpdoMapping.Length)
            {
               this.rpdoMapping[pdoMapOffset].LoadMapData(subIndex, buffer, ref dataLength);
               valid = true;
            }
         }
         else if ((0x1800 <= index) && (0x1A00 > index))
         {
            int pdoMapOffset = (index - 0x1800);

            if (pdoMapOffset < this.tpdoMapping.Length)
            {
               this.tpdoMapping[pdoMapOffset].LoadParameterData(subIndex, buffer, ref dataLength);
               valid = true;
            }
         }
         else if ((0x1A00 <= index) && (0x1C00 > index))
         {
            int pdoMapOffset = (index - 0x1A00);

            if (pdoMapOffset < this.tpdoMapping.Length)
            {
               this.tpdoMapping[pdoMapOffset].LoadMapData(subIndex, buffer, ref dataLength);
               valid = true;
            }
         }
         else if (0x2301 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.CameraSelect);
            valid = true;
         }
         else if ((0x2302 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x2302 == index)  && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.CameraLedDefaultIntensity);
            valid = true;
         }
         else if ((0x2303 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x2303 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.CameraLedIntensity);
            valid = true;
         }
         else if ((0x2304 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x2304 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.CameraLedChannelMask);
            valid = true;
         }
         else if ((0x2310 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x2310 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.Outputs);
            valid = true;
         }
         else if ((0x2311 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if ((0x2311 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.McuTemperature);
            valid = true;
         }
         else if ((0x2311 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.McuErrorTemperature);
            valid = true;
         }
         else if (0x2000 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.DcLinkVoltageByte);
            valid = true;
         }
         else if (0x2400 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserAimEnable);
            valid = true;
         }
         else if (0x2401 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserTimeToMeasure);
            valid = true;
         }
         else if (0x2402 == index)
         {
            if (0x00 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, this.LaserMeasuredDistanceHighest);
               valid = true;
            }
            else if (0x01 == subIndex)
            {
               dataLength = this.MoveDeviceData(buffer, this.LaserMeasuredDistance);
               valid = true;
            }
            else 
            {
               int readingIndex = subIndex - 2;

               if ((readingIndex < this.laserReadingCount) && (readingIndex < this.laserReadings.Length))
               {
                  dataLength = this.MoveDeviceData(buffer, this.laserReadings[readingIndex]);
                  valid = true;
               }
            }
         }
         else if ((0x2403 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if ((0x2403 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserTemperature);
            valid = true;
         }
         else if ((0x2403 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserErrorTemperature);
            valid = true;
         }
         else if (0x2404 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserControlByte);
            valid = true;
         }
         else if (0x2405 == index)
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserStatusByte);
            valid = true;
         }
         else if ((0x2410 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)1);
            valid = true;
         }
         else if ((0x2410 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserScannerPosition);
            valid = true;
         }
         else if ((0x2411 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if ((0x2411 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserScannerTemperature);
            valid = true;
         }
         else if ((0x2411 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.LaserScannerErrorTemperature);
            valid = true;
         }
         #region Main Board IMU
         else if ((0x2440 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)4);
            valid = true;
         }
         else if ((0x2440 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuConfiguration);
            valid = true;
         }
         else if ((0x2440 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuAxisRemap);
            valid = true;
         }
         else if ((0x2440 == index) && (0x03 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuUnitSelection);
            valid = true;
         }
         else if ((0x2440 == index) && (0x04 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuUpdateFrequency);
            valid = true;
         }
         else if ((0x2441 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)3);
            valid = true;
         }
         else if ((0x2441 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuRollValue);
            valid = true;
         }
         else if ((0x2441 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuPitchValue);
            valid = true;
         }
         else if ((0x2441 == index) && (0x03 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuYawValue);
            valid = true;
         }
         else if ((0x2443 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if ((0x2443 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuTemperature);
            valid = true;
         }
         else if ((0x2443 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.MainBoardImuErrorTemperature);
            valid = true;
         }
         #endregion
         #region Target Board IMU
         else if ((0x2444 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuConfiguration);
            valid = true;
         }
         else if ((0x2444 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuAxisRemap);
            valid = true;
         }
         else if ((0x2444 == index) && (0x03 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuUnitSelection);
            valid = true;
         }
         else if ((0x2444 == index) && (0x04 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuUpdateFrequency);
            valid = true;
         }
         else if ((0x2445 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)3);
            valid = true;
         }
         else if ((0x2445 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuRollValue);
            valid = true;
         }
         else if ((0x2445 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuPitchValue);
            valid = true;
         }
         else if ((0x2445 == index) && (0x03 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuYawValue);
            valid = true;
         }
         else if ((0x2446 == index) && (0x00 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, (byte)2);
            valid = true;
         }
         else if ((0x2446 == index) && (0x01 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuTemperature);
            valid = true;
         }
         else if ((0x2446 == index) && (0x02 == subIndex))
         {
            dataLength = this.MoveDeviceData(buffer, this.TargetBoardImuErrorTemperature);
            valid = true;
         }
         #endregion

         if (false == valid)
         {
            for (int i = 0; i < this.motors.Length; i++)
            {
               if (false != valid)
               {
                  break;
               }

               valid = this.motors[i].LoadDeviceData(index, subIndex, buffer, ref dataLength);
            }
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

         if ((0x1006 == index) && (4 == length))
         {
            this.CommunicationCyclePeriod = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x1007 == index) && (4 == length))
         {
            this.SynchronousWindowLength = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x1015 == index) && (2 == length))
         {
            this.EmergencyInhibitTime = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x1016 == index) && (0x01 == subIndex) && (4 == length))
         {
            this.ConsumerHeartbeatTime = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x1017 == index) && (2 == length))
         {
            this.ProducerHeartbeatTime = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x1400 <= index) && (0x1600 > index))
         {
            int pdoMapOffset = (index - 0x1400);

            if (pdoMapOffset < this.rpdoMapping.Length)
            {
               UInt32 value = this.ExtractUInt32(buffer, offset, length);
               valid = this.rpdoMapping[pdoMapOffset].StoreParameterData(subIndex, (int)length, value);
            }
         }
         else if ((0x1600 <= index) && (0x1800 > index))
         {
            int pdoMapOffset = (index - 0x1600);

            if (pdoMapOffset < this.rpdoMapping.Length)
            {
               UInt32 value = this.ExtractUInt32(buffer, offset, length);
               valid = this.rpdoMapping[pdoMapOffset].StoreMapData(subIndex, (int)length, value);
            }
         }
         else if ((0x1800 <= index) && (0x1A00 > index))
         {
            int pdoMapOffset = (index - 0x1800);

            if (pdoMapOffset < this.tpdoMapping.Length)
            {
               UInt32 value = this.ExtractUInt32(buffer, offset, length);
               valid = this.tpdoMapping[pdoMapOffset].StoreParameterData(subIndex, (int)length, value);
            }
         }
         else if ((0x1A00 <= index) && (0x1C00 > index))
         {
            int pdoMapOffset = (index - 0x1A00);

            if (pdoMapOffset < this.tpdoMapping.Length)
            {
               UInt32 value = this.ExtractUInt32(buffer, offset, length);
               valid = this.tpdoMapping[pdoMapOffset].StoreMapData(subIndex, (int)length, value);
            }
         }
         else if ((0x2301 == index) && (1 == length))
         {
            byte selectedValue = buffer[offset];

            if ((selectedValue >= 0) && (selectedValue <= 4))
            {
               this.CameraSelect = selectedValue;
               valid = true;
            }
         }
         else if ((0x2302 == index) && (0x01 == subIndex) && (4 == length))
         {
            this.CameraLedDefaultIntensity = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x2303 == index) && (0x01 == subIndex) && (4 == length))
         {
            this.CameraLedIntensity = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x2304 == index) && (0x01 == subIndex) && (1 == length))
         {
            this.CameraLedChannelMask = buffer[offset];
            valid = true;
         }
         else if ((0x2310 == index) && (1 == length))
         {
            byte value = buffer[offset];

            if ((value >= 0) && (value <= 3))
            {
               this.Outputs = value;
               valid = true;
            }
         }
         else if ((0x2311 == index) && (0x02 == subIndex) && (1 == length))
         {
            this.McuErrorTemperature = buffer[offset];
            valid = true;
         }
         else if ((0x2400 == index) && (1 == length))
         {
            byte value = buffer[offset];

            if ((0 == value) || (1 == value))
            {
               this.LaserAimEnable = value;
               valid = true;
            }
         }
         else if ((0x2401 == index) && (1 == length))
         {
            byte value = buffer[offset];

            if ((value >= 0) && (value <= 25))
            {
               this.LaserTimeToMeasure = value;
               valid = true;
            }
         }
         else if ((0x2403 == index) && (0x02 == subIndex) && (1 == length))
         {
            this.LaserErrorTemperature = buffer[offset];
            valid = true;
         }
         else if ((0x2404 == index) && (1 == length))
         {
            this.LaserControlByte = buffer[offset];
            valid = true;
         }
         else if ((0x2411 == index) && (0x02 == subIndex) && (1 == length))
         {
            this.LaserScannerErrorTemperature = buffer[offset];
            valid = true;
         }
         #region Main Board IMU
         else if ((0x2440 == index) && (0x01 == subIndex) && (4 == length))
         {
            this.MainBoardImuConfiguration = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x2440 == index) && (0x02 == subIndex) && (2 == length))
         {
            UInt16 value = BitConverter.ToUInt16(buffer, offset);

            if ((value >= 0) && (value <= 0x01FF))
            {
               this.MainBoardImuAxisRemap = value;
               valid = true;
            }
         }
         else if ((0x2440 == index) && (0x03 == subIndex) && (1 == length))
         {
            this.MainBoardImuUnitSelection = buffer[offset];
            valid = true;
         }
         else if ((0x2440 == index) && (0x04 == subIndex) && (2 == length))
         {
            this.MainBoardImuUpdateFrequency = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2443 == index) && (0x02 == subIndex) && (1 == length))
         {
            this.MainBoardImuErrorTemperature = buffer[offset];
            valid = true;
         }
         #endregion
         #region Target Board IMU
         else if ((0x2444 == index) && (0x01 == subIndex) && (4 == length))
         {
            this.TargetBoardImuConfiguration = BitConverter.ToUInt32(buffer, offset);
            valid = true;
         }
         else if ((0x2444 == index) && (0x02 == subIndex) && (2 == length))
         {
            UInt16 value = BitConverter.ToUInt16(buffer, offset);

            if ((value >= 0) && (value <= 0x01FF))
            {
               this.TargetBoardImuAxisRemap = value;
               valid = true;
            }
         }
         else if ((0x2444 == index) && (0x03 == subIndex) && (1 == length))
         {
            this.TargetBoardImuUnitSelection = buffer[offset];
            valid = true;
         }
         else if ((0x2444 == index) && (0x04 == subIndex) && (2 == length))
         {
            this.TargetBoardImuUpdateFrequency = BitConverter.ToUInt16(buffer, offset);
            valid = true;
         }
         else if ((0x2446 == index) && (0x02 == subIndex) && (1 == length))
         {
            this.TargetBoardImuErrorTemperature = buffer[offset];
            valid = true;
         }
         #endregion

         if (false == valid)
         {
            for (int i = 0; i < this.motors.Length; i++)
            {
               if (false != valid)
               {
                  break;
               }

               valid = this.motors[i].StoreDeviceData(index, subIndex, buffer, offset, length);
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
         this.rpdoMapping[0].StoreFrameData(frame);
      }

      private void ProcessPdo2Message(byte[] frame)
      {
         this.rpdoMapping[1].StoreFrameData(frame);
      }

      private void ProcessPdo3Message(byte[] frame)
      {
         this.rpdoMapping[2].StoreFrameData(frame);
      }

      private void ProcessPdo4Message(byte[] frame)
      {
         this.rpdoMapping[3].StoreFrameData(frame);
      }

      #endregion

      #region User Events

      private void SetMcuTemperatureButton_Click(object sender, EventArgs e)
      {
         double temperature = 0;

         if (double.TryParse(this.McuTemperatureTextBox.Text, out temperature) != false)
         {
            this.McuTemperature = (byte)temperature;
         }
      }

      private void SetMainBoardImuDirectionsButton_Click(object sender, EventArgs e)
      {
         double roll = 0;
         double pitch = 0;
         double yaw = 0;

         if ((double.TryParse(this.MainBoardImuRollTextBox.Text, out roll) != false) &&
             (double.TryParse(this.MainBoardImuPitchTextBox.Text, out pitch) != false) &&
             (double.TryParse(this.MainBoardImuYawTextBox.Text, out yaw) != false))
         {
            this.MainBoardImuRollValue = (Int16)(roll * 16);
            this.MainBoardImuPitchValue = (Int16)(pitch * 16);
            this.MainBoardImuYawValue = (Int16)(yaw * 16);
         }
      }
      
      private void SetTargetBoardImuDirectionsButton_Click(object sender, EventArgs e)
      {
         double roll = 0;
         double pitch = 0;
         double yaw = 0;

         if ((double.TryParse(this.TargetBoardImuRollTextBox.Text, out roll) != false) &&
             (double.TryParse(this.TargetBoardImuPitchTextBox.Text, out pitch) != false) &&
             (double.TryParse(this.TargetBoardImuYawTextBox.Text, out yaw) != false))
         {
            this.TargetBoardImuRollValue = (Int16)(roll * 16);
            this.TargetBoardImuPitchValue = (Int16)(pitch * 16);
            this.TargetBoardImuYawValue = (Int16)(yaw * 16);
         }
      }

      private void SetLaserMeasuredDistanceButton_Click(object sender, EventArgs e)
      {
         this.LaserMeasuredDistance = this.GetLaserMeasuredDistance();
      }

      private void SerLaserRangeTemperatureButton_Click(object sender, EventArgs e)
      {
         double temperature = 0;

         if (double.TryParse(this.LaserRangeTemperatureTextBox.Text, out temperature) != false)
         {
            this.LaserTemperature = (byte)temperature;
         }
      }

      private void SetLaserScannerPositionButton_Click(object sender, EventArgs e)
      {
         byte laserScannerPosition = 0;

         if (byte.TryParse(this.LaserScannerPositionTextBox.Text,System.Globalization.NumberStyles.HexNumber, null, out laserScannerPosition) != false)
         {
            this.LaserScannerPosition = laserScannerPosition;
         }
      }

      private void SetLaserScannerTemperatureButton_Click(object sender, EventArgs e)
      {
         double temperature = 0;

         if (double.TryParse(this.LaserScannerTemperatureTextBox.Text, out temperature) != false)
         {
            this.LaserScannerTemperature = (byte)temperature;
         }
      }

      private void SetDcVoltageByteButton_Click(object sender, EventArgs e)
      {
         byte dcLinkVoltageByte = 0;

         if (byte.TryParse(this.DcVoltageByteTextBox.Text, out dcLinkVoltageByte) != false)
         {
            this.DcLinkVoltageByte = dcLinkVoltageByte;
         }
      }      

      #endregion

      #region Control Events

      private void ProcessImagePanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.DescriptionTextBox.Focus();
         this.CommunicationProcessImagePanel.VerticalScroll.Value = e.NewValue;
      }

      private void CameraProcessImagePanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.DescriptionTextBox.Focus();
         this.CameraProcessImagePanel.VerticalScroll.Value = e.NewValue;
      }

      private void ImuProcessImagePanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.DescriptionTextBox.Focus();
         this.ImuProcessImagePanel.VerticalScroll.Value = e.NewValue;
      }

      private void LaserRangeFinderProcessImagePanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.DescriptionTextBox.Focus();
         this.LaserRangeFinderProcessImagePanel.VerticalScroll.Value = e.NewValue;
      }

      private void LaserScannerProcessImagePanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.DescriptionTextBox.Focus();
         this.LaserScannerProcessImagePanel.VerticalScroll.Value = e.NewValue;
      }

      private void McuProcessImagePanel_Scroll(object sender, ScrollEventArgs e)
      {
         this.DescriptionTextBox.Focus();
         this.McuProcessImagePanel.VerticalScroll.Value = e.NewValue;
      }

      #endregion

      #region Constructor

      public UlcRoboticsE4Main()
      {
         this.InitializeComponent();

         this.deviceType = 0xFFFF0191;
         this.ErrorRegister = 0;
         this.manufacturerDeviceName = "E4 Main";
         this.manufacturerHardwareVersion = "1.00";
         this.manufacturerSoftwareVersion = "1.00";

         this.nvNodeId = 32;

         this.rpdoMapping = new RPDOMapping[4];
         this.tpdoMapping = new TPDOMapping[8];

         for (int i = 0; i < this.rpdoMapping.Length; i++)
         {
            this.rpdoMapping[i] = new RPDOMapping();
            this.rpdoMapping[i].OnPdoMappable = new RPDOMapping.PdoMappableHandler(this.RPdoMappableHandler);
            this.rpdoMapping[i].OnPdoSize = new RPDOMapping.PdoSizeHandler(this.RPdoSizeHandler);
            this.rpdoMapping[i].OnPdoData = new RPDOMapping.PdoDataHandler(this.RPdoDataHandler);
         }

         for (int i = 0; i < this.tpdoMapping.Length; i++)
         {
            this.tpdoMapping[i] = new TPDOMapping();
            this.tpdoMapping[i].OnPdoMappable = new TPDOMapping.PdoMappableHandler(this.TPdoMappableHandler);
            this.tpdoMapping[i].OnPdoSize = new TPDOMapping.PdoSizeHandler(this.TPdoSizeHandler);
            this.tpdoMapping[i].OnPdoData = new TPDOMapping.PdoDataHandler(this.TPdoDataHandler);
         }

         this.NodeIdTextBox.Text = this.nvNodeId.ToString();

         this.motors = new MainMotor[4];
         this.motors[0] = this.Motor0Motor;
         this.motors[1] = this.Motor1Motor;
         this.motors[2] = this.Motor2Motor;
         this.motors[3] = this.Motor3Motor;

         for (int i = 0; i < this.motors.Length; i++)
         {
            this.motors[i].OnTpdoCheck = new MainMotor.TpdoCheckHandler(this.MotorTpdoCheck);
         }


         this.motors[0].SupportVelocityMode = true;
         this.motors[0].SupportPositionMode = true;
         this.motors[0].SupportHomingMode = false;

         this.motors[1].SupportVelocityMode = true;
         this.motors[1].SupportPositionMode = true;
         this.motors[1].SupportHomingMode = false;

         this.motors[2].SupportVelocityMode = false;
         this.motors[2].SupportPositionMode = true;
         this.motors[2].SupportHomingMode = true;

         this.motors[3].SupportVelocityMode = false;
         this.motors[3].SupportPositionMode = true;
         this.motors[3].SupportHomingMode = true;


         this.motors[0].MotorAbortConnectionOptionLocation = 0x600700;
         this.motors[0].MotorErrorCodeLocation = 0x603F00;
         this.motors[0].DigitalInputsLocation = 0x60FD00;
         this.motors[0].DigitalOutputsHighestLocation = 0x60FE00;
         this.motors[0].DigitalOutputsLocation = 0x60FE01;
         this.motors[0].DigitalOutputsMaskLocation = 0x60FE02;
         this.motors[0].MotorTypeLocation = 0x640200;
         this.motors[0].MotorTemperatureHighestLocation = 0x641000;
         this.motors[0].MotorTemperatureLocation = 0x641001;
         this.motors[0].MotorErrorTemperatureLocation = 0x641002;
         this.motors[0].MotorSupportedDriveModesLocation = 0x650200;
         this.motors[0].SingleDeviceTypeLocation = 0x67FF00;

         this.motors[0].ControlWordLocation = 0x604000;
         this.motors[0].StatusWordLocation = 0x604100;
         this.motors[0].SetModeLocation = 0x606000;
         this.motors[0].GetModeLocation = 0x606100;

         this.motors[0].HomeOffsetLocation = 0x607C00;
         this.motors[0].HomingMethodLocation = 0x609800;
         this.motors[0].HomingSpeedHighestLocation = 0x609900;
         this.motors[0].HomingSwitchSpeedLocation = 0x609901;
         this.motors[0].HomingZeroSpeedLocation = 0x609902;
         this.motors[0].HomingAccelerationLocation = 0x609A00;

         this.motors[0].PositionActualValueLocation = 0x606400;
         this.motors[0].PositionWindowLocation = 0x606700;
         this.motors[0].PositionWindowTimeLocation = 0x606800;
         this.motors[0].PositionControlParameterHighestLocation = 0x60FB00;
         this.motors[0].PositionProportionalGainCoefficientKpLocation = 0x60FB01;
         this.motors[0].PositionIntegralGainCoefficienKiLocation = 0x60FB02;
         this.motors[0].PositionDerivativeGainCoefficientKdLocation = 0x60FB03;

         this.motors[0].TargetPositionLocation = 0x607A00;
         this.motors[0].ProfileVelocityLocation = 0x608100;
         this.motors[0].ProfileAccelerationLocation = 0x608300;
         this.motors[0].ProfileDecelerationLocation = 0x608400;

         this.motors[0].TargetTorqueLocation = 0x607100;
         this.motors[0].CurrentActualValueLocation = 0x607800;

         this.motors[0].VelocityActualValueLocation = 0x606C00;
         this.motors[0].VelocityWindowLocation = 0x606D00;
         this.motors[0].VelocityWindowTimeLocation = 0x606E00;
         this.motors[0].VelocityThresholdLocation = 0x606F00;
         this.motors[0].VelocityThresholdTimeLocation = 0x607000;
         this.motors[0].VelocityControlParameterHighestLocation = 0x60F900;
         this.motors[0].VelocityProportionalGainCoefficientKpLocation = 0x60F901;
         this.motors[0].VelocityIntegralGainCoefficienKiLocation = 0x60F902;
         this.motors[0].VelocityDerivativeGainCoefficientKdLocation = 0x60F903;
         this.motors[0].TargetVelocityLocation = 0x60FF00;

         for (int i = 1; i < this.motors.Length; i++)
         {
            this.motors[i].MotorAbortConnectionOptionLocation = this.motors[i - 1].MotorAbortConnectionOptionLocation + 0x080000;
            this.motors[i].MotorErrorCodeLocation = this.motors[i - 1].MotorErrorCodeLocation + 0x080000;
            this.motors[i].DigitalInputsLocation = this.motors[i - 1].DigitalInputsLocation + 0x080000;
            this.motors[i].DigitalOutputsHighestLocation = this.motors[i - 1].DigitalOutputsHighestLocation + 0x080000;
            this.motors[i].DigitalOutputsLocation = this.motors[i - 1].DigitalOutputsLocation + 0x080000;
            this.motors[i].DigitalOutputsMaskLocation = this.motors[i - 1].DigitalOutputsMaskLocation + 0x080000;
            this.motors[i].MotorTypeLocation = this.motors[i - 1].MotorTypeLocation + 0x080000;

            if (1 == i)
            {
               this.motors[i].MotorTemperatureHighestLocation = this.motors[i - 1].MotorTemperatureHighestLocation + 0x080000;
               this.motors[i].MotorTemperatureLocation = this.motors[i - 1].MotorTemperatureLocation + 0x080000;
               this.motors[i].MotorErrorTemperatureLocation = this.motors[i - 1].MotorErrorTemperatureLocation + 0x080000;
            }
            
            this.motors[i].MotorSupportedDriveModesLocation = this.motors[i - 1].MotorSupportedDriveModesLocation + 0x080000;
            this.motors[i].SingleDeviceTypeLocation = this.motors[i - 1].SingleDeviceTypeLocation + 0x080000;

            this.motors[i].ControlWordLocation = this.motors[i - 1].ControlWordLocation + 0x080000;
            this.motors[i].StatusWordLocation = this.motors[i - 1].StatusWordLocation + 0x080000;
            this.motors[i].SetModeLocation = this.motors[i - 1].SetModeLocation + 0x080000;
            this.motors[i].GetModeLocation = this.motors[i - 1].GetModeLocation + 0x080000;

            this.motors[i].HomeOffsetLocation = this.motors[i - 1].HomeOffsetLocation + 0x080000;
            this.motors[i].HomingMethodLocation = this.motors[i - 1].HomingMethodLocation + 0x080000;
            this.motors[i].HomingSpeedHighestLocation = this.motors[i - 1].HomingSpeedHighestLocation + 0x080000;
            this.motors[i].HomingSwitchSpeedLocation = this.motors[i - 1].HomingSwitchSpeedLocation + 0x080000;
            this.motors[i].HomingZeroSpeedLocation = this.motors[i - 1].HomingZeroSpeedLocation + 0x080000;
            this.motors[i].HomingAccelerationLocation = this.motors[i - 1].HomingAccelerationLocation + 0x080000;

            this.motors[i].PositionActualValueLocation = this.motors[i - 1].PositionActualValueLocation + 0x080000;

            if (1 == i)
            {
               this.motors[i].PositionWindowLocation = this.motors[i - 1].PositionWindowLocation + 0x080000;
               this.motors[i].PositionWindowTimeLocation = this.motors[i - 1].PositionWindowTimeLocation + 0x080000;
               this.motors[i].PositionControlParameterHighestLocation = this.motors[i - 1].PositionControlParameterHighestLocation + 0x080000;
               this.motors[i].PositionProportionalGainCoefficientKpLocation = this.motors[i - 1].PositionProportionalGainCoefficientKpLocation + 0x080000;
               this.motors[i].PositionIntegralGainCoefficienKiLocation = this.motors[i - 1].PositionIntegralGainCoefficienKiLocation + 0x080000;
               this.motors[i].PositionDerivativeGainCoefficientKdLocation = this.motors[i - 1].PositionDerivativeGainCoefficientKdLocation + 0x080000;
            }

            this.motors[i].TargetPositionLocation = this.motors[i - 1].TargetPositionLocation + 0x080000;
            this.motors[i].ProfileVelocityLocation = this.motors[i - 1].ProfileVelocityLocation + 0x080000;
            this.motors[i].ProfileAccelerationLocation = this.motors[i - 1].ProfileAccelerationLocation + 0x080000;
            this.motors[i].ProfileDecelerationLocation = this.motors[i - 1].ProfileDecelerationLocation + 0x080000;

            this.motors[i].TargetTorqueLocation = this.motors[i - 1].TargetTorqueLocation + 0x080000;
            this.motors[i].CurrentActualValueLocation = this.motors[i - 1].CurrentActualValueLocation + 0x080000;


            this.motors[i].VelocityActualValueLocation = this.motors[i - 1].VelocityActualValueLocation + 0x080000;

            if (1 == i)
            {
               this.motors[i].VelocityWindowLocation = this.motors[i - 1].VelocityWindowLocation + 0x080000;
               this.motors[i].VelocityWindowTimeLocation = this.motors[i - 1].VelocityWindowTimeLocation + 0x080000;
               this.motors[i].VelocityThresholdLocation = this.motors[i - 1].VelocityThresholdLocation + 0x080000;
               this.motors[i].VelocityThresholdTimeLocation = this.motors[i - 1].VelocityThresholdTimeLocation + 0x080000;

               this.motors[i].VelocityControlParameterHighestLocation = this.motors[i - 1].VelocityControlParameterHighestLocation + 0x080000;
               this.motors[i].VelocityProportionalGainCoefficientKpLocation = this.motors[i - 1].VelocityProportionalGainCoefficientKpLocation + 0x080000;
               this.motors[i].VelocityIntegralGainCoefficienKiLocation = this.motors[i - 1].VelocityIntegralGainCoefficienKiLocation + 0x080000;
               this.motors[i].VelocityDerivativeGainCoefficientKdLocation = this.motors[i - 1].VelocityDerivativeGainCoefficientKdLocation + 0x080000;
            }

            this.motors[i].TargetVelocityLocation = this.motors[i - 1].TargetVelocityLocation + 0x080000;
         }


         for (int i = 0; i < this.motors.Length; i++)
         {
            this.motors[i].CreateLabels();         
         }

         this.laserReadings = new UInt32[128];


         this.Motor0Motor.Dock = DockStyle.Fill;
         this.Motor1Motor.Dock = DockStyle.Fill;
         this.Motor2Motor.Dock = DockStyle.Fill;
         this.Motor3Motor.Dock = DockStyle.Fill;
         this.CommunicationProcessImagePanel.Dock = DockStyle.Fill;
         this.CameraProcessImagePanel.Dock = DockStyle.Fill;
         this.ImuProcessImagePanel.Dock = DockStyle.Fill;
         this.LaserRangeFinderProcessImagePanel.Dock = DockStyle.Fill;
         this.LaserScannerProcessImagePanel.Dock = DockStyle.Fill;
         this.McuProcessImagePanel.Dock = DockStyle.Fill;
      }

      #endregion

      #region Access Methods

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

         keyValue = appKey.GetValue(deviceTag + "nvNodeId");
         this.nvNodeId = (byte)((null != keyValue) ? (byte.TryParse(keyValue.ToString(), out parsedValue) ? parsedValue : 31) : 31);
         this.NodeIdTextBox.Text = this.nvNodeId.ToString();
      }

      public override void SaveRegistry(RegistryKey appKey, string deviceTag)
      {
         appKey.SetValue(deviceTag + "Enabled", this.EnabledCheckBox.Checked ? "1" : "0");
         appKey.SetValue(deviceTag + "Description", this.DescriptionTextBox.Text);
         appKey.SetValue(deviceTag + "BusId", this.GetBusId());
         appKey.SetValue(deviceTag + "nvNodeId", this.nvNodeId);
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
         else if ("nvNodeId" == name)
         {
            byte.TryParse(reader.Value, out this.nvNodeId);
            this.NodeIdTextBox.Text = this.nvNodeId.ToString();
         }
      }

      public override void Write(XmlWriter writer)
      {
         writer.WriteElementString("Enabled", (false != this.EnabledCheckBox.Checked) ? "1" : "0");
         writer.WriteElementString("Description", this.DescriptionTextBox.Text);
         writer.WriteElementString("BusId", this.GetBusId());
         writer.WriteElementString("nvNodeId", this.nvNodeId.ToString());
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
            else if (COBTypes.SYNC == frameType)
            {
               for (int i = 0; i < this.tpdoMapping.Length; i++)
               {
                  this.tpdoMapping[i].SyncReceived();
               }

               for (int i = 0; i < this.rpdoMapping.Length; i++)
               {
                  this.rpdoMapping[i].SyncReceived();
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
               if ((0 != this.ConsumerHeartbeatTime) &&
                   (nodeId == this.workingConsumerHeartbeatNode))
               {
                  this.workingConsumerHeartbeatActive = true;
                  this.workingConsumerHeartbeatTimeLimit = DateTime.Now.AddMilliseconds(this.workingConsumerHeartbeatTime);
               }
            }
         }
      }

      public override void UpdateDevice()
      {
         if (false != this.active)
         {
            DateTime now = DateTime.Now;

            if ((false != this.workingConsumerHeartbeatActive) &&
                (now > this.workingConsumerHeartbeatTimeLimit) &&
                (DeviceStates.preop != this.deviceState))
            {
               this.Reset(false);
            }

            if (0 != this.ProducerHeartbeatTime)
            {
               if (now > this.workingProducerHeartbeatTimeLimit)
               {
                  this.workingProducerHeartbeatTimeLimit = this.workingProducerHeartbeatTimeLimit.AddMilliseconds(this.ProducerHeartbeatTime);

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

            for (int i = 0; i < this.motors.Length; i++)
            {
               this.motors[i].UpdateDevice();
            }

            if (DeviceStates.running == this.deviceState)
            {
               #region RPDO Activation Check
               
               for (int i = 0; i < this.rpdoMapping.Length; i++)
               {
                  this.rpdoMapping[i].Update();
               }
               
               #endregion

               #region Laser Measurements

               byte laserControlCache = this.LaserControlByte;
               this.activeLaserControlByte = laserControlCache;

               bool laserControl = ((0x80 & laserControlCache) != 0) ? true : false;

               if (false == this.laserMeasureActive)
               {
                  if (false == laserControl)
                  {
                     if (1 != this.LaserMeasuredDistanceHighest)
                     {
                        this.ResetLaserMeasurements();
                     }
                  }
                  else if (false == this.laserMeasureExecuted)
                  {
                     this.activeLaserTimeToMeasure = (0 == this.LaserTimeToMeasure) ? 1 : this.LaserTimeToMeasure;
                     this.laserReadingCount = 0;
                     this.LaserMeasuredDistanceHighest = (byte)(this.laserReadingCount + 1);
                     this.laserReadingLimit = (laserControlCache & 0x7F) + 1;
                     this.laserReadingTimeLimit = now.AddMilliseconds(this.activeLaserTimeToMeasure * 150);
                     this.laserMeasureActive = true;

                     this.LaserStatusByte = 0x02;
                  }
               }
               else
               {
                  if (false == laserControl)
                  {
                     this.ResetLaserMeasurements();
                  }
                  else if (now > this.laserReadingTimeLimit)
                  {
                     this.laserReadings[this.laserReadingCount] = this.GetLaserMeasuredDistance();
                     this.laserReadingCount++;
                     this.LaserMeasuredDistanceHighest = (byte)(this.laserReadingCount + 1);

                     if (this.laserReadingCount < this.laserReadingLimit)
                     {
                        this.laserReadingTimeLimit = this.laserReadingTimeLimit.AddMilliseconds(this.activeLaserTimeToMeasure * 150);
                     }
                     else
                     {
                        UInt64 laserTotal = 0;

                        for (int i = 0; i < this.laserReadingLimit; i++)
                        {
                           laserTotal += this.laserReadings[i];
                        }

                        this.LaserMeasuredDistance = (UInt32)(laserTotal / (UInt32)this.laserReadingLimit);
                        this.laserMeasureActive = false;
                        this.laserMeasureExecuted = true;
                        this.LaserStatusByte = 0x01;
                     }
                  }
               }

               #endregion

               #region TPDO Activation Check

               for (int i = 0; i < this.tpdoMapping.Length; i++)
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

               #endregion
            }
            else
            {
            }
         }
      }

      #endregion

   }
}
