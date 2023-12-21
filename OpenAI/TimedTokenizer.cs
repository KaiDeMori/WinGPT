using Timer = System.Windows.Forms.Timer;
using System.Windows.Forms;

namespace WinGPT.OpenAI;

/// <summary>
/// This class is used to call the tokenizer after a certain amount of inactivity,
/// which will count the number of tokens in the prompt, including the attached files.
/// </summary>
internal static class TimedTokenizer {
   private static readonly Timer timer = new();

   //a callback to call after the timer has elapsed
   public static Action Callback { get; set; }

   static TimedTokenizer() {
      Callback       = () => { };
      timer.Interval = Config.Active.UIable.count_tokens_timer_interval;
      timer.Tick += (sender, args) => {
         timer.Stop();
         //TODO: Count tokens
         Callback();
      };
   }

   /// <summary>
   /// Resets and restarts the timer.
   /// </summary>
   public static void Reset() {
      timer.Stop();
      timer.Start();
   }
}