namespace WinGPT;

internal class SpecialTokens {
   public static readonly string nl = Environment.NewLine;

   //WinGPT tokens
   //public static readonly string Code          = $"📜Code";
   public static readonly string Configuration       = $"🛠️Configuration{nl}";
   public static readonly string ConversationHistory = $"📜ConversationHistory{nl}";

   //API roles
   public static readonly string System    = $"{nl}🎭System{nl}";
   public static readonly string User      = $"{nl}🦧User{nl}";
   public static readonly string Assistant = $"{nl}🤖Assistant{nl}";
   public static readonly string Function  = $"{nl}🧮Function{nl}";


   // Set of special tokens
   public static readonly HashSet<string> specialTokensSet = new() {
      System,
      User,
      Assistant,
      Function,
      //Code,
      Configuration
   };

   //a Hashmap to map the SpecialTokens to the Role enum
   public static readonly Dictionary<string, Role> To_API_Role = new() {
      {System, Role.system},
      {User, Role.user},
      {Assistant, Role.assistant},
      {Function, Role.Function},
   };

   enum specialTokensEnum {
      System,
      User,
      Assistant,
      Function,
      Code,
      Configuration
   }

   public static bool TryParse(string specialToken, out Role parsedRole) {
      if (RoleHelper.RoleTokenBiDictionary.TryGetBySecond(specialToken, out parsedRole))
         return true;

      return false;
   }
}