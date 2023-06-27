# Hugging Face API Unity Integration

This Unity package provides an easy-to-use integration for the Hugging Face Inference API, allowing developers to access and use Hugging Face AI models within their Unity projects.

## Table of Contents
1. [Installation](#installation)
2. [Usage](#usage)
   - [Configuration](#configuration)
   - [Example Scene](#example-scene)
   - [API Usage in Scripts](#api-usage-in-scripts)
3. [Tasks](#tasks)
4. [Support](#support)

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
2. Test the API key.
3. Optionally, update the endpoints to use different models.

### Example Scene

To try the included example scene, follow these steps:

1. Click "Install Examples" in the Hugging Face API Wizard to copy the example files into your project.
2. Navigate to the "Hugging Face API" > "Examples" > "Scenes" folder in your project.
3. Open the "ConversationExample" scene.
4. If prompted by the TMP Importer, click "Import TMP Essentials".
5. Press "Play" to run the example. You should be able to use the UI to interact with the model.

### API Usage in Scripts

The package includes a `HuggingFaceAPI` class that you can use from your scripts.

1. Import the `HuggingFace.API` namespace in your script.
2. Call the API method for the task you want.
```
using HuggingFace.API;

HuggingFaceAPI.TextToImage("a cat in a hat", result => {
    // Do something with the result, which in this case is a Texture2D
}, error => {
    // Handle errors
    Debug.LogError(error);
});
```

For a more advanced scripting example, refer to the included example scripts.

### Tasks

| Task                         | Status    |
| ---------------------------- | --------- |
| Conversation                 | [x]       |
| Text Generation              | [x]       |
| Text to Image                | [x]       |
| Text Classification          | [x]       |
| Question Answering           | [x]       |
| Translation                  | [x]       |
| Summarization                | [x]       |
| Sentence Similarity          | [x]       |
| Speech Recognition           | [x]       |

[x] Implemented.

### Support

If you encounter issues or have questions about the package, open an issue on the repository, or ping me (@IndividualKex#9988) on discord.
