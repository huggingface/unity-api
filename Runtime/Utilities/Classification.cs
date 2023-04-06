namespace HuggingFace.API {
    public class Classification {
        public string label;
        public float score;

        public override string ToString() {
            return $"Label: {label}, Score: {score}";
        }
    }
}