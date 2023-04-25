using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HuggingFace.API {
    [CreateAssetMenu(fileName = "HuggingFaceAPIConfig", menuName = "HuggingFace/API Config", order = 0)]
    public class APIConfig : ScriptableObject, IAPIConfig {
        [SerializeField] private string _apiKey;
        [SerializeField] private bool _useBackupEndpoints = true;
        [SerializeField] private bool _waitForModel = true;
        [SerializeField] private float _maxTimeout = 3f;
        [SerializeField] private List<TaskEndpoint> _taskEndpoints;

        public string apiKey => _apiKey;
        public bool useBackupEndpoints => _useBackupEndpoints;
        public bool waitForModel => _waitForModel;
        public float maxTimeout => _maxTimeout;
        public List<TaskEndpoint> taskEndpoints => _taskEndpoints;

        public APIConfig() {
            if (taskEndpoints == null)
                InitializeTaskEndpoints();
            else
                UpdateTaskEndpoints();
        }

        public void InitializeTaskEndpoints() {
            var taskTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ITask)) && !t.IsInterface && !t.IsAbstract);
            _taskEndpoints = new List<TaskEndpoint>();
            foreach (var taskType in taskTypes) {
                var task = (ITask)Activator.CreateInstance(taskType);
                _taskEndpoints.Add(new TaskEndpoint(task.taskName, task.defaultEndpoint));
            }
        }

        public void UpdateTaskEndpoints() {
            var taskTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ITask)) && !t.IsInterface && !t.IsAbstract);
            HashSet<string> currentTaskNames = new HashSet<string>();
            foreach (var taskType in taskTypes) {
                var task = (ITask)Activator.CreateInstance(taskType);
                currentTaskNames.Add(task.taskName);
                var existingEndpoint = taskEndpoints.FirstOrDefault(e => e.taskName == task.taskName);
                if (existingEndpoint == null) {
                    taskEndpoints.Add(new TaskEndpoint(task.taskName, task.defaultEndpoint));
                } else if (string.IsNullOrEmpty(existingEndpoint.endpoint)) {
                    existingEndpoint.endpoint = task.defaultEndpoint;
                }
            }
            taskEndpoints.RemoveAll(x => !currentTaskNames.Contains(x.taskName));
        }

        public bool GetTaskEndpoint(string taskName, out TaskEndpoint taskEndpoint) {
            foreach (var endpoint in taskEndpoints) {
                if (endpoint.taskName == taskName) {
                    taskEndpoint = endpoint;
                    return true;
                }
            }
            taskEndpoint = null;
            return false;
        }

        public void SetAPIKey(string apiKey) {
            _apiKey = apiKey;
        }

        public void SetUseBackupEndpoints(bool useBackupEndpoints) {
            _useBackupEndpoints = useBackupEndpoints;
        }

        public void SetWaitForModel(bool waitForModel) {
            _waitForModel = waitForModel;
        }

        public void SetMaxTimeout(float maxTimeout) {
            _maxTimeout = maxTimeout;
        }

        public string[] GetTaskNames() {
            if (taskEndpoints == null)
                return new string[0];
            return taskEndpoints.Select(e => e.taskName).ToArray();
        }
    }
}
