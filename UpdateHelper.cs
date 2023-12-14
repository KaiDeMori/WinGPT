using System.Diagnostics;
using System.Xml;
using AutoUpdaterDotNET;
using WinGPT.OpenAI;
using WinGPT.Properties;

namespace WinGPT;

/// <summary>
/// Helper class to check for updates by comparing the current assembly version with the version information from a web server.
/// </summary>
public static class UpdateHelper {
   // URL of the version file on the web server
   private static readonly string VersionUrl = "https://peopleoftheprompt.org/secret_beta/binarisms/Version.xml";

   private static async Task<Version?> GetVersionFromUrlAsync() {
      try {
         using HttpClient httpClient = new HttpClient();
         string xmlContent = await httpClient.GetStringAsync(VersionUrl);

         XmlDocument xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(xmlContent);

         XmlNode? versionNode = xmlDoc.SelectSingleNode("/item/version");
         if (versionNode == null) {
            Debug.WriteLine("The <version> element is missing in the XML content.");
            return null;
         }

         if (Version.TryParse(versionNode.InnerText, out Version? version)) {
            return version;
         } else {
            Debug.WriteLine("The version string in the XML is not in a valid format.");
            return null;
         }
      } catch (Exception ex) {
         Debug.WriteLine($"Exception occurred while getting version from URL: {ex.Message}");
         return null;
      }
   }

   /// <summary>
   /// Checks if an update is available by comparing the current assembly version with the version from the web server.
   /// </summary>
   /// <returns>True if an update is available, false otherwise.</returns>
   public static async Task<bool> check_if_update_available() {
      try {
         // Get the current version of the assembly
         var currentVersion = Tools.Version;

         // Get the version from the web server
         var latestVersion = await GetVersionFromUrlAsync();
         if (latestVersion == null) {
            Debug.WriteLine("Failed to get the latest version from the web server.");
            return false;
         }

         // Compare the versions
         return latestVersion > currentVersion;
      } catch (Exception ex) {
         Debug.WriteLine($"Exception occurred while checking if update is available: {ex.Message}");
         return false;
      }
   }

   public static void StartUpdate(Form form) {
      try {
         AutoUpdater.Icon = Resources.WinGPT_64x64_;
         AutoUpdater.SetOwner(form);
         AutoUpdater.HttpUserAgent = HTTP_Client.UserAgentString;
         AutoUpdater.Start(VersionUrl);
      } catch (Exception ex) {
         Debug.WriteLine($"Exception occurred while starting the update: {ex.Message}");
      }
   }
}