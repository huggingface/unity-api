namespace HuggingFace.API {
    public class FeatureExtractionTask : TaskBase {
        public override string taskName => "FeatureExtraction";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/facebook/bart-large";
    }
}