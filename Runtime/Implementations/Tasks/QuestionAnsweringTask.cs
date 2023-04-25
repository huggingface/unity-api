using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class QuestionAnsweringTask : TaskBase<string, QuestionAnsweringResponse, string> {
        public override string taskName => "QuestionAnswering";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/deepset/roberta-base-squad2";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/distilbert-base-cased-distilled-squad",
                "https://api-inference.huggingface.co/models/deepset/xlm-roberta-large-squad2"
            };
        }

        protected override IPayload GetPayload(string input, string context) {
            return new JObjectPayload(new JObject {
                ["inputs"] = new JObject {
                    new JProperty("question", input),
                    new JProperty("context", context),
                }
            });
        }

        protected override bool PostProcess(object raw, string input, string context, out QuestionAnsweringResponse response, out string error) {
            error = "";
            response = JsonConvert.DeserializeObject<QuestionAnsweringResponse>((string)raw);
            return true;
        }
    }
}