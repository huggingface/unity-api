using UnityEditor;
using UnityEngine;

namespace HuggingFace.API.Editor {
    [InitializeOnLoad]
    public class APIConfigUpdater {
        static APIConfigUpdater() {
            UpdateTaskEndpoints();
        }

        [MenuItem("Window/Hugging Face/Update Task Endpoints")]
        public static void UpdateTaskEndpoints() {
            var apiConfig = Resources.Load<APIConfig>("HuggingFaceAPIConfig");
            if (apiConfig == null) {
                Debug.LogError("HuggingFaceAPIConfig not found in Resources folder.");
                return;
            }
            apiConfig.UpdateTaskEndpoints();
        }
    }
}
