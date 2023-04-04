namespace HuggingFace.API {
    public class SummarizationTask : TaskBase {
        public override string taskName => "Summarization";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/facebook/bart-large-cnn";
    }
}