using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HuggingFace.API.Examples {
    public class ConversationExample : MonoBehaviour {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private TMP_Text conversationText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Color userTextColor = Color.blue;
        [SerializeField] private Color botTextColor = Color.black;

        private Conversation conversation = new Conversation();
        private string userColorHex;
        private string botColorHex;
        private string errorColorHex;
        private bool isWaitingForResponse;

        private void Awake() {
            userColorHex = ColorUtility.ToHtmlStringRGB(userTextColor);
            botColorHex = ColorUtility.ToHtmlStringRGB(botTextColor);
            errorColorHex = ColorUtility.ToHtmlStringRGB(Color.red);
        }

        private void Start() {
            sendButton.onClick.AddListener(SendButtonClicked);
            clearButton.onClick.AddListener(ClearButtonClicked);
            inputField.ActivateInputField();
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }

        private void SendButtonClicked() {
            SendQuery();
        }

        private void OnInputFieldEndEdit(string text) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                SendQuery();
            }
        }

        private void SendQuery() {
            if (isWaitingForResponse) return;

            string inputText = inputField.text;
            if (string.IsNullOrEmpty(inputText)) {
                return;
            }

            isWaitingForResponse = true;
            inputField.interactable = false;
            sendButton.interactable = false;
            inputField.text = "";

            conversationText.text += $"<color=#{userColorHex}>You: {inputText}</color>\n";
            conversationText.text += "Bot is typing...\n";

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;

            HuggingFaceAPI.Conversation(inputText, response => {
                string reply = conversation.GetLatestResponse();
                conversationText.text = conversationText.text.TrimEnd("Bot is typing...\n".ToCharArray());
                conversationText.text += $"\n<color=#{botColorHex}>Bot: {reply}</color>\n\n";
                inputField.interactable = true;
                sendButton.interactable = true;
                inputField.ActivateInputField();
                isWaitingForResponse = false;
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }, error => {
                conversationText.text = conversationText.text.TrimEnd("Bot is typing...\n".ToCharArray());
                conversationText.text += $"\n<color=#{errorColorHex}>Error: {error}</color>\n\n";
                inputField.interactable = true;
                sendButton.interactable = true;
                inputField.ActivateInputField();
                isWaitingForResponse = false;
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }, conversation);
        }

        private void ClearButtonClicked() {
            conversationText.text = "";
            conversation.Clear();
        }
    }
}