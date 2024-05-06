using LiteNetLib.Utils;
using Network_Shared;
using Network_Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Network_Shared.Models;
using TTT.Server.Network_Shared.Packet_Handlers;

namespace TTT.Server.Network_Shared.Packets.Server_Client
{
    
    public struct NetOnMarkedCell : INetPacket
    {
        public PacketType Type => PacketType.OnMarkedCell;
        public string actor {  get; set; }
        public byte index { get; set; }


        public MarkedOutCome markedOutCome { get; set; }

        public WinLineType winLineType { get; set; }
        public void Deserialize(NetDataReader reader)
        {
            actor = reader.GetString();
            index = reader.GetByte();
            markedOutCome = (MarkedOutCome)reader.GetByte();
            winLineType = (WinLineType)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(actor);
            writer.Put(index);
            writer.Put((byte)markedOutCome);
            writer.Put((byte)winLineType);
        }
    }
}
