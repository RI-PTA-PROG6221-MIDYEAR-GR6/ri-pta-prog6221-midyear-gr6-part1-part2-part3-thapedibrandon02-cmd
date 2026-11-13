using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot2
{
    // Delegate (Part 2 requirement)
    public delegate string ResponseProvider(string input, UserMemory memory);

    public class Chatbot
    {
        private readonly Dictionary<string, string> _memory = new();
        private readonly UserMemory _userMemory = new();
        private readonly ResponseProvider _provider;

        public Chatbot()
        {
            _provider = ResponseHandler.GetResponse;
        }

        public string Process(string input)
        {
            if (string.IsNullOrWhiteSpace(_userMemory.Name) && IsLikelyName(input))
            {
                _userMemory.Name = Capitalise(input.Trim());
                return $"Nice to meet you, {_userMemory.Name}! You can ask me about " +
                       "passwords, phishing, safe browsing, scams, or privacy.";
            }

            RememberTopic(input);
            return _provider(input, _userMemory);
        }

        private static bool IsLikelyName(string s)
        {
            s = s.Trim();
            if (s.Length == 0 || s.Length > 30) return false;
            if (s.Contains('?')) return false;
            string[] parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 3) return false;

            string lower = s.ToLowerInvariant();
            string[] keywords = { "password", "phishing", "privacy", "scam", "browsing",
                                  "help", "exit", "quit", "tip", "hello", "hi" };
            foreach (var k in keywords)
                if (lower.Contains(k)) return false;

            return true;
        }

        private static string Capitalise(string s)
        {
            s = s.Trim();
            if (s.Length == 1) return s.ToUpperInvariant();
            return char.ToUpperInvariant(s[0]) + s.Substring(1).ToLowerInvariant();
        }

        private void RememberTopic(string input)
        {
            string lower = input.ToLowerInvariant();
            string[] topics = { "password", "phishing", "privacy", "scam", "browsing" };

            if (lower.Contains("interested in") || lower.Contains("i like"))
            {
                foreach (var t in topics)
                {
                    if (lower.Contains(t))
                    {
                        _userMemory.FavouriteTopic = t;
                        _memory["favTopic"] = t;
                        break;
                    }
                }
            }
        }
    }
}