using System;

namespace CybersecurityAwarenessBot
{
    public static class ResponseHandler
    {
        public static string GetResponse(string? input, string userName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "I didn't quite understand that. Could you rephrase?";
            }

            string normalized = input.Trim().ToLowerInvariant();

            // Exit commands
            if (normalized is "exit" or "quit" or "bye")
                return "__EXIT__";

            // Greetings / small talk
            if (normalized.Contains("how are you"))
                return "I'm running smoothly and ready to help you stay cyber-safe!";

            if (normalized.Contains("purpose"))
                return "My purpose is to help you understand threats like phishing, " +
                       "weak passwords, and unsafe browsing.";

            if (normalized.Contains("what can i ask") || normalized.Contains("help"))
                return "You can ask me about 'passwords', 'phishing', or 'safe browsing'!";

            // Cybersecurity topics
            if (normalized.Contains("password"))
                return "Use long, unique passwords with letters, numbers, and symbols. " +
                       "Consider a password manager, and never reuse passwords across sites.";

            if (normalized.Contains("phishing") || normalized.Contains("scam") || normalized.Contains("email"))
                return "Phishing emails impersonate trusted organisations to trick you into " +
                       "clicking malicious links. Always check the sender's address before clicking anything.";

            if (normalized.Contains("browsing") || normalized.Contains("browser") || normalized.Contains("https"))
                return "Stick to HTTPS sites, avoid downloads from untrusted sources, " +
                       "and keep your browser and antivirus up to date.";

            // Default fallback
            return $"I didn't quite understand that, {userName}. Try asking about " +
                   "'passwords', 'phishing', or 'safe browsing'.";
        }
    }
}
