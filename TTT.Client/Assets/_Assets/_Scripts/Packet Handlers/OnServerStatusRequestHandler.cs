using System;
using Network_Shared;
using Network_Shared.Attributes;
using TTT.Server.Network_Shared.Packet_Handlers;
using TTT.Server.Network_Shared.Packets.Server_Client;
using UnityEngine.SceneManagement;



[HandlerRegister(PacketType.OnServerStatus)]
public class OnServerStatusRequestHandler : IPacketHandler {

    public static Action<NetOnServerStatus> OnServerStatus;
    public void Handle(INetPacket packet, int connectionId) {
        if(SceneManager.GetActiveScene().buildIndex != 1)    {
            return;
        }
        var msg = (NetOnServerStatus)packet;
        OnServerStatus?.Invoke(msg);
    }
}