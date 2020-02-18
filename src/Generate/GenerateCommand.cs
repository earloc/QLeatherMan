using GraphQlClientGenerator;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace QLeatherMan.Generate
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "used via DependencyInjection")]
    internal class GenerateCommand : ICommand
    {
        private readonly GenerateVerb options;
        private readonly SchemaConverter converter;

        public GenerateCommand(GenerateVerb options, SchemaConverter converter)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }
        public async Task RunAsync()
        {
            GraphQlGeneratorConfiguration.IncludeDeprecatedFields = options.GenerateDeprecatedTypes;

            var schema = await converter.ReadAsync(options.Source).ConfigureAwait(false);
            var content = GraphQlGenerator.GenerateFullClientCSharpFile(schema, options.Namespace);
            File.WriteAllText(options.DestinationFile, content);
        }
    }
}
