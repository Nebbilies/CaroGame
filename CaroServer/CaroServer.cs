using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CaroServer
{
    public class CaroServer
    {
        private TcpListener _listener;
        private List<ClientSession> _sessions = new List<ClientSession>();
        private RoomManager _roomManager = new RoomManager();

        public CaroServer(string ip, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            ClientSession session = new ClientSession(client);
            lock (_sessions) { _sessions.Add(session); }

            Console.WriteLine("New Client Connected");

            try
            {
                string line;
                while ((line = session.Receive()) != null)
                {
                    Console.WriteLine($"Received: {line}");
                    ProcessPacket(session, line);
                }
            }
            catch { }
            finally
            {
                HandleDisconnect(session);
            }
        }

        private void ProcessPacket(ClientSession session, string message)
        {
            string[] parts = message.Split('|');
            string command = parts[0];

            switch (command)
            {
                case "LOGIN":
                    if (parts.Length > 1)
                    {
                        session.PlayerData.Name = parts[1];
                        session.Send("LOGIN_SUCCESS");
                        BroadcastLobby();
                        Console.WriteLine($"{session.PlayerData.Name} logged in");
                    }
                    break;

                case "GET_LOBBY":
                    session.Send($"LOBBY_DATA|{_roomManager.GetLobbyJson()}");
                    break;

                case "CREATE_ROOM":
                    if (parts.Length > 1)
                    {
                        string roomName = parts[1];
                        int timePerMove = parts.Length > 2 ? int.Parse(parts[2]) : 30;
                        string roomId = Guid.NewGuid().ToString().Substring(0, 8);
                        _roomManager.CreateRoom(session, roomId, roomName, timePerMove);
                        BroadcastLobby();
                    }
                    break;

                case "JOIN_ROOM":
                    if (parts.Length > 1)
                    {
                        string roomId = parts[1];
                        string hostName = _roomManager.GetOpponentName(roomId, session.PlayerData.Name);
                        ClientSession hostSession = FindHostSession(roomId);

                        _roomManager.JoinRoom(session, roomId, hostSession);
                        BroadcastLobby();
                    }
                    break;

                case "MOVE":
                    if (parts.Length > 3)
                    {
                        string roomId = parts[1];
                        int x = int.Parse(parts[2]);
                        int y = int.Parse(parts[3]);

                        string opponentName = _roomManager.GetOpponentName(roomId, session.PlayerData.Name);
                        ClientSession opponent = FindSessionByName(opponentName);

                        _roomManager.HandleMove(session, roomId, x, y, opponent);
                    }
                    break;
                
                case "REMATCH_REQUEST":
                {
                    string roomId = session.PlayerData.CurrentRoomId;
                    string opponentName = _roomManager.GetOpponentName(roomId, session.PlayerData.Name);
                    ClientSession opponent = FindSessionByName(opponentName);
        
                    if (opponent != null)
                    {
                        opponent.Send("REMATCH_REQUEST|");
                    }
                }
                    break;
                
                case "REMATCH_ACCEPT":
                {
                    string roomId = session.PlayerData.CurrentRoomId;
                    
                    _roomManager.ResetRoom(roomId);
                    
                    ClientSession opponent = FindSessionByName(_roomManager.GetOpponentName(roomId, session.PlayerData.Name));
        
                    session.Send("GAME_RESET|");
                    if (opponent != null) opponent.Send("GAME_RESET|");
                }
                    break;

                case "LEAVE_ROOM":
                    {
                        string roomId = session.PlayerData.CurrentRoomId;
                        if (_roomManager.GetOpponentName(roomId, session.PlayerData.Name) == null)
                        {
                            _roomManager.CancelRoom(session);
                            session.Send("ROOM_CANCELED|");
                        }
                        {
                            ClientSession opponent = FindSessionByName(_roomManager.GetOpponentName(roomId, session.PlayerData.Name));
                            _roomManager.RemovePlayerFromRoom(session, opponent);
                            if (opponent != null)
                            {
                                opponent.Send("OPPONENT_LEFT|");
                            }
                        }
                        BroadcastLobby();
                        break;
                    }
            }
        }

        private void HandleDisconnect(ClientSession session)
        {
            string opponentName = _roomManager.GetOpponentName(session.PlayerData.CurrentRoomId, session.PlayerData.Name);
            ClientSession opponent = FindSessionByName(opponentName);

            _roomManager.RemovePlayerFromRoom(session, opponent);

            session.Close();
            lock (_sessions) { _sessions.Remove(session); }
            BroadcastLobby();
            Console.WriteLine("Client Disconnected");
        }

        private void BroadcastLobby()
        {
            string json = _roomManager.GetLobbyJson();
            lock (_sessions)
            {
                foreach (var s in _sessions)
                {
                    if (string.IsNullOrEmpty(s.PlayerData.CurrentRoomId))
                    {
                        Console.WriteLine($"Broadcasting lobby data to {s.PlayerData.Name}, data: {json}");
                        s.Send($"LOBBY_DATA|{json}\n");
                    }
                }
            }
        }

        private ClientSession FindSessionByName(string name)
        {
            lock (_sessions)
            {
                return _sessions.FirstOrDefault(s => s.PlayerData.Name == name);
            }
        }

        private ClientSession FindHostSession(string roomId)
        {
            lock (_sessions)
            {
                return _sessions.FirstOrDefault(s => s.PlayerData.CurrentRoomId == roomId);
            }
        }
    }
}