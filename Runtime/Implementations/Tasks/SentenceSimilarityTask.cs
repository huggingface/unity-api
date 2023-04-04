using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class SentenceSimilarityTask : TaskBase<string, string, string[]> {
        public override string taskName => "SentenceSimilarity";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2";

        protected override JObject GetPayload(string input, string[] context) {
            return new JObject {
                ["inputs"] = new JObject {
                    ["source_sentence"] = input,
                    ["sentences"] = new JArray(context)
                }
            };
        }
    }
}