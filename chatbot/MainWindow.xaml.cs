using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using chatbot.Function;
using Microsoft.VisualBasic;


namespace chatbot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly User user;
        private readonly UserInteraction interaction;
        private QuizManager quiz;
        private bool isQuizActive = false;
        private QuizQuestions currentQuestion;

        public MainWindow()
        {
            InitializeComponent();

            // ✅ Prompt for user's name using InputBox
            string userName = Interaction.InputBox("Please enter your name:", "Welcome", "User");
            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("Name is required to start the chatbot.");
                Close(); // Exit if user cancels
                return;
            }

            var tips = new CybersecurityTips();
            var taskManager = new TaskManager();
            user = new User { Name = userName }; // ✅ Store actual name
            var response = new Response(user, tips, taskManager);
            interaction = new UserInteraction(response, taskManager);

            interaction.OnBotOutput += ShowMessage;
            interaction.OnQuizCompleted += ShowMessage;

            ShowMessage($"Hello {user.Name}! I'm your Cybersecurity Awareness Bot.\nAsk me about:\n1.Type 'password safety, phishing,authentication, wifi safety' to get tips on the topic\n2. Type 'quiz' to play the mini-game.\n3. Type 'Add task' make sure to add a description for the task\n4. Type 'activity log once you're done with everything to show summary. '");
        }

        private void ShowMessage(string message)
        {
            OutputBox.Text += message + "\n\n";
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string input = InputBox.Text.Trim();
            InputBox.Clear();

            if (string.IsNullOrEmpty(input)) return;

            OutputBox.Text += $"{user.Name}: {input}\n";

            if (isQuizActive)
            {
                // User is answering a quiz question
                if (int.TryParse(input, out int selectedOption))
                {
                    selectedOption -= 1; // Adjust for 0-based index
                    bool correct = quiz.SubmitAnswer(selectedOption);

                    ShowMessage(correct ? "Correct!" : "Incorrect.");
                    ShowMessage($"Explanation: {quiz.GetFeedback()}");

                    if (quiz.NextQuestion())
                    {
                        AskNextQuizQuestion();
                    }
                    else
                    {
                        ShowMessage($"Quiz complete! Your score: {quiz.GetScore()} / {quiz.TotalQuestions}");
                        isQuizActive = false;
                    }
                }
                else
                {
                    ShowMessage("Please enter the number of your answer.");
                }

                return;
            }

            if (input.ToLower().Contains("quiz"))
            {
                quiz = new QuizManager();
                isQuizActive = true;
                AskNextQuizQuestion();
                return;
            }

            // Otherwise process normally
            interaction.ProcessUserMessage(input, user.Name);
        }
        private void AskNextQuizQuestion()
        {
            currentQuestion = quiz.GetCurrentQuestion();
            var output = $"Q{quiz.Questions.IndexOf(currentQuestion) + 1}: {currentQuestion.Question}\n";

            for (int i = 0; i < currentQuestion.Options.Count; i++)
            {
                output += $"  {i + 1}) {currentQuestion.Options[i]}\n";
            }

            ShowMessage(output);
        }
    }
}
