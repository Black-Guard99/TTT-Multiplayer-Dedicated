using Microsoft.Extensions.Logging;
using Network_Shared;
using Network_Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Games;
using TTT.Server.Network_Shared.Packet_Handlers;
using TTT.Server.Network_Shared.Packets.Server_Client;

namespace TTT.Server.Packet_Handlers
{
    [HandlerRegister(PacketType.AcceptPlayAgainRequest)]
    public class AcceptPlayAgainRequestHandler : IPacketHandler {
        private readonly UsersManager usersManager;
        private readonly GamesManager gamesManager;
        private readonly NetworkServer server;
        private readonly ILogger<AcceptPlayAgainRequestHandler> logger;

        public AcceptPlayAgainRequestHandler(UsersManager usersManager,GamesManager gamesManager,NetworkServer server,ILogger<AcceptPlayAgainRequestHandler> logger)
        {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
            this.server = server;
            this.logger = logger;
        }
        public void Handle(INetPacket packet, int connectionId)
        {
            var connnection = usersManager.GetConnection(connectionId);
            var userId = connnection.user.id;
            var game = gamesManager.FindGame(userId);
            game.SetRematchReadiness(userId);
            if (!game.BothPlayerReady())
            {
                logger.LogInformation("All Players Not Ready");
                return;
            }
            game.NewRound();
            var oppentId = game.GetOppoenent(userId);
            var oppoenntConnection = usersManager.GetConnection(oppentId);
            var msg = new NetOnNewRound();
            server.SendClient(connnection.connetionId, msg);
            server.SendClient(oppoenntConnection.connetionId, msg);
            logger.LogInformation($"{connnection.user.id} is Accepting a Rematch.");
            

        }
    }
}
