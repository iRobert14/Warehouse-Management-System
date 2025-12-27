using InventoryCommon.Models;
using InventoryCommon.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace InventoryClient
{
    class ServerConnection
    {
        private TcpClient _serverSocket;
        public PacketReader PacketReader;

        public event Action<string,string> WelcomeMessage;
        public event Action<List<Product>> ProductsReceived;
        public event Action<string> NotInOurDataBaseMessage;

        public ServerConnection()
        {
            _serverSocket = new TcpClient();
        }

        public void ConnectToServer(string username, string password)
        {

            try
            {
                if (_serverSocket == null || !_serverSocket.Connected)
                {
                    _serverSocket = new TcpClient();
                    _serverSocket.Connect("127.0.0.1", 7891);
                    PacketReader = new PacketReader(_serverSocket.GetStream());

                    Task.Run(() => ListenFromServer());
                }

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var loginPacket = new PacketBuilder();
                    loginPacket.WriteOpCode(1);
                    loginPacket.WriteMessage(username);
                    loginPacket.WriteMessage(password);

                    var data = loginPacket.GetPackageBytes();
                    _serverSocket.GetStream().Write(data, 0, data.Length);


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Server is not opened or the connection was refused!");
                _serverSocket = new TcpClient();
            }

        }
        private void ListenFromServer()
        {
            while (true)
            {
                try
                {

                    byte opcode = PacketReader.ReadOpCode();



                    switch (opcode)
                    {
                        case 2: // Login worked!
                            string welcomeMsg = PacketReader.ReadMessage(); 
                            string role = PacketReader.ReadMessage();        
                            WelcomeMessage?.Invoke(welcomeMsg, role);
                            break;

                        case 3: // Getting products
                            string json = PacketReader.ReadMessage(); 
                            var products = JsonSerializer.Deserialize<List<Product>>(json);
                            ProductsReceived?.Invoke(products); 
                            break;

                        case 99: // Login failed
                            string errorMsg = PacketReader.ReadMessage();
                            NotInOurDataBaseMessage?.Invoke(errorMsg);
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Connection lost!");
                    break;
                }
            }
        }
    }


}

