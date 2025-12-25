using InventoryServer.Networking;
using System.Net;
using System.Net.Sockets;

namespace InventoryServer
{
    class Program
    {
        static TcpListener _listener;
        private static List<ClientConnection> _connections;
        static void Main(string[] args)
        {
            _connections = new List<ClientConnection>();

            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                Console.Write("Waiting for a connection...");
                TcpClient client = _listener.AcceptTcpClient();

                var newConnection = new ClientConnection(client);
                _connections.Add(newConnection);
                

                Console.WriteLine($"Connected! Clients online: {_connections.Count}");

            }

        }
    }
}