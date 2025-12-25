using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryCommon.Networking
{
    public class PacketBuilder
    {
      private  MemoryStream _ms = new MemoryStream();

        public PacketBuilder()
        {

        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string message)
        {
            var messageLength = message.Length;

            _ms.Write(BitConverter.GetBytes(messageLength));
            _ms.Write(Encoding.ASCII.GetBytes(message));
        }

        public byte[] GetPackageBytes()
        {
            return _ms.ToArray();
        }
    }
}
