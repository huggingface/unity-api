using System;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class QuestionAnsweringTask : TaskBase<string, string, string> {
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
    }
}