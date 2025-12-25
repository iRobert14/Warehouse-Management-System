using InventoryCommon.Models;
using InventoryCommon.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InventoryServer.Networking
{
    public class ClientConnection
    {
        TcpClient _clientSocket { get; set; }
        PacketReader _packetReader { get; set; }
        User Identity { get; set; }

        public ClientConnection(TcpClient socket)
        {
            _clientSocket = socket;
            _packetReader = new PacketReader(_clientSocket.GetStream());
            Identity = new User();
            Task.Run(() => Process());


        }
        private void Process()
        {
            while (true)
            {
                try
                {
                    byte opcode = _packetReader.ReadOpCode();

                    switch (opcode)
                    {
                        case 1:
                            HandleLogin();
                            break;

                    }
                }
                catch
                {
                    Console.WriteLine("Client disconnected");
                    break;
                }
            }

        }

        private void HandleLogin()
        {
            Identity.Username = _packetReader.ReadMessage();
            Console.WriteLine($"{Identity.Username} has logged!");

            var welcomePacket = new PacketBuilder();
            welcomePacket.WriteOpCode(2);
            welcomePacket.WriteMessage($"Hello {Identity.Username}, welcome back!");
            byte[] data = welcomePacket.GetPackageBytes();
            _clientSocket.GetStream().Write(data, 0, data.Length);
        }

    }

}
