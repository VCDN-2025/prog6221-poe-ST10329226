using System.Media; // Add this directive
using System.IO; // Needed for Path.Combine

namespace AwarenessChatbot
{
    // This class handles playing audio greetings.
    public class AudioPlayer
    {
        public void PlayGreeting()
        {
            try
            {
                // Assuming the audio file is named "greeting.wav" and copied to the output directory
                string audioFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

                if (File.Exists(audioFilePath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioFilePath))
                    {
                        player.Play();
                    }
                }
                else
                {
                    // Fallback to a system beep or log an error if the file isn't found
                    SystemSounds.Beep.Play();
                    MessageBox.Show($"Audio file not found: {audioFilePath}", "Audio Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Log or display any errors during audio playback
                MessageBox.Show($"Error playing audio: {ex.Message}", "Audio Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SystemSounds.Beep.Play(); // Fallback
            }
        }
    }
}