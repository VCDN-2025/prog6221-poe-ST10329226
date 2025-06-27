// FILE: Question.cs
using System;
using System.Collections.Generic;
using System.Linq; // Added for completeness, though not strictly needed in this specific file yet

namespace AwarenessChatbot
{
    public class Question
    {
        public string QuestionText { get; }
        public List<string> Options { get; } // For multiple-choice questions (e.g., "A) Option 1")
        public string CorrectAnswerString { get; } // Stores the exact correct option string (e.g., "B) An attempt...")
        public bool CorrectAnswerBool { get; } // Stores true/false for True/False questions
        public bool IsTrueFalse { get; } // Flag to know if it's a T/F question
        public string Explanation { get; }

        // Constructor for Multiple-Choice Questions
        public Question(string questionText, List<string> options, string correctAnswer, string explanation)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswerString = correctAnswer; // Store the full correct answer string (e.g., "B) An attempt...")
            Explanation = explanation;
            IsTrueFalse = false; // It's a multiple-choice question
        }

        // Constructor for True/False Questions
        public Question(string questionText, bool correctBoolAnswer, string explanation)
        {
            QuestionText = questionText;
            Options = new List<string> { "True", "False" }; // Standard options for T/F
            CorrectAnswerBool = correctBoolAnswer;
            Explanation = explanation;
            IsTrueFalse = true; // It's a true/false question
            CorrectAnswerString = correctBoolAnswer ? "True" : "False"; // Store string for consistency if needed by UI
        }

        /// <summary>
        /// Checks if the user's answer is correct, handling various input formats and case insensitivity.
        /// </summary>
        /// <param name="userAnswer">The user's input string.</param>
        /// <returns>True if the answer is correct, false otherwise.</returns>
        public bool CheckAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
            {
                return false; // No answer provided
            }

            // Normalize user input: trim whitespace and convert to lowercase for flexible comparison
            string normalizedUserAnswer = userAnswer.Trim().ToLowerInvariant();

            if (IsTrueFalse)
            {
                // For True/False questions: accept "true", "false", "t", "f"
                if (CorrectAnswerBool) // If the correct answer for this question is True
                {
                    return normalizedUserAnswer == "true" || normalizedUserAnswer == "t";
                }
                else // If the correct answer for this question is False
                {
                    return normalizedUserAnswer == "false" || normalizedUserAnswer == "f";
                }
            }
            else // For Multiple-Choice Questions
            {
                // Normalize the stored correct answer string (e.g., "B) An attempt..." becomes "b) an attempt...")
                string normalizedCorrectAnswerStored = CorrectAnswerString.Trim().ToLowerInvariant();

                // 1. Check if user answer matches the exact full correct option string
                // Example: If CorrectAnswerString is "B) An attempt...", user types "B) An attempt..."
                if (normalizedUserAnswer == normalizedCorrectAnswerStored)
                {
                    return true;
                }

                // 2. Check if user answer matches just the letter option (e.g., "a", "b", "c", "d")
                // This assumes your options are consistently formatted like "A) Text", "B) Text", etc.
                if (CorrectAnswerString.Length > 0 && char.IsLetter(CorrectAnswerString[0]))
                {
                    char correctLetter = char.ToLowerInvariant(CorrectAnswerString[0]);
                    if (normalizedUserAnswer.Length == 1 && normalizedUserAnswer[0] == correctLetter)
                    {
                        return true;
                    }
                }

                // 3. Check if user answer matches just the text part of the correct answer
                // Example: If CorrectAnswerString is "B) An attempt...", user types "an attempt..."
                int firstParen = CorrectAnswerString.IndexOf(')');
                if (firstParen != -1 && firstParen + 1 < CorrectAnswerString.Length)
                {
                    string correctTextPart = CorrectAnswerString.Substring(firstParen + 1).Trim().ToLowerInvariant();
                    if (normalizedUserAnswer == correctTextPart)
                    {
                        return true;
                    }
                }
            }

            return false; // If no match found after all checks
        }
    }
}