using System.Collections.Generic;
using System.Text;

namespace HuggingFace.API {
    public class TextClassificationResponse {
        public List<Classification> classifications;

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var classification in classifications) {
                sb.AppendLine(classification.ToString());
            }
            return sb.ToString();
        }
    }
}