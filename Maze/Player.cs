using System;

namespace Maze
{
    class Player
    {

        Board _board;
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        Random rand = new Random();

        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;
            _board = board;
        }

        const int MOVE_TICK = 100;
        int _sumTick = 0;
        public void Update(int deltaTick)
        {
            _sumTick += deltaTick;
            if (_sumTick < MOVE_TICK)
                return;
            _sumTick = 0;

            int randomIndex = rand.Next(0, 5);

            switch (randomIndex)
            {
                case 0://상
                    if (PosY - 1 >= 0 && _board.Tile[PosY - 1, PosX] == Board.TileType.Empty)
                        PosY = PosY - 1;
                    break;
                case 1://하
                    if (PosY + 1 < _board.Size && _board.Tile[PosY + 1, PosX] == Board.TileType.Empty)
                        PosY = PosY + 1;
                    break;
                case 2://좌
                    if (PosX - 1 >= 0 && _board.Tile[PosY, PosX - 1] == Board.TileType.Empty)
                        PosX = PosX - 1;
                    break;
                case 3://우
                    if (PosX + 1 < _board.Size && _board.Tile[PosY, PosX + 1] == Board.TileType.Empty)
                        PosX = PosX + 1;
                    break;
            }

        }
    }
}