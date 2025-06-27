// FILE: Form1.cs

// 1. USING DIRECTIVES
// These are necessary for the types and functionalities used in this file.
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks; // For Task.Delay

namespace AwarenessChatbot
{
    // 2. CLASS DECLARATION
    // This defines your main form.
    public partial class Form1 : Form
    {
        // 3. CLASS-LEVEL FIELD DECLARATIONS
        // These are instances of your other custom classes, initialized here for null-safety.
        private ChatbotManager chatbotManager; // Will be initialized in the constructor after others
        // REMOVED: private QuizGame quizGame = new QuizGame(); // QuizGame is now handled by QuizForm
        private UserProfile currentUserProfile = new UserProfile();
        private AudioPlayer audioPlayer = new AudioPlayer();
        private TaskManager taskManager = new TaskManager();
        private ActivityLog activityLog = new ActivityLog();

        // Constant for the user input placeholder text.
        private const string UserInputPlaceholder = "Please Enter Text";

        // 4. FORM CONSTRUCTOR
        // This runs when the form is created. Initialize components and setup initial UI/data.
        public Form1()
        {
            InitializeComponent(); // REQUIRED: Initializes UI controls from Form1.Designer.cs

            // Initialize the ChatbotManager, passing all necessary dependencies.
            // QuizGame is NOT passed here anymore.
            chatbotManager = new ChatbotManager(currentUserProfile, taskManager, activityLog);

            // Subscribe to the event from ChatbotManager for UI resets (e.g., if 'reset' command is typed)
            chatbotManager.OnResetUiRequested += ResetChatbotInteraction;

            // Set initial UI states for textbox and chat output
            rtbChatOutput.SelectionAlignment = HorizontalAlignment.Center; // Set chat output text to be centered by default
            txtUserInput.Text = UserInputPlaceholder; // Set initial placeholder text
            txtUserInput.ForeColor = Color.Gray; // Make placeholder text gray

            // Set up event handlers for UI interactions.
            SetupEventHandlers();

            // Display the initial greeting message when the form loads.
            DisplayInitialGreeting();
        }

        // 5. UI EVENT HANDLERS SETUP
        // Centralized place to subscribe to control events.
        private void SetupEventHandlers()
        {
            // Event handlers for the main Send and Quiz buttons
            btnSend.Click += btnSend_Click;
            btnQuiz.Click += btnQuiz_Click;
            btnMainMenu.Click += btnMainMenu_Click; // Main Menu button click handler
            this.btnActivityLog.Click += btnActivityLog_Click;

            // Event handler for Enter key press in the user input textbox
            txtUserInput.KeyDown += txtUserInput_KeyDown;

            // Event handlers for the user input textbox to handle placeholder text
            txtUserInput.GotFocus += txtUserInput_GotFocus;
            txtUserInput.LostFocus += txtUserInput_LostFocus;

            // btnExit.Click += btnExit_Click;
        }

        // 6. UI EVENT HANDLERS (Specific to controls)

        // Handles the 'GotFocus' event for the user input textbox (when it's clicked/tabbed into)
        private void txtUserInput_GotFocus(object? sender, EventArgs e)
        {
            if (txtUserInput.Text == UserInputPlaceholder)
            {
                txtUserInput.Text = string.Empty; // Clear text
                txtUserInput.ForeColor = SystemColors.WindowText; // Change text color to default
            }
        }

