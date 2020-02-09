using System.Collections.Generic;
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

        private readonly List<string> removedFields = new List<string>();
        internal void Removed(params string[] fields) => removedFields.AddRange(fields);

        private readonly List<string> addedFields = new List<string>();
        internal void Added(params string[] fields) => addedFields.AddRange(fields);

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"- (#) **{type}**");

            foreach (var field in addedFields)
            {
                builder.AppendLine($"  - (+) {field}");
            }
            foreach (var field in removedFields)
            {
                builder.AppendLine($"  -  (-) ~~{field}~~");
            }

            return builder.ToString();
        }
    }
}