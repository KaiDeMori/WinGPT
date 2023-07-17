using Newtonsoft.Json;

namespace WinGPT.Taxonomy;

public class Function_Parmeters {
   public string? summary  { get; set; }
   public string? filename { get; set; }
   public string? category { get; set; }

   public string?   new_category        { get; set; }
   public string[]? existing_categories { get; set; }
   public string?   selected_category   { get; set; }

   public override string ToString() {
      return $"{nameof(summary)}: {summary}\r\n"                         +
             $"{nameof(filename)}: {filename}\r\n"                       +
             $"{nameof(new_category)}: {new_category}\r\n"               +
             $"{nameof(existing_categories)}: {existing_categories}\r\n" +
             $"{nameof(selected_category)}: {selected_category}";
   }
}