using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CaroClient
{
    public class SocketManager
    {
        public static SocketManager Instance = new SocketManager();

        public TcpClient Client { get; private set; }
        public NetworkStream Stream { get; private set; }
        public StreamReader Reader { get; private set; } 
        public string PlayerName { get; set; }

        public bool Connect(string ip, int port)
        {
            try
            {
                Client = new TcpClient();
                Client.Connect(ip, port);
                Stream = Client.GetStream();
                Reader = new StreamReader(Stream, Encoding.UTF8, true, 1024, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Send(string data)
        {
            if (Stream != null)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data + "\n");
                Stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}