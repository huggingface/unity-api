using System.Collections.Generic;
using System.Text;

namespace HuggingFace.API
{
    public class ZeroShotTextClassificationResponse
    {
        public string sequence { get; set; }
        public List<string> labels { get; set; }
        public List<float> scores { get; set; }
        public List<string> warnings { get; set; }
        public List<Classification> classifications { get; set; }
    }
}
