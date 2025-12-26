using InventoryCommon.Models;
using InventoryCommon.Networking;
using InventoryServer.Security;
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
            string username = _packetReader.ReadMessage();
            string rawPassword = _packetReader.ReadMessage();
            string hashedPassword = SecurityHelper.ComputeHash(rawPassword);

            bool isValid = DataBaseManager.CheckUser(username, hashedPassword);

            var responsePacket = new PacketBuilder();

            if (isValid)
            {
                Identity.Username = username;
                Identity.PasswordHash = hashedPassword;

                Console.WriteLine($"[Login] {Identity.Username} connected with Hash: {Identity.PasswordHash}");
                responsePacket.WriteOpCode(2);
                responsePacket.WriteMessage($"Hello {Identity.Username}, welcome back!");


                Program.AddToAuthenticatedClients(this);



            }
            else
            {
                Console.WriteLine($"[Login FAILED] {username} couldnt login!");

                responsePacket.WriteOpCode(99);
                responsePacket.WriteMessage($"ERROR: Username or password WRONG!");
            }





            byte[] data = responsePacket.GetPackageBytes();
            _clientSocket.GetStream().Write(data, 0, data.Length);
        }

    }

}
