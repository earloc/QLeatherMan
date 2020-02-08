using CommandLine;

namespace QLeatherMan.Generate
{
    [Verb("generate", HelpText = "generates a C# client")]
    public class GenerateVerb
    {
        [Option('s', HelpText = "Uri of the GraphQL endpoint used to generate the client-code")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>")]
        public string SourceUri { get; set; } = "";

        [Option('d', HelpText = "Path to the file the generated client will be saved to")]
        public string DestinationFile { get; set; } = "GraphQlClient.generated.cs";

        [Option('n', HelpText = "Namespace used for generated types")]
        public string Namespace { get; set; } = "QLeatherMan.GraphQlClient";
    }
}
