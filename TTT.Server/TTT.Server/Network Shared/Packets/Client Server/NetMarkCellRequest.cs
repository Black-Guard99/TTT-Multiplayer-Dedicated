using LiteNetLib.Utils;
using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Packets.Client_Server
{
    public class NetMarkCellRequest : INetPacket
    {
        public PacketType Type => PacketType.MarkedCellRequest;
        public byte index { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            index = reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(index);
        }
    }
}
