using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXProject2.Classes
{
    public class QuestionCheckResponse
    {
        public int status_code { get; set; }
        public string lang { get; set; }
        public bool run_success { get; set; }
        public string status_runtime { get; set; }
        public string compile_error { get; set; }
        public string runtime_error { get; set; }
        public string last_testcase { get; set; }
        public string expected_output { get; set; }
        public int memory { get; set; }
        public object code_answer { get; set; } //ur code output
        public object expected_code_answer { get; set; } //expected outcome
        public object code_output { get; set; }
        public object std_output_list { get; set; }
        public int elapsed_time { get; set; }
        public long task_finish_time { get; set; }
        public string task_name { get; set; }
        public object total_correct { get; set; }
        public object total_testcases { get; set; }
        public double? runtime_percentile { get; set; }
        public string status_memory { get; set; }
        public double? memory_percentile { get; set; }
        public string pretty_lang { get; set; }
        public string submission_id { get; set; }
        public string status_msg { get; set; }
        public string state { get; set; }

    }
}
