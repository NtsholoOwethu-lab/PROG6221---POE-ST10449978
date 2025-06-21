using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace chatbot.Function
{
   public class Response
    {
        private readonly CybersecurityTips tipsProvider;
        private readonly User user;
        private readonly TaskManager taskManager;
        private TaskItems pendingTask;

        public Response(User user, CybersecurityTips tipsProvider, TaskManager taskManager)
        {
            this.user = user;
            this.tipsProvider = tipsProvider;
            this.taskManager = taskManager;
        }

        public List<TaskItems> GetReminders()
        {
            return taskManager.GetDueReminders();
        }

        public string GetResponse(string input)
        {
            input = input.ToLower().Trim();

            // Handle adding a task
            if (input.StartsWith("add task"))
            {
                var parts = input.Split('-');
                if (parts.Length < 2)
                    return "Please provide the task title after 'add task -'. Example: Add task - Review notes";

                string title = parts[1].Trim();

                pendingTask = new TaskItems
                {
                    Title = title,
                    Description = $"Task: {title}",
                    IsCompleted = false,
                    ReminderDate = null
                };

                return $"Task added with description '{title}'. Would you like a reminder?";
            }

            // Handle user follow-up for reminder after task
            if ((input.Contains("remind me in") || input.StartsWith("yes")) && pendingTask != null)
            {
                var match = Regex.Match(input, @"remind me in (\d+) days");
                if (match.Success)
                {
                    int days = int.Parse(match.Groups[1].Value);
                    pendingTask.ReminderDate = DateTime.Now.AddDays(days);
                    taskManager.AddExistingTask(pendingTask);
                    var response = $"Got it! I’ll remind you in {days} day(s).";
                    pendingTask = null;
                    return response;
                }
                else
                {
                    return "Sure! Please tell me how many days: e.g. 'remind me in 3 days'.";
                }
            }

            // Show activity log
            if (Matches(input, "show activity log", "activity", "what have you done", "log"))
                return taskManager.GetActivityLog();

            // User emotion checks
            if (input.Contains("worried") || input.Contains("nervous"))
                return "It's completely understandable to feel that way. Cybersecurity can be overwhelming, but you're taking the right steps!";
            if (input.Contains("frustrated"))
                return "I’m here to help. Let’s take it step-by-step together.";
            if (input.Contains("confused"))
                return "No problem—cybersecurity can be complex, but we can break it down together.";
            if (input.Contains("overwhelmed"))
                return "That’s okay. Take a deep breath—we’ll work through it one piece at a time.";

            // Help / purpose / topics
            if (input.Contains("how are you"))
                return "I'm always on alert! Cyber threats never sleep. But thanks for asking — how are you today?";
            if (input.Contains("your purpose"))
                return "My mission is to help users like you become more aware of digital threats.";
            if (input.Contains("what can i ask"))
                return "You can ask me about:\n- Password safety\n- Phishing\n- Safe browsing\n- 2FA\n- Public Wi-Fi risks";

            // Cyber tips
            if (input.Contains("password"))
                return "Password safety is crucial. Use a unique password for every account — at least 12 characters long.";
            if (input.Contains("phishing"))
                return "Phishing is a scam where attackers trick you into revealing personal info.";
            if (input.Contains("browsing"))
                return "To browse safely: use HTTPS, avoid pop-ups, and update your browser.";
            if (input.Contains("two-factor") || input.Contains("2fa") || input.Contains("authentication"))
                return "2FA adds an extra layer of security — even if someone has your password.";
            if (input.Contains("public wifi") || input.Contains("wifi"))
                return "Public Wi-Fi is risky. Use a VPN, disable sharing, and avoid sensitive activity.";

            // Personalized topic interest
            if (input.Contains("interested in"))
            {
                foreach (var topic in tipsProvider.GetTopics())
                {
                    if (input.Contains(topic))
                    {
                        user.FavoriteTopic = topic;
                        return $"Great! I'll remember you're interested in {topic}.";
                    }
                }
            }

            // Dynamic tips
            foreach (var topic in tipsProvider.GetTopics())
            {
                if (input.Contains(topic))
                {
                    string randomTip = tipsProvider.GetTip(topic);
                    if (!string.IsNullOrEmpty(user.FavoriteTopic) && input.Contains("more"))
                        return $"Since you're interested in {user.FavoriteTopic}, here's another tip: {randomTip}";
                    return $"Tip on {topic}: {randomTip}";
                }
            }

            // Reminder setting (outside of add-task context)
            if (input.Contains("remind me in"))
            {
                var match = Regex.Match(input, @"remind me in (\d+) days");
                if (match.Success)
                {
                    int days = int.Parse(match.Groups[1].Value);
                    return taskManager.AddTask("Unnamed Task", "Reminder task", days);
                }
            }

            // Show tasks
            if (input.Contains("show tasks"))
                return taskManager.ShowTasks();

            // Complete task
            if (input.StartsWith("complete task"))
            {
                var parts = input.Split(' ');
                if (int.TryParse(parts.Last(), out int taskNum))
                    return taskManager.CompleteTask(taskNum);
            }

            // Delete task
            if (input.StartsWith("delete task"))
            {
                var parts = input.Split(' ');
                if (int.TryParse(parts.Last(), out int taskNum))
                    return taskManager.DeleteTask(taskNum);
            }

            // Launch quiz
            if (input.Contains("quiz") || input.Contains("mini game"))
                return "Launching the cybersecurity quiz...";

            return "I'm not sure how to respond to that yet. Try asking about password safety, phishing, or 2FA.";
        }

        private bool Matches(string input, params string[] keywords)
        {
            foreach (var word in keywords)
                if (input.Contains(word.ToLower()))
                    return true;
            return false;
        }
    }
}
