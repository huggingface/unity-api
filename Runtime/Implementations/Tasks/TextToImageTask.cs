using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HuggingFace.API {
    public class TextToImageTask : ITask {
        public string taskName => "TextToImage";
        public string defaultEndpoint => "https://api-inference.huggingface.co/models/runwayml/stable-diffusion-v1-5";

        public void Query(object input, IAPIClient client, IAPIConfig config, Action<object> onSuccess, Action<string> onError, object context = null) {
            if (!config.GetTaskEndpoint(taskName, out TaskEndpoint taskEndpoint)) {
                onError?.Invoke($"Task endpoint for task {taskName} not found");
                return;
            }
            if (!(input is string inputText)) {
                onError?.Invoke("Input is not a string.");
                return;
            }
            JObject payload = new JObject {
                ["inputs"] = inputText
            };
            client.SendRequest(taskEndpoint.endpoint, config.apiKey, payload, response => {
                if(!(response is byte[] imageBytes)) {
                    onError?.Invoke("Failed to load image.");
                    return;
                }
                Texture2D texture = new Texture2D(2, 2);
                if(texture.LoadImage(imageBytes)) {
                    onSuccess?.Invoke(texture);
                } else {
                    onError?.Invoke("Failed to load image.");
                }
            }, onError).RunCoroutine();
        }
    }
}