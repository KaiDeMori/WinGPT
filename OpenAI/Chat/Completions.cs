using System.Net;
using System.Text;
using Newtonsoft.Json;
using static WinGPT.Tools;

namespace WinGPT.OpenAI.Chat;

public class Completions {
   public static async Task<Response?> POST_Async(Request request) {
      var url = "https://api.openai.com/v1/chat/completions";

      // Serialize the request object to JSON
      //string request_content_json = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings {
      //   NullValueHandling = NullValueHandling.Ignore,
      //});
      string request_content_json = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings {
         TypeNameHandling  = TypeNameHandling.Auto,
         NullValueHandling = NullValueHandling.Ignore,
      });

      File.WriteAllText("_request.json", request_content_json);

      //replace all "system" by "System" -> Bad Request
      //jsonRequest = jsonRequest.Replace("\"system\"", "\"System\"");

      Response? response = null;

      // Create thae HttpContent for the form to be posted.
      StringContent content = new(request_content_json, Encoding.UTF8, "application/json");

      try {
         // Make the request
         HttpResponseMessage responseMessage = await HTTP_Client.Gimme().PostAsync(url, content).ConfigureAwait(false);
         ;

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
               File.WriteAllText("BAD_REQUEST_" + DateTimeOffset.Now + "_.json", request_content_json);
            }

            handle_Error_Response(responseMessage);
         }
      }
      catch (Exception ex) {
         //for now, lets just show it in a messagebox
         MessageBox.Show($"{ex.Message}", $"{ex.GetType().Name}", MessageBoxButtons.OK);
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
         MessageBox.Show($"{errorCode.Overview.Cause}\r\n{errorCode.Overview.Solution}", $"Error: {errorCode.Name}", MessageBoxButtons.OK);
         //TADA we can do so much more here now!
      }
      else {
         //we did not find a matching error code
         MessageBox.Show($"Code: {(int) responseMessage.StatusCode}\r\n{responseMessage.ReasonPhrase}", responseMessage.StatusCode.ToString(),
            MessageBoxButtons.OK);
      }
   }

   [Obsolete]
   public static async Task<ErrorOr<Response, HttpResponseMessage>> POST_Async_old(Request request) {
      var url = "https://api.openai.com/v1/chat/completions";

      // Serialize the request object to JSON
      string jsonRequest = JsonConvert.SerializeObject(request);

      // Create the HttpContent for the form to be posted.
      var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

      try {
         // Make the request
         HttpResponseMessage responseMessage = await HTTP_Client.Gimme().PostAsync(url, content);

         if (responseMessage.IsSuccessStatusCode) {
            // If the request was successful, parse the returned data
            string    jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            Response? response     = JsonConvert.DeserializeObject<Response>(jsonResponse);

            // Return a successful response
            if (response == null)
               throw new InvalidOperationException("We got unparsable JSON from the API");

            Config.Active.TokenCounter.Increment(response);

            return response;
         }
         else {
            // If something went wrong, return the HttpResponseMessage as the error
            return responseMessage;
         }
      }
      catch (Exception ex) {
         // In case of an exception, wrap the exception in an HttpResponseMessage and return as error
         var errorResponse = new HttpResponseMessage {
            StatusCode = HttpStatusCode.Unused,
            Content    = new StringContent(ex.Message)
         };

         return errorResponse;
      }
   }
}