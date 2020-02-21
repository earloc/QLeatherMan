using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace QLeatherMan
{

    internal class CommandFactory
    {
        internal class Options : ICommandFactoryOptions
        {
            public readonly IDictionary<string, Type> commands = new Dictionary<string, Type>();

            public ICommandFactoryOptions Add<T>(string? name = null) where T : class, ICommand
            {
                var type = typeof(T);

                name ??= type.Name.Replace("Command", "", StringComparison.OrdinalIgnoreCase).ToUpperInvariant();

                commands.Add(name, type);

                return this;
            }
        }

        private readonly Options options;
        private readonly IServiceProvider serviceProvider;

        public CommandFactory(Options options, IServiceProvider serviceProvider)
        {
            this.options = options;
            this.serviceProvider = serviceProvider;
        }

        public ICommand Create(string name) => (ICommand)serviceProvider.GetRequiredService(options.commands[name.ToUpperInvariant()]);
    }
}

