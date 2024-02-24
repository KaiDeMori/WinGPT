using System.Net;
using System.Text;
using Newtonsoft.Json;
using static WinGPT.Tools;

namespace WinGPT.OpenAI.Chat;

public class Completions {
   private const           string Endpoint          = "chat/completions";
   private static readonly string Full_Endpoint_URL = HTTP_Client.Get_full_URL_for_endpoint(Endpoint);

   public static async Task<Response?> POST_Async(Request request) {
      // Serialize the request object to JSON
      //string request_content_json = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings {
      //   NullValueHandling = NullValueHandling.Ignore,
      //});
      string request_content_json = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings {
         //TypeNameHandling  = TypeNameHandling.Auto,
         NullValueHandling = NullValueHandling.Ignore,
      });

      File.WriteAllText("_request.json", request_content_json);

      //replace all "system" by "System" -> Bad Request
      //jsonRequest = jsonRequest.Replace("\"system\"", "\"System\"");

      Response? response;

      // Create thae HttpContent for the form to be posted.
      StringContent content = new(request_content_json, Encoding.UTF8, "application/json");

      try {
         // Make the request
         HttpResponseMessage responseMessage = await HTTP_Client.Gimme().PostAsync(Full_Endpoint_URL, content).ConfigureAwait(false);


         if (responseMessage.IsSuccessStatusCode) {
            // If the request was successful, parse the returned data
            string jsonResponse = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            File.WriteAllText("_response.json", jsonResponse);

            response = JsonConvert.DeserializeObject<Response>(jsonResponse);

            if (response == null) {
               //throw new InvalidOperationException("We got unparsable JSON from the API");
               //instead, be nice about it and how a pleaseant message box
               MessageBox.Show("We got unparsable JSON from the API", "Error", MessageBoxButtons.OK);
            }
            else {
               Config.Active.TokenCounter.Increment(response);
            }
         }
         else {
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest) {
               File.WriteAllText("BAD_REQUEST_"
                                 + DateTimeOffset.Now.ToString("yyyy-MM-dd_HH-mm-ss")
                                 + "_.json", request_content_json);
            }

            File.WriteAllText("ERROR_response.txt", responseMessage.ToString());
            handle_Error_Response(responseMessage);
            return null;
         }
      }
      catch (Exception ex) {
         //for now, lets just show it in a messagebox
         MessageBox.Show($"{ex.Message}", $"{ex.GetType().Name}", MessageBoxButtons.OK);
         return null;
      }

      return response;
   }

   private static void handle_Error_Response(HttpResponseMessage responseMessage) {
      var         statusCode = responseMessage.StatusCode;
      ErrorCode[] errorCodes = ErrorCodes.FromDocumentation;
      //we need to find the error code that matches the status code
      ErrorCode? errorCode = errorCodes.FirstOrDefault(errorCode => errorCode.Code == (int) statusCode);
      if (errorCode != null) {
         //we found a matching error code
         MessageBox.Show($"{errorCode.Overview.Cause}\r\n{errorCode.Overview.Solution}", $"Error {errorCode.Code}: {errorCode.Name}", MessageBoxButtons.OK);
         //TADA we can do so much more here now!
      }
      else {
         //we did not find a matching error code
         MessageBox.Show($"Code: {(int) responseMessage.StatusCode}\r\n{responseMessage.ReasonPhrase}", responseMessage.StatusCode.ToString(),
            MessageBoxButtons.OK);
      }
   }

}