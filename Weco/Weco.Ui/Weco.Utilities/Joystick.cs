
namespace Weco.Utilities
{
   using System;
   using System.Threading;
   using System.Runtime.InteropServices;
   using System.Security;

   public class Joystick
   {
      #region WINMM library definitions

      private const String WINMM_NATIVE_LIBRARY = "winmm.dll";
      private const CallingConvention CALLING_CONVENTION = CallingConvention.StdCall;
      
      private const int JOY_RETURNX = 0x00000001;
      private const int JOY_RETURNY = 0x00000002;
      private const int JOY_RETURNZ = 0x00000004;
      private const int JOY_RETURNR = 0x00000008;
      private const int JOY_RETURNU = 0x00000010;
      private const int JOY_RETURNV = 0x00000020;
      private const int JOY_RETURNPOV = 0x00000040;
      private const int JOY_RETURNBUTTONS = 0x00000080;
      private const int JOY_RETURNRAWDATA = 0x00000100;
      private const int JOY_RETURNPOVCTS = 0x00000200;
      private const int JOY_RETURNCENTERED = 0x00000400;
      private const int JOY_USEDEADZONE = 0x00000800;

      private const int JOY_RETURNALL = (JOY_RETURNX | JOY_RETURNY | JOY_RETURNZ | JOY_RETURNR | JOY_RETURNU | JOY_RETURNV | JOY_RETURNPOV | JOY_RETURNBUTTONS);

      [StructLayout(LayoutKind.Sequential)]
      public struct JOYINFOEX
      {
         public Int32 dwSize; // Size, in bytes, of this structure.
         public Int32 dwFlags; // Flags indicating the valid information returned in this structure.
         public Int32 dwXpos; // Current X-coordinate.
         public Int32 dwYpos; // Current Y-coordinate.
         public Int32 dwZpos; // Current Z-coordinate.
         public Int32 dwRpos; // Current position of the rudder or fourth joystick axis.
         public Int32 dwUpos; // Current fifth axis position.
         public Int32 dwVpos; // Current sixth axis position.
         public Int32 dwButtons; // Current state of the 32 joystick buttons (bits)
         public Int32 dwButtonNumber; // Current button number that is pressed.
         public Int32 dwPOV; // Current position of the point-of-view control (0..35,900, deg*100)
         public Int32 dwReserved1; // Reserved; do not use.
         public Int32 dwReserved2; // Reserved; do not use.
      }
      
      [StructLayout(LayoutKind.Sequential)]
      public struct JOYINFO
      {
         public Int32 wXpos; // Current X-coordinate.
         public Int32 wYpos; // Current Y-coordinate.
         public Int32 wZpos; // Current Z-coordinate.
         public Int32 wButtons; // Current state of joystick buttons.
      }

