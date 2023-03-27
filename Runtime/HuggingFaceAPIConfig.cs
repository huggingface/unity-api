using UnityEngine;

namespace HuggingFace.API {
    [CreateAssetMenu(fileName = "HuggingFaceAPIConfig", menuName = "HuggingFace/API Config", order = 0)]
    public class HuggingFaceAPIConfig : ScriptableObject {
        public string apiKey;
        public string apiEndpoint = "https://api-inference.huggingface.co/models/facebook/blenderbot-400M-distill";
    }
}
