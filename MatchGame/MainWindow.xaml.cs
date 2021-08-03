using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int tenthsOfSecondsRemain;
        int matchesFound;
        int requiredAnimal;
        int bestTime = 100;
        int startTime = 100;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            tenthsOfSecondsRemain--;
            timeTextBlock.Text = "Remained : "+ (tenthsOfSecondsRemain / 10F).ToString("0.0s")+"   Elapsed : "+(tenthsOfSecondsElapsed / 10F).ToString("0.0s");

            if (matchesFound == 8)
            {
                if (bestTime > tenthsOfSecondsElapsed)
                {
                    bestTime = tenthsOfSecondsElapsed;
                    labelBestTime.Content = "Best Time : " + (bestTime/10F).ToString("0.0s");
                }

                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text;
                AskPlay();
            }
            else if (tenthsOfSecondsRemain == 0)
            {
                timer.Stop();
                timeTextBlock.Text = "You Lose!!!";
                AskPlay();
            }
        }

        private void AskPlay()
        {
            labelAskToPlayAgain.Visibility = Visibility.Visible;
            buttonYes.Visibility = Visibility.Visible;
        }

        private void SetUpGame()
        {
            requiredAnimal = 0;

            if (bestTime == startTime)
            {
                labelBestTime.Content = "BestTime : -";
            }

            labelAskToPlayAgain.Visibility = Visibility.Hidden;
            buttonYes.Visibility = Visibility.Hidden;

            List<string> animalEmojiCollection = new List<string>()
            {
                "🐵","🐶","🐺","🐱","🦁","🐯","🦒","🦊",
                "🦝","🐮","🐷","🐗","🐭","🐹","🐰","🐻"
            };

            Random random = new Random();

            List<string> animalEmoji = new List<string>() { };
            while (requiredAnimal<8)
            {
                int emojiIndex = random.Next(animalEmojiCollection.Count);
                animalEmoji.Add(animalEmojiCollection[emojiIndex]);
                animalEmoji.Add(animalEmojiCollection[emojiIndex]);
                animalEmojiCollection.RemoveAt(emojiIndex);
                requiredAnimal++;
            }

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }

                timer.Start();
                tenthsOfSecondsElapsed = 0;
                tenthsOfSecondsRemain = startTime;
                matchesFound = 0;
            }
        }


        TextBlock lastClickedTextBlock;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            if (tenthsOfSecondsRemain > 0)
            {
                if (findingMatch == false)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    lastClickedTextBlock = textBlock;
                    findingMatch = true;
                }
                else if (textBlock.Text == lastClickedTextBlock.Text)
                {
                    matchesFound++;
                    textBlock.Visibility = Visibility.Hidden;
                    findingMatch = false;
                }
                else
                {
                    lastClickedTextBlock.Visibility = Visibility.Visible;
                    findingMatch = false;
                }
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }

        private void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                }

                timer.Start();
                tenthsOfSecondsElapsed = 100;
                matchesFound = 0;
            }
            SetUpGame();
        }
    }
}
