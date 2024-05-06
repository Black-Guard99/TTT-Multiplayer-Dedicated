using Network_Shared;
using Network_Shared.Attributes;
using System;
using TTT.Server.Games;
using TTT.Server.Matchmaking;
using TTT.Server.Network_Shared.Packet_Handlers;

namespace TTT.Server.Packet_Handlers {
    [HandlerRegister(PacketType.FindOpponentRequest)]
    public class FindOpponentRequestHandler : IPacketHandler {
        private readonly UsersManager usersManager;
        private readonly MatchMakingManager matchMakingManager;

        public FindOpponentRequestHandler(UsersManager usersManager,MatchMakingManager matchMakingManager)
        {
            this.usersManager = usersManager;
            this.matchMakingManager = matchMakingManager;
        }
        public void Handle(INetPacket packet, int connectionId) {
            Console.WriteLine($"Requested To Find Opponent");
            var connection = usersManager.GetConnection(connectionId);
            matchMakingManager.RegisterPlayer(connection);
        }
    }
}
