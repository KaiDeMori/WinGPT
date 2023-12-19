﻿using System.Runtime.InteropServices;

namespace WinGPT;

public static class FlashWindowExtension {
   // To support flashing.
   [DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl)]
   [return: MarshalAs(UnmanagedType.Bool)]
   private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

   //Flash both the window caption and taskbar button.
   //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
   private const uint FLASHW_ALL = 3;

   // Flash continuously until the window comes to the foreground. 
   private const uint FLASHW_TIMERNOFG = 12;

   [StructLayout(LayoutKind.Sequential)]
   private struct FLASHWINFO {
      public uint   cbSize;
      public IntPtr hwnd;
      public uint   dwFlags;
      public uint   uCount;
      public uint   dwTimeout;
   }

   /// <summary>
   /// Send form taskbar notification, the Window will flash until get's focus
   /// <remarks>
   /// This method allows to Flash a Window, signifying to the user that some major event occurred within the application that requires their attention. 
   /// </remarks>
   /// </summary>
   /// <param name="form"></param>
   /// <returns></returns>
   public static bool FlashNotification(this Form form) {
      IntPtr     hWnd  = form.Handle;
      FLASHWINFO fInfo = new FLASHWINFO();

      fInfo.cbSize    = Convert.ToUInt32(Marshal.SizeOf(fInfo));
      fInfo.hwnd      = hWnd;
      fInfo.dwFlags   = FLASHW_ALL | FLASHW_TIMERNOFG;
      fInfo.uCount    = uint.MaxValue;
      fInfo.dwTimeout = 0;

      return FlashWindowEx(ref fInfo);
   }
}