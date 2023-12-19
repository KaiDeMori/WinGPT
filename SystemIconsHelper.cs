using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinGPT;

public static class SystemIconsHelper {
   // Constants that we need in the SHGetFileInfo call
   private const uint SHGFI_ICON      = 0x100;
   private const uint SHGFI_LARGEICON = 0x0; // 'Large icon
   private const uint SHGFI_SMALLICON = 0x1; // 'Small icon

   // This structure will contain information about the file
   public struct SHFILEINFO {
      public IntPtr hIcon;        // The icon
      public IntPtr iIcon;        // The icon index
      public uint   dwAttributes; // Attributes of the file

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string szDisplayName; // The name of the file

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string szTypeName; // The type of the file
   }

   // The SHGetFileInfo function retrieves information about an object in the file system
   [DllImport("shell32.dll")]
   public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

   // Call this method to get the large or small icon associated with a file or file type
   public static Icon? GetFileIcon(string? filePath, bool smallIcon) {
      SHFILEINFO shinfo = new SHFILEINFO();

      // Use this to get the Icon
      IntPtr hImg = SHGetFileInfo(filePath!, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), SHGFI_ICON | (smallIcon ? SHGFI_SMALLICON : SHGFI_LARGEICON));

      if (hImg == IntPtr.Zero)
         return null;

      // The icon is returned in the hIcon member of the shinfo struct
      Icon myIcon = (Icon) Icon.FromHandle(shinfo.hIcon).Clone();
      DestroyIcon(shinfo.hIcon); // Cleanup
      return myIcon;
   }

   // Required to dispose the Icon handle obtained through SHGetFileInfo correctly
   [DllImport("user32.dll", SetLastError = true)]
   private static extern bool DestroyIcon(IntPtr hIcon);
}

// Example usage:
// Icon largeIcon = SystemIconsHelper.GetFileIcon("C:\\Path\\To\\Your\\File.txt", false);
// Icon smallIcon = SystemIconsHelper.GetFileIcon("C:\\Path\\To\\Your\\File.txt", true);