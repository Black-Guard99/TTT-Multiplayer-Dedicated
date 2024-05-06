using Network_Shared;
using Network_Shared.Attributes;
using System;
using TTT.Server.Network_Shared.Packet_Handlers;

namespace TTT.Server.Packet_Handlers
{
    [HandlerRegister(PacketType.CancleFindOpponentRequest)]
    public class CancleFindOpponentRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            Console.WriteLine($"Requested To Find Opponent");
        }
    }
}
