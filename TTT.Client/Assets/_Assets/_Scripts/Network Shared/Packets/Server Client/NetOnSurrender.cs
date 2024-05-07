using LiteNetLib.Utils;
using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Packets.Server_Client
{
    public struct NetOnSurrender : INetPacket
    {
        public PacketType Type => PacketType.OnSurrender;

        public string winner { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            winner = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(winner);
        }
    }
}
