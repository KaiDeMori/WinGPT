using Timer = System.Windows.Forms.Timer;
using System.Windows.Forms;

namespace WinGPT.OpenAI;

/// <summary>
/// This class is used to call the tokenizer after a certain amount of inactivity,
/// which will count the number of tokens in the prompt, including the attached files.
/// </summary>
internal static class TimedTokenizer {
    private static readonly Timer timer = new();
    // Language selection for the message box (default is French)
    public static string Language { get; set; } = "French";

    static TimedTokenizer() {
        timer.Interval = Config.count_tokens_timer_interval;
        timer.Tick += (sender, args) => {
            timer.Stop();
            //TODO: Count tokens
            
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