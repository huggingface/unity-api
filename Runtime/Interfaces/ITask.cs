using System;

namespace HuggingFace.API {
    public interface ITask {
        string taskName { get; }
        string defaultEndpoint { get; }
        string[] backupEndpoints { get; }
        void Query(object input, IAPIClient client, IAPIConfig config, Action<object> onSuccess, Action<string> onError, object context = null);
    }
}