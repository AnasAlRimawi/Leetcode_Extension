using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VSIXProject2.Classes;
using System.IO;
using VSIXProject2.QuestionTest;

namespace VSIXProject2.QuestionDetails
{
    public partial class QuestionDetailsWindowControl : UserControl
    {

        private Question currentQuestion;
        private Uri baseAddress = new Uri("https://leetcode.com");
        private string _leetcodesession;
        private string _csrftoken;
        public QuestionDetailsWindowControl()
        {
            InitializeComponent();
        }
        public void SetQuestionDetails(Question question, string leetcodesession, string csrftoken)
        {
            currentQuestion = question;
            _leetcodesession = leetcodesession;
            _csrftoken = csrftoken;
            HtmlViewer.NavigateToString(question.content.ToHtml());
            TitleTextBlock.Text = question.titleSlug.ToCapitaizle();
        }
        private async void Start_Question(object sender, RoutedEventArgs e)
        {
            var code = currentQuestion.codeSnippets[0].code;
            await CreateFileAsync($"{currentQuestion.titleSlug}.cs", code.Replace("Solution", currentQuestion.titleSlug.Replace("-", "")));
        }
        private async void Test_Question(object sender, RoutedEventArgs e)
        {
            var code = await GetCodeForQuestion();

            if (string.IsNullOrEmpty(code)) return;

            if (CheckCookies())
            {
                await VS.MessageBox.ShowErrorAsync("Error", "add your leetcodesession and csrftoken");
                return;
            }
            try
            {
                var id = await RunToLeetCodeAsync(code);

                if (string.IsNullOrEmpty(id)) return;

                var result = await GetQuestionResultAsync(id);

                if (result == null) return;

                if (result.status_msg == "Runtime Error") { await VS.MessageBox.ShowAsync("Runtime Error", result.runtime_error); return; }
                if (result.status_msg == "Compile Error") { await VS.MessageBox.ShowAsync("Compile Error", result.compile_error); return; }
                if (result.status_msg == "Time Limit Exceeded") { await VS.MessageBox.ShowAsync("Time Limit Exceeded", ""); return; }
                var questionResult = await QuestionTestWindow.ShowAsync();
                questionResult.Caption = $"{currentQuestion.titleSlug.ToCapitaizle()} Result";
                var questionResultWindow = questionResult.Content as QuestionTestWindowControl;
                questionResultWindow.SetQuestionTestResult(result, currentQuestion);
            }
            catch (Exception ex)
            {
                await ShowGenericErrorMessageAsync();
            }



        }
        private async void Submit_Question(object sender, RoutedEventArgs e)
        {
            var code = await GetCodeForQuestion();

            if (string.IsNullOrEmpty(code)) return;


            if (CheckCookies())
            {
                await VS.MessageBox.ShowErrorAsync("Error", "add your leetcodesession and csrftoken");
                return;
            }

            var id = await SubmitToLeetCodeAsync(code);

            if (id == default) return;

            var result = await GetQuestionResultAsync(id.ToString());

            if (result.status_msg == "Runtime Error") { await VS.MessageBox.ShowAsync("Runtime Error", result.runtime_error); return; }
            if (result.status_msg == "Compile Error") { await VS.MessageBox.ShowAsync("Compile Error", result.compile_error); return; }
            if (result.status_msg == "Time Limit Exceeded") { await VS.MessageBox.ShowAsync("Time Limit Exceeded", ""); return; }

            var questionResult = await QuestionTestWindow.ShowAsync();
            questionResult.Caption = $"{currentQuestion.titleSlug.ToCapitaizle()} Submission";
            var questionResultWindow = questionResult.Content as QuestionTestWindowControl;
            questionResultWindow.SetQuestionSubmissionResult(result, currentQuestion);
        }
        private async Task<string> GetCodeForQuestion()
        {
            Project project = await VS.Solutions.GetActiveProjectAsync();
            string projectDir = Path.GetDirectoryName(project.FullPath);
            string filePath = Path.Combine(projectDir, $"{currentQuestion.titleSlug}.cs");
            if (!File.Exists(filePath))
            {
                await VS.MessageBox.ShowAsync("Invalid", "Start the question first");
                return "";
            }

            var code = File.ReadAllText(filePath);
            return code.Replace(currentQuestion.titleSlug.Replace("-", ""), "Solution");

        }
        private async Task CreateFileAsync(string fileName, string content)
        {

            Project project = await VS.Solutions.GetActiveProjectAsync();
            string projectDir = Path.GetDirectoryName(project.FullPath);
            string filePath = Path.Combine(projectDir, fileName);
            if (File.Exists(filePath))
            {
                await VS.Documents.OpenAsync(filePath);
                return;
            }

            File.WriteAllText(filePath, content);
            await project.AddExistingFilesAsync(filePath);
            await VS.Documents.OpenAsync(filePath);
        }
        private async Task<QuestionCheckResponse> GetQuestionResultAsync(string id)
        {
            using var client = GetClient();
            var endpoint = $"/submissions/detail/{id}/check/";
            var time = DateTime.UtcNow.AddSeconds(30);
            while (DateTime.UtcNow <= time)
            {
                var response = await client.PostAsync(endpoint, null);
                response.EnsureSuccessStatusCode();
                var jsonresult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<QuestionCheckResponse>(jsonresult);

                if (result != null && result.state == "SUCCESS")
                {
                    return result;
                }
                await Task.Delay(1000);
            }

            await ShowGenericErrorMessageAsync();
            return null;
        }
        private async Task<string> RunToLeetCodeAsync(string code)
        {

            var endpoint = $"/problems/{currentQuestion.titleSlug}/interpret_solution/";
            var payload = new SubmitPayload(question_id: currentQuestion.questionId, code, data_input: currentQuestion.data_input);

            using var client = GetClient();
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<QuestionRunResponse>(json);

            if (data == null) await ShowGenericErrorMessageAsync();
            return data.interpret_id;
        }
        private async Task<long> SubmitToLeetCodeAsync(string code)
        {

            var endpoint = $"/problems/{currentQuestion.titleSlug}/submit/";
            var payload = new SubmitPayload(question_id: currentQuestion.questionId, code, data_input: currentQuestion.data_input);

            using var client = GetClient();
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<QuestionRunResponse>(json);

            if (data == null) await ShowGenericErrorMessageAsync();
            return data.submission_id;
        }
        private HttpClient GetClient()
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };
            handler.CookieContainer.Add(baseAddress, new Cookie("LEETCODE_SESSION", _leetcodesession));
            handler.CookieContainer.Add(baseAddress, new Cookie("csrftoken", _csrftoken));

            var client = new HttpClient(handler)
            {
                BaseAddress = baseAddress,
            };

            // Set headers
            client.DefaultRequestHeaders.Add("x-csrftoken", _csrftoken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Referrer = new Uri($"{baseAddress}/problems/{currentQuestion.titleSlug}/");


            return client;
        }
        private async Task ShowGenericErrorMessageAsync()
        {
            await VS.MessageBox.ShowErrorAsync("Error", "Something Went Wrong, update your leetcodesession and csrftoken");
        }

        private bool CheckCookies()
        {
            var cookies = DataContext as SessionViewModel;
            return string.IsNullOrEmpty(cookies.CsrfToken) || string.IsNullOrEmpty(cookies.LeetcodeSession);
        }
    }
}
