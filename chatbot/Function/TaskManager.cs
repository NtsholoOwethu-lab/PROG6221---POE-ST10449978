using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatbot.Function
{
   public class TaskManager
    {
        // A private list that holds all current tasks
        private readonly List<TaskItems> tasks = new();

        private List<string> actionHistory = new();

        // Adds a new task to the list with a title, description, and optional reminder in days.
        public string AddTask(string title, string description, int? remindInDays)
        {
            // Create a new TaskItem object using the provided information.
            var task = new TaskItems
            {
                Title = title,
                Description = description,
                IsCompleted = false,
                ReminderDate = remindInDays.HasValue ? DateTime.Now.AddDays(remindInDays.Value) : null // what
            };
            // adds a new task to the list
            tasks.Add(task);
            return $"Task added: {title}. {(remindInDays.HasValue ? $"I’ll remind you in {remindInDays.Value} days." : "")}";
        }
        //NLP - updated for activity log
        public void AddExistingTask(TaskItems task)
        {
            tasks.Add(task);
            string log = $"Task added: {task.Title}" +
                         (task.ReminderDate.HasValue ? $" (Reminder on {task.ReminderDate.Value.ToShortDateString()})" : "");
            LogAction(log);
        }
        /// Activity log

        private List<string> activityLog = new();
        // Log action for activity log
        public void LogAction(string description)
        {
            string entry = $"{DateTime.Now:HH:mm} - {description}"; // know what this does
            activityLog.Add(entry);

            // Optional: Keep only last 10 entries
            if (activityLog.Count > 10)
                activityLog.RemoveAt(0);
        }

        public string GetActionSummary()
        {
            if (actionHistory.Count == 0)
                return "No actions recorded yet.";

            string summary = "Here's a summary of recent actions:\n";
            for (int i = 0; i < actionHistory.Count; i++)
                summary += $"{i + 1}. {actionHistory[i]}\n";
            return summary;
        }

        // getting and displaying the activity log
        public string GetActivityLog()
        {
            if (activityLog.Count == 0)
                return "No recent activity yet.";

            string summary = "Here's a summary of recent actions:\n";
            for (int i = 0; i < activityLog.Count; i++)
                summary += $"{i + 1}. {activityLog[i]}\n";
            return summary;
        }

        // method that shows all of the tasks(if any)
        public string ShowTasks()
        {
            // If the task list is empty, return a message indicating that.
            if (tasks.Count == 0) return "You don't have any tasks yet.";

            // Start building the task list output.
            string output = "Here are your current cybersecurity tasks:\n";
            int i = 1;
            foreach (var task in tasks)
            {
                output += $"{i++}. [{(task.IsCompleted ? "✔" : " ")}] {task.Title} - {task.Description}";
                if (task.ReminderDate.HasValue)
                    output += $" (Reminder: {task.ReminderDate.Value.ToShortDateString()})";
                output += "\n";
            }

            return output;
        }

        // Marks a task as completed by its number in the list. - updated for activity log
        public string CompleteTask(int taskNumber)
        {
            if (taskNumber <= 0 || taskNumber > tasks.Count)
                return "Invalid task number.";

            tasks[taskNumber - 1].IsCompleted = true;
            LogAction($"Task completed: {tasks[taskNumber - 1].Title}");
            return $"Marked task {taskNumber} as completed.";
        }

        // Deletes a task by its number in the list.
        public string DeleteTask(int taskNumber)
        {
            if (taskNumber <= 0 || taskNumber > tasks.Count)
                return "Invalid task number.";

            var title = tasks[taskNumber - 1].Title;
            tasks.RemoveAt(taskNumber - 1);
            LogAction($"Task deleted: {title}");
            return $"Deleted task: {title}";
        }

        public List<TaskItems> GetDueReminders()
        {
            return tasks.Where(t => t.ReminderDate.HasValue && t.ReminderDate.Value.Date == DateTime.Today && !t.IsCompleted).ToList();
        }

        /*public void AddExistingTask(TaskItems task) // added for better task memory
        {
            tasks.Add(task);
        }*/
    }
}
