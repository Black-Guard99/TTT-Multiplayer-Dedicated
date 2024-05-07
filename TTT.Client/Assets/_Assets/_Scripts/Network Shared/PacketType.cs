using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Shared
{
    public enum PacketType : byte
    {
        #region Client
        Invalid = 0,
        AuthRequest = 1,
        ServerStatusRequest = 2,
        FindOpponentRequest = 3,
        CancleFindOpponentRequest = 4,
        MarkedCellRequest = 5,
        PlayAgainRequest = 6,
        AcceptPlayAgainRequest = 7,
        SurrenderRequest = 8,
        QuitGameRequest = 9,
        #endregion

        #region ServerType
        OnAuth = 100,
        OnAuthFaild = 101,
        OnServerStatus = 102,
        OnFindOpponentRequest = 103,
        OnStartGame = 104,
        OnMarkedCell = 105,
        OnPlayAgainRequest = 106,
        OnNewRound = 107,
        OnSurrender = 108,
        OnQuitGameRequest = 109,
        #endregion

    }
    public interface INetPacket : INetSerializable
    {
        PacketType Type { get; }
    }
}
