using Microsoft.DeepDev;

namespace WinGPT.Tokenizer;

internal class DeepTokenizer {
   private static readonly Dictionary<string, ITokenizer> ModelName_to_Tokeinzer = new();

   private static ITokenizer GetTokenizer(string modelName) {
      if (ModelName_to_Tokeinzer.TryGetValue(modelName, out var tokenizer))
         return tokenizer;

      tokenizer = CreateByModelNameAsync(modelName);
      ModelName_to_Tokeinzer.Add(modelName, tokenizer);
      return tokenizer;
   }

   private static List<int> Encode(string text, string modelName) {
      var tokenizer = GetTokenizer(modelName);
      return tokenizer.Encode(text, new List<string>());
   }

   public static int count_tokens(string text, string modelName) {
      var tokenizer = GetTokenizer(modelName);
      return tokenizer.Encode(text, new List<string>()).Count;
   }

   public static Func<string, int> count_tokens(string modelName) {
      var tokenizer = GetTokenizer(modelName);
      return text => tokenizer.Encode(text, new List<string>()).Count;
   }

   /// <summary>
   /// Whatever. This code is so bad, doesn't even deserve to copy-paste the comments
   /// </summary>
   /// <param name="modelName"></param>
   /// <param name="extraSpecialTokens">:-)))</param>
   /// <returns></returns>
   private static ITokenizer CreateByModelNameAsync(
      string modelName, IReadOnlyDictionary<string, int>? extraSpecialTokens = null) {
      if (MODEL_TO_ENCODING.TryGetValue(modelName, out var encoder))
         return CreateByEncoderName(encoder, extraSpecialTokens);

      foreach (var kvp in MODEL_PREFIX_TO_ENCODING) {
         if (!modelName.StartsWith(kvp.Key))
            continue;
         encoder = kvp.Value;
         break;
      }

      return CreateByEncoderName(encoder, extraSpecialTokens);
   }

