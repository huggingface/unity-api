using System;
using System.Collections;

namespace HuggingFace.API {
    public interface IAPIClient {
        IEnumerator SendRequest(string url, string apiKey, IPayload payload, Action<object> onSuccess, Action<string> onError,
                                string[] backupEndpoints = null, bool waitForModel = true, float maxTimeout = 0f);
        IEnumerator TestAPIKey(string apiKey, Action<string> onSuccess, Action<string> onError);
    }
}
