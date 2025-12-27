using InventoryCommon.Models;
using InventoryCommon.Networking;
using InventoryServer.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Client disconnected. Reason: {ex.Message}");
                    break;
                }
            }

        }

        private void HandleLogin()
        {
            string username = _packetReader.ReadMessage();
            string rawPassword = _packetReader.ReadMessage();
            string hashedPassword = SecurityHelper.ComputeHash(rawPassword);

            User authenticateUser = DataBaseManager.Authenticate(username, hashedPassword);

            var responsePacket = new PacketBuilder();

            if (authenticateUser != null)
            {
                this.Identity = authenticateUser;

                Console.WriteLine($"[SUCCESS] {Identity.Username} ({Identity.UserRole}) logged in.");
                Console.WriteLine($"Hash used: {hashedPassword}");

                responsePacket.WriteOpCode(2);
                responsePacket.WriteMessage($"Hello {Identity.Username}, welcome back!");
                responsePacket.WriteMessage(Identity.UserRole.ToString());


                Program.AddToAuthenticatedClients(this);

                this.SendProductList();



            }
            else
            {
                Console.WriteLine($"[FAILED] Login attempt: {username}");

                responsePacket.WriteOpCode(99);
                responsePacket.WriteMessage($"ERROR: Username or password WRONG!");
            }

            byte[] data = responsePacket.GetPackageBytes();
            _clientSocket.GetStream().Write(data, 0, data.Length);
        }


        private void SendProductList()
        {
            var products = DataBaseManager.GetAllProducts();
            string jsonProdutcs = JsonSerializer.Serialize(products);
            var responsePacket = new PacketBuilder();
            responsePacket.WriteOpCode(3);
            responsePacket.WriteMessage(jsonProdutcs);
            _clientSocket.GetStream().Write(responsePacket.GetPackageBytes(), 0, responsePacket.GetPackageBytes().Length);

        }

    }

}
