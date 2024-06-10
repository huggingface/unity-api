# Hugging Face API for Unity 🤗

This Unity package provides an **easy-to-use integration for the Hugging Face Inference API**, allowing developers to access and **use Hugging Face AI models within their Unity projects**.

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
In the Hugging Face Website:

1. Generate an API key in https://huggingface.co/settings/tokens, we **advise you to create a Fine-Grained Token.

2. When the API key is created click on Set Permissions

<img src="https://huggingface.co/datasets/huggingface-ml-4-games-course/course-images/resolve/main/en/communication/api_step2.png" alt="Hugging Face Token Step 2"/>

3. Authorize Inference with this API key

<img src="https://huggingface.co/datasets/huggingface-ml-4-games-course/course-images/resolve/main/en/communication/api_step3.png" alt="Hugging Face Token Step 3"/>

After installation, the Hugging Face API wizard should open. If not, open it by clicking "Window" > "Hugging Face API Wizard".

4. Test the API key.

5. Optionally, update the endpoints to use different models.

### Try our tutorial

To help you getting started, we wrote a tutorial where you create a robot agent that understands text orders and executes them.


The tutorial 👉 https://thomassimonini.substack.com/p/building-a-smart-robot-ai-using-hugging

The demo 👉 https://huggingface.co/spaces/ThomasSimonini/SmartRobot

<img src="https://substackcdn.com/image/fetch/w_1456,c_limit,f_webp,q_auto:good,fl_progressive:steep/https%3A%2F%2Fsubstack-post-media.s3.amazonaws.com%2Fpublic%2Fimages%2F893d861c-e6ee-416c-bc44-9a588caf4d42_1920x1080.png" alt="Jammo Robot"/>

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
| Conversation                 | ✅     |
| Text Generation              | ✅     |
| Text to Image                | ✅     |
| Text Classification          | ✅     |
| Zero Shot Text Classification| ✅     |
| Question Answering           | ✅     |
| Translation                  | ✅     |
| Summarization                | ✅     |
| Sentence Similarity          | ✅     |
| Speech Recognition           | ✅     |

### Support

If you encounter issues or have questions about the package, open an issue on the repository.
