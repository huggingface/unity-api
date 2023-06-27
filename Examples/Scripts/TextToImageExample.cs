using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HuggingFace.API.Examples {
    public class TextToImageExample : MonoBehaviour {
        [SerializeField] private Image image;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Button generateButton;

        private string normalColorHex;
        private string errorColorHex;
        private bool isWaitingForResponse;

        private void Awake() {
            normalColorHex = ColorUtility.ToHtmlStringRGB(statusText.color);
            errorColorHex = ColorUtility.ToHtmlStringRGB(Color.red);
            image.color = Color.black;
        }

        private void Start() {
            generateButton.onClick.AddListener(GenerateButtonClicked);
            inputField.ActivateInputField();
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }

        private void GenerateButtonClicked() {
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

            statusText.text = $"<color=#{normalColorHex}>Generating...</color>";
            image.color = Color.black;

            isWaitingForResponse = true;
            inputField.interactable = false;
            generateButton.interactable = false;
            inputField.text = "";

            HuggingFaceAPI.TextToImage(inputText, texture => {
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.color = Color.white;
                statusText.text = $"";
                isWaitingForResponse = false;
                inputField.interactable = true;
                generateButton.interactable = true;
                inputField.ActivateInputField();
            }, error => {
                statusText.text = $"<color=#{errorColorHex}>Error: {error}</color>";
                isWaitingForResponse = false;
                inputField.interactable = true;
                generateButton.interactable = true;
                inputField.ActivateInputField();
            });
        }
    }
}