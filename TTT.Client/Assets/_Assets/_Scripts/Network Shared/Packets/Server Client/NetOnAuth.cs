using LiteNetLib.Utils;
using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Packets.Server_Client {
    public struct NetOnAuth : INetPacket {
        public PacketType Type => PacketType.OnAuth;

        public void Deserialize(NetDataReader reader) {
            
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
        }
    }
}
