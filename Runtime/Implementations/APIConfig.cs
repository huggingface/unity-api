using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HuggingFace.API {
    [CreateAssetMenu(fileName = "HuggingFaceAPIConfig", menuName = "HuggingFace/API Config", order = 0)]
    public class APIConfig : ScriptableObject, IAPIConfig {
        [SerializeField] private string _apiKey;
        [SerializeField] private List<TaskEndpoint> _taskEndpoints;

        public string apiKey => _apiKey;
        public List<TaskEndpoint> taskEndpoints => _taskEndpoints;

        public APIConfig() {
            InitializeTaskEndpoints();
        }  

        private void InitializeTaskEndpoints() {
            var taskTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ITask)) && !t.IsInterface && !t.IsAbstract);
            _taskEndpoints = new List<TaskEndpoint>();
            foreach (var taskType in taskTypes) {
                var task = (ITask)Activator.CreateInstance(taskType);
                _taskEndpoints.Add(new TaskEndpoint(task.taskName, task.defaultEndpoint));
            }
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

        public string[] GetTaskNames() {
            if(taskEndpoints == null)
                return new string[0];
            return taskEndpoints.Select(e => e.taskName).ToArray();
        }
    }
}
