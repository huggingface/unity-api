using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HuggingFace.API {
    public class APIClient : IAPIClient {
        public IEnumerator SendRequest(string url, string apiKey, IPayload payload, Action<object> onSuccess, Action<string> onError,
                                       string[] backupEndpoints, bool waitForModel, float maxTimeout) {
            List<string> endpoints = new List<string> { url };
            if (backupEndpoints != null) {
                endpoints.AddRange(backupEndpoints);
            }

            bool success = false;
            string lastError = string.Empty;

            foreach (string endpoint in endpoints) {
                if (success) break;

                yield return AttemptRequest(endpoint, apiKey, payload, response => {
                    success = true;
                    onSuccess?.Invoke(response);
                }, error => {
                    lastError = error;
                    Debug.LogWarning($"Attempted request to {endpoint} failed: {error}");
                }, waitForModel, maxTimeout);
            }

            if (!success) {
                if (backupEndpoints != null) {
                    onError?.Invoke($"Attempted all backup endpoints, last error: {lastError}");
                } else {
                    onError?.Invoke(lastError);
                }
            }
        }

        private IEnumerator AttemptRequest(string url, string apiKey, IPayload payload, Action<object> onSuccess, Action<string> onError, bool waitForModel, float maxTimeout) {
            float startTime = Time.realtimeSinceStartup;
            bool retryWithLoadingOption = false;

            while (true) {
                using (UnityWebRequest request = new UnityWebRequest(url, "POST")) {
                    request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                    if (retryWithLoadingOption && payload is IJObjectPayload jObjectPayload) {
                        jObjectPayload.WaitForModel();
                    }

                    payload.Prepare(request);
                    request.downloadHandler = new DownloadHandlerBuffer();

                    UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();

                    while (!asyncOp.isDone && (maxTimeout == 0 || (Time.realtimeSinceStartup - startTime) < maxTimeout)) {
                        yield return null;
                    }

                    if (!asyncOp.isDone) {
                        request.Abort();
                        onError?.Invoke($"Request to {url} timed out after {maxTimeout} seconds");
                        break;
                    }

                    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                        string serverErrorMessage = request.downloadHandler.text;
                        if (!string.IsNullOrEmpty(serverErrorMessage)) {
                            if (waitForModel && serverErrorMessage.Contains("loading") && payload is IJObjectPayload) {
                                retryWithLoadingOption = true;
                                startTime = Time.realtimeSinceStartup;
                                Debug.LogWarning($"Attempted request to {url} failed: {request.error} - {serverErrorMessage}");
                                Debug.LogWarning("Retrying with wait_for_load option");
                                continue;
                            } else {
                                onError?.Invoke($"{request.error} - {serverErrorMessage}");
                                break;
                            }
                        } else {
                            onError?.Invoke(request.error);
                            break;
                        }
                    } else {
                        string contentType = request.GetResponseHeader("Content-Type");
                        if (contentType != null && (contentType.StartsWith("text") || contentType.Equals("application/json"))) {
                            onSuccess?.Invoke(request.downloadHandler.text);
                        } else {
                            onSuccess?.Invoke(request.downloadHandler.data);
                        }
                        break;
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
