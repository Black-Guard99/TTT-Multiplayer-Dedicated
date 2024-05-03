using LiteNetLib;
using Microsoft.Extensions.Logging;
using Network_Shared;
using Network_Shared.Attributes;
using Network_Shared.Packets.ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Games;
using TTT.Server.Network_Shared.Packets.Server_Client;
using TTT.Server.Network_Shared.Registery;
using TTT.Server.Network_Shared.Packet_Handlers;

namespace TTT.Server.Packet_Handlers {

    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler {
        private readonly ILogger<AuthRequestHandler> _logger;
        private readonly UsersManager usersManager;
        private readonly NetworkServer server;
        public AuthRequestHandler(ILogger<AuthRequestHandler> logger, UsersManager usersManager,NetworkServer server) {
            _logger = logger;
            this.usersManager = usersManager;
            this.server = server;
        }
        public void Handle(INetPacket packet, int connectionId) {
            var message = (NetAuthRequest)packet;

            _logger.LogInformation($"Recived Login Request for User : {message.Username} with pass: {message.Password}");
            var logInSuccess = usersManager.LogInOrResgister(connectionId,message.Username,message.Password);
            INetPacket rmsfg;
            if(logInSuccess)
            {
                rmsfg = new NetOnAuth();
            }
            else
            {
                rmsfg = new NetOnAuthFail();
            }
            server.SendClient(connectionId, rmsfg);

            if (logInSuccess) {
                NotifiOtherPlayer(connectionId);
            }
            // Logging..
            // Logging Registery
            // if Success > send Back Net OAuth Message.
            // else > send Back Net OAuth Message.
        }

        private void NotifiOtherPlayer(int excludedconnectionId) {
            // TODO : Impletemt Fully........
            var rsmg = new NetOnServerStatus();
            var otherIds = usersManager.GetOtherConnectionsIds(excludedconnectionId);

            foreach (var otherId in otherIds)
            {
                server.SendClient(otherId,rsmg);
            }

        }
    }
}
