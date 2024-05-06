using System.Collections;
using System.Collections.Generic;
using Network_Shared;
using LiteNetLib.Utils;

public struct NetServerStatusRequest : INetPacket {
    public PacketType Type => PacketType.ServerStatusRequest;

    public void Deserialize(NetDataReader reader) {
        
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put((byte)Type);
    }
}