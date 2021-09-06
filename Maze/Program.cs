using System;

namespace Maze
{
    class Program
    {
        const int WAIT_TICK = 1000 / 30;

        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            int lastTick = 0;

            board.Initialize(25, player);
            player.Initialize(1, 1, board);

            while (true)
            {
                #region 프레임관리
                int currentTick = Environment.TickCount;
                int deltaTick = currentTick - lastTick;
                if (deltaTick < WAIT_TICK)
                    continue;
                lastTick = currentTick;
                #endregion

                //입력

                //로직
                player.Update(deltaTick);

                //렌더링
                board.Render();
            }
        }
    }
}