   private static ITokenizer CreateByEncoderName(
      string?                           encoderName,
      IReadOnlyDictionary<string, int>? extraSpecialTokens = null) {
      switch (encoderName) {
         case "cl100k_base":
            var regexPatternStr       = @"(?i:'s|'t|'re|'ve|'m|'ll|'d)|[^\r\n\p{L}\p{N}]?\p{L}+|\p{N}{1,3}| ?[^\s\p{L}\p{N}]+[\r\n]*|\s*[\r\n]+|\s+(?!\S)|\s+";
            var mergeableRanksFileUrl = cl100k_base_BPE_Rank_File_name;
            var specialTokens = new Dictionary<string, int> {
               {ENDOFTEXT, 100257},
               {FIM_PREFIX, 100258},
               {FIM_MIDDLE, 100259},
               {FIM_SUFFIX, 100260},
               {ENDOFPROMPT, 100276}
            };
            if (extraSpecialTokens is not null) {
               specialTokens = specialTokens.Concat(extraSpecialTokens)
                  .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            return CreateTokenizer(regexPatternStr, mergeableRanksFileUrl, specialTokens);

         case "p50k_base":
            regexPatternStr       = @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+";
            mergeableRanksFileUrl = p50k_base_BPE_Rank_File_name;
            specialTokens = new Dictionary<string, int> {
               {ENDOFTEXT, 50256}
            };
            if (extraSpecialTokens is not null) {
               specialTokens = specialTokens.Concat(extraSpecialTokens)
                  .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            return CreateTokenizer(regexPatternStr, mergeableRanksFileUrl, specialTokens);

         case "p50k_edit":
            regexPatternStr       = @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+";
            mergeableRanksFileUrl = p50k_base_BPE_Rank_File_name;
            specialTokens = new Dictionary<string, int> {
               {ENDOFTEXT, 50256},
               {FIM_PREFIX, 50281},
               {FIM_MIDDLE, 50282},
               {FIM_SUFFIX, 50283}
            };
            if (extraSpecialTokens is not null) {
               specialTokens = specialTokens.Concat(extraSpecialTokens)
                  .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            return CreateTokenizer(regexPatternStr, mergeableRanksFileUrl, specialTokens);

         case "r50k_base":
            regexPatternStr       = @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+";
            mergeableRanksFileUrl = r50k_base_BPE_Rank_File_name;
            specialTokens = new Dictionary<string, int> {
               {ENDOFTEXT, 50256},
            };
            if (extraSpecialTokens is not null) {
               specialTokens = specialTokens.Concat(extraSpecialTokens)
                  .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            return CreateTokenizer(regexPatternStr, mergeableRanksFileUrl, specialTokens);

         //Just no.
         //case "gpt2":
         //   regexPatternStr       = @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+";
         //   mergeableRanksFileUrl = gpt2_tiktoken_BPE_Rank_File_name;
         //   specialTokens = new Dictionary<string, int> {
         //      {ENDOFTEXT, 50256},
         //   };
         //   if (!(extraSpecialTokens is null)) {
         //      specialTokens = specialTokens.Concat(extraSpecialTokens)
         //         .ToDictionary(pair => pair.Key, pair => pair.Value);
         //   }

         //   return await CreateTokenizerAsync(regexPatternStr, mergeableRanksFileUrl, specialTokens);

         default:
            throw new NotImplementedException($"Doesn't support this encoder [{encoderName}]");
      }
   }


   private static ITokenizer CreateTokenizer(
      string                           regexPatternStr,
      string                           mergeableRanksFilePath,
      IReadOnlyDictionary<string, int> specialTokens) {
      using Stream stream = File.OpenRead(mergeableRanksFilePath);
      var tokenizer = actually_CreateTokenizer(
         stream,
         (IReadOnlyDictionary<string, int>) specialTokens,
         regexPatternStr);
      return tokenizer;
   }

   private static readonly IReadOnlyDictionary<string, string> MODEL_PREFIX_TO_ENCODING =
      new Dictionary<string, string> {
         // chat
         {"gpt-4-", "cl100k_base"},        // e.g., gpt-4-0314, etc., plus gpt-4-32k
         {"gpt-3.5-turbo-", "cl100k_base"} // e.g, gpt-3.5-turbo-0301, -0401, etc.
      };

   private static readonly IReadOnlyDictionary<string, string> MODEL_TO_ENCODING =
      new Dictionary<string, string> {
         // chat
         {"gpt-4", "cl100k_base"},
         {"gpt-3.5-turbo", "cl100k_base"},
         // text
         {"text-davinci-003", "p50k_base"},
         {"text-davinci-002", "p50k_base"},
         {"text-davinci-001", "r50k_base"},
         {"text-curie-001", "r50k_base"},
         {"text-babbage-001", "r50k_base"},
         {"text-ada-001", "r50k_base"},
         {"davinci", "r50k_base"},
         {"curie", "r50k_base"},
         {"babbage", "r50k_base"},
         {"ada", "r50k_base"},
         // code
         {"code-davinci-002", "p50k_base"},
         {"code-davinci-001", "p50k_base"},
         {"code-cushman-002", "p50k_base"},
         {"code-cushman-001", "p50k_base"},
         {"davinci-codex", "p50k_base"},
         {"cushman-codex", "p50k_base"},
         // edit
         {"text-davinci-edit-001", "p50k_edit"},
         {"code-davinci-edit-001", "p50k_edit"},
         // embeddings
         {"text-embedding-ada-002", "cl100k_base"},
         // old embeddings
         {"text-similarity-davinci-001", "r50k_base"},
         {"text-similarity-curie-001", "r50k_base"},
         {"text-similarity-babbage-001", "r50k_base"},
         {"text-similarity-ada-001", "r50k_base"},
         {"text-search-davinci-doc-001", "r50k_base"},
         {"text-search-curie-doc-001", "r50k_base"},
         {"text-search-babbage-doc-001", "r50k_base"},
         {"text-search-ada-doc-001", "r50k_base"},
         {"code-search-babbage-code-001", "r50k_base"},
         {"code-search-ada-code-001", "r50k_base"},
         //open source
         {"gpt2", "gpt2"}
      };

   private const string ENDOFTEXT   = "<|endoftext|>";
   private const string FIM_PREFIX  = "<|fim_prefix|>";
   private const string FIM_MIDDLE  = "<|fim_middle|>";
   private const string FIM_SUFFIX  = "<|fim_suffix|>";
   private const string ENDOFPROMPT = "<|endofprompt|>";

   private const string cl100k_base_BPE_Rank_File_name = "Tokenizer/cl100k_base.tiktoken";
   private const string p50k_base_BPE_Rank_File_name   = "Tokenizer/p50k_base.tiktoken";
   private const string r50k_base_BPE_Rank_File_name   = "Tokenizer/r50k_base.tiktoken";


   private static ITokenizer actually_CreateTokenizer(
      Stream                           tikTokenBpeFileStream,
      IReadOnlyDictionary<string, int> specialTokensEncoder,
      string                           pattern,
      int                              cacheSize = 8192) {
      return (ITokenizer) new TikTokenizer(tikTokenBpeFileStream, specialTokensEncoder, pattern, cacheSize);
   }

   public void the_way_openai_counts_tokens(List<OpenAI.Chat.Message> messages, string model_name) {
      int token_count = 0;
      foreach (var message in messages) {
         token_count += 3; //added by OpenAI API
         token_count += count_tokens(message.role.ToString(), model_name);
         token_count += count_tokens(message.content,         model_name);
         if (message.name != null) 
            token_count += count_tokens(message.name, model_name);
      }
      token_count += 3; // Add tokens to account for ending
   }
}