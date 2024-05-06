using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Games;
using TTT.Server.Network_Shared.Packets.Server_Client;

namespace TTT.Server.Matchmaking
{
    public class MatchMakingManager {
        private readonly ILogger<MatchMakingManager> logger;
        private readonly GamesManager gamesManager;
        private readonly NetworkServer server;
        private List<MMRequest> pool = new List<MMRequest>();

        public MatchMakingManager(ILogger<MatchMakingManager> logger,GamesManager gamesManager,NetworkServer server) {
            this.logger = logger;
            this.gamesManager = gamesManager;
            this.server = server;
        }
        public void RegisterPlayer(ServerConnection connectionToRegister)
        {
            if(pool.Any(x => x.connections.user.id == connectionToRegister.user.id))
            {
                logger.LogWarning($"{connectionToRegister.user.id} is Already Register");
                return;
            }
            var request = new MMRequest
            {
                connections = connectionToRegister,
                serachStartDT = DateTime.UtcNow
            };
            pool.Add(request);

            logger.LogInformation($"{request.connections.user.id} register in the Matchmaking Pool");
            DoMatchMaking();
        }

        public void TryUnregisterPlayer(string userName)
        {
            var request = pool.FirstOrDefault(r => r.connections.user.id == userName);
            if(request != null)
            {
                logger.LogInformation($"{request.connections.user.id} Removed from Matchmaking Pool");
                pool.Remove(request);
            }
        }

        private void DoMatchMaking() {
            // set MatchMaking....
            var matchedRequest = new List<MMRequest>();
            foreach(var request in pool)
            {
                var match = pool.FirstOrDefault(x => !x.matchFound && 
                    x.connections.connetionId != request.connections.connetionId);
                if(match == null)
                {
                    continue;
                }
                request.matchFound = true;
                match.matchFound = false;
                matchedRequest.Add(request);
                matchedRequest.Add(match);

                var xUser = request.connections.user.id;
                var oUser = match.connections.user.id;

                var gameId = gamesManager.RegisterGame(xUser,oUser);
                request.connections.gameId = gameId;
                match.connections.gameId = gameId;

                // send NetOnStartGame();
                var msg = new NetOnStartGame
                {
                    gameId = gameId,
                    xUser = request.connections.user.id,
                    oUser = match.connections.user.id,
                };
                var p1 = request.connections.connetionId;
                var p2 = match.connections.connetionId;


                server.SendClient(p1,msg);
                server.SendClient(p2, msg);


                logger.LogInformation($"Match Found Players : {request.connections.user.id} and {match.connections.user.id}");
            }
            foreach(var request in matchedRequest)
            {
                pool.Remove(request);
            }
        }
    }
}
