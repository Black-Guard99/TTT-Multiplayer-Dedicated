using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;

namespace TTT.Server.Games {


    public class UsersManager {

        private Dictionary<int, ServerConnection> connections = new Dictionary<int, ServerConnection>();
        private readonly IUserRepository userRepository;
        public UsersManager(IUserRepository usersRepos) {
            connections = new Dictionary<int, ServerConnection>();
            userRepository = usersRepos;
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

        public void DisConnect(int peerId) {
            var connection = GetConnection(peerId);
            if(connection.user  != null)
            {
                var userId = connection.user.id;
                userRepository.SetOffline(userId);

                // Match Macking Unregseter.

                // gameManager CloseGame...........
            }
            connections.Remove(peerId);
        }

        public ServerConnection GetConnection(int id) {
            return connections[id];
        }
        public int[] GetOtherConnectionsIds(int excludedconnectionId) {
            return connections.Keys.Where(y => y != excludedconnectionId).ToArray();
        }
    }
}
