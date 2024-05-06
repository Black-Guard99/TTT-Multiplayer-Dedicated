using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using TTT.Server.Network_Shared.Packet_Handlers;
using Network_Shared;
using Network_Shared.Attributes;
using System;
using TTT.Server.Network_Shared.Packets.Server_Client;
using TTT.Server.Network_Shared.Models;

[HandlerRegister(PacketType.OnMarkedCell)]
public class OnMarkedCellHandler : IPacketHandler {
    public static event Action<NetOnMarkedCell> onMarkedCell;
    public void Handle(INetPacket packet, int connectionId) {
        var msg = (NetOnMarkedCell)packet;
        GameManager.instance.activeGame.SwithCurrentPlayer();
        Debug.Log("Game Outcome " + msg.markedOutCome.ToString());
        if(GameManager.instance.IsMyTurn && msg.markedOutCome == MarkedOutCome.None){
            GameManager.instance.inputsEnable = true;
            Debug.Log("Input enable For " + msg.actor);
        }
        if(msg.markedOutCome > MarkedOutCome.None){
            GameManager.instance.inputsEnable = false;
            Debug.Log("Input Desable For EveryOne");

        }
        onMarkedCell?.Invoke(msg);
    }
}