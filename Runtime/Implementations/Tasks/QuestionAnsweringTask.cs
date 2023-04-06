using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class QuestionAnsweringTask : TaskBase<string, QuestionAnsweringResponse, string> {
        public override string taskName => "QuestionAnswering";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/bert-large-uncased-whole-word-masking-finetuned-squad";

        protected override JObject GetPayload(string input, string context) {
            return new JObject {
                ["inputs"] = new JObject {
                    new JProperty("question", input),
                    new JProperty("context", context),
                }
            };
        }

        protected override bool PostProcess(object raw, string input, string context, out QuestionAnsweringResponse response, out string error) {
            error = "";
            response = JsonConvert.DeserializeObject<QuestionAnsweringResponse>((string)raw);
            return true;
        }
    }
}