using System;

// note:
// send json using JsonConvert.SerializeObject(object)
// receive json using JsonConvert.DeserializeObject<T>(jsonString)

namespace CaroLibrary
{
    using Newtonsoft.Json;
    using System.Net.Sockets;

    namespace CaroLibrary
    {
        public class CaroConfig
        {
            public const int BOARD_SIZE = 32;
            public const int WIN_CONDITION = 5;
            public const int EMPTY = 0;
            public const int PLAYER_O = 1;
            public const int PLAYER_X = 2;
        }

        // use for server side Player
        public class Player
        {
            public string Name { get; set; }
            public TcpClient Client { get; set; }
            public NetworkStream Stream { get; set; }
            public string CurrentRoomId { get; set; }
        }

        public class RoomSummary
        {
            public string RoomId { get; set; }
            public int CurrentPlayerCount { get; set; }
            public bool IsFull => CurrentPlayerCount >= 2;
            public bool IsGameStarted { get; set; }
        }

        public class RoomInfo
        {
            public string RoomId { get; set; }
            public string PlayerXName { get; set; }
            public string PlayerOName { get; set; }
            public int CurrentTurn { get; set; }
            public int TimePerMove { get; set; }
            public int CurrentPlayerCount { get; set; }
            public bool IsFull => CurrentPlayerCount >= 2;
            public bool IsGameStarted { get; set; }
            public int[,] Board { get; set; }
            public RoomInfo()
            {
                Board = new int[CaroConfig.BOARD_SIZE, CaroConfig.BOARD_SIZE];
                CurrentTurn = CaroConfig.PLAYER_O;
                CurrentPlayerCount = 0;
            }
        }
        public static class GameLogic
        {
            public static bool CheckWin(int[,] board, int lastX, int lastY, int player)
            {
                return CheckDirection(board, lastX, lastY, player, 1, 0) ||
                       CheckDirection(board, lastX, lastY, player, 0, 1) ||
                       CheckDirection(board, lastX, lastY, player, 1, 1) ||
                       CheckDirection(board, lastX, lastY, player, 1, -1);
            }

            private static bool CheckDirection(int[,] board, int x, int y, int player, int dx, int dy)
            {
                int count = 1;

                count += CountInDirection(board, x, y, player, dx, dy);
                count += CountInDirection(board, x, y, player, -dx, -dy);

                return count >= CaroConfig.WIN_CONDITION;
            }

            private static int CountInDirection(int[,] board, int x, int y, int player, int dx, int dy)
            {
                int count = 0;
                int curX = x + dx;
                int curY = y + dy;

                while (curX >= 0 && curX < CaroConfig.BOARD_SIZE &&
                       curY >= 0 && curY < CaroConfig.BOARD_SIZE &&
                       board[curX, curY] == player)
                {
                    count++;
                    curX += dx;
                    curY += dy;
                }
                return count;
            }

            public static bool CheckDraw(int[,] board)
            {
                for (int i = 0; i < CaroConfig.BOARD_SIZE; i++)
                    for (int j = 0; j < CaroConfig.BOARD_SIZE; j++)
                        if (board[i, j] == CaroConfig.EMPTY) return false;
                return true;
            }
        }
    }

}
