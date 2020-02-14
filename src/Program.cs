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

            services.AddScoped<SchemaComparisonBuilder>();

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
                .WithNotParsed(errors =>
                    services.AddSingleton<ICommand>(_ => new ShowErrorCommand(errors))
                )
            ;

            var provider = services.BuildServiceProvider();

            var command = provider.GetRequiredService<ICommand>();

            await command.RunAsync().ConfigureAwait(false);
        }
    }
}
