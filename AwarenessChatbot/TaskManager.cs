// TaskManager.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwarenessChatbot
{
    public class TaskManager
    {
        private List<TaskItem> tasks;

        public TaskManager()
        {
            tasks = new List<TaskItem>();
        }

        public void AddTask(string title, string description, DateTime? reminderDate = null)
        {
            tasks.Add(new TaskItem(title, description, reminderDate));
        }

        public List<TaskItem> GetAllTasks()
        {
            return new List<TaskItem>(tasks); // Return a copy
        }

        public bool MarkTaskCompleted(string title)
        {
            TaskItem? task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                task.IsCompleted = true;
                return true;
            }
            return false;
        }

        public bool DeleteTask(string title)
        {
            int initialCount = tasks.Count;
            tasks.RemoveAll(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            return tasks.Count < initialCount;
        }
    }
}