using LiteNetLib;
using System;
using TTT.Server.Data;

namespace TTT.Server.Games {
    public class ServerConnection {
        public int connetionId { get; set; }
        public User user { get; set; }
        public NetPeer peer { get; set; }
        public Guid gameId { get; set; }
    }
}
