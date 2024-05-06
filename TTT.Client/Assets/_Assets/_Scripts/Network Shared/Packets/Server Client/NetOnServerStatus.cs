using LiteNetLib.Utils;
using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Packets.Server_Client
{
    public struct NetOnServerStatus : INetPacket
    {
        public PacketType Type => PacketType.OnServerStatus;
        public ushort playerCount { get; set; }

        public PlayerNetDTO[] topPlayer { get; set; }


        public void Deserialize(NetDataReader reader)
        {
            playerCount = reader.GetUShort();

            var playerLength = reader.GetUShort();

            topPlayer = new PlayerNetDTO[playerLength];
            for (int i = 0; i < playerLength; i++) {
                topPlayer[i] = reader.Get<PlayerNetDTO>();
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);

            writer.Put(playerCount);
            
            writer.Put((ushort)topPlayer.Length);
            for (int i = 0; i < topPlayer.Length; i++) {
                writer.Put(topPlayer[i]);
            }
        }
    }


    public struct PlayerNetDTO : INetSerializable
    {

        public string userName { get; set; }
        public ushort score { get; set; }
        public bool isOnline { get; set; }
        public void Deserialize(NetDataReader reader)
        {
            userName = reader.GetString();
            score = reader.GetUShort();
            isOnline = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(userName);
            writer.Put(score);
            writer.Put(isOnline);
        }
    }
}
