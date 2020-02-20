using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using QLeatherMan.Diff;
using QLeatherMan.Generate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace QLeatherMan
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton<SchemaComparisonBuilder>();
            services.AddSingleton<SchemaConverter>();

            services.AddCommands(_ => _
                .Add<CompareCommand>()
                .Add<GenerateCommand>()
            );

            var result = Parser.Default.ParseArguments<GenerateVerb, CompareVerb, ConfigVerb>(args);
            var invokedVerbs = new List<string>();

            result
                .WithParsed<GenerateVerb>(options =>
                {
                    services.AddSingleton(options);
                    invokedVerbs.Add(options.Name);
                })
                .WithParsed<CompareVerb>(options =>
                {
                    services.AddSingleton(options);
                    invokedVerbs.Add(options.Name);
                })
                .WithParsed<ConfigVerb>(options =>
                {
                    services.AddSingleton(options);
                    invokedVerbs.Add(options.Name);
                })
                .WithNotParsed(errors =>
                {
                    if (errors.FirstOrDefault()?.Tag != ErrorType.NoVerbSelectedError)
                    {
                        return;
                    }
                    var verbs = ReadConfig(services);
                    if (verbs != null)
                        invokedVerbs.AddRange(verbs);
                })
            ;

            var provider = services.BuildServiceProvider();

            var factory = provider.GetRequiredService<CommandFactory>();

            foreach (var verb in invokedVerbs)
            {
                var command = factory.Create(verb);
                await command.RunAsync().ConfigureAwait(false);
            }
        }

        private static IEnumerable<string> ReadConfig(IServiceCollection services)
        {

            var configFile = new FileInfo(".qlman");

            if (!configFile.Exists)
            {
                return Enumerable.Empty<string>();
            }

            var config = JsonSerializer.Deserialize<AutoRunSettings>(File.ReadAllText(configFile.FullName));

            var verbs = new List<string>();

            if (config.Generate != null)
            {
                services.AddSingleton(config.Generate);
                verbs.Add(config.Generate.Name);
            }

            if (config.Compare != null)
            {
                services.AddSingleton(config.Compare);
                verbs.Add(config.Compare.Name);
            }

            return verbs;
        }
    }
}
