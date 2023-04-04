namespace HuggingFace.API {
    public class FillMaskTask : TaskBase {
        public override string taskName => "FillMask";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/bert-base-uncased";
    }
}