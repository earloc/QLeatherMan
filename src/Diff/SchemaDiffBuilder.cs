using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public string ToMarkdown(Uri? leftUri, Uri? rightUri)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"# Differences between");
            builder.AppendLine($"- (l) {leftUri}");
            builder.AppendLine($"- (r) {rightUri}");


            builder.AppendLine($"## Types");

            foreach (var type in addedTypes)
            {
                builder.AppendLine($"- **{type}**");
            }

            foreach (var type in removedTypes)
            {
                builder.AppendLine($"- ~~{type}~~");
            }

            foreach (var type in modifiedTypes)
            {
                builder.AppendLine(type.ToString());
            }

            return builder.ToString();
        }
    }
}
