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
        #endregion

        #region ServerType
        OnAuth = 100,
        OnAuthFaild = 101,
        OnServerStatus = 102,
        OnFindOpponentRequest = 103,
        OnStartGame = 104,
        OnMarkedCell = 105,
        #endregion

    }
    public interface INetPacket : INetSerializable
    {
        PacketType Type { get; }
    }
}
