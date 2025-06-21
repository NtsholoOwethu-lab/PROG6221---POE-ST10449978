using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatbot.Function
{
   public class TaskItems
    {
        // title of the task
        public string Title { get; set; }
        // detailed Desciption of task
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
