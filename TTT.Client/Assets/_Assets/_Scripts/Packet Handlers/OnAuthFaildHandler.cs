using Network_Shared;
using Network_Shared.Attributes;
using System;
using TTT.Server.Network_Shared.Packet_Handlers;
using TTT.Server.Network_Shared.Packets.Server_Client;
using UnityEngine;

namespace TTT.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuthFaild)]
    public class OnAuthFaildHandler : IPacketHandler {
        public static event Action<NetOnAuthFail> OnAuthFail;
        public void Handle(INetPacket packet, int connectionId) {
            // On Auth Failed............
            var msg = (NetOnAuthFail)packet;
            OnAuthFail?.Invoke(msg);
        }
    }
}