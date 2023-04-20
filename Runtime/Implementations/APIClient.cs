using UnityEngine.Networking;
using System;
using System.Collections;

namespace HuggingFace.API {
    public class APIClient : IAPIClient {
        public IEnumerator SendRequest(string url, string apiKey, IPayload payload, Action<object> onSuccess, Action<string> onError) {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST")) {
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                payload.Prepare(request);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                    string serverErrorMessage = request.downloadHandler.text;
                    if (!string.IsNullOrEmpty(serverErrorMessage)) {
                        onError?.Invoke($"{request.error} - {serverErrorMessage}");
                    } else {
                        onError?.Invoke(request.error);
                    }
                    yield break;
                } else {
                    string contentType = request.GetResponseHeader("Content-Type");
                    if (contentType != null && (contentType.StartsWith("text") || contentType.Equals("application/json"))) {
                        onSuccess?.Invoke(request.downloadHandler.text);
                    } else {
                        onSuccess?.Invoke(request.downloadHandler.data);
                    }
                }
            }
        }

        public IEnumerator TestAPIKey(string apiKey, Action<string> onSuccess, Action<string> onError) {
            using (UnityWebRequest request = new UnityWebRequest("https://huggingface.co/api/whoami-v2", "GET")) {
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                    string serverErrorMessage = request.downloadHandler.text;
                    if (!string.IsNullOrEmpty(serverErrorMessage)) {
                        onError?.Invoke($"{request.error} - {serverErrorMessage}");
                    } else {
                        onError?.Invoke(request.error);
                    }
                    yield break;
                } else {
                    string response = request.downloadHandler.text;
                    onSuccess?.Invoke(response);
                }
            }
        }
    }
}
