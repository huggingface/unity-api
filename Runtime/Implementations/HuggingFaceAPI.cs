using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HuggingFace.API {
    public static class HuggingFaceAPI {
        private static IAPIConfig _config;
        private static IAPIConfig config {
            get {
                if (_config == null) {
                    _config = Resources.Load<APIConfig>("HuggingFaceAPIConfig");
                    if (_config == null) {
                        Debug.LogError("HuggingFaceAPIConfig asset not found.");
                    }
                }
                return _config;
            }
        }

        private static IAPIClient _apiClient;
        private static IAPIClient apiClient {
            get {
                if (_apiClient == null) {
                    _apiClient = new APIClient();
                }
                return _apiClient;
            }
        }

        private static Dictionary<string, ITask> tasks;

        static HuggingFaceAPI() {
            LoadTasks();
        }

        private static void LoadTasks() {
            tasks = new Dictionary<string, ITask>();

            var taskTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ITask)) && !t.IsInterface && !t.IsAbstract);

            foreach (var taskType in taskTypes) {
                var task = (ITask)Activator.CreateInstance(taskType);
                tasks.Add(task.taskName, task);
            }
        }

        /// <summary>
        /// Generic query method for all tasks.
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        /// <param name="context"></param>
        public static void Query(string taskName, object input, Action<object> onSuccess, Action<string> onError, object context = null) {
            if (tasks.TryGetValue(taskName, out var task)) {
                task.Query(input, apiClient, config, onSuccess, onError, context);
            } else {
                onError($"Task {taskName} not found.");
            }
        }

        /// <summary>
        /// Queries the conversation task and returns a response as a string.<br/>
        /// Provide a conversation context to continue a conversation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        /// <param name="context"></param>
        public static void Conversation(string input, Action<string> onSuccess, Action<string> onError, Conversation context = null) {
            Query<string, string, Conversation>("Conversation", input, onSuccess, onError, context);
        }

        /// <summary>
        /// Generates an image from text and returns the response as a Texture2D.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void TextToImage(string input, Action<Texture2D> onSuccess, Action<string> onError) {
            Query<string, Texture2D>("TextToImage", input, onSuccess, onError);
        }

        /// <summary>
        /// Classifies the input text and returns labels and scores in a TextClassificationResponse object.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void TextClassification(string input, Action<TextClassificationResponse> onSuccess, Action<string> onError) {
            Query<string, TextClassificationResponse>("TextClassification", input, onSuccess, onError);
        }

        /// <summary>
        /// Answers the question based on the context and returns the response in a QuestionAnsweringResponse object.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        /// <param name="context"></param>
        public static void QuestionAnswering(string input, Action<QuestionAnsweringResponse> onSuccess, Action<string> onError, string context) {
            Query<string, QuestionAnsweringResponse, string>("QuestionAnswering", input, onSuccess, onError, context);
        }

        /// <summary>
        /// Finds which sentences in the context are similar to the input and returns the scores as a float array.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        /// <param name="context"></param>
        public static void SentenceSimilarity(string input, Action<float[]> onSuccess, Action<string> onError, string[] context) {
            Query<string, float[], string[]>("SentenceSimilarity", input, onSuccess, onError, context);
        }

        /// <summary>
        /// Translates the input text according to the model and returns the response as a string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void Translation(string input, Action<string> onSuccess, Action<string> onError) {
            Query<string, string>("Translation", input, onSuccess, onError);
        }

        /// <summary>
        /// Generates a summary of the input text and returns the response as a string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void Summarization(string input, Action<string> onSuccess, Action<string> onError) {
            Query<string, string>("Summarization", input, onSuccess, onError);
        }

        /// <summary>
        /// Generates a text from the input text and returns the response as a string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void TextGeneration(string input, Action<string> onSuccess, Action<string> onError) {
            Query<string, string>("TextGeneration", input, onSuccess, onError);
        }

        /// <summary>
        /// Generates a text from the input text and returns the response as a string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void AutomaticSpeechRecognition(byte[] input, Action<string> onSuccess, Action<string> onError) {
            Query<byte[], string>("AutomaticSpeechRecognition", input, onSuccess, onError);
        }

        /// <summary>
        /// Classifies the input text based on user labels and returns labels and scores in a ZeroShotTextClassificationResponse object.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void ZeroShotTextClassification(string input, Action<ZeroShotTextClassificationResponse> onSuccess, Action<string> onError, string[] context)
        {
            Query<string, ZeroShotTextClassificationResponse, string[]>("ZeroShotTextClassification", input, onSuccess, onError, context);
        }


        /// <summary>
        /// Test the API key by sending a request to the API.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public static void TestAPIKey(string apiKey, Action<string> onSuccess, Action<string> onError) {
            apiClient.TestAPIKey(apiKey, onSuccess, onError).RunCoroutine();
        }

        private static void Query<TInput, TResponse>(string taskName, TInput input, Action<TResponse> onSuccess, Action<string> onError) {
            if (!(input is TInput)) {
                onError?.Invoke($"Input is not of type {typeof(TInput)}");
                return;
            }
            Query(taskName, input, (response) => {
                if (!(response is TResponse)) {
                    onError?.Invoke($"Response is not of type {typeof(TResponse)}");
                    return;
                }
                onSuccess?.Invoke((TResponse)response);
            }, onError);
        }

        private static void Query<TInput, TResponse, TContext>(string taskName, TInput input, Action<TResponse> onSuccess, Action<string> onError, TContext context = default(TContext)) {
            if (!(input is TInput)) {
                onError?.Invoke($"Input is not of type {typeof(TInput)}");
                return;
            }
            if (!(context == null || context is TContext)) {
                onError?.Invoke($"Context is not of type {typeof(TContext)}");
                return;
            }
            Query(taskName, input, (response) => {
                if (!(response is TResponse)) {
                    onError?.Invoke($"Response is not of type {typeof(TResponse)}");
                    return;
                }
                onSuccess?.Invoke((TResponse)response);
            }, onError, context);
        }
    }
}
