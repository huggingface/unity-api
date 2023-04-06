using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class SentenceSimilarityTask : TaskBase<string, float[], string[]> {
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

        protected override bool PostProcess(object raw, string input, string[] context, out float[] response, out string error) {
            error = "";
            response = JsonConvert.DeserializeObject<float[]>((string)raw);
            return true;
        }
    }
}