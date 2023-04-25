using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class TranslationTask : TaskBase {
        public override string taskName => "Translation";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/t5-base";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/t5-small",
                "https://api-inference.huggingface.co/models/Helsinki-NLP/opus-mt-en-zh"
            };
        }

        protected override bool PostProcess(object raw, string input, object context, out string response, out string error) {
            error = "";
            JArray jsonArray = JArray.Parse((string)raw);
            if (jsonArray != null && jsonArray.Count > 0) {
                JObject jsonObject = (JObject)jsonArray[0];
                if (jsonObject != null && jsonObject.TryGetValue("translation_text", out JToken translationText)) {
                    response = translationText.ToString();
                    return true;
                }
            }
            response = null;
            error = $"Failed to extract translation_text from response.";
            return false;
        }
    }
}