        // Handles the 'LostFocus' event for the user input textbox (when focus moves away)
        private void txtUserInput_LostFocus(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserInput.Text))
            {
                txtUserInput.Text = UserInputPlaceholder; // Restore placeholder if empty
                txtUserInput.ForeColor = Color.Gray; // Restore gray color
            }
        }

        // Event handler for the Send button click
        // This method processes user input, directs it to chatbot logic.
        private async void btnSend_Click(object? sender, EventArgs e)
        {
            string userInput = txtUserInput.Text.Trim();

            // Ignore empty or placeholder input
            if (string.IsNullOrWhiteSpace(userInput) || userInput == UserInputPlaceholder)
            {
                return;
            }

            // Display user input in blue in the chat output
            AppendChatOutput("You: " + userInput, Color.Blue);
            txtUserInput.Clear(); // Clear the input textbox
            txtUserInput.Focus(); // Keep focus for next input

            // Simulate a typing delay for a more natural feel
            await Task.Delay(500);

            // --- IMPORTANT: QUIZ INTERACTION LOGIC REMOVED FROM HERE ---
            // The quiz is now handled entirely within QuizForm.cs.
            // Form1.cs no longer manages the quiz state or input.

            // --- GENERAL CHATBOT LOGIC ---
            // If not in quiz mode, check for special commands like 'exit' or delegate to ChatbotManager.

            // Handle 'exit' command
            if (userInput.ToLower() == "exit")
            {
                AppendChatOutput("Chatbot: Goodbye! Stay safe online.");
                btnSend.Enabled = false; // Disable send button
                txtUserInput.Enabled = false; // Disable input textbox
                activityLog.RecordAction("User exited the application."); // Log exit
                return; // Exit the method
            }

            // Process general user input through the ChatbotManager
            // The ChatbotManager handles initial name recognition, task commands, activity log, etc.
            string botResponse = chatbotManager.ProcessUserInput(userInput);
            AppendChatOutput("Chatbot: " + botResponse);
        }

        // Event handler for Enter key press in the user input textbox
        private void txtUserInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(sender, e); // Trigger the send button click event
                e.Handled = true; // Prevent the default 'ding' sound for Enter key
                e.SuppressKeyPress = true; // Prevent further processing of the key press
            }
        }

        // Event handler for the Quiz button click
        private void btnQuiz_Click(object? sender, EventArgs e)
        {
            // NEW: Open the QuizForm when the Quiz button is clicked.
            activityLog.RecordAction("User opened QuizForm."); // Log the action

            // Create an instance of your new QuizForm, passing necessary data
            // (UserProfile and ActivityLog are shared and passed to QuizForm)
            QuizForm quizForm = new QuizForm(currentUserProfile, activityLog);

            // Show QuizForm as a modal dialog.
            // This means Form1 will be "paused" and inaccessible until QuizForm is closed.
            quizForm.ShowDialog();

            // After QuizForm is closed, you might want to display a message on Form1
            AppendChatOutput("Chatbot: Welcome back from the quiz!");
            txtUserInput.Focus(); // Ensure input focus returns to Form1
        }

        // Event handler for the Main Menu button click
        private void btnMainMenu_Click(object? sender, EventArgs e)
        {
            // Call the reset method to return to the initial state.
            ResetChatbotInteraction();
            DisplayInitialGreeting();
        }

        // If you added an "Exit" button, uncomment and use this:
        // private void btnExit_Click(object? sender, EventArgs e)
        // {
        //     activityLog.RecordAction("User exited the application."); // Log the action
        //     Application.Exit(); // Close the application
        // }


        // 7. CHATBOT HELPER METHODS (Quiz-related methods are moved to QuizForm.cs)

        // Displays the initial greeting messages when the form starts or resets.
        private void DisplayInitialGreeting()
        {
            audioPlayer.PlayGreeting(); // Play initial greeting sound

            // Display introductory messages in the RichTextBox
            AppendChatOutput("Chatbot: Hello! Welcome to the Cybersecurity Awareness Bot.");
            AppendChatOutput("Chatbot: I'm here to help you learn about staying safe online.");
            AppendChatOutput("Chatbot: You can ask me about topics like 'password', 'scam', 'privacy', 'phishing tips', 'malware', or '2FA'.");
            AppendChatOutput("Chatbot: You can also tell me how you feel (e.g., 'I'm worried about scams').");
            AppendChatOutput("Chatbot: Type 'exit' to quit the conversation, or 'reset' to start over."); // Added reset command info
            AppendChatOutput("Chatbot: What's your name?");

            txtUserInput.Focus(); // Set focus to the input box for the user to type their name
        }

        // Helper method to append text to the chat output RichTextBox.
        // It handles color and ensures text is centered.
        private void AppendChatOutput(string text, Color? color = null)
        {
            rtbChatOutput.SelectionStart = rtbChatOutput.TextLength; // Move caret to the end
            rtbChatOutput.SelectionLength = 0; // Clear any existing selection

            // Set alignment for the new text. It's applied to the current selection/insertion point.
            rtbChatOutput.SelectionAlignment = HorizontalAlignment.Center;

            if (color.HasValue)
            {
                rtbChatOutput.SelectionColor = color.Value;
            }
            rtbChatOutput.AppendText(text + Environment.NewLine); // Append the text
            rtbChatOutput.SelectionColor = rtbChatOutput.ForeColor; // Reset color to default for subsequent text
            // rtbChatOutput.SelectionAlignment = HorizontalAlignment.Left; // OPTIONAL: Reset alignment if only THIS line should be centered
            rtbChatOutput.ScrollToCaret(); // Scroll to the end to show the latest message
        }

        // Resets the chatbot and UI to the initial state, prompting for the user's name again.
        private void ResetChatbotInteraction()
        {
            // No quiz active to end in Form1, as quiz is now in QuizForm.
            // We just ensure user profile is reset and UI is ready for new interaction.

            // 1. Reset User Profile Name and FavoriteTopic
            currentUserProfile.Name = "User";
            currentUserProfile.FavoriteTopic = null;

            // 2. Ensure controls are enabled BEFORE clearing/setting focus
            txtUserInput.Enabled = true;
            btnSend.Enabled = true;
            btnQuiz.Enabled = true;
            btnMainMenu.Enabled = true;
            // If you added an Exit button and it gets disabled:
            // btnExit.Enabled = true;


            // 3. Clear Chat Output and Display Initial Greeting
            rtbChatOutput.Clear();
            DisplayInitialGreeting(); // This method already exists and should prompt for name

            // 4. Clear user input box and set focus (restore placeholder)
            txtUserInput.Text = UserInputPlaceholder; // Set placeholder
            txtUserInput.ForeColor = Color.Gray;     // Set placeholder color
            txtUserInput.Focus();
            activityLog.RecordAction("Chatbot interaction reset.");
        }

        private async void btnActivityLog_Click(object? sender, EventArgs e) // <-- This method needs to be 'async'
        {
            // Simulate typing the "show activity log" command.
            AppendChatOutput("You: show activity log", Color.Blue); // Optional: show "You: show activity log" in chat

            // Simulate a small typing delay for visual effect (optional)
            await Task.Delay(50); // <-- 'await' requires 'async' method

            string botResponse = chatbotManager.ProcessUserInput("show activity log");
            AppendChatOutput("Chatbot: " + botResponse);

            txtUserInput.Focus(); // Ensure focus is back on the input box
        }

        // 8. UNUSED/REDUNDANT METHODS (These can be safely removed from your project if not used or linked in designer)
        // Check your Form1.Designer.cs to ensure these are NOT hooked up to any events.
        // private void DisplayAsciiArt() { /* Can be removed if not used */ }
        // private void Form1_Load(object sender, EventArgs e) { /* Can be removed if empty and not used */ }
        // private void rtbChatOutput_TextChanged(object sender, EventArgs e) { /* Can be removed if empty and not used */ }
        // private void textBox2_TextChanged(object sender, EventArgs e) { /* Can be removed if empty and not used */ }
        // private void rtbChatOutput_TextChanged_1(object sender, EventArgs e) { /* Can be removed if empty and not used */ }
    }
}