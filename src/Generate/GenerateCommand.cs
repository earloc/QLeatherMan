using GraphQlClientGenerator;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace QLeatherMan.Generate
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "used via DependencyInjection")]
    internal class GenerateCommand : ICommand
    {
        private readonly GenerateVerb options;

        public GenerateCommand(GenerateVerb options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task RunAsync()
        {
            var schema = await GraphQlGenerator.RetrieveSchema(options.Source).ConfigureAwait(false);
            var content = GraphQlGenerator.GenerateFullClientCSharpFile(schema, options.Namespace);
            File.WriteAllText(options.DestinationFile, content);
        }
    }
}
