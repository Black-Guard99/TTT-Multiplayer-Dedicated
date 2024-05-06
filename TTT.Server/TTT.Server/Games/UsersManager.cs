using LiteNetLib;
using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;
using TTT.Server.Network_Shared.Packets.Server_Client;

namespace TTT.Server.Games {


    public class UsersManager {

        private Dictionary<int, ServerConnection> connections = new Dictionary<int, ServerConnection>();
        private readonly IUserRepository userRepository;
        private readonly NetworkServer server;

        public UsersManager(IUserRepository usersRepos,NetworkServer server) {
            connections = new Dictionary<int, ServerConnection>();
            userRepository = usersRepos;
            this.server = server;
        }
        public void AddConnections(NetPeer peer)
        {
            connections.Add(peer.Id, new ServerConnection
            {
                connetionId = peer.Id,
                peer = peer
            });
        }
        public bool LogInOrResgister(int connectionId,string username,string Password) {
            var dbUser = userRepository.Get(username);
            if(dbUser != null) {
                if(dbUser.password != Password)
                {
                    return false;// user Present but passward is wrong
                }
            }
            else
            {
                var newUser = new User
                {
                    id = username,
                    password = Password,
                    IsOnline = true,
                    score = 0,
                };
                userRepository.Add(newUser);
                dbUser = newUser;
            }
            if (connections.ContainsKey(connectionId))
            {
                dbUser.IsOnline = true;// incase is User Offline.
                connections[connectionId].user = dbUser;
            }
            return true;
        }

        public void Disconnect(int peerId) {
            var connection = GetConnection(peerId);
            if (connection.user != null)
            {
                var userId = connection.user.id;
                userRepository.SetOffline(userId);
                //userRepository.SetOffline(userId);

                // Match Macking Unregseter.

                // gameManager CloseGame...........


                NotifiOtherPlayer(peerId);
            }
            connections.Remove(peerId);
        }

        public ServerConnection GetConnection(int peerId) {
            return connections[peerId];
        }
        public ServerConnection GetConnection(string userId)
        {
            return connections.FirstOrDefault(x => x.Value.user.id == userId).Value;
        }
        public int[] GetOtherConnectionsIds(int excludedconnectionId) {
            return connections.Keys.Where(y => y != excludedconnectionId).ToArray();
        }

        public PlayerNetDTO[] GetTopPlayers()
        {
            return userRepository.GetQuerry().OrderByDescending(x => x.score).Select(u=> new PlayerNetDTO
            {
                userName = u.id,
                score = u.score,
                isOnline = u.IsOnline,
            }).Take(9).ToArray();
        }
        private void NotifiOtherPlayer(int excludedconnectionId)
        {
            // TODO : Impletemt Fully........
            var rsmg = new NetOnServerStatus
            {
                playerCount = userRepository.GetTotalCount(),
                topPlayer = GetTopPlayers()
            };
            var otherIds = GetOtherConnectionsIds(excludedconnectionId);

            foreach (var otherId in otherIds)
            {
                server.SendClient(otherId, rsmg);
            }

        }

        public void IncreaseScore(string userId)
        {
            var user = userRepository.Get(userId);
            user.score = 10;
            userRepository.Update(user);
        }
    }
}
