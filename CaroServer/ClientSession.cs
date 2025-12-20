using CaroLibrary.CaroLibrary;
using System.IO;
using System.Net.Sockets;

namespace CaroServer
{
    public class ClientSession
    {
        public Player PlayerData { get; private set; }
        private StreamReader _reader;
        private StreamWriter _writer;

        public ClientSession(TcpClient client)
        {
            PlayerData = new Player
            {
                Client = client,
                Stream = client.GetStream(),
                Name = "Unknown"
            };
            _reader = new StreamReader(PlayerData.Stream);
            _writer = new StreamWriter(PlayerData.Stream) { AutoFlush = true };
        }

        public void Send(string message)
        {
            try
            {
                if (PlayerData.Client.Connected)
                {
                    _writer.WriteLine(message);
                }
            }
            catch { }
        }

        public string Receive()
        {
            try
            {
                return _reader.ReadLine();
            }
            catch
            {
                return null;
            }
        }

        public void Close()
        {
            PlayerData.Client?.Close();
        }
    }
}