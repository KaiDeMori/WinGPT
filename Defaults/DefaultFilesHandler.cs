using Newtonsoft.Json;

namespace WinGPT.Defaults;

/// <summary>
/// Handles loading JSON files from the main directory, 
/// falling back on physically copying it from the Defaults folder if needed.
/// If both attempts fail, it throws an exception.
/// Don't get defaulted on this one 🤗
/// </summary>
public static class DefaultFilesHandler {
   /// <summary>
   /// Loads a JSON file and deserializes its content into the specified type <typeparamref name="T"/>.
   /// If loading from the main directory fails (file missing or bad JSON),
   /// it tries to copy the same file from the "Defaults" subdirectory into the main directory
   /// then attempts to parse it again.
   /// If both attempts fail, it throws an exception.
   /// </summary>
   /// <typeparam name="T">The type to which the JSON content will be deserialized.</typeparam>
   /// <param name="file_name">The name of the file to load (e.g., "File_Types.json").</param>
   /// <returns>An instance of <typeparamref name="T"/> populated from the file's JSON content.</returns>
   /// <exception cref="NotSupportedException">Thrown if the file is not a JSON file.</exception>
   /// <exception cref="FileNotFoundException">Thrown if the file cannot be found in both the main and Defaults folder.</exception>
   /// <exception cref="IOException">Thrown if the file could not be copied from Defaults to the main directory.</exception>
   /// <exception cref="Exception">Thrown if deserialization fails after both attempts.</exception>
   public static T load_json_file<T>(string file_name) {
      // We only support JSON for now.
      if (!file_name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
         throw new NotSupportedException(
            $"File format not supported. Only .json is supported. Provided file: {file_name}."
         );

      // Build paths for main directory and Defaults folder
      var main_file_path     = Path.Combine(Application.StartupPath, file_name);
      var defaults_file_path = Path.Combine(Application.StartupPath, "Defaults", file_name);

      // Helper method to try parsing the file in the main directory, also indicating if the file was found
      bool try_parse_main_directory_file(out T? parsed, out bool file_found) {
         parsed     = default;
         file_found = File.Exists(main_file_path);
         if (!file_found) {
            return false; // File is missing
         }

         try {
            var json = File.ReadAllText(main_file_path);
            parsed = JsonConvert.DeserializeObject<T>(json);
            return parsed is not null;
         }
         catch {
            return false; // Could not parse (defective)
         }
      }

      // First, try parsing directly from the main directory
      if (try_parse_main_directory_file(out T? result, out bool file_was_found))
         return result!;

      // If parsing fails, we must determine the reason
      // (missing file vs. defective file)
      bool file_was_missing = !file_was_found;

      // Ensure the Defaults file exists, otherwise we can't restore
      if (!File.Exists(defaults_file_path)) {
         throw new FileNotFoundException(
            $"Could not find {file_name} in the main directory or the Defaults folder.",
            file_name
         );
      }

      // If the file existed but couldn't be parsed, rename the existing file to *.defective
      if (!file_was_missing) {
         var defective_file_path = main_file_path + ".defective";
         if (File.Exists(defective_file_path)) {
            File.Delete(defective_file_path); // Overwrite if it already exists
         }

         // Rename the defective file
         File.Move(main_file_path, defective_file_path);
      }

      // Copy file from Defaults
      try {
         File.Copy(defaults_file_path, main_file_path, true);
         var reason_explanation = file_was_missing
            ? "it was missing"
            : "the existing one could not be parsed (marked as *.defective)";

         MessageBox.Show(
            $"Restored '{file_name}' from Defaults folder because {reason_explanation}.",
            "File Copied",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
         );
      }
      catch (Exception ex) {
         throw new IOException(
            $"Failed to copy default file '{file_name}' from Defaults folder to main directory.",
            ex
         );
      }

      // Attempt parsing again from the main directory after copying
      if (try_parse_main_directory_file(out result, out _))
         return result!;

      // If we've gotten this far, nothing worked
      throw new Exception(
         $"Failed to load and parse '{file_name}' from both the main directory and the Defaults folder."
      );
   }
}