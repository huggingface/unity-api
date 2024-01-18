using System.Collections.Generic;
using System.Text;

namespace HuggingFace.API
{
    public class ZeroShotTextClassificationInput
    {
        public string input;
        public string[] labels;

        // Constructor
        public ZeroShotTextClassificationInput(string input, string[] labels)
        {
            this.input = input;
            this.labels = labels;
        }
    }
}