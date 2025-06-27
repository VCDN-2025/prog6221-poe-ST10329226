// TaskItem.cs
using System;

namespace AwarenessChatbot
{
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; } // Can be empty if not provided by user
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem(string title, string description, DateTime? reminderDate = null)
        {
            Title = title;
            Description = description;
            ReminderDate = reminderDate;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "[COMPLETED]" : "[PENDING]";
            string reminderInfo = ReminderDate.HasValue ? $" (Remind: {ReminderDate.Value.ToShortDateString()})" : "";
            return $"{status} {Title}: {Description}{reminderInfo}";
        }
    }
}