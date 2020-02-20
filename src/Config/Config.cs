using QLeatherMan.Diff;
using QLeatherMan.Generate;

namespace QLeatherMan
{
    public class Config
    {
        public GenerateVerb Generate { get; set; } = new GenerateVerb();
        public CompareVerb Compare { get; set; } = new CompareVerb();
    }
}
