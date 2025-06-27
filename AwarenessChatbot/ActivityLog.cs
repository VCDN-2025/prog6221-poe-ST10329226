// ActivityLog.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwarenessChatbot
{
    public class ActivityLog
    {
        private List<string> logEntries;
        private const int MaxLogEntries = 100; // Limit total log size for performance

        public ActivityLog()
        {
            logEntries = new List<string>();
        }

        /// <summary>
        /// Records an action with a timestamp.
        /// </summary>
        /// <param name="action">Description of the action.</param>
        public void RecordAction(string action)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            logEntries.Add($"[{timestamp}] {action}");

            // Keep log length manageable
            if (logEntries.Count > MaxLogEntries)
            {
                logEntries.RemoveAt(0); // Remove the oldest entry
            }
        }

        /// <summary>
        /// Gets a specified number of the most recent actions.
        /// </summary>
        /// <param name="count">The number of recent actions to retrieve.</param>
        /// <returns>A list of recent action strings.</returns>
        public List<string> GetRecentActions(int count)
        {
            // Ensure we don't try to get more entries than exist
            int startIndex = Math.Max(0, logEntries.Count - count);
            return logEntries.Skip(startIndex).Take(count).ToList();
        }

        /// <summary>
        /// Gets all recorded actions.
        /// </summary>
        /// <returns>A list of all action strings.</returns>
        public List<string> GetAllActions()
        {
            return new List<string>(logEntries); // Return a copy
        }
    }
}