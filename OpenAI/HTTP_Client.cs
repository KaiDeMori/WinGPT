namespace WinGPT.OpenAI; 

internal static class HTTP_Client {
   private static readonly HttpClient _httpClient   = new HttpClient();
   private static          bool       IsInitialized = false;

   public static HttpClient Gimme() {
      if (!IsInitialized)
         throw new InvalidOperationException("HTTP_Client.Init() must be called before any other HTTP_Client methods.");
      return _httpClient;
   }

   public static void Init(string OpenAI_API_Key) {
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + OpenAI_API_Key);
      _httpClient.DefaultRequestHeaders.Add("User-Agent",    "WinGPT");
      //TADA add to config
      _httpClient.Timeout = TimeSpan.FromMinutes(10);
      IsInitialized       = true;
   }
}