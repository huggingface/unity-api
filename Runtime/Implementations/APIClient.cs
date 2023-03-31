using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;

namespace HuggingFace.API {
    public class APIClient : IAPIClient {
        public IEnumerator SendRequest(string url, string apiKey, JObject payload, Action<string> onSuccess, Action<string> onError) {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST")) {
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                request.SetRequestHeader("Content-Type", "application/json");
                request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(payload.ToString()));
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                    onError?.Invoke(request.error);
                    yield break;
                } else {
                    string response = request.downloadHandler.text;
                    onSuccess?.Invoke(response);
                }
            }
        }

        public IEnumerator TestAPIKey(string apiKey, Action<string> onSuccess, Action<string> onError) {
            using (UnityWebRequest request = new UnityWebRequest("https://huggingface.co/api/whoami-v2", "GET")) {
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                    onError?.Invoke(request.error);
                } else {
                    string response = request.downloadHandler.text;
                    onSuccess?.Invoke(response);
                }
            }
        }
    }
}
