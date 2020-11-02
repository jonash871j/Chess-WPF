using System.Windows;

namespace Chees_CSharp_WPF_21112019
{
    class ChessBricks
    {
        private char brickStore = ' ';
        private int brickDire = 0;
        private int xLast, yLast;

        System.Media.SoundPlayer snd_click01 = new System.Media.SoundPlayer(Chess.Properties.Resources.Click01);
        System.Media.SoundPlayer snd_click02 = new System.Media.SoundPlayer(Chess.Properties.Resources.Click02);

        // Used to set direction considered by if it's the black- or white brick
        private void CheckBrickChange(bool brickChange)
        {
            if (brickChange == true)
                brickDire = 1;
            else
                brickDire = -1;
        }

        // Used to check where the brick is allowed to move
        private void CheckMovement(int loopLength, int x, int y, int xDire, int yDire, bool brickChange, bool brickBlock)
        {
            for (int i = 1; i < loopLength; i++)
            {
                if ((CheckCellEnemy(x + (-i * xDire), y + (-i * yDire), brickChange)))
                    ChessMap.SetMoveArray(x + (-i * xDire), y + (-i * yDire), '#');
                if (ChessMap.CheckMapArray(x + (-i * xDire), y + (-i * yDire), ' '))
                {
                    ChessMap.SetMoveArray(x + (-i * xDire), y + (-i * yDire), '*');
                    ChessMap.SetMapArray(x + (-i * xDire), y + (-i * yDire), '*');
                }
                else if (brickBlock == true) return;
            }
        }

        // Used set brick variables
        private void SetBrickVariables(char brickChar, int x, int y)
        {
            ChessMap.brickSelected = true;
            brickStore = brickChar;
            xLast = x;
            yLast = y;
        }

        // Used to check eneme cell
        private bool CheckCellEnemy(int x, int y, bool brickChange)
        {
            // Check enemys for white brick
            if (brickChange == true)
            {
                for (int i = 97; i < 103; i++)
                    if (ChessMap.CheckMapArray(x, y, (char)i))
                        return true;
            }
            // Check enemys for black brick
            else
            {
                for (int i = 65; i < 71; i++)
                    if (ChessMap.CheckMapArray(x, y, (char)i))
                        return true;
            }
            return false;
        }

        // Changes the brick team
        private void TeamChange(int brickCounter)
        {
            switch (ChessMap.brickTeam)
            {
                case true:
                    ChessMap.brickBlackCounter -= brickCounter;
                    ChessMap.brickTeam = false;
                    break;
                case false:
                    ChessMap.brickWhiteCounter -= brickCounter;
                    ChessMap.brickTeam = true;
                    break;
            }
        }
        
