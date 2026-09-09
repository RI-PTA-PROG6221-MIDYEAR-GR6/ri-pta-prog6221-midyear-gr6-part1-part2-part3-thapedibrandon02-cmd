using System;

namespace CybersecurityAwarenessBot
{
    public static class ResponseHandler
    {
        public static string GetResponse(string input, string userName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "I didn't quite understand that. Could you rephrase?";
            }

            string normalized = input.Trim().ToLower();

            switch (normalized)
            {
                case "exit":
                case "quit":
                case "bye":
                    return "__EXIT__";

                case "how are you":
                case "how are you?":
                    return "I'm running smoothly and ready to help you stay cyber-safe!";

                case "what's your purpose":
                case "what is your purpose":
                case "what's your purpose?":
                    return "My purpose is to help you understand threats like phishing, weak passwords, and unsafe browsing.";

                case "what can i ask you about":
                case "what can i ask you about?":
                    return "You can ask me about 'passwords', 'phishing', or 'safe browsing'!";

                case "password safety":
                case "passwords":
                    return "Use long, unique passwords with letters, numbers, and symbols. Consider a password manager, and never reuse passwords across sites.";

                case "phishing":
                    return "Phishing emails impersonate trusted organisations to trick you into clicking malicious links. Always check the sender's address before clicking anything.";

                case "safe browsing":
                case "browsing":
                    return "Stick to HTTPS sites, avoid downloads from untrusted sources, and keep your browser and antivirus up to date.";

                default:
                    return $"I didn't quite understand that, {userName}. Try asking about 'passwords', 'phishing', or 'safe browsing'.";
            }
        }
    }
}
