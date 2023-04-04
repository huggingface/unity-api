namespace HuggingFace.API {
    public class TokenClassificationTask : TaskBase {
        public override string taskName => "TokenClassification";
        public override string defaultEndpoint => "https://api-inference.huggingface.co/models/Davlan/distilbert-base-multilingual-cased-ner-hrl";
    }
}