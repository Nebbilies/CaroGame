using System;
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
        private readonly object _sendLock = new object();


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
                lock (_sendLock)
                {
                    if (PlayerData.Client == null || !PlayerData.Client.Connected)
                        return;

                    _writer.WriteLine(message);
                    _writer.Flush();
                }

                Console.WriteLine($"[Server -> {PlayerData.Name}] {message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO error sending to {PlayerData.Name}: {ex.Message}");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine($"Stream disposed for {PlayerData.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected send error to {PlayerData.Name}: {ex}");
            }
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