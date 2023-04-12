using System;
using UnityEngine.Networking;

namespace HuggingFace.API {
    public class ByteArrayPayload : IPayload {
        public byte[] payload { get; private set; }

        public ByteArrayPayload(byte[] payload) {
            this.payload = payload;
        }

        public void Prepare(UnityWebRequest request) {
            request.SetRequestHeader("Content-Type", "application/octet-stream");
            request.uploadHandler = new UploadHandlerRaw(payload);
        }

        public override string ToString() {
            return BitConverter.ToString(payload);
        }
    }
}
