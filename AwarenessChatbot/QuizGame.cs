// FILE: QuizGame.cs
using System;
using System.Collections.Generic;
using System.Linq; // Needed for .Count() and .ToList()

namespace AwarenessChatbot
{
    public class QuizGame
    {
        private List<Question> questions = null!;
        private int currentQuestionIndex;
        private int score;
        public bool IsQuizActive { get; private set; }

        public QuizGame()
        {
            InitializeQuestions();
            ResetQuiz();
        }

        /// <summary>
        /// Populates the list of cybersecurity quiz questions.
        /// You need at least 10 questions as per the rubric.
        /// </summary>
        private void InitializeQuestions()
        {
            questions = new List<Question>
            {
                // Multiple-Choice Question 1
                new Question(
                    "What is phishing?",
                    new List<string> { "A) A type of online game", "B) An attempt to trick you into revealing personal information", "C) A fishing technique", "D) A secure way to browse the internet" },
                    "B) An attempt to trick you into revealing personal information", // Correct answer string
                    "Phishing is a deceptive attempt to acquire sensitive information such as usernames, passwords, and credit card details, often for malicious reasons, by masquerading as a trustworthy entity in an electronic communication."
                ),
                // Multiple-Choice Question 2
                new Question(
                    "Which of the following is an example of a strong password?",
                    new List<string> { "A) Password123", "B) 12345678", "C) MyDogName", "D) L@rgeP@$$w0rd!" },
                    "D) L@rgeP@$$w0rd!", // Correct answer string
                    "A strong password combines uppercase and lowercase letters, numbers, and symbols, and is generally long (12+ characters)."
                ),
                // True/False Question 3
                new Question(
                    "True or False: It is safe to click on links from unknown senders if they look interesting.",
                    false, // Correct answer is False (boolean)
                    "False. Clicking on links from unknown senders can lead to malware infections or phishing sites. Always verify the sender before clicking."
                ),
                // Multiple-Choice Question 4
                new Question(
                    "What does '2FA' stand for in cybersecurity?",
                    new List<string> { "A) Two-Factor Authentication", "B) Two-Function Access", "C) Double File Access", "D) Second Firewall Activation" },
                    "A) Two-Factor Authentication", // Correct answer string
                    "2FA adds an extra layer of security by requiring two different methods of authentication before granting access."
                ),
                // Multiple-Choice Question 5
                new Question(
                    "What is malware?",
                    new List<string> { "A) Software used for email management", "B) Malicious software designed to damage or disable computer systems", "C) A tool for website development", "D) A type of computer hardware" },
                    "B) Malicious software designed to damage or disable computer systems", // Correct answer string
                    "Malware is a general term for malicious software like viruses, worms, and ransomware, designed to harm or exploit computer systems."
                ),
                // True/False Question 6
                new Question(
                    "True or False: Using public Wi-Fi without a VPN is always safe for sensitive activities like online banking.",
                    false, // Correct answer is False (boolean)
                    "False. Public Wi-Fi networks are often unsecured, making your data vulnerable to interception. A VPN encrypts your traffic and provides protection."
                ),
                // Multiple-Choice Question 7
                new Question(
                    "What is a common sign of a phishing email?",
                    new List<string> { "A) Perfect grammar and spelling", "B) Requests for urgent action or personal information", "C) Comes from a well-known company's official domain", "D) Contains detailed contact information for the sender" },
                    "B) Requests for urgent action or personal information", // Correct answer string
                    "Phishing emails often create a sense of urgency, contain grammatical errors, or ask for sensitive data like passwords."
                ),
                // Multiple-Choice Question 8
                new Question(
                    "What is the best practice for backing up important data?",
                    new List<string> { "A) Only store it on your computer's hard drive", "B) Use multiple methods, including cloud storage and external drives", "C) Email it to yourself as the only backup", "D) Don't back up; data loss is rare" },
                    "B) Use multiple methods, including cloud storage and external drives", // Correct answer string
                    "The 3-2-1 backup rule suggests having three copies of your data, on two different media, with one copy offsite."
                ),
                // True/False Question 9
                new Question(
                    "True or False: Antivirus software can protect you from all types of cyber threats.",
                    false, // Correct answer is False (boolean)
                    "False. While antivirus software is crucial, it's not foolproof. A multi-layered approach including strong passwords, firewalls, and cautious online behavior is necessary."
                ),
                // Multiple-Choice Question 10
                new Question(
                    "What is social engineering in cybersecurity?",
                    new List<string> { "A) Using social media to promote cybersecurity awareness", "B) Manipulating people to trick them into divulging confidential information", "C) Designing secure social networks", "D) Automated systems for detecting online fraud" },
                    "B) Manipulating people to trick them into divulging confidential information", // Correct answer string
                    "Social engineering preys on human psychology to trick individuals into performing actions or divulging confidential information."
                )
            };

            // Optional: Shuffle questions for variety each time the quiz is played
            questions = questions.OrderBy(q => Guid.NewGuid()).ToList();
        }

        public int CurrentQuestionNumber
        {
            get { return currentQuestionIndex + 1; }
        }

        /// <summary>
        /// Resets the quiz to its initial state.
        /// </summary>
        public void ResetQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            IsQuizActive = false; // Quiz is not active immediately after reset
            // Re-shuffle questions for a new game
            questions = questions.OrderBy(q => Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Starts the quiz.
        /// </summary>
        public void StartQuiz()
        {
            ResetQuiz(); // Ensure quiz state is clean before starting
            IsQuizActive = true;
        }

        /// <summary>
        /// Gets the current question.
        /// </summary>
        /// <returns>The current Question object, or null if quiz is not active or no more questions.</returns>
        public Question? GetCurrentQuestion()
        {
            if (IsQuizActive && currentQuestionIndex < questions.Count)
            {
                return questions[currentQuestionIndex];
            }
            return null;
        }

        /// <summary>
        /// Moves to the next question.
        /// </summary>
        /// <returns>True if there's a next question, false if the quiz is over.</returns>
        public bool NextQuestion()
        {
            currentQuestionIndex++;
            return IsQuizActive && currentQuestionIndex < questions.Count; // Check IsQuizActive here too
        }

        /// <summary>
        /// Submits an answer for the current question.
        /// </summary>
        /// <param name="userAnswer">The answer provided by the user.</param>
        /// <returns>True if the answer was correct, false otherwise.</returns>
        public bool SubmitAnswer(string userAnswer)
        {
            Question? currentQuestion = GetCurrentQuestion();
            if (currentQuestion != null)
            {
                bool isCorrect = currentQuestion.CheckAnswer(userAnswer); // Use the robust CheckAnswer
                if (isCorrect)
                {
                    score++;
                }
                return isCorrect;
            }
            return false; // No current question to answer
        }

        /// <summary>
        /// Gets the current score.
        /// </summary>
        public int GetScore()
        {
            return score;
        }

        /// <summary>
        /// Gets the total number of questions in the quiz.
        /// </summary>
        public int GetTotalQuestions()
        {
            return questions.Count;
        }

        /// <summary>
        /// Ends the quiz.
        /// </summary>
        public void EndQuiz()
        {
            IsQuizActive = false;
            // Optionally, you might want to reset quiz state immediately here
            // but usually ResetQuiz() is called at the start of a new game.
        }
    }
}