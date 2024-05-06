﻿using LiteNetLib.Utils;
using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Packets.Client_Server
{
    public struct NetPlayAgainRequest : INetPacket
    {
        public PacketType Type => PacketType.PlayAgainRequest;

        public void Deserialize(NetDataReader reader)
        {
            
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
