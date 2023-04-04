namespace HuggingFace.API {
    public class TranslationTask : TaskBase {
        public override string taskName => "Translation";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/t5-base";
    }
}