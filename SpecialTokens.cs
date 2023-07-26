namespace WinGPT;

internal class SpecialTokens {
   public static readonly string nl = Environment.NewLine;

   //Tulpa Tokens
   public static readonly string Tulpa_Code_Token         = $"🔮Code{nl}";
   public static readonly string Tulpa_Config_Token       = $"🛠️Configuration{nl}";
   //public static readonly string Tulpa_SamplePrompt_Token = $"📝SamplePrompt{nl}";

   //Conversation tokens
   public static readonly string ConversationHistory = $"📜ConversationHistory{nl}";

   //API roles
   public static readonly string System    = $"{nl}🎭System{nl}";
   public static readonly string User      = $"{nl}🦧User{nl}";
   public static readonly string Assistant = $"{nl}🤖Assistant{nl}";
   public static readonly string Function  = $"{nl}🧮Function{nl}";


   // Set of special tokens
   //public static readonly HashSet<string> specialTokensSet = new() {
   //   System,
   //   User,
   //   Assistant,
   //   Function,
   //   //Code,
   //   Configuration
   //};

   /// <summary>
   /// A Hashmap to map the SpecialTokens to the Role enum
   /// </summary>
   public static readonly Dictionary<string, Role> To_API_Role = new() {
      {System, Role.system},
      {User, Role.user},
      {Assistant, Role.assistant},
      {Function, Role.function},
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