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
    [HandlerRegister(PacketType.PlayAgainRequest)]
    public class PlayAgainRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GamesManager gamesManager;
        private readonly NetworkServer server;

        public PlayAgainRequestHandler(UsersManager usersManager,GamesManager gamesManager,NetworkServer server)
        {
            this.usersManager = usersManager;
            this.gamesManager = gamesManager;
            this.server = server;
        }
        public void Handle(INetPacket packet, int connectionId)
        {
            var connectionsUser = usersManager.GetConnection(connectionId);
            var userId = connectionsUser.user.id;
            var game = gamesManager.FindGame(userId);
            game.SetRematchReadiness(userId);


            var rmsg = new OnNetPlayAgain();
            var oppentId = game.GetOppoenent(userId);
            var oppoenntConnection = usersManager.GetConnection(oppentId);
            server.SendClient(oppoenntConnection.connetionId,rmsg);
            
        }
    }
}
