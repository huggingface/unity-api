namespace HuggingFace.API {
    public class QuestionAnsweringResponse {
        public float score;
        public int start;
        public int end;
        public string answer;

        public override string ToString() {
            return $"Score: {score}, Start: {start}, End: {end}, Answer: {answer}";
        }
    }
}