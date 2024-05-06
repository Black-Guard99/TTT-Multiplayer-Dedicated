using Network_Shared;
using Network_Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;
using TTT.Server.Games;
using TTT.Server.Network_Shared.Packet_Handlers;
using TTT.Server.Network_Shared.Packets.Server_Client;

namespace TTT.Server.Packet_Handlers {
    [HandlerRegister(PacketType.ServerStatusRequest)]
    public class ServerRequestStatusHandler : IPacketHandler {
        private readonly UsersManager usersManager;
        private readonly NetworkServer server;
        private readonly IUserRepository userRepository;

        public ServerRequestStatusHandler(UsersManager usersManager,NetworkServer server,IUserRepository userRepository) {
            this.usersManager = usersManager;
            this.server = server;
            this.userRepository = userRepository;
        }
        public void Handle(INetPacket packet, int connectionId) {
            var rmsg = new NetOnServerStatus
            {
                playerCount = userRepository.GetTotalCount(),
                topPlayer = usersManager.GetTopPlayers(),
            };


            server.SendClient(connectionId, rmsg);
        }
    }
}
