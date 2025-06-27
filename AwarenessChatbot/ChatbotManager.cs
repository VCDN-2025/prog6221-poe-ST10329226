// FILE: ChatbotManager.cs

// 1. USING DIRECTIVES
// These are necessary for the types and functionalities used in this file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;         // Needed for StringBuilder
using System.Text.RegularExpressions; // Needed for Regex

namespace AwarenessChatbot
{
    // 2. CLASS DECLARATION
    public class ChatbotManager
    {
        // 3. DELEGATES AND EVENTS
        // This event is raised when the UI needs to be reset (e.g., if user types 'reset').
        public delegate void ResetUiRequestedHandler();
        public event ResetUiRequestedHandler? OnResetUiRequested;

        // 4. CLASS-LEVEL FIELD DECLARATIONS
        private CoreChatbot _coreChatbot; // Handles general chat responses (now more for fallback)
        private TaskManager _taskManager; // Handles task management
        private ActivityLog _activityLog; // Handles the activity log
        private UserProfile _userProfile; // Stores user profile data
        private string? _lastRecognizedTopic; // Persists the last recognized cybersecurity topic
        private Random _random; // For varying responses
        private bool _awaitingNameInput;// user enters their name

        // --- ENHANCED REGEX PATTERNS FOR FLEXIBLE PHRASING ---
        // General Chat & Personalization
        private static readonly Regex GreetingRegex = new Regex(@"^(hi|hello|hey|good (morning|afternoon|evening))\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex NameQuestionRegex = new Regex(@"^(what is your name\??|who are you\??|tell me about yourself\??|your name\??)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex MyNameIsRegex = new Regex(@"^(my name is|i am|you can call me|call me)\s+(?<name>.+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ThankYouRegex = new Regex(@"^(thank you|thanks|cheers|much appreciated)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex GoodbyeRegex = new Regex(@"^(bye|goodbye|see you|farewell|later)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ResetCommandRegex = new Regex(@"^(reset|start over|clear conversation|new conversation)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Task Management
        private static readonly Regex AddTaskRegex = new Regex(@"^(add|create|new)\s+(a\s*)?(task|to-do|item|chore)[:\s]*(?<taskDescription>.+?)(?: (due|on|by|at)\s*(?<dueDate>.+))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex CompleteTaskRegex = new Regex(@"(complete|finish|done with|mark as done)\s+(task|to-do|item)?\s*[:\s]*(?<taskTitle>.+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex DeleteTaskRegex = new Regex(@"(delete|remove|erase)\s+(task|to-do|item)?\s*[:\s]*(?<taskTitle>.+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ShowTasksRegex = new Regex(@"(show|list|display|view|what are|tell me my)\s+(my\s*)?(tasks|to-dos|items|agenda|outstanding|things to do)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Activity Log
        // Improved Regex for showing the activity log
        private static readonly Regex ShowLogRegex = new Regex(
            @"^(show|display|what's\s+in\s+the|get|give\s+me)\s+(my\s+)?(activity\s+)?log(s)?(\s+please)?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );
        
        // Cybersecurity Information Retrieval (more topics added)
        private static readonly Regex CybersecurityInfoRegex = new Regex(
            @"^(tell me about|what is|explain|define|can you tell me about)\s+(?<topic>phishing|malware|2fa|two-factor authentication|password|encryption|firewall|virus|ransomware|vpn|cybersecurity|data breach|social engineering|spyware|trojan)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);


        // 5. CONSTRUCTOR
        public ChatbotManager(UserProfile userProfile, TaskManager taskManager, ActivityLog activityLog)
        {
            _userProfile = userProfile;
            _taskManager = taskManager;
            _activityLog = activityLog;
            _coreChatbot = new CoreChatbot(); // Initialize CoreChatbot
            _lastRecognizedTopic = string.Empty; // Initialize last topic
            _random = new Random(); // Initialize Random for varied responses
            _awaitingNameInput = true;
        }

        // 6. PUBLIC METHODS

        /// <summary>
        /// Processes user input and returns a response based on detected intent.
        /// This is the main entry point for processing user commands.
        /// </summary>
        public string ProcessUserInput(string userInput)
        {
            string normalizedInput = userInput.ToLowerInvariant();
            string response = string.Empty;

            // Log the user's input for activity tracking.
            _activityLog.RecordAction($"User Input: \"{userInput}\"");

            // --- High-Priority System Commands ---
            if (ResetCommandRegex.IsMatch(userInput)) // Use the new Regex for reset
            {
                OnResetUiRequested?.Invoke();
                _activityLog.RecordAction("Chatbot initiated UI reset.");
             

                _awaitingNameInput = true; // Set flag to expect name after reset
                _userProfile.Name = "there"; // Reset user name to default or null

                return "Okay, resetting our conversation. What's your name?";
            }

            Match myNameIsMatch = MyNameIsRegex.Match(userInput);
            if (myNameIsMatch.Success)
            {
                string newName = myNameIsMatch.Groups["name"].Value.Trim();
                if (_userProfile.Name.Equals(newName, StringComparison.OrdinalIgnoreCase))
                {
                    response = $"I already know you as {newName}, {_userProfile.Name}! How can I help today?";
                }
                else
                {
                    // Capitalize the first letter and make the rest lowercase
                    if (!string.IsNullOrEmpty(newName))
                    {
                        _userProfile.Name = char.ToUpper(newName[0]) + newName.Substring(1).ToLower(); // <-- MODIFIED HERE
                    }
                    else
                    {
                        _userProfile.Name = newName; // Handle empty string case
                    }
                    response = $"Nice to meet you, {_userProfile.Name}! How can I help you stay cyber-safe today?";
                }
                _awaitingNameInput = false;
                _activityLog.RecordAction($"User set name to '{_userProfile.Name}'.");
                return response;
            }
            // This prioritizes general commands. If no command is matched AND we're awaiting a name,
            // then assume the input is the name.
            if (_awaitingNameInput &&
                !GreetingRegex.IsMatch(userInput) && // Check if it's not a common greeting
                !NameQuestionRegex.IsMatch(userInput) && // Avoid taking "what's your name?" as a name
                !ThankYouRegex.IsMatch(userInput) && // Avoid taking "thanks" as a name
                !GoodbyeRegex.IsMatch(userInput) && // Avoid taking "bye" as a name
                !AddTaskRegex.IsMatch(userInput) && // Avoid taking task commands as names
                !CompleteTaskRegex.IsMatch(userInput) &&
                !DeleteTaskRegex.IsMatch(userInput) &&
                !ShowTasksRegex.IsMatch(userInput) &&
                !ShowLogRegex.IsMatch(userInput) &&
                !CybersecurityInfoRegex.IsMatch(userInput) // Avoid taking info requests as names
                )
            { 
                string providedName = userInput.Trim();
            if (!string.IsNullOrEmpty(providedName))
            {
                _userProfile.Name = char.ToUpper(providedName[0]) + providedName.Substring(1).ToLower(); // <-- MODIFIED HERE
            }
            else
            {
                _userProfile.Name = providedName; // Handle empty string case
            }
            _awaitingNameInput = false;
            _activityLog.RecordAction($"User provided name: '{_userProfile.Name}'.");
            return $"Nice to meet you, {_userProfile.Name}! How can I help you stay cyber-safe today?";
        }

            // --- General Chat & Personalization ---
            if (GreetingRegex.IsMatch(userInput))
            {
                response = GetRandomGreeting();
            }
            else if (NameQuestionRegex.IsMatch(userInput))
            {
                response = $"I am AwarenessBot, your personal cybersecurity assistant. And you are, {_userProfile.Name}?";
            }
            else if (MyNameIsRegex.IsMatch(userInput))
            {
                Match match = MyNameIsRegex.Match(userInput);
                string newName = match.Groups["name"].Value.Trim();
                _userProfile.Name = newName; // Update user profile
                response = $"Nice to meet you, {_userProfile.Name}! How can I help you stay cyber-safe today?";
            }
            else if (ThankYouRegex.IsMatch(userInput))
            {
                response = GetRandomThankYouResponse();
            }
            else if (GoodbyeRegex.IsMatch(userInput))
            {
                response = GetRandomGoodbyeResponse();
            }
            // --- Task Management Commands ---
            else if (HandleTaskCommand(userInput, out response)) // HandleTaskCommand now returns bool and 'out' response
            {
                // Response is set by HandleTaskCommand
            }
            // --- Activity Log Command ---
            else if (HandleActivityLogCommand(userInput, out response)) // HandleActivityLogCommand now returns bool and 'out' response
            {
                // Response is set by HandleActivityLogCommand
            }
            // --- Cybersecurity Information Retrieval ---
            else if (CybersecurityInfoRegex.IsMatch(userInput))
            {
                Match match = CybersecurityInfoRegex.Match(userInput);
                string topic = match.Groups["topic"].Value.Trim().ToLowerInvariant();
                _lastRecognizedTopic = topic; // Update last recognized topic
                response = GetCybersecurityInfo(topic);
            }
            // --- Fallback to CoreChatbot / Unrecognized Command ---
            else
            {
                // Delegate to CoreChatbot, potentially passing context like _lastRecognizedTopic
                response = _coreChatbot.GetResponse(userInput, _userProfile, ref _lastRecognizedTopic);

                // If CoreChatbot has a generic "I don't understand" or empty response,
                // you might override with a more helpful random unrecognized message.
                if (string.IsNullOrEmpty(response) || response.Contains("I don't understand", StringComparison.OrdinalIgnoreCase)) // Adjust based on CoreChatbot's actual fallback
                {
                    response = GetRandomUnrecognizedResponse();
                }
            }

            _activityLog.RecordAction($"Chatbot responded: \"{response}\"");
            return response;
        }

        // 7. PRIVATE HELPER METHODS

        /// <summary>
        /// Handles user input related to task management (add, complete, show, delete tasks).
        /// </summary>
        /// <param name="userInput">The normalized user input.</param>
        /// <param name="response">The chatbot's response for the task command.</param>
        /// <returns>True if a task command was recognized and handled, false otherwise.</returns>
        private bool HandleTaskCommand(string userInput, out string response)
        {
            response = string.Empty;
            Match match;

            // Add Task
            match = AddTaskRegex.Match(userInput);
            if (match.Success)
            {
                string taskTitle = match.Groups["taskDescription"].Value.Trim();
                DateTime? reminderDate = null;
                string dateString = match.Groups["dueDate"].Success ? match.Groups["dueDate"].Value : string.Empty;

                if (!string.IsNullOrEmpty(dateString))
                {
                    if (dateString.Contains("tomorrow", StringComparison.OrdinalIgnoreCase))
                    {
                        reminderDate = DateTime.Today.AddDays(1);
                    }
                    else if (Regex.IsMatch(dateString, @"in\s+(\d+)\s+days", RegexOptions.IgnoreCase))
                    {
                        Match daysMatch = Regex.Match(dateString, @"in\s+(\d+)\s+days", RegexOptions.IgnoreCase);
                        if (int.TryParse(daysMatch.Groups[1].Value, out int days))
                        {
                            reminderDate = DateTime.Today.AddDays(days);
                        }
                    }
                    else if (DateTime.TryParse(dateString, out DateTime parsedDate))
                    {
                        reminderDate = parsedDate;
                    }
                    else
                    {
                        response = $"I can add '{taskTitle}', but I couldn't understand the date '{dateString}'. Please use a clear date like 'tomorrow', 'in 7 days', or 'YYYY-MM-DD'.";
                        return true; // Command recognized, but date parsing failed.
                    }
                }

                _taskManager.AddTask(taskTitle, "", reminderDate); // Description is empty for now
                response = $"Okay, I've added '{taskTitle}' to your tasks{(reminderDate.HasValue ? $" with a reminder for {reminderDate.Value.ToShortDateString()}" : "")}.";
                _activityLog.RecordAction($"Task added: '{taskTitle}'{(reminderDate.HasValue ? $" due {reminderDate.Value.ToShortDateString()}" : "")}.");
                return true;
            }

            // Show Tasks
            match = ShowTasksRegex.Match(userInput);
            if (match.Success)
            {
                List<TaskItem> allTasks = _taskManager.GetAllTasks();
                if (allTasks.Any())
                {
                    StringBuilder sb = new StringBuilder("Here are your current tasks:\n");
                    foreach (TaskItem task in allTasks)
                    {
                        sb.AppendLine($"- {task.ToString()}");
                    }
                    response = sb.ToString();
                }
                else
                {
                    response = "You don't have any tasks right now. Would you like to add one?";
                }
                _activityLog.RecordAction("Chatbot displayed task list.");
                return true;
            }

            // Complete Task
            match = CompleteTaskRegex.Match(userInput);
            if (match.Success)
            {
                string taskTitle = match.Groups["taskTitle"].Value.Trim();
                if (_taskManager.MarkTaskCompleted(taskTitle))
                {
                    response = $"Great! Task '{taskTitle}' marked as completed. Well done, {_userProfile.Name}!";
                    _activityLog.RecordAction($"Task marked as completed: '{taskTitle}'.");
                }
                else
                {
                    response = $"I couldn't find a task named '{taskTitle}' to mark as complete. Please check the spelling.";
                }
                return true;
            }

            // Delete Task
            match = DeleteTaskRegex.Match(userInput);
            if (match.Success)
            {
                string taskTitle = match.Groups["taskTitle"].Value.Trim();
                if (_taskManager.DeleteTask(taskTitle))
                {
                    response = $"Okay, I've deleted task '{taskTitle}'.";
                    _activityLog.RecordAction($"Task deleted: '{taskTitle}'.");
                }
                else
                {
                    response = $"I couldn't find a task named '{taskTitle}' to delete. Please check the spelling.";
                }
                return true;
            }

            return false; // No task command recognized
        }

        /// <summary>
        /// Handles user input related to viewing the activity log.
        /// </summary>
        /// <param name="userInput">The normalized user input.</param>
        /// <param name="response">The chatbot's response for the activity log command.</param>
        /// <returns>True if an activity log command was recognized and handled, false otherwise.</returns>
        private bool HandleActivityLogCommand(string userInput, out string response)
        {
            response = string.Empty;
            Match match = ShowLogRegex.Match(userInput);
            if (match.Success)
            {
                List<string> recentActions = _activityLog.GetRecentActions(10); // Get last 10 actions
                if (recentActions.Any())
                {
                    StringBuilder sb = new StringBuilder("Here’s a summary of recent actions:\n");
                    for (int i = 0; i < recentActions.Count; i++)
                    {
                        sb.AppendLine($"{i + 1}. {recentActions[i]}");
                    }
                    response = sb.ToString();
                }
                else
                {
                    response = "No recent activities to show.";
                }
                _activityLog.RecordAction("Chatbot displayed activity log.");
                return true;
            }
            return false; // No activity log command recognized
        }

        /// <summary>
        /// Retrieves detailed information about a cybersecurity topic.
        /// </summary>
        /// <param name="topic">The specific cybersecurity topic.</param>
        /// <returns>A string containing information about the topic.</returns>
        private string GetCybersecurityInfo(string topic)
        {
            switch (topic)
            {
                case "phishing":
                    return "Phishing is a cybercrime where attackers trick individuals into revealing sensitive information, often through fake emails or websites. Always check the sender and URL carefully!";
                case "malware":
                    return "Malware is malicious software, like viruses or ransomware, designed to damage or gain unauthorized access to computer systems. Keep your antivirus updated!";
                case "2fa":
                case "two-factor authentication":
                    return "Two-Factor Authentication (2FA) adds an extra layer of security by requiring two different methods of verification to log in, like a password and a code from your phone.";
                case "password":
                    return "A strong password is long (12+ characters), unique, and combines uppercase, lowercase, numbers, and symbols. Consider using a password manager!";
                case "encryption":
                    return "Encryption is the process of converting information or data into a code to prevent unauthorized access. It's crucial for protecting sensitive data.";
                case "firewall":
                    return "A firewall is a network security system that monitors and controls incoming and outgoing network traffic based on predetermined security rules. It acts as a barrier between your internal network and external sources.";
                case "virus":
                    return "A computer virus is a type of malicious software that, when executed, replicates itself by modifying other computer programs and inserting its own code.";
                case "ransomware":
                    return "Ransomware is a type of malicious software that threatens to publish the victim's data or perpetually block access to it unless a ransom is paid.";
                case "vpn":
                    return "A Virtual Private Network (VPN) creates a secure, encrypted connection over a less secure network, such as the internet. It helps protect your online privacy and security.";
                case "cybersecurity":
                    return "Cybersecurity is the practice of protecting systems, networks, and programs from digital attacks. It's about ensuring confidentiality, integrity, and availability of information.";
                case "data breach":
                    return "A data breach is a security incident where sensitive, protected, or confidential data is copied, transmitted, viewed, stolen, or used by an individual unauthorized to do so.";
                case "social engineering":
                    return "Social engineering is the psychological manipulation of people into performing actions or divulging confidential information. Attackers often use deception to trick victims.";
                case "spyware":
                    return "Spyware is malicious software designed to enter your computer device, gather data about you, and forward it to a third party without your knowledge or consent.";
                case "trojan":
                    return "A Trojan horse, or Trojan, is a type of malicious code or software that looks legitimate but can take control of your computer. It is designed to damage, disrupt, steal, or inflict other harmful actions.";
                default:
                    return $"I can provide information on phishing, malware, 2FA, passwords, and more. What specific topic would you like to know about?";
            }
        }

        // --- Helper Methods for Varied Responses ---
        private string GetRandomGreeting()
        {
            string[] greetings = {
                $"Hello, {_userProfile.Name}! How can I assist you with cybersecurity today?",
                $"Hi there, {_userProfile.Name}! Ready to learn more about staying safe online?",
                $"Greetings, {_userProfile.Name}! What's on your mind regarding cybersecurity?",
                $"Hey, {_userProfile.Name}! How can I help secure your digital life?"
            };
            return greetings[_random.Next(greetings.Length)];
        }

        private string GetRandomThankYouResponse()
        {
            string[] responses = {
                "You're welcome!",
                "My pleasure, always happy to help!",
                "No problem at all!",
                "Glad I could assist!"
            };
            return responses[_random.Next(responses.Length)];
        }

        private string GetRandomGoodbyeResponse()
        {
            string[] responses = {
                "Goodbye! Stay safe online!",
                "Farewell! Remember to practice good cybersecurity habits!",
                "See you later, {_userProfile.Name}! Have a secure day!",
                "Until next time! Keep your digital guard up!"
            };
            return responses[_random.Next(responses.Length)];
        }

        private string GetRandomUnrecognizedResponse()
        {
            string[] responses = {
                "I'm sorry, I didn't quite understand that. Can you rephrase or try a command like 'add task', 'show tasks', 'start quiz', or 'show activity log'?",
                "My apologies, I'm still learning! Perhaps you could try a command like 'tell me about phishing' or 'set reminder'?",
                "I'm not sure what you mean. Please use simple commands like 'reset', 'show tasks', or ask a question about a cybersecurity topic.",
                "Could you please clarify? I can help with tasks, reminders, quizzes, and cybersecurity information."
            };
            return responses[_random.Next(responses.Length)];
        }
    }
}