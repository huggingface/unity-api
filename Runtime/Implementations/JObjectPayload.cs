using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace HuggingFace.API {
    public class JObjectPayload : IJObjectPayload {
        public JObject payload { get; private set; }

        public JObjectPayload(JObject payload) {
            this.payload = payload;
        }

        public void Prepare(UnityWebRequest request) {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(payload.ToString()));
        }

        public void WaitForModel() {
            var options = new Dictionary<string, object> {
                { "wait_for_model", true }
            };
            payload["options"] = JObject.FromObject(options);
        }

        public override string ToString() {
            return payload.ToString();
        }
    }
}
