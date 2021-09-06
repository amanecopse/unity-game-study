using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Maze
{
    class Pos
    {
        public int Y;
        public int X;
        public Pos(int y, int x) { Y = y; X = x; }
    }

    class Player
    {

        Board _board;
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        Random rand = new Random();
        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        List<Pos> _points = new List<Pos>();

        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;
            _board = board;

            DFS();
        }

        void DFS()
        {
            int[] deltaY = { -1, 0, 1, 0 };
            int[] deltaX = { 0, -1, 0, 1 };
            Queue<Pos> q = new Queue<Pos>();
            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];
            int[,] distance = new int[_board.Size, _board.Size];
            q.Enqueue(new Pos(1, 1));
            found[1, 1] = true;
            parent[1, 1] = new Pos(1, 1);
            distance[1, 1] = 0;

            int nowY, nowX;
            int destY = _board.DestY, destX = _board.DestX;
            while (q.Count > 0)
            {
                Pos pos = q.Dequeue();
                nowY = pos.Y;
                nowX = pos.X;
                for (int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];

                    if (nextY < 0 || nextX < 0 || nextY >= _board.Size || nextX >= _board.Size)
                        continue;
                    if (found[nextY, nextX])
                        continue;
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);
                    distance[nextY, nextX] = distance[nowY, nowX] + 1;

                }
            }

            int y = destY;
            int x = destX;
            _points.Add(new Pos(y, x));
            while (!(y == 1 && x == 1))
            {
                Pos parentPos = parent[y, x];
                _points.Add(parentPos);
                y = parentPos.Y;
                x = parentPos.X;
            }
            _points.Reverse();
        }

        const int MOVE_TICK = 100;
        int _sumTick = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            _sumTick += deltaTick;
            if (_sumTick < MOVE_TICK)
                return;
            _sumTick = 0;

            if (_lastIndex >= _points.Count)
                return;

            PosY = _points[_lastIndex].Y;
            PosX = _points[_lastIndex].X;
            _lastIndex++;

        }
    }
}