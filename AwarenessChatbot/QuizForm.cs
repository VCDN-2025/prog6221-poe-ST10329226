// FILE: QuizForm.cs

// 1. USING DIRECTIVES
// These are necessary for the types and functionalities used in this file.
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AwarenessChatbot
{
    // 2. CLASS DECLARATION
    // This defines your quiz form.
    public partial class QuizForm : Form
    {
        // 3. CLASS-LEVEL FIELD DECLARATIONS
        // Instances needed for the quiz functionality.
        private QuizGame _quizGame;          // Manages the quiz logic (questions, scoring)
        private UserProfile _userProfile;    // To access/update user data if needed (e.g., high scores)
        private ActivityLog _activityLog;    // To log quiz events

        // 4. QUIZFORM CONSTRUCTOR
        // This constructor accepts necessary dependencies from Form1.
        public QuizForm(UserProfile userProfile, ActivityLog activityLog)
        {
            InitializeComponent(); // REQUIRED: Initializes UI controls from QuizForm.Designer.cs

            // Assign the passed dependencies to class fields.
            _userProfile = userProfile;
            _activityLog = activityLog;

            // Create a new instance of QuizGame specifically for this quiz session.
            _quizGame = new QuizGame();

            // Set up event handlers for this form's UI controls.
            SetupQuizEventHandlers();

            // Optionally, set initial properties for the RichTextBox output
            rtbQuizOutput.SelectionAlignment = HorizontalAlignment.Center; // Example: center quiz text

            // Start the quiz automatically when the form loads.
            StartQuizGame();
        }

        // 5. EVENT HANDLERS SETUP
        // Centralized place to subscribe to control events within QuizForm.
        private void SetupQuizEventHandlers()
        {
            // Event handler for the quiz answer submission button.
            btnQuizSubmit.Click += btnQuizSubmit_Click;

            // Event handler for Enter key press in the quiz answer input textbox.
            txtQuizInput.KeyDown += txtQuizInput_KeyDown;

            // Event handler for the optional Close Quiz button.
            // Uncomment the line below if you added a button named btnCloseQuiz.
            btnCloseQuiz.Click += btnCloseQuiz_Click;
        }

        // 6. UI EVENT HANDLERS (Specific to QuizForm controls)

        // Handles the click event for the "Submit Answer" button.
        private void btnQuizSubmit_Click(object? sender, EventArgs e)
        {
            string userAnswer = txtQuizInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(userAnswer))
            {
                AppendQuizOutput("Please type your answer before submitting.");
                return;
            }

            // Process the user's answer through the quiz logic.
            HandleQuizInteraction(userAnswer);
        }

        // Handles the 'KeyDown' event for the quiz answer input textbox (for Enter key).
        private void txtQuizInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnQuizSubmit_Click(sender, e); // Trigger the submit button click event
                e.Handled = true;              // Prevent the default 'ding' sound for Enter key
                e.SuppressKeyPress = true;     // Prevent further processing of the key press
            }
        }

        // Handles the click event for the optional "Close Quiz" button.
        // This will close the QuizForm and return control to Form1.
        private void btnCloseQuiz_Click(object? sender, EventArgs e)
        {
            if (_quizGame.IsQuizActive)
            {
                _quizGame.EndQuiz(); // Ensure quiz state is reset if ending prematurely
                _activityLog.RecordAction("Quiz ended prematurely by user via 'Close Quiz' button.");
            }
            this.Close(); // Close the QuizForm
        }


        // 7. QUIZ GAME LOGIC METHODS

        // Initiates a new quiz session.
        private void StartQuizGame()
        {
            _quizGame.StartQuiz(); // Call the QuizGame's method to prepare questions
            AppendQuizOutput("Alright, let's test your cybersecurity knowledge!");
            _activityLog.RecordAction("Quiz started in QuizForm.");
            DisplayNextQuestion(); // Display the first question
        }

        // Displays the current quiz question and its options in the quiz output area.
        private void DisplayNextQuestion()
        {
            Question? question = _quizGame.GetCurrentQuestion();
            if (question != null)
            {
                AppendQuizOutput($"Question {_quizGame.CurrentQuestionNumber}/{_quizGame.GetTotalQuestions()}: {question.QuestionText}");

                if (question.IsTrueFalse)
                {
                    AppendQuizOutput("Please type 'True' or 'False'.");
                }
                else
                {
                    AppendQuizOutput("Please choose an option (e.g., A, B, C, or D), or type the full answer:");
                    foreach (string option in question.Options)
                    {
                        AppendQuizOutput($"- {option}");
                    }
                }
                txtQuizInput.Clear(); // Clear the answer input box for the new question
                txtQuizInput.Focus(); // Set focus for easy input
            }
            else
            {
                // No more questions, quiz is finished (should be handled by EndQuizAndDisplayResults)
                EndQuizAndDisplayResults();
            }
        }

        // Handles the user's answer submission during the quiz.
        private void HandleQuizInteraction(string userAnswer)
        {
            Question? currentQuestion = _quizGame.GetCurrentQuestion();

            // Safety check: ensure there's a current question
            if (currentQuestion == null)
            {
                AppendQuizOutput("Something went wrong with the quiz. Please close and re-open the quiz.");
                _quizGame.EndQuiz(); // Reset quiz state
                _activityLog.RecordAction("Quiz ended prematurely due to error (no current question).");
                return;
            }

            // Submit the answer and log the result
            bool isCorrect = _quizGame.SubmitAnswer(userAnswer);
            _activityLog.RecordAction($"Quiz: Answered \"{userAnswer}\" for \"{currentQuestion.QuestionText}\" - {(isCorrect ? "Correct" : "Incorrect")}");

            // Display feedback to the user
            if (isCorrect)
            {
                AppendQuizOutput("Correct! 🎉", Color.Green);
            }
            else
            {
                AppendQuizOutput("Incorrect. ❌", Color.Red);
            }
            AppendQuizOutput($"Explanation: {currentQuestion.Explanation}");

            // Move to the next question or end the quiz
            if (_quizGame.NextQuestion())
            {
                DisplayNextQuestion();
            }
            else
            {
                EndQuizAndDisplayResults(); // All questions answered
            }
        }

        // Finalizes the quiz, displays results, and prepares for closure.
        private void EndQuizAndDisplayResults()
        {
            int finalScore = _quizGame.GetScore();
            int totalQuestions = _quizGame.GetTotalQuestions();
            AppendQuizOutput($"Quiz finished! You scored {finalScore} out of {totalQuestions}.");

            // Provide a personalized message based on the score
            if (finalScore == totalQuestions)
            {
                AppendQuizOutput("Amazing! You're a true cybersecurity expert! 🏆");
            }
            else if (finalScore >= totalQuestions / 2)
            {
                AppendQuizOutput("Great job! You have a solid understanding of cybersecurity. Keep learning! 👍");
            }
            else
            {
                AppendQuizOutput("Keep learning to stay safe online! Every bit of knowledge helps. 💪");
            }

            AppendQuizOutput("You can close this window to return to the main chatbot.");
            _activityLog.RecordAction($"Quiz finished with score {finalScore}/{totalQuestions}.");
            _quizGame.EndQuiz(); // Ensure the quiz game state is fully reset

            // Disable input controls as the quiz is over
            txtQuizInput.Enabled = false;
            btnQuizSubmit.Enabled = false;
        }

        // 8. HELPER METHOD FOR OUTPUT
        // Helper method to append text to the quiz output RichTextBox.
        // It handles color and ensures scrolling.
        private void AppendQuizOutput(string text, Color? color = null)
        {
            rtbQuizOutput.SelectionStart = rtbQuizOutput.TextLength; // Move caret to the end
            rtbQuizOutput.SelectionLength = 0;                       // Clear any existing selection

            // You can choose to center all text, or set it to Left/Right
            rtbQuizOutput.SelectionAlignment = HorizontalAlignment.Center; // Example: keeps all text centered

            if (color.HasValue)
            {
                rtbQuizOutput.SelectionColor = color.Value;
            }
            rtbQuizOutput.AppendText(text + Environment.NewLine); // Append the text
            rtbQuizOutput.SelectionColor = rtbQuizOutput.ForeColor; // Reset color to default for subsequent text
            rtbQuizOutput.ScrollToCaret(); // Scroll to the end to show the latest message
        }

        // 9. FORM LOAD/TEXT CHANGED (If needed, otherwise can be removed if empty)
        // These are often generated by the designer but may not be needed.
        // Only keep if you add specific logic here.
        private void QuizForm_Load(object sender, EventArgs e)
        {
            // You can add logic here that runs when the form is first loaded (e.g., initial setup, data loading)
        }

        private void rtbQuizOutput_TextChanged(object sender, EventArgs e)
        {
            // Logic for when the text in rtbQuizOutput changes (usually not needed)
        }
    }
}