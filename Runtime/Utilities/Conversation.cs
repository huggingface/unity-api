using System.Collections.Generic;
using UnityEngine;

namespace HuggingFace.API {
    public class Conversation {
        private List<string> pastUserInputs = new List<string>();
        private List<string> generatedResponses = new List<string>();

        public void AddUserInput(string userInput) {
            pastUserInputs.Add(userInput);
        }

        public void AddGeneratedResponse(string generatedResponse) {
            generatedResponses.Add(generatedResponse);
        }

        public string GetLatestResponse() {
            if(generatedResponses.Count == 0) {
                Debug.LogWarning("No generated responses found.");
                return "";
            }
            return generatedResponses[generatedResponses.Count - 1];
        }

        public List<string> GetPastUserInputs() {
            return pastUserInputs;
        }

        public List<string> GetGeneratedResponses() {
            return generatedResponses;
        }

        public string GetFormattedConversation() {
            string conversation = "";

            for (int i = 0; i < pastUserInputs.Count; i++) {
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
}