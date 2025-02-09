using LiteNetLib.Utils;
using Network_Shared;
namespace Network_Shared.Packets.ClientServer
{
    public struct NetAuthRequest : INetPacket {
        public PacketType Type => PacketType.AuthRequest;

        public string Username { get; set; }
        public string Password { get; set; }

        public void Deserialize(NetDataReader reader) {
            Username = reader.GetString();
            Password = reader.GetString();
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
            writer.Put(Username);
            writer.Put(Password);

        }
    }
}
