using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class ConversationTask : TaskBase<string, string, Conversation> {
        public override string taskName => "Conversation";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/facebook/blenderbot-400M-distill";

        protected override string[] LoadBackupEndpoints() {
            return new string[] {
                "https://api-inference.huggingface.co/models/microsoft/DialoGPT-medium",
                "https://api-inference.huggingface.co/models/facebook/blenderbot-3B"
            };
        }

        protected override bool VerifyContext(object context, out Conversation conversation) {
            conversation = null;
            if (context == null) {
                conversation = new Conversation();
                return true;
            } else if (context is Conversation) {
                conversation = (Conversation)context;
                return true;
            }
            return false;
        }

        protected override IPayload GetPayload(string input, Conversation conversation) {
            return new JObjectPayload(new JObject {
                ["inputs"] = new JObject {
                    new JProperty("past_user_inputs", new JArray(conversation.GetPastUserInputs().ToArray())),
                    new JProperty("generated_responses", new JArray(conversation.GetGeneratedResponses().ToArray())),
                    new JProperty("text", input)
                }
            });
        }

        protected override bool PostProcess(object raw, string input, Conversation conversation, out string response, out string error) {
            error = "";
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>((string)raw);
            if (!jsonResponse.TryGetValue("generated_text", out JToken responseObject)) {
                error = "Response does not contain a generated_text field.";
                response = null;
                return false;
            }
            string generatedResponse = responseObject.ToString();
            conversation.AddUserInput((string)input);
            conversation.AddGeneratedResponse(generatedResponse);
            response = generatedResponse;
            return true;
        }
    }
}