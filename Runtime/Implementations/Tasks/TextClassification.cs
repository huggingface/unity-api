namespace HuggingFace.API {
    public class TextClassification : TaskBase {
        public override string taskName => "TextClassification";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/distilbert-base-uncased-finetuned-sst-2-english";
    }
}