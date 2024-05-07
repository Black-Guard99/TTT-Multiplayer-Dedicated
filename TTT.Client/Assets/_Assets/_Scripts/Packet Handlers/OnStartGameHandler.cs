using TTT.Server.Network_Shared.Packet_Handlers;
using Network_Shared;
using Network_Shared.Attributes;
using TTT.Server.Network_Shared.Packets.Server_Client;
using UnityEngine.SceneManagement;


[HandlerRegister(PacketType.OnStartGame)]
public class OnStartGameHandler : IPacketHandler {
    public void Handle(INetPacket packet, int connectionId) {
        var msg = (NetOnStartGame)packet;
        GameManager.instance.RegisterGame(msg.gameId,msg.xUser,msg.oUser,msg.xScore,msg.oScore);
        SceneManager.LoadScene(2);// Game Scene.
    }
}