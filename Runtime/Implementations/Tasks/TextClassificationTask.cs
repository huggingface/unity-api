using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class TextClassificationTask : TaskBase<string, TextClassificationResponse> {
        public override string taskName => "TextClassification";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/distilbert-base-uncased-finetuned-sst-2-english";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/ProsusAI/finbert",
                "https://api-inference.huggingface.co/models/cardiffnlp/twitter-roberta-base-sentiment"
            };
        }

        protected override IPayload GetPayload(string input, object context) {
            return new JObjectPayload(new JObject {
                ["inputs"] = input
            });
        }

        protected override bool PostProcess(object raw, string input, object context, out TextClassificationResponse response, out string error) {
            error = "";
            JArray jsonArray = JArray.Parse((string)raw);
            if (jsonArray != null && jsonArray.Count > 0) {
                List<Classification> classifications = jsonArray[0].ToObject<List<Classification>>();
                response = new TextClassificationResponse {
                    classifications = classifications
                };
                return true;
            }
            response = null;
            error = $"Failed to cast response to {nameof(TextClassificationResponse)}.";
            return false;
        }
    }
}