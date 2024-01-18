using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class ZeroShotTextClassificationTask : TaskBase<string, ZeroShotTextClassificationResponse, string[]> {
        public override string taskName => "ZeroShotTextClassification";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/facebook/bart-large-mnli";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/MoritzLaurer/DeBERTa-v3-base-mnli-fever-anli",
                "https://api-inference.huggingface.co/models/MoritzLaurer/mDeBERTa-v3-base-mnli-xnli"
            };
        }


        protected override IPayload GetPayload(string input, string[] context)
        {
            return new JObjectPayload(new JObject
            {
                ["inputs"] = input,
                ["parameters"] = new JObject
                {
                    ["candidate_labels"] = new JArray(context)
                }
            });
        }


        protected override bool PostProcess(object raw, string input, string[] context, out ZeroShotTextClassificationResponse response, out string error)
        {
            error = "";
            response = JsonConvert.DeserializeObject<ZeroShotTextClassificationResponse>((string)raw);

            if (response != null)
            {
                // Populate classifications property
                response.classifications = new List<Classification>();
                for (int i = 0; i < response.labels.Count; i++)
                {
                    Classification classification = new Classification
                    {
                        label = response.labels[i],
                        score = response.scores[i]
                        
                };
                    response.classifications.Add(classification);
                }
                return true;
            }
            response = null;
            error = $"Failed to cast response to {nameof(ZeroShotTextClassificationResponse)}.";
            return false;
        }
    }
}