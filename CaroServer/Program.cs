namespace CaroServer
{
    class Program
    {
        static void Main(string[] args)
        {
            CaroServer server = new CaroServer("127.0.0.1", 8888);
            server.Start();
        }
    }
}
