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
using TTT.Server.Network_Shared.Packets.Client_Server;
using TTT.Server.Network_Shared.Packets.Server_Client;

namespace TTT.Server.Packet_Handlers
{
    [HandlerRegister(PacketType.QuitGameRequest)]
    public class QuitGameRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GamesManager gamesManager;
        private readonly NetworkServer server;
        private readonly ILogger<QuitGameRequestHandler> logger;

        public QuitGameRequestHandler(UsersManager usersManager,GamesManager gamesManager,NetworkServer server,ILogger<QuitGameRequestHandler> logger)
        {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
            this.server = server;
            this.logger = logger;
        }
        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = usersManager.GetConnection(connectionId);
            var rmsg = new NetOnQuitGame
            {
                quitter = connection.user.id,
            };
            if(gamesManager.GameExist(connection.user.id))
            {
                var closedGame = gamesManager.ClosedGame(connection.user.id);
                var opponent = closedGame.GetOppoenent(connection.user.id);
                var opponenetConnection = usersManager.GetConnection(opponent);
                server.SendClient(opponenetConnection.connetionId, rmsg);
            }
            server.SendClient(connection.connetionId, rmsg);
            logger.LogInformation($"{connection.user.id} Quit the Game");
        }
    }
}
