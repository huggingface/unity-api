namespace HuggingFace.API {
    [System.Serializable]
    public class TaskEndpoint {
        public string taskName;
        public string endpoint;

        public TaskEndpoint(string taskName, string endpoint) {
            this.taskName = taskName;
            this.endpoint = endpoint;
        }
    }
}
