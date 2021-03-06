﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace QLeatherMan.Diff
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "used via DependencyInjection")]
    internal class SchemaComparisonBuilder
    {
        private readonly IList<string> removedTypes = new List<string>();
        internal void Removed(string type) => removedTypes.Add(type);

        private readonly IList<string> addedTypes = new List<string>();
        internal void Added(string type) => addedTypes.Add(type);

        private readonly IList<TypeComparisonBuilder> modifiedTypes = new List<TypeComparisonBuilder>();

        public bool HasBreakingChanges =>
            removedTypes.Any() ||
            modifiedTypes.Any(x => x.HasBreakingChanges);

        internal TypeComparisonBuilder Modified(string type)
        {
            var builder = new TypeComparisonBuilder(type);
            modifiedTypes.Add(builder);

            return builder;
        }

        public string ToMarkdown(string? fromUri, string? toUri)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"# Differences between");
            builder.AppendLine($"- {fromUri}");
            builder.AppendLine($"- {toUri}");

            if (!HasChanges())
            {
                builder.AppendLine("looking good, no changes detected");
                return builder.ToString();
            }

            builder.AppendLine($@"## legend");
            builder.AppendLine($"- (+) ->   addition");
            builder.AppendLine($"- (-) -> ~~removal~~");
            builder.AppendLine($"- (#) -> **modification**");
            builder.AppendLine($"- (~) -> __deprecation__ (not implemented)");
            builder.AppendLine($"- ! -> non-nullable");
            builder.AppendLine($"- ? -> nullable");
            builder.AppendLine($"");

            builder.AppendLine($"## Types");

            foreach (var type in addedTypes)
            {
                builder.AppendLine($"- (+)   {type}  ");
            }

            foreach (var type in removedTypes)
            {
                builder.AppendLine($"- (-) ~~{type}~~");
            }

            foreach (var type in modifiedTypes)
            {
                builder.AppendLine(type.ToString());
            }

            return builder.ToString();
        }

        private bool HasChanges() 
            => addedTypes.Any() || removedTypes.Any() || modifiedTypes.Any();
    }
}
