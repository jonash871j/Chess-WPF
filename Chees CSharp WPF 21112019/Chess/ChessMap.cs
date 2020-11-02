namespace Chees_CSharp_WPF_21112019
{
    static class ChessMap
    {
        static public char[,] mapArray = new char[8, 8];
        static public char[,] moveArray = new char[8, 8];
        static public int brickWhiteCounter = 16;
        static public int brickBlackCounter = 16;
        static public bool brickSelected = false;
        static public bool brickTeam = true; // true White, false black
        static public bool enableMarker = true;
        static public bool enableTimer = true;
        static public bool enableSound = true;
        static public bool enableRandomizer10000 = true;
        static public bool stopTimer = false;
        static public int tSecond = 0;
        static public int tMin = 0;

        static public void Reset()
        {
            brickWhiteCounter = 16;
            brickBlackCounter = 16;
            brickSelected = false;
            brickTeam = true;

            moveArray = new char[8, 8];
            mapArray = new char[8, 8]
            {
                { 'b', 'c', 'd', 'f', 'e', 'd', 'c', 'b' }, // A, a : Pawn
                { 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a' }, // B, b : Rock
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }, // C, c : Knight
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }, // D, d : Bishop
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }, // E, e : Queen
                { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }, // F, f : King
                { 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A' },
                { 'B', 'C', 'D', 'F', 'E', 'D', 'C', 'B' },
            };
        }
        static public void SetMapArray(int x, int y, char character)
        {
            if ((character == '*') && (enableMarker == false))
                return;

            if ((x >= 0) && (x <= 7) && (y >= 0) && (y <= 7))
                mapArray[y, x] = character;
        }
        static public void SetMoveArray(int x, int y, char character)
        {
            if ((x >= 0) && (x <= 7) && (y >= 0) && (y <= 7))
                moveArray[y, x] = character;
        }
        static public bool CheckMapArray(int x, int y, char character)
        {
            if ((x >= 0) && (x <= 7) && (y >= 0) && (y <= 7))
                if (mapArray[y, x] == character)
                    return true;
            return false;
        }
        static public bool CheckMoveArray(int x, int y, char character)
        {
            if ((x >= 0) && (x <= 7) && (y >= 0) && (y <= 7))
                if (moveArray[y, x] == character)
                    return true;
            return false;
        }
    }
}
