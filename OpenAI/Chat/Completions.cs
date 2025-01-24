using System.Net;
using System.Text;
using Newtonsoft.Json;
using static WinGPT.Tools;

namespace WinGPT.OpenAI.Chat;

public class Completions {
   private const           string Endpoint          = "chat/completions";
   private static readonly string Full_Endpoint_URL = HTTP_Client.Get_full_URL_for_endpoint(Endpoint);

   public static readonly JsonSerializerSettings JSON_Serializer_Settings = new() {
      //TypeNameHandling  = TypeNameHandling.Auto,
      NullValueHandling = NullValueHandling.Ignore,
      Converters = new List<JsonConverter> {
         new ToolChoiceJsonConverter()
      }
   };

   public static async Task<Response?> POST_Async(Request request) {
      //quick and very dirty hack for the reasoning models.
      if (request.model.StartsWith("o1")) {
         //we need to remove temperature from the request
         request.temperature      = null;
         request.reasoning_effort = Config.Active.UIable.reasoning_effort;
      }

      string request_content_json = JsonConvert.SerializeObject(request, Formatting.None, JSON_Serializer_Settings);
      File.WriteAllText("_request.json", request_content_json);

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
               Config.Active.Token_Counter.Increment(response);
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
      //get the content of the response as string synchronously
      var json_content = responseMessage.Content.ReadAsStringAsync().Result;
      var content      = make_json_human_readable(json_content);
      var statusCode   = responseMessage.StatusCode;

      //show the error message
      MessageBox.Show($"{content}", $"{statusCode}", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }

   private static void show_known_errormessage(ErrorCode errorCode) {
      // Prepare the message
      var message = new StringBuilder();
      message.AppendLine($"Error Code: {errorCode.Code}");
      message.AppendLine($"Error Name: {errorCode.Name}");
      message.AppendLine();
      message.AppendLine("Overview:");
      message.AppendLine($"Cause: {errorCode.Overview.Cause}");
      message.AppendLine($"Solution: {errorCode.Overview.Solution}");
      message.AppendLine();
      message.AppendLine("Detail:");
      message.AppendLine($"Description: {errorCode.Detail.Description}");
      message.AppendLine("Reasons:");
      foreach (var reason in errorCode.Detail.Reasons) {
         message.AppendLine($"- {reason}");
      }

      message.AppendLine("Resolve Steps:");
      foreach (var step in errorCode.Detail.ResolveSteps) {
         message.AppendLine($"- {step}");
      }

      // Show the message box
      MessageBox.Show(message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
}