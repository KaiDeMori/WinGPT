using System.Reflection;

namespace WinGPT;

partial class AboutBox : Form {
   public AboutBox() {
      InitializeComponent();
      // ReSharper disable once VirtualMemberCallInConstructor
      this.Text                    = $"About {AssemblyTitle}";
      this.labelProductName.Text   = AssemblyProduct;
      this.labelVersion.Text       = $"Version {AssemblyVersion}";
      this.labelCopyright.Text     = AssemblyCopyright;
      this.labelCompanyName.Text   = AssemblyCompany;
      this.textBoxDescription.Text = AssemblyDescription;
   }

   #region Assembly Attribute Accessors

   public static string AssemblyTitle {
      get {
         object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
         if (attributes.Length > 0) {
            AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute) attributes[0];
            if (titleAttribute.Title != "") {
               return titleAttribute.Title;
            }
         }

         return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
      }
   }

   public static string AssemblyVersion => Tools.Version.ToString();

   public static string AssemblyDescription {
      get {
         object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
         if (attributes.Length == 0) {
            return "";
         }

         return ((AssemblyDescriptionAttribute) attributes[0]).Description;
      }
   }

   public static string AssemblyProduct {
      get {
         object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
         if (attributes.Length == 0) {
            return "";
         }

         return ((AssemblyProductAttribute) attributes[0]).Product;
      }
   }

   public static string AssemblyCopyright {
      get {
         object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
         if (attributes.Length == 0) {
            return "";
         }

         return ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
      }
   }

   public static string AssemblyCompany {
      get {
         object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
         if (attributes.Length == 0) {
            return "";
         }

         return ((AssemblyCompanyAttribute) attributes[0]).Company;
      }
   }

   #endregion
}