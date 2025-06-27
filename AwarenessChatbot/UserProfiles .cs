using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwarenessChatbot
{
    // Represents the user's profile and memory for personalization.
    // This class holds information about the current user during the conversation.
    public class UserProfile
    {
        // Automatic property to store the user's name.
        public string Name { get; set; }
        // Automatic property to store the user's favorite cybersecurity topic.
        public string FavoriteTopic { get; set; }

        // Constructor to initialize properties to null.
        public UserProfile()
        {
            Name = "User";
            FavoriteTopic = string.Empty;
        }
    }
}