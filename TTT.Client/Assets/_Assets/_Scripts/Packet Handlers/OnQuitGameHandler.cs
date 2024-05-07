using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using TTT.Server.Network_Shared.Packet_Handlers;
using Network_Shared.Attributes;
using Network_Shared;
using TTT.Server.Network_Shared.Packets.Server_Client;
using UnityEngine.SceneManagement;
using System;

[HandlerRegister(PacketType.OnQuitGameRequest)]
public class OnQuitGameHandler : IPacketHandler {
    public static event Action<NetOnQuitGame> onQuitGame;
    public void Handle(INetPacket packet, int connectionId) {
        var msg = (NetOnQuitGame)packet;
        if(GameManager.instance.myUserName == msg.quitter){
            SceneManager.LoadScene(1);
            return;
        }
        onQuitGame?.Invoke(msg);

    }
}