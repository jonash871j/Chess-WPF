using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Chees_CSharp_WPF_21112019
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChessBoard chessBoard;

        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists("Config.ini"))
            {
                try
                {
                    // Try to read data
                    TextReader gameConfig = new StreamReader("Config.ini");
                    ChessMap.enableRandomizer10000 = Convert.ToBoolean(gameConfig.ReadLine());
                    ChessMap.enableMarker = Convert.ToBoolean(gameConfig.ReadLine());
                    ChessMap.enableTimer = Convert.ToBoolean(gameConfig.ReadLine());
                    ChessMap.enableSound = Convert.ToBoolean(gameConfig.ReadLine());
                    gameConfig.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Key input
            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyUpEvent, new KeyEventHandler(keyShortcutEvent), true);

            // Chessboard
            chessBoard = new ChessBoard(gridMain, UI01, UI02, UI03, UI04);
            chessBoard.Update();

            // Timer
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        // Timer
        private void Dt_Tick(object sender, EventArgs e)
        {
            chessBoard.Update();
            if (ChessMap.stopTimer == false)
            {
                ChessMap.tSecond++;
                string sSecond;
                string sMin;

                if (ChessMap.tSecond >= 60)
                {
                    ChessMap.tSecond = 0;
                    ChessMap.tMin++;
                }

                if (ChessMap.tSecond < 10)
                    sSecond = "0" + ChessMap.tSecond.ToString();
                else
                    sSecond = "" + ChessMap.tSecond.ToString();

                if (ChessMap.tMin < 10)
                    sMin = "0" + ChessMap.tMin.ToString();
                else
                    sMin = "" + ChessMap.tMin.ToString();

                UI_TimerSec.Text = sSecond;
                UI_TimerMin.Text = sMin;
            }

            if (ChessMap.enableTimer == false)
            {
                UI_TimerSec.Visibility = Visibility.Hidden;
                UI_TimerCol.Visibility = Visibility.Hidden;
                UI_TimerMin.Visibility = Visibility.Hidden;
            }
            else
            {
                UI_TimerSec.Visibility = Visibility.Visible;
                UI_TimerCol.Visibility = Visibility.Visible;
                UI_TimerMin.Visibility = Visibility.Visible;
            }
        }

        private void MenuItem_NewGame(object sender, RoutedEventArgs e)
        {
            ChessMap.tSecond = -1;
            ChessMap.tMin = 0;
            ChessMap.Reset();
            chessBoard.Update();
        }

        private void MenuItem_ConfigGame(object sender, RoutedEventArgs e)
        {
            ConfigGameWindow objConfigGame = new ConfigGameWindow();
            objConfigGame.Owner = this;
            objConfigGame.Show();
        }

        private void MT_fullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (MT_fullScreen.IsChecked == true)
            {
                this.Visibility = Visibility.Collapsed;
                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowState = WindowState.Maximized;
                this.Topmost = true;
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.ResizeMode = ResizeMode.CanResize;
                this.Topmost = false;
            }
        }

        private void MenuItem_About(object sender, RoutedEventArgs e)
        {
            AboutWindow objAboutWindow = new AboutWindow("Version 1.0 release");
            objAboutWindow.Owner = this;
            objAboutWindow.Show();
        }
        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Close();
       
        }

        private void keyShortcutEvent(object sender, KeyEventArgs e)
        {
            // Newgame 
            if (e.Key == Key.F1)
                MenuItem_NewGame(0, e);
            // Fullscreen 
            else if (e.Key == Key.F12)
            {
                switch (MT_fullScreen.IsChecked)
                {
                    case true: MT_fullScreen.IsChecked = false; break;
                    case false: MT_fullScreen.IsChecked = true; break;
                }
                MT_fullScreen_Click(0, e);
            }
        }
    }
}