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
    [HandlerRegister(PacketType.SurrenderRequest)]
    public class SurrenderRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GamesManager gamesManager;
        private readonly NetworkServer server;
        private readonly ILogger<SurrenderRequestHandler> logger;

        public SurrenderRequestHandler(UsersManager usersManager,GamesManager gamesManager,NetworkServer server,ILogger<SurrenderRequestHandler> logger) {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
            this.server = server;
            this.logger = logger;
        }
        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = usersManager.GetConnection(connectionId);
            var game = gamesManager.FindGame(connection.user.id);
            var opponentId = game.GetOppoenent(connection.user.id);

            game.AddWin(opponentId);

            usersManager.IncreaseScore(opponentId);

            var rmsg = new NetOnSurrender
            {
                winner = opponentId,
            };
            var opponentConnection = usersManager.GetConnection(opponentId);
            server.SendClient(opponentConnection.connetionId, rmsg);
            server.SendClient(connectionId, rmsg);
            logger.LogInformation($"sending surrender message to {opponentConnection.connetionId} and {connection.user.id}");

        }
    }
}
