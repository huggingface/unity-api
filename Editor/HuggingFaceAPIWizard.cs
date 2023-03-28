using System.IO;
using UnityEditor;
using UnityEngine;

namespace HuggingFace.API.Editor {
    [InitializeOnLoad]
    public class HuggingFaceAPIWizard : EditorWindow {
        private string editorInputText = "Hello";
        private string responseText = string.Empty;
        private string statusMessage = string.Empty;

        private static HuggingFaceAPIConfig config;
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
            if(string.IsNullOrEmpty(config.apiKey)) {
                ShowWindow();
            }
        }

        private static void LoadOrCreateConfig() {
            string resourcesPath = "Assets/Resources";
            if(!AssetDatabase.IsValidFolder(resourcesPath)) {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            string configPath = $"{resourcesPath}/HuggingFaceAPIConfig.asset";
            config = AssetDatabase.LoadAssetAtPath<HuggingFaceAPIConfig>(configPath);
            if(config == null) {
                config = ScriptableObject.CreateInstance<HuggingFaceAPIConfig>();
                config.apiEndpoint = "https://api-inference.huggingface.co/models/facebook/blenderbot-400M-distill";
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
            EditorGUILayout.HelpBox("1. Enter your API key. Generate keys at: https://huggingface.co/settings/profile\n2. Optionally, update the endpoint for a different conversation model.\n3. Test the API configuration by sending a query.", MessageType.Info);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Hugging Face API Setup", EditorStyles.boldLabel);

            config.apiKey = EditorGUILayout.TextField("API Key", config.apiKey);
            config.apiEndpoint = EditorGUILayout.TextField("API Endpoint", config.apiEndpoint);
            editorInputText = EditorGUILayout.TextField("Input Text", editorInputText);

            if(GUILayout.Button("Send Test Query")) {
                statusMessage = "<color=white>Waiting for API response...</color>";
                Repaint();
                HuggingFaceAPIConversation conversation = new HuggingFaceAPIConversation();
                HuggingFaceAPI.Query(conversation, editorInputText, OnSuccess, OnError);
            }

            EditorGUILayout.LabelField("Status:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(statusMessage, new GUIStyle());


            GUILayout.Space(10);
            EditorGUILayout.LabelField("Response", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextArea(responseText, GUILayout.Height(100));
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Examples", EditorStyles.boldLabel);

            bool examplesInstalled = Directory.Exists(destinationPath);
            EditorGUI.BeginDisabledGroup(examplesInstalled);

            if(GUILayout.Button("Install Examples")) {
                InstallExamples();
            }

            EditorGUI.EndDisabledGroup();
            if(examplesInstalled) {
                EditorGUILayout.HelpBox("Examples are installed. You can find them in the HuggingFaceAPI/Examples folder.", MessageType.Info);
            }
        }

        private static void InstallExamples() {
            if(!Directory.Exists(sourcePath)) {
                Debug.LogError($"Examples not found at {sourcePath}");
                return;
            }

            if(!Directory.Exists(destinationPath)) {
                Directory.CreateDirectory(destinationPath);
            }

            CopyFilesRecursively(new DirectoryInfo(sourcePath), new DirectoryInfo(destinationPath));
            AssetDatabase.Refresh();

            Debug.Log($"Examples installed to {destinationPath}");
        }

        private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target) {
            foreach(DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

            foreach(FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }

        private void OnSuccess(string response) {
            responseText = response;
            statusMessage = "<color=#5cb85c>API call succeeded! You may now close this window.</color>";
        }

        private void OnError(string error) {
            responseText = $"Error: {error}";
            statusMessage = $"<color=#d9534f>API call failed: {error}</color>";
        }
    }
}