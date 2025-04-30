using Newtonsoft.Json.Linq;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VSIXProject2.Classes;

namespace VSIXProject2.QuestionTest
{
    public partial class QuestionTestWindowControl : UserControl
    {
        public QuestionTestWindowControl()
        {
            InitializeComponent();
        }
        public void SetQuestionTestResult(QuestionCheckResponse questionRunResponse, Question currentQuestion)
        {
            questionResult.Items.Clear();
            var code_answer = ((JArray)questionRunResponse.code_answer).ToList();
            var expected_code_answer = ((JArray)questionRunResponse.expected_code_answer).ToList();

            for (int i = 0; i < currentQuestion.exampleTestcaseList.Count; i++)
            {
                string testCase = currentQuestion.exampleTestcaseList[i];
                var output = code_answer[i].ToString();
                var expected = expected_code_answer[i].ToString();
                // Create the TabItem
                var isCorrect = output.Equals(expected);
                TabItem newTabItem = new TabItem
                {
                    Header = $"Case {i + 1}",
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                    Foreground = isCorrect ? Brushes.LightGreen : Brushes.Red,
                    Padding = new Thickness(15),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"))
                };

                // Create the StackPanel
                StackPanel stackPanel = new StackPanel
                {
                    Margin = new Thickness(10),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"))
                };

                // Add the Input section
                stackPanel.Children.Add(new TextBlock
                {
                    Text = "Input",
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 10),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
                });
                stackPanel.Children.Add(new TextBlock
                {
                    Text = testCase,
                    FontSize = 14,
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 0, 0, 20),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                    Foreground = Brushes.White,
                });

                // Add the Output section
                stackPanel.Children.Add(new TextBlock
                {
                    Text = "Output",
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 10),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
                });
                stackPanel.Children.Add(new TextBlock
                {
                    Text = output,
                    FontSize = 14,
                    Margin = new Thickness(0, 0, 0, 20),
                    Padding = new Thickness(10),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                    Foreground = Brushes.White,
                });

                // Add the Expected section
                stackPanel.Children.Add(new TextBlock
                {
                    Text = "Expected",
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 10),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
                });
                stackPanel.Children.Add(new TextBlock
                {
                    Text = expected,
                    FontSize = 14,
                    Padding = new Thickness(10),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                    Foreground = Brushes.White,
                });

                newTabItem.Content = stackPanel;

                questionResult.Items.Add(newTabItem);

            }

        }

        public void SetQuestionSubmissionResult(QuestionCheckResponse questionRunResponse, Question currentQuestion)
        {
            questionResult.Items.Clear();

            if (questionRunResponse.status_msg == "Accepted")
            {
                HandleCorrectSubmission(questionRunResponse);
            }
            else
            {
                HandleInCorrectSubmission(questionRunResponse);
            }

        }

        private void HandleInCorrectSubmission(QuestionCheckResponse questionRunResponse)
        {
            var output = questionRunResponse.code_output;
            var input = questionRunResponse.last_testcase;
            var expected = questionRunResponse.expected_output;


            TabItem newTabItem = new TabItem
            {
                Header = $"Result",
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                //Foreground = isCorrect ? Brushes.LightGreen : Brushes.Red,
                Padding = new Thickness(15),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"))
            };

            // Create the StackPanel
            StackPanel stackPanel = new StackPanel
            {
                Margin = new Thickness(10),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"))
            };

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{questionRunResponse.status_msg}  ({questionRunResponse.total_correct}/{questionRunResponse.total_testcases})",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 30),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
            });

            // Add the Input section
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Input",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = input,
                FontSize = 14,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 20),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                Foreground = Brushes.White,
            });

            // Add the Output section
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Output",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = output.ToString(),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 20),
                Padding = new Thickness(10),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                Foreground = Brushes.White,
            });

            // Add the Expected section
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Expected",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = expected,
                FontSize = 14,
                Padding = new Thickness(10),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                Foreground = Brushes.White,
            });

            newTabItem.Content = stackPanel;

            questionResult.Items.Add(newTabItem);
        }
        private void HandleCorrectSubmission(QuestionCheckResponse questionRunResponse)
        {
            var runtime = questionRunResponse.status_runtime;
            var runtime_perc = questionRunResponse.runtime_percentile.ToString().Substring(0, 5);
            var mem = questionRunResponse.memory;
            var mem_perc = questionRunResponse.memory_percentile.ToString().Substring(0, 5);

            TabItem newTabItem = new TabItem
            {
                Header = $"Result",
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                //Foreground = isCorrect ? Brushes.LightGreen : Brushes.Red,
                Padding = new Thickness(15),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"))
            };

            // Create the StackPanel
            StackPanel stackPanel = new StackPanel
            {
                Margin = new Thickness(10),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"))
            };

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{questionRunResponse.status_msg}  ({questionRunResponse.total_correct}/{questionRunResponse.total_testcases})",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 30),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
            });
            // Add the Output section
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Runtime",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{runtime} - Beats {runtime_perc}%",
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 20),
                Padding = new Thickness(10),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                Foreground = Brushes.White,
            });

            // Add the Expected section
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Memory",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#877cb2"))
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{mem / (1024 * 1024)} MB - Beats {mem_perc}%",
                FontSize = 14,
                Padding = new Thickness(10),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333")),
                Foreground = Brushes.White,
            });

            newTabItem.Content = stackPanel;

            questionResult.Items.Add(newTabItem);
        }

    }
}
