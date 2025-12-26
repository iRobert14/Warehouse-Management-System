using InventoryServer.Networking;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;

namespace InventoryServer
{
    class Program
    {
        static TcpListener _listener;
        private static List<ClientConnection> _connections;
        public static string ConnectionString { get; private set; }


       
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            ConnectionString = config.GetConnectionString("DefaultConnection");


            _connections = new List<ClientConnection>();

            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            Console.WriteLine("--- SERVER INVENTAR ACTIV ---");

            while (true)
            {

                TcpClient client = _listener.AcceptTcpClient();
                var newConnection = new ClientConnection(client);

                Console.WriteLine($"Connected! Clients online: {_connections.Count}");


            }

        }


        public static void AddToAuthenticatedClients(ClientConnection client)
        {
            _connections.Add(client);
            Console.WriteLine($"People logged: {_connections.Count}");
        }
    }
}