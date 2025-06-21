using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatbot.Function
{
    public class QuizManager
    {
        private int currentIndex = 0;
        private int score = 0;

        public List<QuizQuestions> Questions { get; private set; }

        public QuizManager()
        {
            Questions = new List<QuizQuestions>
            {
                new QuizQuestions {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new() { "Reply", "Delete", "Report as phishing", "Ignore" },
                    CorrectOptionIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams."
                },
                new QuizQuestions {
                    Question = "Is '123456' a secure password?",
                    Options = new() { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "'123456' is one of the most common and insecure passwords."
                },
                new QuizQuestions {
                    Question = "What is phishing?",
                    Options = new() { "Email scam", "Password manager", "VPN method", "Antivirus software" },
                    CorrectOptionIndex = 0,
                    Explanation = "Phishing is a fraud attempt to steal personal information via fake messages."
                },
                new QuizQuestions {
                    Question = "Using the same password everywhere is safe.",
                    Options = new() { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "One breach can expose all your accounts."
                },
                new QuizQuestions {
                    Question = "Which of these is a strong password?",
                    Options = new() { "mypassword", "John1990", "P@ssw0rd123!", "123456" },
                    CorrectOptionIndex = 2,
                    Explanation = "Strong passwords include uppercase, lowercase, symbols, and numbers."
                },
                new QuizQuestions {
                    Question = "What is two-factor authentication?",
                    Options = new() { "Antivirus", "Firewall", "Two-step login", "Password reset" },
                    CorrectOptionIndex = 2,
                    Explanation = "2FA provides an extra layer of protection beyond your password."
                },
                new QuizQuestions {
                    Question = "Clicking pop-ups offering free iPhones is safe.",
                    Options = new() { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "These pop-ups often lead to scams or malware."
                },
                new QuizQuestions {
                    Question = "Should you use public Wi-Fi without a VPN?",
                    Options = new() { "Yes", "No" },
                    CorrectOptionIndex = 1,
                    Explanation = "Public Wi-Fi is risky without encryption."
                },
                new QuizQuestions {
                    Question = "What helps protect your privacy online?",
                    Options = new() { "Using a VPN", "Sharing your location", "Reusing passwords", "Clicking ads" },
                    CorrectOptionIndex = 0,
                    Explanation = "VPNs hide your traffic and mask your IP address."
                },
                new QuizQuestions {
                    Question = "Should you update your software regularly?",
                    Options = new() { "Yes", "No" },
                    CorrectOptionIndex = 0,
                    Explanation = "Updates patch known security flaws."
                }
            };
        }
        // needed for the quiz to be in console
        public QuizQuestions GetCurrentQuestion() => Questions[currentIndex];

        public bool SubmitAnswer(int selectedIndex)
        {
            bool correct = selectedIndex == Questions[currentIndex].CorrectOptionIndex;
            if (correct) score++;
            return correct;
        }

        public string GetFeedback() => Questions[currentIndex].Explanation;

        public bool NextQuestion()
        {
            currentIndex++;
            return currentIndex < Questions.Count;
        }

        public int GetScore() => score;

        public int TotalQuestions => Questions.Count;
    }
}

