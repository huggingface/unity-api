using UnityEngine.Networking;

namespace HuggingFace.API {
    public interface IPayload {
        void Prepare(UnityWebRequest request);
    }
}
