using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using QLeatherMan.Diff;
using QLeatherMan.Generate;
using System.Collections.Generic;
using System.Linq;
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

            var result = Parser.Default.ParseArguments<GenerateVerb, CompareVerb>(args);
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
                //.WithNotParsed(errors =>
                //{
                //    if (errors.FirstOrDefault()?.Tag == ErrorType.NoVerbSelectedError)
                //    {
                //        var config = TryReadConfig()
                //    }
                //});
            ;

            var provider = services.BuildServiceProvider();

            var factory = provider.GetRequiredService<CommandFactory>();

            foreach (var verb in invokedVerbs)
            {
                var command = factory.Create(verb);
                await command.RunAsync().ConfigureAwait(false);
            }
        }
    }
}
