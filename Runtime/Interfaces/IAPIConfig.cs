using System.Collections.Generic;

namespace HuggingFace.API {
    public interface IAPIConfig {
        string apiKey { get; }
        bool useBackupEndpoints { get; }
        bool waitForModel { get; }
        float maxTimeout { get; }
        List<TaskEndpoint> taskEndpoints { get; }
        bool GetTaskEndpoint(string taskName, out TaskEndpoint taskEndpoint);
    }
}