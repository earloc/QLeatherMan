using CommandLine;

namespace QLeatherMan.Diff
{
    [Verb(name, HelpText = "performs a difference-analysis of two GraphQL-schemas")]
    public class CompareVerb
    {
        private const string name = "compare";
        internal readonly string Name = name;

        [Value(0, Required = true, HelpText = "Uri of the left hand side for the diff-comparison")]
        public string? Left { get; set; }

        [Value(1, Required = true, HelpText = "Uri of the right hand side for the diff-comparison")]
        public string? Right { get; set; }

        [Option('o', Required = false, HelpText = "Path to file for generaten comparison report in Markdown-Format")]
        public string? ReportMarkdownPath { get; set; }

        [Option('s', Required = false, Default = false,  HelpText = "Do not print comparison report on console")]
        public bool Silent { get; internal set; }
    }
}
