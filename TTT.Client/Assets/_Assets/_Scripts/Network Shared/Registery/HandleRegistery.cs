using Network_Shared;
using Network_Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Network_Shared.Packet_Handlers;

namespace TTT.Server.Network_Shared.Registery
{
    public class HandleRegistery {
        private Dictionary<PacketType, Type> _handlers = new Dictionary<PacketType, Type>();

        public Dictionary<PacketType,Type> Handlers {
            get {
                if(_handlers.Count == 0) {
                    Initalize();
                }
                return _handlers;
            }
        }
        private void Initalize() {
            var newhandlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition)
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                .Select(t => (type: t, attr: t.GetCustomAttribute<HandlerRegisterAttribute>()))
                .Where(x => x.attr != null);

            foreach(var (type,attr) in newhandlers) {
                if (!_handlers.ContainsKey(attr.PacketType)) {
                    _handlers[attr.PacketType] = type;
                }
                else
                {
                    throw new Exception($"Multiple Handlers'{attr.PacketType}' Packet type detected only one Handler is Supported");
                }
            }
        }
    }
}
