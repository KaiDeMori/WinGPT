namespace WinGPT.OpenAI;

public class Models {
   public static readonly string nl = "\n";

   public static readonly string[] Supported = {
      gpt_4_1106_preview,
      gpt_4_vision_preview,
      //dall_e_3,
      gpt_4,
      gpt_3_5_turbo_16k,
      gpt_3_5_turbo,
      gpt_4_0613,
      gpt_3_5_turbo_16k_0613,
      gpt_3_5_turbo_0613,
   };

   //We need to create const properties for every entry in the Supported
   public const string gpt_4_1106_preview     = "gpt-4-1106-preview";
   public const string gpt_4_vision_preview   = "gpt-4-vision-preview";
   public const string dall_e_3               = "dall-e-3";
   public const string gpt_4                  = "gpt-4";
   public const string gpt_3_5_turbo_16k      = "gpt-3.5-turbo-16k";
   public const string gpt_3_5_turbo          = "gpt-3.5-turbo";
   public const string gpt_4_0613             = "gpt-4-0613";
   public const string gpt_3_5_turbo_16k_0613 = "gpt-3.5-turbo-16k-0613";
   public const string gpt_3_5_turbo_0613     = "gpt-3.5-turbo-0613";
}