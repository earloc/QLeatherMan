﻿using CommandLine;

namespace QLeatherMan.Generate
{
    [Verb("generate", HelpText = "generates a C# client")]
    public class GenerateVerb
    {
        [Value(0, Required = false, Default = "https://api.spacex.land/graphql/", HelpText = "Uri of the GraphQL endpoint used to generate the client-code")]
        public string? Source { get; set; }

        [Value(1, Required = false, Default = "GraphQlClientGenerator.generated.cs", HelpText = "Path to the file the generated client will be saved to")]
        public string DestinationFile { get; set; } = "GraphQlClientGenerator.generated.cs";

        [Value(3, Required = false, Default = "GraphQlClientGenerator.Generated", HelpText = "Namespace used for generated types")]
        public string Namespace { get; set; } = "GraphQlClientGenerator.Generated";

        [Option('d', Default = false, HelpText = "generate types and members even when they are deprecated in the schema")]
        public bool GenerateDeprecatedTypes { get; set; }
    }
}
