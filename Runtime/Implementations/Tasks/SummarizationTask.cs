using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class SummarizationTask : TaskBase {
        public override string taskName => "Summarization";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/facebook/bart-large-cnn";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/philschmid/bart-large-cnn-samsum",
                "https://api-inference.huggingface.co/models/sshleifer/distilbart-cnn-12-6"
            };
        }

        protected override bool PostProcess(object raw, string input, object context, out string response, out string error) {
            error = "";
            JArray jsonArray = JArray.Parse((string)raw);
            if (jsonArray != null && jsonArray.Count > 0) {
                JObject jsonObject = (JObject)jsonArray[0];
                if (jsonObject != null && jsonObject.TryGetValue("summary_text", out JToken translationText)) {
                    response = translationText.ToString();
                    return true;
                }
            }
            response = null;
            error = $"Failed to extract summarized_text from response.";
            return false;
        }
    }
}