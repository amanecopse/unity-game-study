using System;

namespace Maze
{
    class Board
    {
        public int Size { get; private set; }
        public int DestY { get; private set; }
        public int DestX { get; private set; }
        public TileType[,] Tile { get; private set; }
        Player _player;
        const char CIRCLE = '\u25cf';
        public enum TileType
        {
            Empty,
            Wall
        }

        public void Initialize(int size, Player player)
        {
            Size = size;
            Tile = new TileType[size, size];
            _player = player;
            DestY = size - 2;
            DestX = size - 2;

            if (size % 2 == 0)
                return;

            GeneratedSideWinderTree();
            // GeneratedByBinaryTree();
        }
        public void Render()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            ConsoleColor defaultColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (_player.PosY == y && _player.PosX == x)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (y == DestY && x == DestX)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);
                    System.Console.Write(CIRCLE);
                }
                System.Console.WriteLine();
            }

            Console.ForegroundColor = defaultColor;
        }

        ConsoleColor GetTileColor(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;
            }
        }

        void GeneratedByBinaryTree()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x == Size - 2 && y == Size - 2)
                    {
                        continue;
                    }
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    int dir = rand.Next(0, 2);

                    if (dir == 0)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                    }
                    else
                    {
                        Tile[y, x + 1] = TileType.Empty;
                    }
                }
            }
        }

        void GeneratedSideWinderTree()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x == Size - 2 && y == Size - 2)
                    {
                        continue;
                    }
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    int dir = rand.Next(0, 2);
                    int count = 1;
                    if (dir == 0)
                    {
                        int randomIndex = rand.Next(0, count);
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;
                        count = 1;
                    }
                    else
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                }
            }
        }
    }
}