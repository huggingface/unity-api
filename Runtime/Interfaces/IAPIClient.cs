using System;
using System.Collections;

namespace HuggingFace.API {
    public interface IAPIClient {
        IEnumerator SendRequest(string url, string apiKey, IPayload payload, Action<object> onSuccess, Action<string> onError);
        IEnumerator TestAPIKey(string apiKey, Action<string> onSuccess, Action<string> onError);
    }
}
