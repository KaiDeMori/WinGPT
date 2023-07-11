namespace WinGPT;

/// <summary>
/// This uses the OpenAI API endpoint wording.
/// </summary>
public enum Role {
   system,
   user,
   assistant,
   Function,
}

public static class RoleHelper {
   public static readonly BiDictionary<Role, string> RoleTokenBiDictionary = new() {
      {Role.system, SpecialTokens.System},
      {Role.user, SpecialTokens.User},
      {Role.assistant, SpecialTokens.Assistant},
      {Role.Function, SpecialTokens.Function},
   };

   public static string ToSpecialToken(this Role role) {
      if (RoleTokenBiDictionary.TryGetByFirst(role, out var specialToken))
         return specialToken;

      throw new ArgumentOutOfRangeException(nameof(role), role, null);
   }
}