using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using VSIXProject2.Classes;
using VSIXProject2.QuestionDetails;

namespace VSIXProject2
{
    public partial class MyToolWindowControl : UserControl
    {
        private Dictionary<string, List<Question>> questions { get; set; }
        public MyToolWindowControl()
        {
            InitializeComponent();
            this.DataContext = new SessionViewModel();
            LoadList();
        }


        private void LoadList()
        {
            questions = Helper.GetQuestions();
            LoadCategory();
        }

        private void LoadCategory()
        {
            foreach (var category in questions.Keys)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Margin = new Thickness(10),
                    Background = new SolidColorBrush(Color.FromRgb(50, 50, 50)),
                };

                foreach (var question in questions[category])
                {
                    Border textBlockBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)),
                        CornerRadius = new CornerRadius(5),
                        Margin = new Thickness(5),
                        Padding = new Thickness(5)
                    };

                    StackPanel itemStackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalAlignment = VerticalAlignment.Center
                    };


                    Button iconButton = new Button
                    {
                        Width = 16,
                        Height = 16,
                        Padding = new Thickness(0),
                        Margin = new Thickness(0, 0, 5, 0), 
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent,
                        Content = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/VSIXProject2;component/Resources/leetcode.png", UriKind.Absolute)),
                            Width = 16,
                            Height = 16
                        },
                        BorderThickness = new Thickness(0),
                        Tag = question,
                    };

                    iconButton.Click += ShowQuestion;
                    // Add the text
                    TextBlock textBlock = new TextBlock
                    {
                        Text = question.titleSlug.ToCapitaizle(),
                        Foreground = Helper.GetColor(question.difficulty),
                        FontSize = 14,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    
                    // Add icon and text to the horizontal StackPanel
                    itemStackPanel.Children.Add(iconButton);
                    itemStackPanel.Children.Add(textBlock);

                    textBlockBorder.Child = itemStackPanel; // Set horizontal StackPanel as the Border content
                    stackPanel.Children.Add(textBlockBorder);
                }

                Expander expander = new Expander
                {
                    Header = new TextBlock
                    {
                        Text = category.ToCapitaizle(),
                        FontSize = 16,
                        Foreground = Brushes.White,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5),
                    },
                    Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                    BorderThickness = new Thickness(1),
                    Content = stackPanel
                };

                container.Children.Add(expander);
            }
        }
        private async void ShowQuestion(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;

            Question question = (Question)button.Tag;

            var questionDetailsPane = await QuestionDetailsWindow.ShowAsync();
            questionDetailsPane.Caption = $"{question.titleSlug.ToCapitaizle()} Description";
            var questionDetailsControl = questionDetailsPane.Content as QuestionDetailsWindowControl;
            questionDetailsControl.DataContext = this.DataContext;
            questionDetailsControl.SetQuestionDetails(question, leetcodesession.Text, csrftoken.Text);

        }
    }
}