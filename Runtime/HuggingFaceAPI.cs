using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using System.Collections;

namespace HuggingFace.API {
    public static class HuggingFaceAPI {
        private static HuggingFaceAPIConfig config;

        public static void Query(HuggingFaceAPIConversation conversation, string inputText, Action<string> onSuccess, Action<string> onError) {
            if(config == null) {
                config = Resources.Load<HuggingFaceAPIConfig>("HuggingFaceAPIConfig");
                if(config == null) {
                    Debug.LogError("HuggingFaceAPIConfig asset not found.");
                    onError?.Invoke("HuggingFaceAPIConfig asset not found.");
                    return;
                }
            }
            Query(config.apiKey, config.apiEndpoint, conversation, inputText, onSuccess, onError);
        }

        public static void Query(string apiKey, string apiEndpoint, HuggingFaceAPIConversation conversation, string inputText, Action<string> onSuccess, Action<string> onError) {
            JObject payload = new JObject {
                ["inputs"] = new JObject {
                new JProperty("past_user_inputs", new JArray(conversation.GetPastUserInputs().ToArray())),
                new JProperty("generated_responses", new JArray(conversation.GetGeneratedResponses().ToArray())),
                new JProperty("text", inputText)
            }
            };

            RunCoroutine(SendRequest(apiEndpoint, apiKey, payload, response => {
                conversation.AddUserInput(inputText);
                conversation.AddGeneratedResponse(response);
                onSuccess?.Invoke(response);
            }, onError));
        }

        private static IEnumerator SendRequest(string url, string apiKey, JObject payload, Action<string> onSuccess, Action<string> onError) {
            using(UnityWebRequest request = new UnityWebRequest(url, "POST")) {
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                request.SetRequestHeader("Content-Type", "application/json");
                request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(payload.ToString()));
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                    onError?.Invoke(request.error);
                    yield break;
                } else {
                    string response = request.downloadHandler.text;
                    JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response);
                    if(!jsonResponse.TryGetValue("generated_text", out JToken responseObject)) {
                        onError?.Invoke("Response does not contain a generated_text field.");
                        yield break;
                    }
                    string generatedResponse = responseObject.ToString();
                    onSuccess?.Invoke(generatedResponse);
                }
            }
        }

        private class CoroutineRunner : MonoBehaviour { }

        private static void RunCoroutine(IEnumerator coroutine) {
            GameObject coroutineRunnerObject = new GameObject("HuggingFaceAPI_CoroutineRunner");
            CoroutineRunner coroutineRunner = coroutineRunnerObject.AddComponent<CoroutineRunner>();
            coroutineRunner.StartCoroutine(RunAndDestroy(coroutine, coroutineRunnerObject));
        }

        private static IEnumerator RunAndDestroy(IEnumerator coroutine, GameObject coroutineRunnerObject) {
            yield return coroutine;
#if UNITY_EDITOR
            GameObject.DestroyImmediate(coroutineRunnerObject);
#else
            GameObject.Destroy(coroutineRunnerObject);
#endif
        }
    }
}
