using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // Task List data
        private ObservableCollection<TaskItem> _tasks = new();

        // Mini-Game data
        private int _gameScore = 0;
        private bool _gameActive = false;
        private int _currentQuestionIndex = 0;
        private List<(string Question, bool IsPhishing, string Explanation)> _gameQuestions;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            InitializeTasks();
            InitializeGameQuestions();
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
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "greeting.wav");
                if (System.IO.File.Exists(path))
                {
                    using SoundPlayer player = new SoundPlayer(path);
                    player.PlaySync();
                }
            }
            catch { StatusText.Text = "Voice greeting unavailable."; }
        }

        // ================= TASK LIST LOGIC =================
        private void InitializeTasks()
        {
            _tasks = new ObservableCollection<TaskItem>
            {
                new TaskItem { Title = "Use strong, unique passwords" },
                new TaskItem { Title = "Enable Two-Factor Authentication (2FA)" },
                new TaskItem { Title = "Check links before clicking" },
                new TaskItem { Title = "Update software regularly" },
                new TaskItem { Title = "Review privacy settings" }
            };
            TaskListBox.ItemsSource = _tasks;
        }

        // ================= MINI-GAME LOGIC =================
        private void InitializeGameQuestions()
        {
            _gameQuestions = new List<(string, bool, string)>
            {
                ("Email: 'Your account is locked! Click here immediately to verify.'", true, "Urgency and links are classic phishing signs."),
                ("Email from your boss asking for gift cards urgently.", true, "CEO fraud is common. Verify by calling them."),
                ("Website URL: https://www.absa.co.za", false, "This is a legitimate, secure HTTPS banking site."),
                ("Text: 'You won a new iPhone! Pay R50 shipping.'", true, "If it sounds too good to be true, it is."),
                ("Popup: 'Your Windows PC is infected! Call this number.'", true, "Microsoft will never ask you to call a popup number.")
            };
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_gameActive)
            {
                // Start Game
                _gameActive = true;
                _gameScore = 0;
                _currentQuestionIndex = 0;
                GameButton.Content = "THIS IS PHISHING";
                GameButton.Background = Brushes.IndianRed;
                SafeButton.Visibility = Visibility.Visible;
                ShowNextQuestion();
            }
            else
            {
                // User guessed "Phishing"
                CheckGuess(true);
            }
        }

        private void SafeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameActive) CheckGuess(false);
        }

        private void ShowNextQuestion()
        {
            if (_currentQuestionIndex >= _gameQuestions.Count)
            {
                // End Game
                _gameActive = false;
                GameQuestion.Text = $"Game Over! Score: {_gameScore}/{_gameQuestions.Count}";
                GameButton.Content = "Play Again";
                GameButton.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xD1, 0x66));
                SafeButton.Visibility = Visibility.Collapsed;
                return;
            }

            GameQuestion.Text = _gameQuestions[_currentQuestionIndex].Question;
        }

        private void CheckGuess(bool userGuessedPhishing)
        {
            var currentQ = _gameQuestions[_currentQuestionIndex];
            bool isCorrect = (userGuessedPhishing == currentQ.IsPhishing);

            if (isCorrect)
            {
                _gameScore++;
                AppendBotMessage($"[Game] Correct! {currentQ.Explanation}");
            }
            else
            {
                AppendBotMessage($"[Game] Incorrect. {currentQ.Explanation}");
            }

            _currentQuestionIndex++;
            ShowNextQuestion();
        }

        // ================= CHAT LOGIC =================
        private void SendButton_Click(object sender, RoutedEventArgs e) => HandleUserInput();
        private void UserInput_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) HandleUserInput(); }

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
                MaxWidth = 500,
                Child = new TextBlock { Text = text, Foreground = Brushes.White, TextWrapping = TextWrapping.Wrap }
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
                MaxWidth = 500,
                Child = new TextBlock { Text = text, Foreground = new SolidColorBrush(Color.FromRgb(0x9F, 0xE8, 0xFF)), TextWrapping = TextWrapping.Wrap }
            };
            ChatPanel.Children.Add(bubble);
            ChatScroll.ScrollToEnd();
        }
    }
}