using System;
using System.Media;

namespace CybersecurityAwarenessBot
{
    public class Chatbot
    {
        public string UserName { get; private set; }

        public void Start()
        {
            PlayGreeting();
            ConsoleUI.DisplayLogo();
            GreetUser();
            RunConversationLoop();
        }

        private void PlayGreeting()
        {
            try
            {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "greeting.wav");
                using (SoundPlayer player = new SoundPlayer(path))
                {
                    player.PlaySync();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[Voice greeting unavailable]");
            }
        }

        private void GreetUser()
        {
            ConsoleUI.PrintBorder();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Hello! Welcome to the Cybersecurity Awareness Bot.");
            Console.ResetColor();
            Console.Write("What's your name? ");
            string input = Console.ReadLine();

            UserName = string.IsNullOrWhiteSpace(input) ? "Friend" : input.Trim();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nNice to meet you, {UserName}! I'm here to help you stay safe online.");
            Console.ResetColor();
            ConsoleUI.PrintBorder();
        }

        private void RunConversationLoop()
        {
            bool running = true;
            while (running)
            {
                Console.Write($"\n{UserName}> ");
                string input = Console.ReadLine();
                string response = ResponseHandler.GetResponse(input, UserName);

                if (response == "__EXIT__")
                {
                    running = false;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Stay safe online! Goodbye.");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(response);
                }
            }
        }
    }
}
