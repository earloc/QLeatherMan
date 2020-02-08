using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace QLeatherMan.Diff
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "used via DependencyInjection")]
    internal class SchemaDiffBuilder
    {
        private readonly IList<string> removedTypes = new List<string>();
        internal void Removed(string type) => removedTypes.Add(type);

        private readonly IList<string> addedTypes = new List<string>();
        internal void Added(string type) => addedTypes.Add(type);

        private readonly IList<TypeDiffBuilder> modifiedTypes = new List<TypeDiffBuilder>();
        internal TypeDiffBuilder Modified(string type)
        {
            var builder = new TypeDiffBuilder(type);
            modifiedTypes.Add(builder);

            return builder;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (addedTypes.Any()) builder.AppendLine($"## {nameof(Added)} Types");

            foreach (var type in addedTypes)
            {
                builder.AppendLine($"- (+) **{type}**");
            }

            if (removedTypes.Any()) builder.AppendLine($"## {nameof(Removed)} Types");

            foreach (var type in removedTypes)
            {
                builder.AppendLine($"- (-) ~~{type}~~");
            }

            if (modifiedTypes.Any()) builder.AppendLine($"## {nameof(Modified)} Types");

            foreach (var type in modifiedTypes)
            {
                builder.AppendLine(type.ToString());
            }

            return builder.ToString();
        }
    }
}
