using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatbot.Function
{
   
        public class UserInteraction
        {
            private readonly Response responder;
            private readonly TaskManager taskManager;

            public event Action<string> OnBotOutput;
            public event Action<string> OnQuizCompleted;

            public UserInteraction(Response responder, TaskManager taskManager)
            {
                this.responder = responder;
                this.taskManager = taskManager;
            }

            public async void ProcessUserMessage(string input, string userName)
            {
                // Show reminders
                var reminders = responder.GetReminders();
                foreach (var r in reminders)
                {
                    OnBotOutput?.Invoke($"Reminder: {r.Title} - {r.Description}");
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    OnBotOutput?.Invoke("I didn't quite catch that. Could you rephrase?");
                    return;
                }

                if (input.ToLower() == "exit")
                {
                    OnBotOutput?.Invoke("Goodbye!");
                    return;
                }

                if (input.ToLower().Contains("quiz") || input.ToLower().Contains("mini game"))
                {
                    await StartQuizAsync();
                    return;
                }

                string response = responder.GetResponse(input);
                OnBotOutput?.Invoke(response);
            }

            public async Task StartQuizAsync()
            {
                var quiz = new QuizManager();
                string output = "Welcome to the Cybersecurity Quiz!\n\n";

                while (true)
                {
                    var question = quiz.GetCurrentQuestion();
                    output += $"Q{quiz.Questions.IndexOf(question) + 1}: {question.Question}\n";

                    for (int i = 0; i < question.Options.Count; i++)
                    {
                        output += $"  {i + 1}) {question.Options[i]}\n";
                    }

                    // Simulate correct answer for now
                    await Task.Delay(500); // Simulate thinking time
                    quiz.SubmitAnswer(question.CorrectOptionIndex);

                    output += "Correct!\n";
                    output += $"{quiz.GetFeedback()}\n\n";

                    if (!quiz.NextQuestion()) break;
                }

                output += $"\nQuiz Complete! Your score: {quiz.GetScore()} / {quiz.TotalQuestions}\n";

                if (quiz.GetScore() == quiz.TotalQuestions)
                    output += "Excellent! You're a cybersecurity pro!\n";
                else if (quiz.GetScore() >= quiz.TotalQuestions / 2)
                    output += "Good effort, keep brushing up on those skills.\n";
                else
                    output += "Keep learning — cybersecurity is important!\n";

                OnQuizCompleted?.Invoke(output);
            }
        }
    }

