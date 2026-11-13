using System.Collections.Generic;

namespace CybersecurityAwarenessBot2
{
    public enum Sentiment { Neutral, Worried, Curious, Frustrated }

    public static class SentimentDetector
    {
        private static readonly Dictionary<Sentiment, string[]> Keywords = new()
        {
            [Sentiment.Worried] = new[] { "worried", "scared", "afraid", "anxious", "nervous" },
            [Sentiment.Curious] = new[] { "curious", "wondering", "interested", "want to know" },
            [Sentiment.Frustrated] = new[] { "frustrated", "annoyed", "confused", "stuck", "angry" }
        };

        public static Sentiment Detect(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return Sentiment.Neutral;
            string lower = input.ToLowerInvariant();

            foreach (var kvp in Keywords)
                foreach (var word in kvp.Value)
                    if (lower.Contains(word)) return kvp.Key;

            return Sentiment.Neutral;
        }

        public static string EmpathyPrefix(Sentiment s) => s switch
        {
            Sentiment.Worried => "It's completely understandable to feel that way. ",
            Sentiment.Curious => "Great question — curiosity is how we stay safe! ",
            Sentiment.Frustrated => "I hear you, let's take it one step at a time. ",
            _ => string.Empty
        };
    }
}