        // Used to move the brick
        protected void MoveBrick(int y, int x)
        {
            if ((y != yLast) || (x != xLast))
            {
                if ((ChessMap.CheckMoveArray(x, y, '*')) || (ChessMap.CheckMoveArray(x, y, '#')))
                {
                    // Pawn rule when touching the edge of the map
                    if ((y == 0) && (brickStore == 'A'))
                        brickStore = 'E';
                    if ((y == 7) && (brickStore == 'a'))
                        brickStore = 'e';

                    int brickCounter = 0;
                    if (ChessMap.CheckMoveArray(x, y, '#'))
                    {
                        brickCounter = 1;
                        if (ChessMap.enableSound == true)
                            snd_click02.Play();
                    }
                    else if (ChessMap.enableSound == true)
                        snd_click01.Play();

                    ChessMap.SetMapArray(x, y, brickStore);
                    ChessMap.SetMoveArray(x, y, ' ');
                    ChessMap.SetMapArray(xLast, yLast, ' ');
                    brickStore = '!';

                    // Changes the brick team
                    TeamChange(brickCounter);
                }
                ChessMap.brickSelected = false;
            }
        }
        // Used clear all brick markers
        protected void ClearBrickMarker()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (ChessMap.brickSelected == false)
                    {
                        if (ChessMap.CheckMoveArray(x, y, '*'))
                        {
                            ChessMap.SetMoveArray(x, y, ' ');
                            ChessMap.SetMapArray(x, y, ' ');
                        }
                        if (ChessMap.CheckMoveArray(x, y, '#'))
                            ChessMap.SetMoveArray(x, y, ' ');
                    }
                }
            }
        }
        void Win(string message)
        {
            ChessMap.stopTimer = true;
            MessageBox.Show(message, "Checkmate", MessageBoxButton.OK, MessageBoxImage.Information);
            ChessMap.stopTimer = false;
            ChessMap.tSecond = -1;
            ChessMap.tMin = 0;
            ChessMap.Reset();
        }

        protected void CheckWin()
        {
            bool kingWhite = false;
            bool kingBlack = false;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (ChessMap.CheckMapArray(x, y, 'F'))
                        kingWhite = true;
                    if (ChessMap.CheckMapArray(x, y, 'f'))
                        kingBlack = true;
                }
            }
            if (kingWhite == false)
                Win("Black wins!");
            if (kingBlack == false)
                Win("White wins!");
        }

        /*******************************************************************/
        // PAWN LOGIC                                                      */
        /*******************************************************************/
        protected void CheckPawn(int y, int x, char brickChar, bool brickChange)
        {
            // Used to set what direction the brick should go on the y axis
            CheckBrickChange(brickChange);

            if (ChessMap.CheckMapArray(x, y, brickChar))
            {
                // Start position with pawn allows to move 1 step more forward
                int cellMove = 1;
                if ((y == 6) && (brickChange == true))
                    cellMove = 2;
                if ((y == 1) && (brickChange == false))
                    cellMove = 2;

                // Set marker where the pawn brick can move on the y- axis
                for (int i = 1; i < 1 + cellMove; i++)
                {
                    if (ChessMap.CheckMapArray(x, y + (-i * brickDire), ' '))
                    {
                        ChessMap.SetMoveArray(x, y + (-i * brickDire), '*');
                        ChessMap.SetMapArray(x, y + (-i * brickDire), '*');
                    }
                    else break;
                }
                // Set stafe marker where the pawn brick can kill
                for (int j = -1; j < 2; j++)
                    if ((CheckCellEnemy(x + j, y + (-1 * brickDire), brickChange)) && (j != 0))
                        ChessMap.SetMoveArray(x + j, y + (-1 * brickDire), '#');

                // Used to set brick variables
                SetBrickVariables(brickChar, x, y);
            }
        }
        /*******************************************************************/
        // ROCK LOGIC                                                      */
        /*******************************************************************/
        protected void CheckRock(int y, int x, char brickChar, bool brickChange)
        {
            // Used to set what direction the brick should go on the y axis
            CheckBrickChange(brickChange);

            if (ChessMap.CheckMapArray(x, y, brickChar))
            {
                // Set marker where the rock brick can move and kill
                CheckMovement(8, x, y, 0, brickDire, brickChange, true);  // y- axis
                CheckMovement(8, x, y, 0, -brickDire, brickChange, true); // y+ axis
                CheckMovement(8, x, y, brickDire, 0, brickChange, true);  // x- axis
                CheckMovement(8, x, y, -brickDire, 0, brickChange, true); // x+ axis

                // Used to set brick variables
                SetBrickVariables(brickChar, x, y);
            }
        }

        /*******************************************************************/
        // KNIGHT LOGIC                                                    */
        /*******************************************************************/
        protected void CheckKnight(int y, int x, char brickChar, bool brickChange)
        {
            // Used to set what direction the brick should go on the y axis
            CheckBrickChange(brickChange);

            if (ChessMap.CheckMapArray(x, y, brickChar))
            {
                // Set marker where the horse brick can move and kill
                CheckMovement(2, x, y, brickDire, brickDire * 2, brickChange, false);
                CheckMovement(2, x, y, -brickDire, brickDire * 2, brickChange, false);
                CheckMovement(2, x, y, brickDire, -brickDire * 2, brickChange, false);
                CheckMovement(2, x, y, -brickDire, -brickDire * 2, brickChange, false);
                CheckMovement(2, x, y, brickDire * 2, brickDire, brickChange, false);
                CheckMovement(2, x, y, brickDire * 2, -brickDire, brickChange, false);
                CheckMovement(2, x, y, -brickDire * 2, brickDire, brickChange, false);
                CheckMovement(2, x, y, -brickDire * 2, -brickDire, brickChange, false);

                // Used to set brick variables
                SetBrickVariables(brickChar, x, y);
            }
        }

        /*******************************************************************/
        // BISHOP LOGIC                                                    */
        /*******************************************************************/
        protected void CheckBishop(int y, int x, char brickChar, bool brickChange)
        {
            // Used to set what direction the brick should go on the y axis
            CheckBrickChange(brickChange);

            if (ChessMap.CheckMapArray(x, y, brickChar))
            {
                // Set marker where the bishop brick can move and kill
                CheckMovement(8, x, y, brickDire, brickDire, brickChange, true); // xy-    axis
                CheckMovement(8, x, y,-brickDire,-brickDire, brickChange, true); // xy+    axis
                CheckMovement(8, x, y, brickDire,-brickDire, brickChange, true); // x-, y+ axis
                CheckMovement(8, x, y,-brickDire, brickDire, brickChange, true); // x+, y- axis

                // Used to set brick variables
                SetBrickVariables(brickChar, x, y);
            }
        }

        /*******************************************************************/
        // QUEEN LOGIC                                                     */
        /*******************************************************************/
        protected void CheckQueen(int y, int x, char brickChar, bool brickChange)
        {
            // Used to set what direction the brick should go on the y axis
            CheckBrickChange(brickChange);

            if (ChessMap.CheckMapArray(x, y, brickChar))
            {
                // Set marker where the queen brick can move and kill
                CheckMovement(8, x, y, 0, brickDire, brickChange, true);         // y- axis
                CheckMovement(8, x, y, 0,-brickDire, brickChange, true);         // y+ axis
                CheckMovement(8, x, y, brickDire, 0, brickChange, true);         // x- axis
                CheckMovement(8, x, y,-brickDire, 0, brickChange, true);         // x+ axis
                CheckMovement(8, x, y, brickDire, brickDire, brickChange, true); // xy-    axis
                CheckMovement(8, x, y,-brickDire,-brickDire, brickChange, true); // xy+    axis
                CheckMovement(8, x, y, brickDire,-brickDire, brickChange, true); // x-, y+ axis
                CheckMovement(8, x, y,-brickDire, brickDire, brickChange, true); // x+, y- axis

                // Used to set brick variables
                SetBrickVariables(brickChar, x, y);
            }
        }

        /*******************************************************************/
        // KING LOGIC                                                      */
        /*******************************************************************/
        protected void CheckKing(int y, int x, char brickChar, bool brickChange)
        {
            // Used to set what direction the brick should go on the y axis
            CheckBrickChange(brickChange);

            if (ChessMap.CheckMapArray(x, y, brickChar))
            {
                // Set marker where the queen brick can move on y- axis
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        // Set marker where the queen can kill
                        if ((CheckCellEnemy(x + (-j * brickDire), y + (-i * brickDire), brickChange)))
                            ChessMap.SetMoveArray(x + (-j * brickDire), y + (-i * brickDire), '#');

                        // Set marker where the queen can move
                        if (ChessMap.CheckMapArray(x + (-j * brickDire), y + (-i * brickDire), ' '))
                        {
                            ChessMap.SetMoveArray(x + (-j * brickDire), y + (-i * brickDire), '*');
                            ChessMap.SetMapArray(x + (-j * brickDire), y + (-i * brickDire), '*');
                        }
                    }
                }
                // Used to set brick variables
                SetBrickVariables(brickChar, x, y);
            }
        }
    }
}
