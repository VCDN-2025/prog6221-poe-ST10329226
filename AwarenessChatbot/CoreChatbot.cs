using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwarenessChatbot
{
    // Delegate for handling chatbot responses (demonstrates delegate usage as per rubric).
    public delegate string ChatbotResponseHandler(string input, UserProfile profile, ref string lastTopic);

    // This class contains the core logic for the chatbot's responses.
    public class CoreChatbot
    {
        // Dictionary to store keyword-based responses. Keys are keywords, values are lists of possible responses.
        // This allows for random selection of responses for a given keyword.
        private readonly Dictionary<string, List<string>> _keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            {"password", new List<string>
                {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                    "A strong password combines uppercase and lowercase letters, numbers, and symbols. Aim for at least 12 characters!",
                    "Using a password manager can help you create and store complex, unique passwords securely."
                }
            },
            {"scam", new List<string>
                {
                    "Scams often involve urgent requests for money or personal information. Always verify the sender before acting.",
                    "Be wary of unsolicited messages or calls promising big rewards. If it sounds too good to be true, it probably is a scam.",
                    "Common scams include phishing, tech support scams, and romance scams. Always be skeptical."
                }
            },
            {"privacy", new List<string>
                {
                    "Review the privacy settings on your social media and online accounts regularly. Limit what information you share publicly.",
                    "Understanding privacy policies helps you know how your data is being used. Always read them if you can!",
                    "Protecting your online privacy means being mindful of what you share and who you share it with."
                }
            },
            {"phishing", new List<string>
                {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "Always check the sender's email address and look for suspicious links in phishing attempts. Don't click on them!",
                    "If an email seems suspicious, don't reply or click any links. Instead, go directly to the official website or contact the organization by phone."
                }
            },
            {"malware", new List<string>
                {
                    "Malware is malicious software designed to harm or exploit your computer system. It includes viruses, worms, and ransomware.",
                    "To protect against malware, keep your software updated, use reputable antivirus software, and be careful about opening suspicious attachments."
                }
            },
            {"2fa", new List<string>
                {
                    "Two-factor authentication (2FA) adds an extra layer of security by requiring a second verification step, like a code from your phone, in addition to your password.",
                    "Always enable 2FA wherever possible, especially for sensitive accounts like email and banking."
                }
            }
        };

        // Dictionary for more detailed follow-up responses based on the last recognized topic.
        private readonly Dictionary<string, string> _followUpResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"password", "For more on passwords: Consider using a password manager to securely store and generate complex passwords. Two-factor authentication (2FA) adds another layer of security."},
            {"scam", "To elaborate on scams: Common types include phishing, tech support scams, and romance scams. Always be skeptical and verify independently. Never give out personal details over the phone unless you initiated the call."},
            {"privacy", "More about privacy: Data breaches can expose your personal information. Regularly check if your email or passwords have been compromised on sites like Have I Been Pwned. Also, be mindful of app permissions."},
            {"phishing", "Further phishing tips: Phishing can also happen via text messages (smishing) or voice calls (vishing). The goal is always to trick you into revealing sensitive data. If in doubt, delete the message."},
            {"malware", "Expanding on malware: Ransomware encrypts your files and demands payment, while spyware secretly monitors your activity. Adware floods you with unwanted ads. Regular backups are crucial to recover from ransomware."},
            {"2fa", "More about 2FA: While SMS-based 2FA is common, authenticator apps (like Google Authenticator or Authy) are generally more secure as they don't rely on phone numbers that can be swapped."}
        };

        // Dictionary for sentiment-based empathetic phrases.
        private readonly Dictionary<string, string> _sentimentPhrases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"worried", "It's completely understandable to feel that way. "},
            {"frustrated", "I understand that can be frustrating. "},
            {"curious", "That's a great question to be curious about! "},
            {"confused", "It's okay to feel confused, cybersecurity can be complex. "},
            {"overwhelmed", "Feeling overwhelmed is common when dealing with these topics. "}
        };

        // Default response for unrecognized input.
        private const string DefaultResponse = "I'm not sure I understand. Can you try rephrasing or asking about a specific cybersecurity topic like 'password', 'scam', 'privacy', 'phishing tips', 'malware', or '2FA'?";

        private readonly Random _random = new Random();

        // Public method to get a response from the chatbot.
        // It takes user input, the user's profile (for memory), and the last recognized topic (for conversation flow).
        public string GetResponse(string userInput, UserProfile userProfile, ref string lastRecognizedTopic)
        {
            string lowerInput = userInput.ToLower();
            string sentimentPrefix = ""; // For sentiments
            string mainResponse = "";   // For keywords, follow-ups, direct memory acknowledgements

            bool sentimentDetected = false;
            bool directMemoryInteraction = false; // Flag if the input was primarily a memory update

            // 1. Sentiment Detection
            foreach (var entry in _sentimentPhrases)
            {
                if (lowerInput.Contains(entry.Key))
                {
                    sentimentPrefix = entry.Value; // Store sentiment prefix
                    sentimentDetected = true;
                    break;
                }
            }

            // 2. Memory and Recall - Extracting User Information
            Match nameMatch = Regex.Match(lowerInput, @"(my name is|i am)\s+([a-z]+)");
            if (nameMatch.Success && nameMatch.Groups.Count > 2)
            {
                string name = nameMatch.Groups[2].Value;
                // Ensure name is not null or empty before trying to access name[0]
                if (!string.IsNullOrEmpty(name))
                {
                    userProfile.Name = char.ToUpper(name[0]) + name.Substring(1);
                    mainResponse += $"Hello {userProfile.Name}! It's nice to meet you. ";
                    directMemoryInteraction = true;
                }
            }

            Match topicMatch = Regex.Match(lowerInput, @"(i'm interested in|my favorite topic is|i like)\s+([a-z\s]+)");
            if (topicMatch.Success && topicMatch.Groups.Count > 2)
            {
                string topic = topicMatch.Groups[2].Value.Trim();
                // Ensure topic is not null or empty
                if (!string.IsNullOrEmpty(topic))
                {
                    userProfile.FavoriteTopic = topic;
                    mainResponse += $"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online. ";
                    directMemoryInteraction = true;
                }
            }

            // 3. Conversation Flow - Handle Follow-up Questions
            // We check for follow-ups or keywords if not solely a direct memory interaction that already formed a full response,
            // OR if a sentiment was detected and needs content, OR if mainResponse is still empty.
            if (!directMemoryInteraction || sentimentDetected || string.IsNullOrWhiteSpace(mainResponse))
            {
                string[] followUpPhrases = { "tell me more", "explain", "more details", "elaborate", "what else", "can you tell me more", "more", "anymore", "let me know more", "what else" };
                bool isFollowUp = followUpPhrases.Any(phrase => lowerInput.Contains(phrase));

                if (isFollowUp && !string.IsNullOrEmpty(lastRecognizedTopic) && _followUpResponses.ContainsKey(lastRecognizedTopic))
                {
                    mainResponse += _followUpResponses[lastRecognizedTopic];
                }
                // Only look for keywords if it wasn't a follow-up AND (mainResponse is empty OR a sentiment needs content)
                else if (!isFollowUp && (string.IsNullOrWhiteSpace(mainResponse) || sentimentDetected))
                {
                    // 4. Keyword Recognition
                    string? matchedKeyword = null;
                    foreach (var keyword in _keywordResponses.Keys)
                    {
                        if (lowerInput.Contains(keyword))
                        {
                            matchedKeyword = keyword;
                            break;
                        }
                    }

                    if (matchedKeyword != null)
                    {
                        lastRecognizedTopic = matchedKeyword; // Update the last recognized topic
                        List<string> possibleResponses = _keywordResponses[matchedKeyword];
                        string chosenResponse = possibleResponses[_random.Next(possibleResponses.Count)];

                        // Personalize response
                        // Check if the favorite topic is part of the input to make personalization more specific for that turn
                        if (userProfile.FavoriteTopic != null && lowerInput.Contains(userProfile.FavoriteTopic.ToLower()))
                        {
                            // If memory was just updated for favorite topic, mainResponse might already have an acknowledgement.
                            // We need to decide if we append or construct a new response.
                            // For simplicity, let's allow appending here.
                            mainResponse += $"As someone interested in {userProfile.FavoriteTopic}, {chosenResponse} ";
                        }
                        // Avoid "Hi Name, Hi Name..." if name was just set and acknowledged in mainResponse
                        else if (userProfile.Name != null && !mainResponse.Contains($"Hello {userProfile.Name}"))
                        {
                            mainResponse += $"Hi {userProfile.Name}, {chosenResponse} ";
                        }
                        else
                        {
                            mainResponse += chosenResponse;
                        }
                    }
                }
            }

            // 5. Combine prefix and main response, and apply default if necessary
            string finalBotResponse = (sentimentPrefix + mainResponse).Trim();

            if (string.IsNullOrWhiteSpace(finalBotResponse))
            {
                // If, after everything, the response is still blank (no sentiment, no memory, no follow-up, no keyword).
                finalBotResponse = DefaultResponse;
            }
            else if (sentimentDetected && string.IsNullOrWhiteSpace(mainResponse.Trim()))
            {
                // If there was a sentiment prefix but no main content was generated from other rules.
                // e.g., User says "I'm worried" (sentiment detected) but "worried" doesn't match a keyword and no other content is generated.
                finalBotResponse = (sentimentPrefix + DefaultResponse).Trim(); // Append default to the sentiment.
            }

            return finalBotResponse;
        }
    }
}