      [DllImport(WINMM_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
      public static extern Int32 joyGetNumDevs();

      [DllImport(WINMM_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
      public static extern Int32 joyGetPos(Int32 uJoyID, ref JOYINFO pji);
      
      [DllImport(WINMM_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
      public static extern Int32 joyGetPosEx(Int32 uJoyID, ref JOYINFOEX pji);
      
      #endregion

      #region Fields

      private static Joystick instance = null;

      //private JOYINFO js;
      private JOYINFOEX jsx;
      //private Boolean joyEx;
      //private Int32 joyId;

      private bool valid;
      private string faultReason;

      private ushort xaxis;
      private ushort yaxis;
      private ushort zaxis;
      private ushort throttle;

      private bool button1Pressed;
      private bool button2Pressed;
      private bool button3Pressed;
      private bool button4Pressed;
      private bool button6Pressed;
      private bool button7Pressed;
      private bool button8Pressed;
      private bool button9Pressed;
      private bool button10Pressed;
      private bool button11Pressed;
      private bool button12Pressed;

      private bool povPressed;
      private int povValue;

      #endregion

      #region Properties

      public static Joystick Instance
      {
         get
         {
            if (instance == null)
            {
               instance = new Joystick();
               instance.Initialize();
            }

            return instance;
         }
      }

      public bool Valid
      {
         get { return (this.valid); }
      }

      public ushort XAxis
      {
         get { return (this.xaxis); }
      }

      public ushort YAxis
      {
         get { return (this.yaxis); }
      }

      public ushort ZAxis
      {
         get { return (this.zaxis); }
      }

      public ushort Throttle
      {
         get { return (this.throttle); }
      }

      public bool Button1Pressed
      {
         get { return (this.button1Pressed); }
      }

      public bool Button2Pressed
      {
         get { return (this.button2Pressed); }
      }

      public bool Button3Pressed
      {
         get { return (this.button3Pressed); }
      }

      public bool Button4Pressed
      {
         get { return (this.button4Pressed); }
      }

      public bool Button6Pressed
      {
         get { return (this.button6Pressed); }
      }

      public bool Button7Pressed
      {
         get { return (this.button7Pressed); }
      }

      public bool Button8Pressed
      {
         get { return (this.button8Pressed); }
      }

      public bool Button9Pressed
      {
         get { return (this.button9Pressed); }
      }

      public bool Button10Pressed
      {
         get { return (this.button10Pressed); }
      }

      public bool Button11Pressed
      {
         get { return (this.button11Pressed); }
      }

      public bool Button12Pressed
      {
         get { return (this.button12Pressed); }
      }

      public bool PovPressed
      {
         get { return (this.povPressed); }
      }

      public int PovValue
      {
         get { return (this.povValue); }
      }

      public string FaultReason { get { return (this.faultReason); } }

      #endregion

      #region Helper Functions
            
      private void EvaluateButton(int mask, int status, ref bool pressed)
      {
         if ((mask & status) != 0)
         {
            if (false == pressed)
            {
               pressed = true;
            }
         }
         else
         {
            if (false != pressed)
            {
               pressed = false;
            }
         }
      }

      private bool GetPosition()
      {
         bool result = false;

         if (joyGetPosEx(0, ref jsx) == 0)
         {
            this.xaxis = (ushort)jsx.dwXpos;
            this.yaxis = (ushort)jsx.dwYpos;
            this.zaxis = (ushort)jsx.dwRpos;
            this.throttle = (ushort)jsx.dwZpos;

            this.EvaluateButton(1, jsx.dwButtons, ref this.button1Pressed);
            this.EvaluateButton(2, jsx.dwButtons, ref this.button2Pressed);
            this.EvaluateButton(4, jsx.dwButtons, ref this.button3Pressed);
            this.EvaluateButton(8, jsx.dwButtons, ref this.button4Pressed);
            this.EvaluateButton(32, jsx.dwButtons, ref this.button6Pressed);
            this.EvaluateButton(64, jsx.dwButtons, ref this.button7Pressed);
            this.EvaluateButton(128, jsx.dwButtons, ref this.button8Pressed);
            this.EvaluateButton(256, jsx.dwButtons, ref this.button9Pressed);
            this.EvaluateButton(512, jsx.dwButtons, ref this.button10Pressed);            
            this.EvaluateButton(1024, jsx.dwButtons, ref this.button11Pressed);
            this.EvaluateButton(2048, jsx.dwButtons, ref this.button12Pressed);
            this.povPressed = (65535 != jsx.dwPOV) ? true : false;
            this.povValue = (((jsx.dwPOV / 100) + 44) / 45) * 45;

            this.faultReason = null;
            result = true;
         }
         else
         {
            this.faultReason = "missing";
         }

         return (result);
      }

      private void Initialize()
      {
         this.jsx = new JOYINFOEX();
         this.jsx.dwSize = Marshal.SizeOf(jsx);
         this.jsx.dwFlags = JOY_RETURNALL;
         
         this.valid = GetPosition();
      }

      #endregion

      #region Constructor

      private Joystick()
      {
      }

      #endregion

      #region Access Methods

      public void Update()
      {
         this.valid = this.GetPosition();
      }

      #endregion

   }
}
