using CommandLine;
using GraphQlClientGenerator;

namespace QLeatherMan.Generate
{
    [Verb(name, HelpText = "generates a C# client")]
    public class GenerateVerb
    {
        private const string name = "generate";
        internal readonly string Name = name;

        [Value(0, Required = false, Default = "https://api.spacex.land/graphql/", HelpText = "Uri of the GraphQL endpoint used to generate the client-code")]
        public string? Source { get; set; }

        [Value(1, Required = false, Default = "GraphQlClientGenerator.generated.cs", HelpText = "Path to the file the generated client will be saved to")]
        public string DestinationFile { get; set; } = "GraphQlClientGenerator.generated.cs";

        [Value(3, Required = false, Default = "GraphQlClientGenerator.Generated", HelpText = "Namespace used for generated types")]
        public string Namespace { get; set; } = "GraphQlClientGenerator.Generated";

        [Option('d', Default = false, HelpText = "generate types and members even when they are deprecated in the schema")]
        public bool GenerateDeprecatedTypes { get; set; }

        [Option('n', Default = false, HelpText = "use C# 8.0 with nullable-reference-type support")]
        public bool UseNullable { get; set; }

        [Option('j', HelpText = "specifies the strategy with which properteis are generated, Default is 'CaseInsensitive'", MetaValue = "CaseInsensitive")]

        public JsonPropertyGenerationOption JsonPropertyGeneration { get; set; } = JsonPropertyGenerationOption.CaseInsensitive;
    }
}
