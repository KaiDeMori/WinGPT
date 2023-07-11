using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGPT.OpenAI;

public class Models {
   public static readonly string[] Supported = {
      gpt_4,
      gpt_3_5_turbo_16k,
      gpt_3_5_turbo,
      gpt_4_0613,
      gpt_3_5_turbo_16k_0613,
      gpt_3_5_turbo_0613,
      //that's what co-pilot suggested! what does it know that we don't?
      //"gpt-neo-2.7B",
   };

   //We need to create const properties for every entry in the Supported
   public const string gpt_4                  = "gpt-4";
   public const string gpt_3_5_turbo_16k      = "gpt-3.5-turbo-16k";
   public const string gpt_3_5_turbo          = "gpt-3.5-turbo";
   public const string gpt_4_0613             = "gpt-4-0613";
   public const string gpt_3_5_turbo_16k_0613 = "gpt-3.5-turbo-16k-0613";
   public const string gpt_3_5_turbo_0613     = "gpt-3.5-turbo-0613";

   //that's what co-pilot suggested! what does it know that we don't?
   //public const string gpt_neo_2_7B = "gpt-neo-2.7B";
}