using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;
using TTT.Server.Extensions;
using TTT.Server.Games;
using TTT.Server.Matchmaking;
using TTT.Server.Network_Shared.Registery;
namespace TTT.Server.Infrastructure
{
    public static class Container {
        public static IServiceProvider Configure()
        {
            var service = new ServiceCollection();
            ConfigureServices(service);
            return service.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection service) {
            service.AddLogging(c => c.AddSimpleConsole());
            service.AddSingleton<NetworkServer>();
            service.AddSingleton<PacketRegistery>();
            service.AddSingleton<HandleRegistery>();
            service.AddSingleton<UsersManager>();
            service.AddSingleton<MatchMakingManager>();
            service.AddSingleton<GamesManager>();
            service.AddSingleton<IUserRepository,InMemoryUserRepository>();
            service.AddPacketHandlers();
        }
    }
}
