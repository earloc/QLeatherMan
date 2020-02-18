using CommandLine;

namespace QLeatherMan.Diff
{
    [Verb(name, HelpText = "creates a sample config for autorun-mode")]
    public class ConfigVerb
    {
        private const string name = "config";
        internal readonly string Name = name;
    }
}
