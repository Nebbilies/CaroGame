using CaroLibrary.CaroLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaroServer
{
    public class RoomManager
    {
        private List<RoomInfo> _rooms = new List<RoomInfo>();
        private object _lock = new object();

        public void CreateRoom(ClientSession session, string roomId, string roomName, int timePerMove)
        {
            lock (_lock)
            {
                if (_rooms.Any(r => r.RoomId == roomId))
                {
                    session.Send("ERROR|Room ID exists");
                    return;
                }

                RoomInfo newRoom = new RoomInfo
                {
                    RoomId = roomId,
                    RoomName = roomName,
                    TimePerMove = timePerMove,
                    PlayerOName = session.PlayerData.Name,
                    CurrentPlayerCount = 1,
                    IsGameStarted = false,
                    CurrentTurn = CaroConfig.PLAYER_O
                };

                session.PlayerData.CurrentRoomId = roomId;
                _rooms.Add(newRoom);
                session.Send($"ROOM_CREATED|{roomId}");
                Console.WriteLine($"Room {roomName} created with ID {roomId} by {session.PlayerData.Name}");
            }
        }

        public void JoinRoom(ClientSession session, string roomId, ClientSession hostSession)
        {
            lock (_lock)
            {
                RoomInfo room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (room == null || room.IsFull)
                {
                    session.Send("ERROR|Cannot join room");
                    return;
                }

                room.PlayerXName = session.PlayerData.Name;
                room.CurrentPlayerCount = 2;
                room.IsGameStarted = true;
                session.PlayerData.CurrentRoomId = roomId;
                int timePerMove = room.TimePerMove;

                string startMsg = $"GAME_START|{room.PlayerOName}|{room.PlayerXName}|{timePerMove}";

                Console.WriteLine($"Room {room.RoomName}: {startMsg}");
                hostSession.Send(startMsg + "|1\n");
                session.Send(startMsg + "|2\n");
            }
        }

        public void HandleMove(ClientSession session, string roomId, int x, int y, ClientSession opponentSession)
        {
            lock (_lock)
            {
                RoomInfo room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (room == null || !room.IsGameStarted) return;

                int playerId = (session.PlayerData.Name == room.PlayerOName) ? CaroConfig.PLAYER_O : CaroConfig.PLAYER_X;

                if (playerId != room.CurrentTurn || room.Board[x, y] != CaroConfig.EMPTY)
                {
                    session.Send("ERROR|Invalid move");
                    return;
                }

                room.Board[x, y] = playerId;

                bool isWin = GameLogic.CheckWin(room.Board, x, y, playerId);
                bool isDraw = GameLogic.CheckDraw(room.Board);
                int nextTurn = (playerId == CaroConfig.PLAYER_O) ? CaroConfig.PLAYER_X : CaroConfig.PLAYER_O;
                room.CurrentTurn = nextTurn;

                string updateMsg = $"MOVE_UPDATE|{x}|{y}|{playerId}|{nextTurn}";
                session.Send(updateMsg);
                opponentSession?.Send(updateMsg);

                if (isWin)
                {
                    string winMsg = $"GAME_OVER|WIN|{playerId}";
                    session.Send(winMsg);
                    opponentSession?.Send(winMsg);
                    _rooms.Remove(room);
                }
                else if (isDraw)
                {
                    string drawMsg = "GAME_OVER|DRAW";
                    session.Send(drawMsg);
                    opponentSession?.Send(drawMsg);
                    _rooms.Remove(room);
                }
            }
        }

        public string GetLobbyJson()
        {
            lock (_lock)
            {
                var summaries = _rooms.Select(r => new RoomSummary
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    CurrentPlayerCount = r.CurrentPlayerCount,
                    IsGameStarted = r.IsGameStarted
                }).ToList();
                return JsonConvert.SerializeObject(summaries);
            }
        }

        public void RemovePlayerFromRoom(ClientSession session, ClientSession opponentSession)
        {
            if (string.IsNullOrEmpty(session.PlayerData.CurrentRoomId)) return;

            lock (_lock)
            {
                RoomInfo room = _rooms.FirstOrDefault(r => r.RoomId == session.PlayerData.CurrentRoomId);
                if (room != null)
                {
                    if (opponentSession != null)
                    {
                        opponentSession.Send("GAME_OVER|OPPONENT_LEFT");
                    }
                    _rooms.Remove(room);
                }
            }
        }

        public string GetOpponentName(string roomId, string myName)
        {
            lock (_lock)
            {
                RoomInfo room = _rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (room == null) return null;
                return (room.PlayerOName == myName) ? room.PlayerXName : room.PlayerOName;
            }
        }
    }
}