using Timer = System.Windows.Forms.Timer;

namespace WinGPT.OpenAI;

/// <summary>
/// This class is used to call the tokenizer after a certain amount of inactivity,
/// which will count the number of tokens in the prompt, including the attached files.
/// </summary>
internal static class Prompt_Actions_Timer {
   private static readonly Timer timer = new();

   /// <summary>
   /// A list of callbacks to be called when the timer elapses.
   /// </summary>
   public static readonly List<Action> Callback = [];

   static Prompt_Actions_Timer() {
      timer.Interval = Config.Active.UIable.prompt_actions_timer_interval;
      timer.Tick += (_, _) => {
         timer.Stop();
         foreach (var callback in Callback)
            callback();
      };
   }

   /// <summary>
   /// Resets and restarts the timer.
   /// </summary>
   public static void Reset() {
      timer.Stop();
      timer.Interval = Config.Active.UIable.prompt_actions_timer_interval;
      timer.Start();
   }
}