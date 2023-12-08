using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace WinGPT;

/// <summary>
/// Helper class to check for updates by comparing the current assembly version with the version information from a web server.
/// </summary>
public static class UpdateHelper {
   // URL of the version file on the web server
   private static readonly string VersionUrl = "https://peopleoftheprompt.org/secret_beta/binarisms/current_version.txt";

   /// <summary>
   /// Checks if an update is available by comparing the current assembly version with the version from the web server.
   /// </summary>
   /// <returns>True if an update is available, false otherwise.</returns>
   public static bool check_if_update_available() {
      try {
         // Get the current version of the assembly
         var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
         if (currentVersion == null)
            throw new InvalidOperationException("Could not determine the current assembly version.");

         // Get the version string from the web server
         using var httpClient          = new HttpClient();
         string    latestVersionString = httpClient.GetStringAsync(VersionUrl).Result.Trim();

         // Parse the version string
         if (Version.TryParse(latestVersionString, out var latestVersion)) {
            //return latestVersion.Major > currentVersion.Major || (latestVersion.Major == currentVersion.Major && latestVersion.Minor > currentVersion.Minor);
            return latestVersion > currentVersion;
         }
         else {
            throw new FormatException("The version string from the server is not in a correct format.");
         }
      }
      catch (Exception ex) {
         // Log the exception (logging mechanism to be implemented)
         Debug.WriteLine($"Error checking for updates: {ex.Message}");
         return false;
      }
   }
}