
namespace UlcRobotics.Ui.Controls
{
   using System;
   using System.Runtime.InteropServices;

   public class user32
   {
      public const int TOUCHEVENTF_MOVE = 0x0001;
      public const int TOUCHEVENTF_DOWN = 0x0002;
      public const int TOUCHEVENTF_UP = 0x0004;
      public const int TOUCHEVENTF_PRIMARY = 0x0010;

      [StructLayout(LayoutKind.Sequential)]
      public struct TOUCHINPUT
      {
         public int x;
         public int y;
         public System.IntPtr hSource;
         public int dwID;
         public int dwFlags;
         public int dwMask;
         public int dwTime;
         public System.IntPtr dwExtraInfo;
         public int cxContact;
         public int cyContact;
      }

      // Currently touch/multitouch access is done through unmanaged code
      // We must p/invoke into user32 [winuser.h]
      [DllImport("user32")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool RegisterTouchWindow(System.IntPtr hWnd, uint ulFlags);

      [DllImport("user32")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool GetTouchInputInfo(System.IntPtr hTouchInput, int cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);

      [DllImport("user32")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern void CloseTouchInputHandle(System.IntPtr lParam);

      [DllImport("user32.dll")]
      public static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);
   }
}