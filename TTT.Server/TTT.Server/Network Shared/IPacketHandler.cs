using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Packet_Handlers
{
    public interface IPacketHandler {
        void Handle(INetPacket packet,int connectionId);
    }
}
