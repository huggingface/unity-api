# Hugging Face API Unity Integration

This Unity package provides an easy-to-use integration for the Hugging Face Inference API, allowing developers to access and use Hugging Face AI models within their Unity projects.

## Installation

### Via Git URL

1. Open the Unity project you want to add the package to.
2. Go to "Window" > "Package Manager" to open the Package Manager.
3. Click the "+" in the upper left hand corner and select "Add package from git URL".
4. Enter the URL of this repository and click "Add": https://github.com/huggingface/unity-api.git

## Usage

### Configuration

After installation, the Hugging Face API wizard should open. If not, open it by clicking "Window" > "Hugging Face API Wizard".

1. Enter your API key. Generate keys at: https://huggingface.co/settings/profile
2. Optionally, update the endpoint for a different conversation model.
3. Test the API configuration by sending a query.
4. Click "Save Configuration" to finalize settings and start using the API.

### Example Scene

To try the included example scene, follow these steps:

1. Click "Install Examples" in the Hugging Face API Wizard to copy the example files into your project.
2. Navigate to the "Hugging Face API" > "Examples" > "Scenes" folder in your project.
3. Open the "ConversationExample" scene.
4. Press "Play" to run the example. You should be able to use the UI to interact with the model.

### API Usage in Scripts

The package includes a `HuggingFaceAPI` class that you can use from your scripts:

1. Import the `HuggingFace.API` namespace in your script.
2. Create a new `HuggingFaceAPIConversation` instance to manage the conversation state.
3. Use the `HuggingFaceAPI.Query()` method to send a message and receive a response.

For a detailed example of how to use API in a script, refer to the included `HuggingFaceAPIExampleUI` script.

### Support

If you encounter issues or have questions about the package, open an issue on the repository, or ping me (@IndividualKex#9988) on discord.