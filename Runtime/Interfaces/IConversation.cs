using System.Collections.Generic;

namespace HuggingFace.API {
    public interface IConversation {
        void AddUserInput(string userInput);
        void AddGeneratedResponse(string generatedResponse);
        string GetLatestResponse();
        List<string> GetPastUserInputs();
        List<string> GetGeneratedResponses();
        void Clear();
    }
}
