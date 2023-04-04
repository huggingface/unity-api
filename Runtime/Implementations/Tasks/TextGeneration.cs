namespace HuggingFace.API {
    public class TextGeneration : TaskBase {
        public override string taskName => "TextGeneration";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/gpt2";
    }
}