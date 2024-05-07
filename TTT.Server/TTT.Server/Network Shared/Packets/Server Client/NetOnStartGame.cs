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
        public int xScore {  get; set; }
        public int oScore { get; set; }
        public Guid gameId { get; set; }


        public void Deserialize(NetDataReader reader)
        {
            xUser = reader.GetString();
            oUser = reader.GetString();
            xScore = (int)reader.GetByte();
            oScore = (int)reader.GetByte();
            gameId = Guid.Parse(reader.GetString());
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(xUser);
            writer.Put(oUser);
            writer.Put((byte)xScore);
            writer.Put((byte)oScore);
            writer.Put(gameId.ToString());
        }
    }
}
