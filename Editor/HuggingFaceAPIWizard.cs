using System.IO;
using UnityEditor;
using UnityEngine;

namespace HuggingFace.API.Editor {
    [InitializeOnLoad]
    public class HuggingFaceAPIWizard : EditorWindow {
        private string apiKey;
        private string apiEndpoint = "https://api-inference.huggingface.co/models/facebook/blenderbot-400M-distill";
        private string editorInputText = "Hello";
        private string responseText = string.Empty;
        private string statusMessage = string.Empty;

        private static string sourcePath = Path.Combine(Application.dataPath, "../Packages/com.huggingface.api/Examples");
        private static string destinationPath = Path.Combine(Application.dataPath, "HuggingFaceAPI/Examples");

        static HuggingFaceAPIWizard() {
            EditorApplication.update += CheckConfig;
        }

        private static void CheckConfig() {
            EditorApplication.update -= CheckConfig;
            if(!File.Exists("Assets/Resources/HuggingFaceAPIConfig.asset")) {
                ShowWindow();
            }
        }

        [MenuItem("Window/Hugging Face API Wizard")]
        public static void ShowWindow() {
            GetWindow<HuggingFaceAPIWizard>("Hugging Face API Wizard");
        }

        private void OnGUI() {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Instructions:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("1. Enter your API key. Generate keys at: https://huggingface.co/settings/profile\n2. Optionally, update the endpoint for a different conversation model.\n3. Test the API configuration by sending a query.\n4. Click \"Save Configuration\" to finalize settings and start using the API.", MessageType.Info);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Hugging Face API Setup", EditorStyles.boldLabel);

            apiKey = EditorGUILayout.TextField("API Key", apiKey);
            apiEndpoint = EditorGUILayout.TextField("API Endpoint", apiEndpoint);
            editorInputText = EditorGUILayout.TextField("Input Text", editorInputText);

            if(GUILayout.Button("Send Test Query")) {
                statusMessage = "Waiting for API response...";
                Repaint();
                HuggingFaceAPIConversation conversation = new HuggingFaceAPIConversation();
                HuggingFaceAPI.Query(apiKey, apiEndpoint, conversation, editorInputText, OnSuccess, OnError);
            }

            EditorGUILayout.LabelField("Status:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(statusMessage);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Response", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextArea(responseText, GUILayout.Height(100));
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);

            if(GUILayout.Button("Save Configuration")) {
                CreateAndSaveConfig();
            }

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
            statusMessage = "API call succeeded";
        }

        private void OnError(string error) {
            responseText = $"Error: {error}";
            statusMessage = $"API call failed: {error}";
        }

        private void CreateAndSaveConfig() {
            string resourcesPath = "Assets/Resources";
            if(!AssetDatabase.IsValidFolder(resourcesPath)) {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string configPath = $"{resourcesPath}/HuggingFaceAPIConfig.asset";
            HuggingFaceAPIConfig existingConfig = AssetDatabase.LoadAssetAtPath<HuggingFaceAPIConfig>(configPath);
            if(existingConfig != null) {
                existingConfig.apiKey = apiKey;
                existingConfig.apiEndpoint = apiEndpoint;
                EditorUtility.SetDirty(existingConfig);
            } else {
                HuggingFaceAPIConfig config = ScriptableObject.CreateInstance<HuggingFaceAPIConfig>();
                config.apiKey = apiKey;
                config.apiEndpoint = apiEndpoint;
                AssetDatabase.CreateAsset(config, $"{resourcesPath}/HuggingFaceAPIConfig.asset");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            statusMessage = $"Configuration saved to {configPath}";
        }
    }
}