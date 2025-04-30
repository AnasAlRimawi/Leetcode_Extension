using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXProject2.Classes
{
    public class Question
    {
        public string questionId { get; set; }
        public string titleSlug { get; set; }
        public string content { get; set; }
        public string difficulty { get; set; }
        public List<string> hints { get; set; }
        public bool isPaidOnly { get; set; }
        public List<string> exampleTestcaseList { get; set; }
        public List<CodeSnippet> codeSnippets { get; set; }
        public string data_input => string.Join("\n", exampleTestcaseList);

    }

    public class CodeSnippet
    {
        public string lang { get; set; }
        public string langSlug { get; set; }
        public string code { get; set; }
    }
}
