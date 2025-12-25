using InventoryCommon.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClient
{
    class ServerConnection
    {
        private TcpClient _serverSocket;
        public PacketReader PacketReader;

        public event Action<string>  MessageReceivedEvent;

        public ServerConnection()
        {
            _serverSocket = new TcpClient();
        }

        public void ConnectToServer(string username,string password)
        {
            if (!_serverSocket.Connected)
            {
                _serverSocket.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_serverSocket.GetStream());


                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var loginPacket = new PacketBuilder();
                    loginPacket.WriteOpCode(1);
                    loginPacket.WriteMessage(username);
                    loginPacket.WriteMessage(password);
                    var data = loginPacket.GetPackageBytes();
                    _serverSocket.GetStream().Write(data, 0, data.Length);

                    Task.Run(() => ListenFromServer());

                }
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
                        case 2:
                            string msg = PacketReader.ReadMessage();
                            MessageReceivedEvent?.Invoke(msg);
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

