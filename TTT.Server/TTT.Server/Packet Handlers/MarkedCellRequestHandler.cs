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
using TTT.Server.Utilities;

namespace TTT.Server.Packet_Handlers
{
    [HandlerRegister(PacketType.MarkedCellRequest)]
    public class MarkedCellRequestHandler : IPacketHandler {
        private readonly UsersManager usersManager;
        private readonly GamesManager gamesManager;
        private readonly NetworkServer server;
        private readonly ILogger<MarkedCellRequestHandler> logger;

        public MarkedCellRequestHandler(UsersManager usersManager,GamesManager gamesManager,NetworkServer server,ILogger<MarkedCellRequestHandler> logger)
        {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
            this.server = server;
            this.logger = logger;
        }
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (NetMarkCellRequest)packet;
            var connections = usersManager.GetConnection(connectionId);
            var userId = connections.user.id;
            var game = gamesManager.FindGame(userId);

            // Validate the reqest.
            Validate(msg.index,userId, game);
            // get the game and Invoke game.MarkedCell() and get outcome.
            var result = game.MarkeCell(msg.index);

            var rmsg = new NetOnMarkedCell
            {
                actor = userId,
                index = msg.index,
                markedOutCome = result.MarkedOutCome,
                winLineType = result.LineType
            };
            var oppentId = game.GetOppoenent(userId);
            var oppoenntConnection = usersManager.GetConnection(oppentId);
            server.SendClient(connections.connetionId, rmsg);
            server.SendClient(oppoenntConnection.connetionId, rmsg);

            logger.LogInformation($"{userId} marked cell at index {msg.index}");
            // based on the Outcome do the Following:
            if(result.MarkedOutCome == Network_Shared.Models.MarkedOutCome.None)
            {
                // None > SwitchPlayerTurn();
                game.SwitchCurrentPlayer();
                return;
            }
            if(result.MarkedOutCome == Network_Shared.Models.MarkedOutCome.Win)
            {
                // win ? Increase Player score and Add Win.
                game.AddWin(userId);
                usersManager.IncreaseScore(userId);
                logger.LogInformation($"{userId} is the Winner.");

            }
            // Draw ? Do Noting.
        }

        private void Validate(byte index, string userId, Game game)
        {
            if(game.currentUser != userId)
            {
                //throw new ArgumentException($"[Bad Reqwust] acto '{userId}' isn not the current User");
                logger.LogError($"[Bad Request] acto '{userId}' isn not the current User");
                //return;
            }
            var (row, col) = BasicsExtensions.GetRowCol(index);
            if (game.grid[0,0] != 0) {
                //throw new ArgumentException($"[Bad Reqwust] row '{row}' and col {col} is Already Marked");
                logger.LogError($"[Bad Request] row '{row}' and col {col} is Already Marked");
                //return;
            }
        }
    }
}
