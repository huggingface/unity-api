using System.Collections.Generic;

public class HuggingFaceAPIConversation {
    private List<string> pastUserInputs = new List<string>();
    private List<string> generatedResponses = new List<string>();

    public void AddUserInput(string userInput) {
        pastUserInputs.Add(userInput);
    }

    public void AddGeneratedResponse(string generatedResponse) {
        generatedResponses.Add(generatedResponse);
    }

    public List<string> GetPastUserInputs() {
        return pastUserInputs;
    }

    public List<string> GetGeneratedResponses() {
        return generatedResponses;
    }

    public string GetFormattedConversation() {
        string conversation = "";

        for(int i = 0; i < pastUserInputs.Count; i++) {
            conversation += "User: " + pastUserInputs[i] + "\n";
            conversation += "Bot: " + generatedResponses[i] + "\n\n";
        }

        return conversation;
    }

    public void Clear() {
        pastUserInputs.Clear();
        generatedResponses.Clear();
    }
}
