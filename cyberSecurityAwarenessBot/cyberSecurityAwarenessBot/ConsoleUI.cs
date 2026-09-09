using System;

namespace CybersecurityAwarenessBot
{
    public static class ConsoleUI
    {
        public static void PrintBorder()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('=', 60));
            Console.ResetColor();
        }

        public static void DisplayLogo()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
   _____      _                _____ ______ _____ 
  / ____|    | |              / ____|  ____/ ____|
 | |    _   _| |__   ___ _ __| (___ | |__ | |     
 | |   | | | | '_ \ / _ \ '__|\___ \|  __|| |     
 | |___| |_| | |_) |  __/ |   ____) | |___| |____ 
  \_____\__, |_.__/ \___|_|  |_____/|______\_____|
         __/ |                                     
        |___/    Awareness Bot
");
            Console.ResetColor();
        }
    }
}
