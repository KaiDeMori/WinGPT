namespace WinGPT.OpenAI;

internal static class HTTP_Client {
   private static readonly HttpClient _httpClient = new HttpClient();

   private static bool IsInitialized = false;

   private const string OpenAI_API_Key_Pattern = "sk-";

   private static string Base_URL { get; set; } = OPENAICOM_Base_URL;

   private const string AZURE_APIM_Base_URL = "https://people-of-the-prompt-apim.azure-api.net/openaicom/";
   private const string OPENAICOM_Base_URL  = "https://api.openai.com/v1/";

   public static HttpClient Gimme() {
      if (!IsInitialized)
         throw new InvalidOperationException("HTTP_Client.Init() must be called before any other HTTP_Client methods.");
      return _httpClient;
   }

   public static void Init(string OpenAI_API_Key) {
      _httpClient.DefaultRequestHeaders.Clear();
      if (OpenAI_API_Key.StartsWith(OpenAI_API_Key_Pattern)) {
         _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + OpenAI_API_Key);
      }
      else {
         //let's assume it's an APIM key
         Base_URL = AZURE_APIM_Base_URL;
         _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", OpenAI_API_Key);
      }

      _httpClient.DefaultRequestHeaders.Add("User-Agent", "WinGPT");
      //TADA add to config
      _httpClient.Timeout = TimeSpan.FromMinutes(10);
      IsInitialized       = true;
   }

   public static string Get_full_URL_for_endpoint(string endpoint) => Base_URL + endpoint;
}