using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace HuggingFace.API {
    public class JObjectPayload : IPayload {
        public JObject payload { get; private set; }

        public JObjectPayload(JObject payload) {
            this.payload = payload;
        }

        public void Prepare(UnityWebRequest request) {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(payload.ToString()));
        }

        public override string ToString() {
            return payload.ToString();
        }
    }
}
