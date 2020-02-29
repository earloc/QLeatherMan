﻿using GraphQlClientGenerator;
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
            GraphQlGeneratorConfiguration.CSharpVersion = options.UseNullable ? CSharpVersion.NewestWithNullableReferences : CSharpVersion.Compatible;

            var schema = await converter.ReadAsync(options.Source).ConfigureAwait(false);
            var content = GraphQlGenerator.GenerateFullClientCSharpFile(schema, options.Namespace);

            Console.WriteLine($"writing client-code to {options.DestinationFile}");
            File.WriteAllText(options.DestinationFile, content);
        }
    }
}
