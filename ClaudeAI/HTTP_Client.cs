namespace WinGPT.ClaudeAI;

public static class HTTP_Client {
   public static readonly string UserAgentString = $"WinGPT/{Tools.Version} a.k.a WinForms meets Sci-Fi";

   private static readonly HttpClient _httpClient = new() {
      DefaultRequestHeaders = {
         {"User-Agent", UserAgentString},
      }
   };

   private static bool IsInitialized = false;

   private const string ClaudeAI_API_Key_Pattern = "sk-ant-";

   private static string Base_URL { get; set; } = CLAUDEAICOM_Base_URL;

   private const string CLAUDEAICOM_Base_URL = "https://api.anthropic.com/v1/";

   public static HttpClient Gimme() {
      if (!IsInitialized)
         throw new InvalidOperationException("HTTP_Client.Init() must be called before any other HTTP_Client methods.");
      return _httpClient;
   }

   public static void Init(string ClaudeAI_API_Key) {
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("x-api-key", ClaudeAI_API_Key);

      _httpClient.DefaultRequestHeaders.Add("User-Agent", "WinGPT");
      //TADA add to config
      _httpClient.Timeout = TimeSpan.FromMinutes(10);
      IsInitialized       = true;
   }

   public static string Get_full_URL_for_endpoint(string endpoint) => Base_URL + endpoint;
}