using System;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace CaroLibrary.CaroLibrary
{
    public class CaroConfig
    {
        public const int BOARD_SIZE = 32;
        public const int WIN_CONDITION = 5;
        public const int EMPTY = 0;
        public const int PLAYER_O = 1;
        public const int PLAYER_X = 2;
    }

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
        
        public string RoomName { get; set; }
    }

    public class RoomInfo
    {
        public string RoomId { get; set; }
        
        public string RoomName { get; set; }
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

    public enum MoveResult
    {
        Success,    // Move accepted, turn switched
        Invalid,    // Occupied or out of bounds
        Win,        // Move caused a win
        Draw        // Move caused a draw
    }
    
    public class CaroGameManager
    {
        public RoomInfo GameSession { get; private set; }
        public int PlayerOScore { get; private set; } 
        public int PlayerXScore { get; private set; }
        public bool IsGameOver { get; private set; }

        public CaroGameManager()
        {
            PlayerOScore = 0;
            PlayerXScore = 0;
            StartNewGame();
        }

        public void StartNewGame()
        {
            GameSession = new RoomInfo
            {
                PlayerOName = "Player 1 (Blue O)",
                PlayerXName = "Player 2 (Red X)",
                CurrentTurn = CaroConfig.PLAYER_O
            };
            IsGameOver = false;
        }

        public MoveResult MakeMove(int x, int y)
        {
            if (IsGameOver) return MoveResult.Invalid;

            // 1. Validation
            if (x < 0 || x >= CaroConfig.BOARD_SIZE || y < 0 || y >= CaroConfig.BOARD_SIZE)
                return MoveResult.Invalid;
            
            if (GameSession.Board[x, y] != CaroConfig.EMPTY)
                return MoveResult.Invalid;

            // 2. Update Board
            int currentPlayer = GameSession.CurrentTurn;
            GameSession.Board[x, y] = currentPlayer;

            // 3. Check Outcome
            if (GameLogic.CheckWin(GameSession.Board, x, y, currentPlayer))
            {
                IsGameOver = true;
                if (currentPlayer == CaroConfig.PLAYER_O) PlayerOScore++;
                else PlayerXScore++;
                return MoveResult.Win;
            }

            if (GameLogic.CheckDraw(GameSession.Board))
            {
                IsGameOver = true;
                return MoveResult.Draw;
            }

            SwitchTurn();
            return MoveResult.Success;
        }

        public void SwitchTurn()
        {
            GameSession.CurrentTurn = (GameSession.CurrentTurn == CaroConfig.PLAYER_O)
                ? CaroConfig.PLAYER_X
                : CaroConfig.PLAYER_O;
        }
    }
}