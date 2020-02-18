using Microsoft.Extensions.DependencyInjection;
using System;

namespace QLeatherMan
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddCommands(this IServiceCollection services, Action<ICommandFactoryOptions> configure)
        {
            var options = new CommandFactory.Options();
            configure?.Invoke(options);

            foreach (var type in options.commands.Values)
                services.AddScoped(type);

            services.AddSingleton(options);

            services.AddSingleton<CommandFactory>();

            return services;
        }
    }
}

