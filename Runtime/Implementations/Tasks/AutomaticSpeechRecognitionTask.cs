using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class AutomaticSpeechRecognitionTask : TaskBase<byte[], string> {
        public override string taskName => "AutomaticSpeechRecognition";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/openai/whisper-tiny";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/facebook/wav2vec2-base-960h",
                "https://api-inference.huggingface.co/models/nvidia/stt_en_conformer_transducer_xlarge"
            };
        }

        protected override IPayload GetPayload(byte[] input, object context) {
            return new ByteArrayPayload(input);
        }

        protected override bool PostProcess(object raw, byte[] input, object context, out string response, out string error) {
            error = "";
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>((string)raw);
            if (!jsonResponse.TryGetValue("text", out JToken responseObject)) {
                error = "Response does not contain a text field.";
                response = null;
                return false;
            }
            response = responseObject.ToString();
            return true;
        }
    }
}