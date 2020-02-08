using CommandLine;
using System;

namespace QLeatherMan.Diff
{
    [Verb("diff", HelpText = "performs a difference-analysis of two GraphQL-schemas")]
    public class DiffVerb
    {
        [Option('l', Required = true, HelpText = "Uri of te left hand side for the diff-comparison")]
        public Uri? LeftUri { get; set; }

        [Option('r', Required = true, HelpText = "Uri of te right hand side for the diff-comparison")]
        public Uri? RightUri { get; set; }
    }
}
