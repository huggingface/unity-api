using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class TextGenerationTask : TaskBase {
        public override string taskName => "TextGeneration";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/gpt2";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/bigscience/bloom",
                "https://api-inference.huggingface.co/models/EleutherAI/gpt-j-6b"
            };
        }

        protected override bool PostProcess(object raw, string input, object context, out string response, out string error) {
            error = "";
            JArray jsonArray = JArray.Parse((string)raw);
            if (jsonArray != null && jsonArray.Count > 0) {
                JObject jsonObject = (JObject)jsonArray[0];
                if (jsonObject != null && jsonObject.TryGetValue("generated_text", out JToken translationText)) {
                    response = translationText.ToString();
                    return true;
                }
            }
            response = null;
            error = $"Failed to extract generated_text from response.";
            return false;
        }
    }
}