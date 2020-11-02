using System.IO;
using System.Windows;

namespace Chees_CSharp_WPF_21112019
{
    /// <summary>
    /// Interaction logic for ConfigGame.xaml
    /// </summary>
    public partial class ConfigGameWindow : Window
    {
        public ConfigGameWindow()
        {
            InitializeComponent();
            UpdateCheckboxes();
        }

        void UpdateCheckboxes()
        {
            if (ChessMap.enableRandomizer10000 == false)
                rb_YourSelf.IsChecked = true;
            else
                rb_Rand10000.IsChecked = true;

            switch (ChessMap.enableMarker)
            {
                case true: cb_brickMaker.IsChecked = true; break;
                case false: cb_brickMaker.IsChecked = false; break;
            }

            switch (ChessMap.enableTimer)
            {
                case true: cb_timer.IsChecked = true; break;
                case false: cb_timer.IsChecked = false; break;
            }

            switch (ChessMap.enableSound)
            {
                case true: cb_sound.IsChecked = true; break;
                case false: cb_sound.IsChecked = false; break;
            }
        }
        private void rb_YourSelf_Click(object sender, RoutedEventArgs e)
        {
            if (rb_YourSelf.IsChecked == true)
                ChessMap.enableRandomizer10000 = false;
            else
                ChessMap.enableRandomizer10000 = true;
        }
        
        private void rb_Rand10000_Click(object sender, RoutedEventArgs e)
        {
            if (rb_YourSelf.IsChecked == true)
                ChessMap.enableRandomizer10000 = false;
            else
                ChessMap.enableRandomizer10000 = true;
        }

        private void cb_brickMaker_Click(object sender, RoutedEventArgs e)
        {
            switch (cb_brickMaker.IsChecked)
            {
                case true: ChessMap.enableMarker = true; break;
                case false: ChessMap.enableMarker = false; break;
            }
        }

        private void cb_timer_Click(object sender, RoutedEventArgs e)
        {
            switch (cb_timer.IsChecked)
            {
                case true: ChessMap.enableTimer = true; break;
                case false: ChessMap.enableTimer = false; break;
            }
        }

        private void cb_sound_Click(object sender, RoutedEventArgs e)
        {
            switch (cb_sound.IsChecked)
            {
                case true: ChessMap.enableSound = true; break;
                case false: ChessMap.enableSound = false; break;
            }
        }

        private void bn_default_Click(object sender, RoutedEventArgs e)
        {
            ChessMap.enableRandomizer10000 = true;
            ChessMap.enableMarker = true;
            ChessMap.enableTimer = true;
            ChessMap.enableSound = true;
            UpdateCheckboxes();
        }

        private void bn_ok_Click(object sender, RoutedEventArgs e)
        {
            TextWriter gameConfig = new StreamWriter("Config.ini");
            gameConfig.WriteLine(ChessMap.enableRandomizer10000);
            gameConfig.WriteLine(ChessMap.enableMarker);
            gameConfig.WriteLine(ChessMap.enableTimer);
            gameConfig.WriteLine(ChessMap.enableSound);
            gameConfig.Close();

            this.Close();
        }
    }
}
