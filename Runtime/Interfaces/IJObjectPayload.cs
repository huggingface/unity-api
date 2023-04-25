using Newtonsoft.Json.Linq;

namespace HuggingFace.API {
    public interface IJObjectPayload : IPayload {
        JObject payload { get; }
        void WaitForModel();
    }
}
