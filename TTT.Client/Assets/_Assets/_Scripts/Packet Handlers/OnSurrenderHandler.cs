using TTT.Server.Network_Shared.Packet_Handlers;
using Network_Shared.Attributes;
using Network_Shared;
using System;
using TTT.Server.Network_Shared.Packets.Server_Client;

[HandlerRegister(PacketType.OnSurrender)]
public class OnSurrenderHandler : IPacketHandler {
    public static event Action<NetOnSurrender> onSurrender;
    public void Handle(INetPacket packet, int connectionId) {
        var msg = (NetOnSurrender)packet;
        onSurrender?.Invoke(msg);
    }
}