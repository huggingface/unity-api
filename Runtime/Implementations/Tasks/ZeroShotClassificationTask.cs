using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class ZeroShotClassificationTask : TaskBase<string, string, string[]> {
        public override string taskName => "ZeroShotClassification";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/valhalla/distilbart-mnli-12-1";

        protected override JObject GetPayload(string input, string[] context) {
            return new JObject {
                new JProperty("inputs", input),
                new JProperty("parameters", new JObject {
                    new JProperty("candidate_labels", new JArray(context))
                })
            };
        }
    }
}