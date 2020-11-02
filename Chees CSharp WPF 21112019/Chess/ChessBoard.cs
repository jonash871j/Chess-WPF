using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chees_CSharp_WPF_21112019
{
    class ChessBoard : ChessBricks
    {
        string pathWhite = @"./Resources/Sprite/spr_w";
        string pathBlack = @"./Resources/Sprite/spr_b";
        string pathCurrent;
        string[] pathSprite = new string[]
        {
            "Tile.bmp", "Marker.bmp", "wPawn.bmp", "bPawn.bmp", "wTower.bmp", "bTower.bmp", "wKnight.bmp",
            "bKnight.bmp", "wBishop.bmp", "bBishop.bmp", "wQueen.bmp", "bQueen.bmp", "wKing.bmp", "bKing.bmp"
        };
        char[] charSprite = new char[] { ' ', '*', 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f' };

        Button[,] mapButton = new Button[8, 8];
        Image[,] img = new Image[8, 8];
        Random random = new Random();
        Grid grid;
        TextBlock UI01;
        TextBlock UI02;
        TextBlock UI03;
        TextBlock UI04;  

        bool TileIsOdd(int count)
        {
            if (count % 2 == 0)
                return false;
            else
                return true;
        }
        void ImportImage(int x, int y, char character, string path)
        {
            if (ChessMap.CheckMapArray(x, y, character))
            {

                BitmapImage btm = new BitmapImage(new Uri(path, UriKind.Relative));
                btm.UriSource = new Uri(path, UriKind.Relative);

                img[y, x].Source = btm;

                mapButton[y, x].Content = img[y, x];
            }
        }

        //******************************************************//
        // GAME LOGIC                                           //
        //******************************************************//
        void button_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((((Button)sender).Tag));
            Game(id);

            while ((ChessMap.brickTeam == false) && (ChessMap.enableRandomizer10000 == true)) 
            {
                int randomNumber = random.Next(0, 63);
                Game(randomNumber);
            }
            Update();
        }

        void Game(int id)
        {
            int idCounter = 0;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (id == idCounter)
                    {
                        if (ChessMap.brickSelected == false)
                        {
                            // White brick
                            if (ChessMap.brickTeam == true)
                            {
                                CheckPawn(y, x, 'A', true);
                                CheckRock(y, x, 'B', true);
                                CheckKnight(y, x, 'C', true);
                                CheckBishop(y, x, 'D', true);
                                CheckQueen(y, x, 'E', true);
                                CheckKing(y, x, 'F', true);
                            }
                            // Black brick
                            else
                            {
                                CheckPawn(y, x, 'a', false);
                                CheckRock(y, x, 'b', false);
                                CheckKnight(y, x, 'c', false);
                                CheckBishop(y, x, 'd', false);
                                CheckQueen(y, x, 'e', false);
                                CheckKing(y, x, 'f', false);
                            }
                        }
                        else MoveBrick(y, x);
                    }
                    idCounter++;
                }
            }
            ClearBrickMarker();
            CheckWin();
        }

        public ChessBoard(Grid grid, TextBlock UI01, TextBlock UI02, TextBlock UI03, TextBlock UI04)
        {
            ChessMap.Reset();
            this.grid = grid;
            this.UI01 = UI01;
            this.UI02 = UI02;
            this.UI03 = UI03;
            this.UI04 = UI04;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    mapButton[y, x] = new Button();
                    mapButton[y, x].Tag = y * 8 + x;
                    mapButton[y, x].Click += new RoutedEventHandler(button_Click);

                    img[y, x] = new Image();
                }
            }
        }
        public void Update()
        {
            // UI
            UI01.Text = ChessMap.brickBlackCounter.ToString();
            UI02.Text = ChessMap.brickWhiteCounter.ToString();
            switch (ChessMap.brickTeam)
            {
                case true:
                    UI03.Visibility = Visibility.Hidden;
                    UI04.Visibility = Visibility.Visible;
                    break;
                case false:
                    UI03.Visibility = Visibility.Visible;
                    UI04.Visibility = Visibility.Hidden;
                    break;
            }
        
            int count = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    // Removes old buttons
                    grid.Children.Remove(mapButton[y, x]);

                    // Set button settings
                    Grid.SetColumn(mapButton[y, x], x);
                    Grid.SetRow(mapButton[y, x], y);
                    mapButton[y, x].Background = Brushes.LightGray;

                    // Used to switch image between black- and white tile
                    if (TileIsOdd(count) == false)
                        pathCurrent = pathWhite;
                    else
                        pathCurrent = pathBlack;

                    // Import sprites
                    for (int i = 0; i < pathSprite.Length; i++)
                        ImportImage(x, y, charSprite[i], pathCurrent + pathSprite[i]);

                    // Adds button to grid
                    grid.Children.Add(mapButton[y, x]);

                    // Count the tile offset
                    count++;
                }
                // Count the tile offset
                count++;
            }
        }
    }
}