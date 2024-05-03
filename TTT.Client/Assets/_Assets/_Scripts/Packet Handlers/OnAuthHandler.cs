using UnityEngine;
using Network_Shared;
using System.Collections;
using System.Collections.Generic;
using TTT.Server.Network_Shared.Packet_Handlers;
using Network_Shared.Attributes;
using UnityEngine.SceneManagement;

namespace TTT.Client.PacketHandlers {
    [HandlerRegister(PacketType.OnAuth)]
    public class OnAuthHandler : IPacketHandler {
        public void Handle(INetPacket packet, int connectionId)
        {
            Debug.Log("On Auth Handler Triggerd ");
            SceneManager.LoadScene(1);
        }
    }
}