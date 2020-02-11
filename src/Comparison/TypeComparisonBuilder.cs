using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLeatherMan.Diff
{
    internal class TypeComparisonBuilder
    {
        private readonly string type;

        public TypeComparisonBuilder(string type)
        {
            this.type = type;
        }

        private readonly List<(string name, string type, bool nonNull)> removedFields = new List<(string name, string type, bool nonNull)>();
        internal void Removed(params (string name, string type, bool nonNull)[] fields) => removedFields.AddRange(fields);

        private readonly List<(string name, string type, bool nonNull)> addedFields = new List<(string name, string type, bool nonNull)>();

        public bool HasBreakingChanges =>
            removedFields.Any();

        internal void Added(params (string name, string type, bool nonNull)[] fields) => addedFields.AddRange(fields);

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"- (#) **{type}**");

            foreach (var field in addedFields)
            {
                builder.AppendLine($"  - (+) {field.name} : {field.type}{(field.nonNull ? "!" : "?")}");
            }
            foreach (var field in removedFields)
            {
                builder.AppendLine($"  -  (-) ~~{field.name} : {field.type}{(field.nonNull ? "!" : "?")}~~");
            }

            return builder.ToString();
        }
    }
}