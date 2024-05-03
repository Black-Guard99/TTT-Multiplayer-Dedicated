using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Network_Shared.Attributes;
using System;
using System.Linq;
using System.Reflection;
using TTT.Server.Network_Shared.Packet_Handlers;
using TTT.Server.Packet_Handlers;

namespace TTT.Server.Extensions {
    public static class ServiceCollectionsExtensions {
        public static IServiceCollection AddPacketHandlers(this IServiceCollection services)
        {
            var newhandlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition)
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                .Select(t => (type: t, attr: t.GetCustomAttribute<HandlerRegisterAttribute>()))
                .Where(x => x.attr != null);
            foreach (var (type,attr) in newhandlers) {
                services.AddScoped(type);
            }
            return services;
        }
    }
}
