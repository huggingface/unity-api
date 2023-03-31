using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public class ConversationTask : ITask {
        public string taskName => "Conversation";
        public string defaultEndpoint => "https://api-inference.huggingface.co/models/facebook/blenderbot-400M-distill";

        public void Query(object input, IAPIClient client, IAPIConfig config, Action<object> onSuccess, Action<string> onError, object context = null) {
            if (!config.GetTaskEndpoint(taskName, out TaskEndpoint taskEndpoint)) {
                onError?.Invoke($"Task endpoint for task {taskName} not found");
                return;
            }
            if (!(input is string inputText)) {
                onError?.Invoke("Input is not a string.");
                return;
            }
            Conversation conversation = null;
            if (context == null) {
                context = conversation = new Conversation();
            } else if (context is Conversation) {
                conversation = (Conversation)context;
            } else {
                onError?.Invoke("Context is not a Conversation.");
                return;
            }
            JObject payload = new JObject {
                ["inputs"] = new JObject {
                    new JProperty("past_user_inputs", new JArray(conversation.GetPastUserInputs().ToArray())),
                    new JProperty("generated_responses", new JArray(conversation.GetGeneratedResponses().ToArray())),
                    new JProperty("text", inputText)
                }
            };
            client.SendRequest(taskEndpoint.endpoint, config.apiKey, payload, response => {
                JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response);
                if (!jsonResponse.TryGetValue("generated_text", out JToken responseObject)) {
                    onError?.Invoke("Response does not contain a generated_text field.");
                    return;
                }
                string generatedResponse = responseObject.ToString();
                conversation.AddUserInput(inputText);
                conversation.AddGeneratedResponse(generatedResponse);
                onSuccess?.Invoke(conversation);
            }, onError).RunCoroutine();
        }
    }
}