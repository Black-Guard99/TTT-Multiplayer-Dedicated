using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using TTT.Server.Network_Shared.Packet_Handlers;
using Network_Shared.Attributes;
using Network_Shared;
using System;

[HandlerRegister(PacketType.OnNewRound)]
public class OnNewRoundHandler : IPacketHandler
{
    public static event Action onNewround;
    public void Handle(INetPacket packet, int connectionId)
    {
        onNewround?.Invoke();
        GameManager.instance.activeGame.ResetGame();
        GameManager.instance.inputsEnable = true;
    }
}