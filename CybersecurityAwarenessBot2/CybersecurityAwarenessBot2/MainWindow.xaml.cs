using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityAwarenessBot2
{
    public partial class MainWindow : Window
    {
        private readonly Chatbot _bot = new();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayGreeting();
            AppendBotMessage("Hello! I'm the Cybersecurity Awareness Bot. What's your name?");
            UserInput.Focus();
        }

        private void PlayGreeting()
        {
            try
            {
                string path = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "Assets", "greeting.wav");

                if (System.IO.File.Exists(path))
                {
                    using SoundPlayer player = new SoundPlayer(path);
                    player.PlaySync();
                }
                else
                {
                    StatusText.Text = "Voice greeting file not found (Assets/greeting.wav).";
                }
            }
            catch
            {
                StatusText.Text = "Voice greeting unavailable.";
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => HandleUserInput();

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) HandleUserInput();
        }

        private void HandleUserInput()
        {
            string input = UserInput.Text;
            UserInput.Clear();

            if (string.IsNullOrWhiteSpace(input))
            {
                AppendBotMessage("I didn't quite understand that. Could you rephrase?");
                return;
            }

            AppendUserMessage(input);
            string response = _bot.Process(input);

            if (response == "__EXIT__")
            {
                AppendBotMessage("Stay safe online! Goodbye.");
                SendButton.IsEnabled = false;
                UserInput.IsEnabled = false;
                return;
            }

            AppendBotMessage(response);
        }

        private void AppendUserMessage(string text)
        {
            var bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x00, 0x7B, 0xB8)),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                Margin = new Thickness(80, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 600,
                Child = new TextBlock
                {
                    Text = text,
                    Foreground = Brushes.White,
                    TextWrapping = TextWrapping.Wrap
                }
            };
            ChatPanel.Children.Add(bubble);
            ChatScroll.ScrollToEnd();
        }

        private void AppendBotMessage(string text)
        {
            var bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x1B, 0x2E, 0x4F)),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 80, 5),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 600,
                Child = new TextBlock
                {
                    Text = text,
                    Foreground = new SolidColorBrush(Color.FromRgb(0x9F, 0xE8, 0xFF)),
                    TextWrapping = TextWrapping.Wrap
                }
            };
            ChatPanel.Children.Add(bubble);
            ChatScroll.ScrollToEnd();
        }
    }
}