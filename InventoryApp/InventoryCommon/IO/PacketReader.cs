using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InventoryCommon.Networking
{
    public class PacketReader : BinaryReader
    {
        private NetworkStream _ns;

        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public byte ReadOpCode()
        {
            return ReadByte();
        }

        public string ReadMessage()
        {
            int length = ReadInt32();

            byte[] msgBuffer = ReadBytes(length);

            return Encoding.ASCII.GetString(msgBuffer);

        }


    }
}
