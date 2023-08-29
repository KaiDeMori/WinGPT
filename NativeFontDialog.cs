using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinGPT;

public class NativeFontDialog {
   public Font? open(Font font) {
      // Create a LOGFONT structure from the Font object
      LOGFONT lf = new LOGFONT {
         lfHeight        = -Convert.ToInt32(font.SizeInPoints),
         lfFaceName      = font.Name,
         lfWeight        = font.Bold ? 700 : 400,
         lfItalic        = font.Italic ? (byte) 1 : (byte) 0,
         lfUnderline     = font.Underline ? (byte) 1 : (byte) 0,
         lfStrikeOut     = font.Strikeout ? (byte) 1 : (byte) 0,
         lfCharSet       = (byte) font.GdiCharSet,
         lfClipPrecision = (byte) font.GdiCharSet,
      };

      // Allocate memory for the LOGFONT structure
      IntPtr hlf = Marshal.AllocHGlobal(Marshal.SizeOf<LOGFONT>());
      Marshal.StructureToPtr(lf, hlf, false);

      // Create a CHOOSEFONT structure and set its fields
      CHOOSEFONT cf = new CHOOSEFONT {
         lStructSize = Marshal.SizeOf<CHOOSEFONT>(),
         Flags = CHOOSEFONTFLAGS.CF_EFFECTS        |
                 CHOOSEFONTFLAGS.CF_FORCEFONTEXIST |
                 CHOOSEFONTFLAGS.CF_NOSCRIPTSEL    |
                 CHOOSEFONTFLAGS.CF_NOVERTFONTS    |
                 CHOOSEFONTFLAGS.CF_SCALABLEONLY   |
                 CHOOSEFONTFLAGS.CF_INITTOLOGFONTSTRUCT,
         lpLogFont = hlf
      };

      Font? newfont = null;
      // Show the font dialog
      try {
         if (ChooseFont(ref cf)) {
            // If the user clicked OK, create a Font object from the LOGFONT structure
            LOGFONT resultLf = Marshal.PtrToStructure<LOGFONT>(cf.lpLogFont);
            Debug.WriteLine(
               $"LOGFONT after ChooseFont: {resultLf.lfHeight}, {resultLf.lfFaceName}, {resultLf.lfWeight}, {resultLf.lfItalic}, {resultLf.lfUnderline}, {resultLf.lfStrikeOut}, {resultLf.lfCharSet}, {resultLf.lfClipPrecision}");
            newfont = Font.FromLogFont(cf.lpLogFont);
         }
      }
      catch (Exception ex) {
         Debug.WriteLine($"Exception during ChooseFont or Font.FromLogFont: {ex}");
      }

      // Free the memory allocated for the LOGFONT structure
      Marshal.FreeHGlobal(hlf);

      return newfont;
   }

   [DllImport("comdlg32", SetLastError = true)]
   public static extern bool ChooseFont(ref CHOOSEFONT cf);
}

public struct CHOOSEFONT {
   public  int             lStructSize;
   public  IntPtr          hwndOwner;
   public  IntPtr          hDC;
   public  IntPtr          lpLogFont;
   public  int             iPointSize;
   public  CHOOSEFONTFLAGS Flags;
   public  int             rgbColors;
   public  IntPtr          lCustData;
   public  IntPtr          lpfnHook;
   public  string          lpTemplateName;
   public  IntPtr          hInstance;
   public  string          lpszStyle;
   public  short           nFontType;
   private short           __MISSING_ALIGNMENT__;
   public  int             nSizeMin;
   public  int             nSizeMax;
}

[Flags]
public enum CHOOSEFONTFLAGS : int {
   CF_SCREENFONTS          = 0x00000001,
   CF_PRINTERFONTS         = 0x00000002,
   CF_BOTH                 = (CF_SCREENFONTS | CF_PRINTERFONTS),
   CF_SHOWHELP             = 0x00000004,
   CF_ENABLEHOOK           = 0x00000008,
   CF_ENABLETEMPLATE       = 0x00000010,
   CF_ENABLETEMPLATEHANDLE = 0x00000020,
   CF_INITTOLOGFONTSTRUCT  = 0x00000040,
   CF_USESTYLE             = 0x00000080,
   CF_EFFECTS              = 0x00000100,
   CF_APPLY                = 0x00000200,
   CF_ANSIONLY             = 0x00000400,
   CF_SCRIPTSONLY          = CF_ANSIONLY,
   CF_NOVECTORFONTS        = 0x00000800,
   CF_NOOEMFONTS           = CF_NOVECTORFONTS,
   CF_NOSIMULATIONS        = 0x00001000,
   CF_LIMITSIZE            = 0x00002000,
   CF_FIXEDPITCHONLY       = 0x00004000,
   CF_WYSIWYG              = 0x00008000,
   CF_FORCEFONTEXIST       = 0x00010000,
   CF_SCALABLEONLY         = 0x00020000,
   CF_TTONLY               = 0x00040000,
   CF_NOFACESEL            = 0x00080000,
   CF_NOSTYLESEL           = 0x00100000,
   CF_NOSIZESEL            = 0x00200000,
   CF_SELECTSCRIPT         = 0x00400000,
   CF_NOSCRIPTSEL          = 0x00800000,
   CF_NOVERTFONTS          = 0x01000000,
   CF_INACTIVEFONTS        = 0x02000000
}

public struct LOGFONT {
   public int  lfHeight;
   public int  lfWidth;
   public int  lfEscapement;
   public int  lfOrientation;
   public int  lfWeight;
   public byte lfItalic;
   public byte lfUnderline;
   public byte lfStrikeOut;
   public byte lfCharSet;
   public byte lfOutPrecision;
   public byte lfClipPrecision;
   public byte lfQuality;
   public byte lfPitchAndFamily;

   [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
   public string lfFaceName;
}