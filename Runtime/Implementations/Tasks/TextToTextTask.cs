namespace HuggingFace.API {
    public class TextToTextTask : TaskBase {
        public override string taskName => "TextToText";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/facebook/mbart-large-50";
    }
}