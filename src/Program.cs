using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using QLeatherMan.Diff;
using QLeatherMan.Generate;
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

            var result = Parser.Default.ParseArguments<GenerateVerb, CompareVerb>(args);

            result
                .WithParsed<GenerateVerb>(options =>
                {
                    services.AddSingleton(options);
                    services.AddSingleton<ICommand, GenerateCommand>();
                })
                .WithParsed<CompareVerb>(options =>
                {
                    services.AddSingleton(options);
                    services.AddSingleton<ICommand, CompareCommand>();
                })
            ;

            var provider = services.BuildServiceProvider();

            var command = provider.GetService<ICommand>();

            if (command is null)
                return;

            await command.RunAsync().ConfigureAwait(false);
        }
    }
}
