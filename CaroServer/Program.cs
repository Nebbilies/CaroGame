namespace CaroServer
{
    class Program
    {
        static void Main(string[] args)
        {
            CaroServer server = new CaroServer("0.0.0.0", 8888);
            server.Start();
        }
    }
}
