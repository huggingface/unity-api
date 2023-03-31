using Newtonsoft.Json.Linq;
using System;
using System.Collections;

namespace HuggingFace.API {
    public interface IAPIClient {
        IEnumerator SendRequest(string url, string apiKey, JObject payload, Action<string> onSuccess, Action<string> onError);
        IEnumerator TestAPIKey(string apiKey, Action<string> onSuccess, Action<string> onError);
    }
}
