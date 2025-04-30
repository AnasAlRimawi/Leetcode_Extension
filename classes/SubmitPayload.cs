using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXProject2.Classes
{
    public class SubmitPayload
    {
        public string lang = "csharp";
        public string question_id { get; set; }
        public string typed_code { get; set; }
        public string data_input { get; set; }

        public SubmitPayload(string question_id, string typed_code, string data_input)
        {
            this.question_id = question_id;
            this.typed_code = typed_code;
            this.data_input = data_input;
        }
    }
}
