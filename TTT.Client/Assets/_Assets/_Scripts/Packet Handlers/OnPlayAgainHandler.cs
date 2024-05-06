using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using TTT.Server.Network_Shared.Packet_Handlers;
using Network_Shared;
using Network_Shared.Attributes;
using System;
using TTT.Server.Network_Shared.Packets.Server_Client;

[HandlerRegister(PacketType.OnPlayAgainRequest)]
public class OnPlayAgainHandler : IPacketHandler
{
    public static event Action<OnNetPlayAgain> onPlayAgain;
    public void Handle(INetPacket packet, int connectionId)
    {
        var msg = (OnNetPlayAgain)packet;
        Debug.Log("Oppoennt Request Rematched");
        onPlayAgain?.Invoke(msg);
    }
}