using GraphQlClientGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QLeatherMan.Diff
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "used via DependencyInjection")]

    internal class CompareCommand : ICommand
    {
        private readonly CompareVerb options;
        private readonly SchemaComparisonBuilder diff;

        public CompareCommand(CompareVerb options, SchemaComparisonBuilder diff)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.diff = diff ?? throw new ArgumentNullException(nameof(diff));
        }
        
        public async Task RunAsync()
        {
            var schemas = await Task.WhenAll(
                SchemaConverter.ReadAsync(options.From),
                SchemaConverter.ReadAsync(options.To)
            ).ConfigureAwait(false);

            var from = schemas.First();
            var to = schemas.Last();

            SchemaConverter.WriteFileAsync("from", from);
            SchemaConverter.WriteFileAsync("to", to);

            ShowDiff(from, to);
        }

        private void ShowDiff(GraphQlSchema from, GraphQlSchema to)
        {
            var fromTypes = from.Types.ToDictionary(x => x.Name);
            var toTypes = to.Types.ToDictionary(x => x.Name);

            CompareTypes(fromTypes, toTypes);
            CompareFields(fromTypes, toTypes);

            var comparisonReport = diff.ToMarkdown(options.From, options.To);

            if (!(options.ReportMarkdownPath is null))
            {
                File.WriteAllText(options.ReportMarkdownPath, comparisonReport);
            }

            if (diff.HasBreakingChanges)
            {
                Console.Error.WriteLine($"looks like {options.To} introduces breaking-changes from {options.From}");
            }

            if (!options.Silent)
            {
                Console.WriteLine(comparisonReport);
            }
        }

        private void CompareTypes(Dictionary<string, GraphQlType> fromTypes, Dictionary<string, GraphQlType> toTypes)
        {
            var removedTypes = fromTypes.Keys.Except(toTypes.Keys).ToArray();
            foreach (var type in removedTypes)
                diff.Removed(type);

            var addedTypes = toTypes.Keys.Except(fromTypes.Keys).ToArray();
            foreach (var type in addedTypes)
                diff.Added(type);
        }

        private void CompareFields(Dictionary<string, GraphQlType> fromTypes, Dictionary<string, GraphQlType> toTypes)
        {

            foreach (var to in toTypes)
            {
                if (!fromTypes.TryGetValue(to.Key, out var from))
                {
                    return;
                }

                var fromFields = from.Fields?.ToDictionary(x => x.Name) ?? new Dictionary<string, GraphQlField>();
                var toFields = to.Value.Fields?.ToDictionary(x => x.Name) ?? new Dictionary<string, GraphQlField>();

                var removedFieldNames = fromFields.Keys.Except(toFields.Keys).ToArray();
                var removedFields = removedFieldNames.Select(x => fromFields[x]).ToArray();

                var addedFieldNames = toFields.Keys.Except(fromFields.Keys).ToArray();
                var addedFields = addedFieldNames.Select(x => toFields[x]).ToArray();

                if (!removedFieldNames.Any() && !addedFieldNames.Any())
                {
                    continue;
                }

                var modified = diff.Modified(to.Key);

                modified.Removed(removedFields.Select(x =>
                    (
                        name: x.Name,
                        type: x.Type.Name ?? x.Type.OfType.Name,
                        nonNull: x.Type.Kind == GraphQlTypeKind.NonNull
                    )
                ).ToArray());

                modified.Added(addedFields.Select(x =>
                    (
                        name: x.Name,
                        type: x.Type.Name ?? x.Type.OfType.Name,
                        nonNull: x.Type.Kind == GraphQlTypeKind.NonNull
                    )
                ).ToArray());
            }
        }
    }
}