using System.IO;
using UnityEditor;
using UnityEngine;

namespace HuggingFace.API.Editor {
    [InitializeOnLoad]
    public class HuggingFaceAPIWizard : EditorWindow {
        private string statusMessage = string.Empty;

        private static APIConfig config;
        private static string sourcePath;
        private static string destinationPath;

        private void OnEnable() {
            sourcePath = Path.GetFullPath("Packages/com.huggingface.api/Examples");
            destinationPath = Path.Combine(Application.dataPath, "HuggingFaceAPI/Examples");
        }

        static HuggingFaceAPIWizard() {
            EditorApplication.update += CheckConfig;
        }

        private static void CheckConfig() {
            EditorApplication.update -= CheckConfig;
            LoadOrCreateConfig();
            if (string.IsNullOrEmpty(config.apiKey)) {
                ShowWindow();
            }
        }

        private static void LoadOrCreateConfig() {
            string resourcesPath = "Assets/Resources";
            if (!AssetDatabase.IsValidFolder(resourcesPath)) {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            string configPath = $"{resourcesPath}/HuggingFaceAPIConfig.asset";
            config = AssetDatabase.LoadAssetAtPath<APIConfig>(configPath);
            if (config == null) {
                config = ScriptableObject.CreateInstance<APIConfig>();
                AssetDatabase.CreateAsset(config, configPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("Window/Hugging Face API Wizard")]
        public static void ShowWindow() {
            GetWindow<HuggingFaceAPIWizard>("Hugging Face API Wizard");
        }

        private void OnGUI() {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Instructions:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("1. Enter your API key. Generate keys at: https://huggingface.co/settings/profile\n2. Test the API key.\n3. Optionally, update the endpoints to use different models.", MessageType.Info);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Hugging Face API Setup", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            string apiKey = EditorGUILayout.TextField("API Key", config.apiKey);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(config, "Change API Key");
                config.SetAPIKey(apiKey);
                EditorUtility.SetDirty(config);
            }

            if (GUILayout.Button("Test API Key")) {
                statusMessage = "<color=white>Waiting for API response...</color>";
                Repaint();
                HuggingFaceAPI.TestAPIKey(apiKey, OnSuccess, OnError);
            }

            EditorGUILayout.LabelField("Status:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(statusMessage, new GUIStyle());

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Task Endpoints", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            for (int i = 0; i < config.taskEndpoints.Count; i++) {
                TaskEndpoint endpoint = config.taskEndpoints[i];
                EditorGUI.BeginChangeCheck();
                string newEndpoint = EditorGUILayout.TextField(endpoint.taskName, endpoint.endpoint);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(config, "Change Task Endpoint");
                    config.taskEndpoints[i] = new TaskEndpoint(endpoint.taskName, newEndpoint);
                    EditorUtility.SetDirty(config);
                }
            }

            EditorGUI.indentLevel--;

            if (GUILayout.Button("Reset to Defaults")) {
                Undo.RecordObject(config, "Reset Task Endpoints");
                config.InitializeTaskEndpoints();
                EditorUtility.SetDirty(config);
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            var content = new GUIContent("Use Backup Endpoints", "If the primary endpoint fails, the API will try to use the backup endpoints.");
            bool useBackupEndpoints = EditorGUILayout.Toggle(content, config.useBackupEndpoints);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(config, "Change Use Backup Endpoints");
                config.SetUseBackupEndpoints(useBackupEndpoints);
                EditorUtility.SetDirty(config);
            }

            EditorGUI.BeginChangeCheck();
            content = new GUIContent("Wait for Model", "If true, the API will wait for the model to load if necessary. If false, the API will send the request immediately and return an error if the model is not loaded.");
            bool waitForModel = EditorGUILayout.Toggle(content, config.waitForModel);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(config, "Change Wait for Model");
                config.SetWaitForModel(waitForModel);
                EditorUtility.SetDirty(config);
            }

            EditorGUI.BeginChangeCheck();
            content = new GUIContent("Max Timeout", "The maximum time to wait for a response from the API. If the API does not respond within this time, the request will be cancelled. Set to 0 to disable timeout.");
            float maxTimeout = EditorGUILayout.FloatField(content, config.maxTimeout);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(config, "Change Max Timeout");
                config.SetMaxTimeout(maxTimeout);
                EditorUtility.SetDirty(config);
            }

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Examples", EditorStyles.boldLabel);

            bool examplesInstalled = Directory.Exists(destinationPath);
            EditorGUI.BeginDisabledGroup(examplesInstalled);

            if (GUILayout.Button("Install Examples")) {
                InstallExamples();
            }

            EditorGUI.EndDisabledGroup();
            if (examplesInstalled) {
                EditorGUILayout.HelpBox("Examples are installed. You can find them in the HuggingFaceAPI/Examples folder.", MessageType.Info);
            }
        }

        private static void InstallExamples() {
            if (!Directory.Exists(sourcePath)) {
                Debug.LogError($"Examples not found at {sourcePath}");
                return;
            }

            if (!Directory.Exists(destinationPath)) {
                Directory.CreateDirectory(destinationPath);
            }

            CopyFilesRecursively(new DirectoryInfo(sourcePath), new DirectoryInfo(destinationPath));
            AssetDatabase.Refresh();

            Debug.Log($"Examples installed to {destinationPath}");
        }

        private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target) {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }

        private void OnSuccess(string response) {
            statusMessage = "<color=#5cb85c>API key is valid!</color>";
        }

        private void OnError(string error) {
            statusMessage = $"<color=#d9534f>{error}</color>";
        }
    }
}