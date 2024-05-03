using Network_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Network_Shared.Registery
{
    public class PacketRegistery {
        private Dictionary<PacketType, Type> _packetTypes = new Dictionary<PacketType, Type>();
        public Dictionary<PacketType,Type> PacketTypes
        {
            get
            {
                if(_packetTypes.Count == 0)
                {
                    Initalize();
                }
                return _packetTypes;
            }
        }
        private void Initalize()
        {
            var packetTypes = typeof(INetPacket);
            var packets = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => packetTypes.IsAssignableFrom(p) && !p.IsInterface);
            foreach (var packet in packets) {
                var instance = (INetPacket)Activator.CreateInstance(packet);
                _packetTypes.Add(instance.Type, packet);
            }

        }

    }
}
