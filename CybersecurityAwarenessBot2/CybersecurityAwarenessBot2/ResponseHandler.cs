using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot2
{
    public static class ResponseHandler
    {
        private static readonly Random _rng = new();

        private static readonly Dictionary<string, List<string>> Tips = new()
        {
            ["password"] = new()
            {
                "Use long, unique passwords with letters, numbers, and symbols.",
                "Consider a password manager — you only need to remember one strong master password.",
                "Never reuse passwords across sites. A breach on one site shouldn't unlock the rest.",
                "Turn on two-factor authentication wherever it's offered."
            },
            ["phishing"] = new()
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Always check the sender's address — the display name can be spoofed.",
                "Hover over links before clicking to preview the real URL.",
                "If an email creates urgency ('act now!'), treat it as a red flag."
            },
            ["privacy"] = new()
            {
                "Review your social media privacy settings at least twice a year.",
                "Limit what personal data you share publicly — attackers piece it together.",
                "Use a VPN on public Wi-Fi to protect your traffic."
            },
            ["scam"] = new()
            {
                "If an offer sounds too good to be true, it almost certainly is.",
                "Never send money or gift cards to someone you met online.",
                "Verify by calling the organisation on a number you look up yourself."
            },
            ["browsing"] = new()
            {
                "Stick to HTTPS sites — look for the padlock icon.",
                "Avoid downloads from untrusted sources; keep your browser updated.",
                "Keep your antivirus and OS patches current."
            }
        };

        public static string GetResponse(string input, UserMemory memory)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "I didn't quite understand that. Could you rephrase?";

            string lower = input.Trim().ToLowerInvariant();

            if (lower is "exit" or "quit" or "bye")
                return "__EXIT__";

            if (IsFollowUp(lower))
            {
                if (!string.IsNullOrEmpty(memory.LastTopic))
                    return $"Here's another tip on {memory.LastTopic}: {PickTip(memory.LastTopic)}";
                return "Sure — what topic would you like more on? Password, phishing, privacy, scams, or browsing?";
            }

            if (lower.Contains("how are you"))
                return "I'm running smoothly and ready to help you stay cyber-safe!";

            if (lower.Contains("purpose"))
                return "My purpose is to help you understand threats like phishing, weak passwords, and unsafe browsing.";

            if (lower.Contains("what can i ask") || lower == "help")
                return "You can ask me about passwords, phishing, safe browsing, scams, or privacy. " +
                       "You can also say 'tell me more' for another tip.";

            if (lower.Contains("my name") || lower.Contains("who am i"))
                return string.IsNullOrEmpty(memory.Name)
                    ? "I don't think you've told me your name yet. What should I call you?"
                    : $"You're {memory.Name}, of course!";

            if ((lower.Contains("my favourite") || lower.Contains("my favorite") || lower.Contains("what am i interested"))
                && !string.IsNullOrEmpty(memory.FavouriteTopic))
                return $"You told me you're interested in {memory.FavouriteTopic}. " +
                       $"As someone interested in {memory.FavouriteTopic}, you might want to review your {memory.FavouriteTopic} habits regularly.";

            var sentiment = SentimentDetector.Detect(lower);
            string empathy = SentimentDetector.EmpathyPrefix(sentiment);

            string? topic = FindTopic(lower);
            if (topic != null)
            {
                memory.LastTopic = topic;
                string tip = PickTip(topic);

                if (sentiment != Sentiment.Neutral)
                    return $"{empathy}Let me share a tip on {topic}: {tip}";

                return tip;
            }

            return $"I didn't quite understand that, {memory.Name ?? "friend"}. " +
                   "Try asking about 'passwords', 'phishing', 'privacy', 'scams', or 'browsing'.";
        }

        private static bool IsFollowUp(string lower) =>
            lower.Contains("another tip") ||
            lower.Contains("tell me more") ||
            lower.Contains("explain more") ||
            lower.Contains("one more") ||
            lower.Contains("more info");

        private static string? FindTopic(string lower)
        {
            foreach (var key in Tips.Keys)
                if (lower.Contains(key)) return key;

            if (lower.Contains("email")) return "phishing";
            if (lower.Contains("browser") || lower.Contains("https")) return "browsing";
            return null;
        }

        private static string PickTip(string topic)
        {
            if (!Tips.TryGetValue(topic, out var list) || list.Count == 0)
                return "Stay alert online — that's the best first defence.";

            return list[_rng.Next(list.Count)];
        }
    }
}