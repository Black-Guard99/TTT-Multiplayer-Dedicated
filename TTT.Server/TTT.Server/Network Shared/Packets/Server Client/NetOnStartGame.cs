using LiteNetLib.Utils;
using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Packets.Server_Client
{
    public class NetOnStartGame : INetPacket
    {
        public PacketType Type => PacketType.OnStartGame;

        public string xUser {  get; set; }
        public string oUser { get; set; }
        public Guid gameId { get; set; }


        public void Deserialize(NetDataReader reader)
        {
            xUser = reader.GetString();
            oUser = reader.GetString();
            gameId = Guid.Parse(reader.GetString());
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(xUser);
            writer.Put(oUser);
            writer.Put(gameId.ToString());
        }
    }
}
