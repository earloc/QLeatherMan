using GraphQlClientGenerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
        private const string MyFileExtension = ".qlman.json";

        private readonly CompareVerb options;
        private readonly SchemaComparisonBuilder diff;

        public CompareCommand(CompareVerb options, SchemaComparisonBuilder diff)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.diff = diff ?? throw new ArgumentNullException(nameof(diff));
        }

        internal static readonly JsonSerializerSettings SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };
        public async Task RunAsync()
        {
            var schemas = await Task.WhenAll(
                ReadSchemaAsync(options.Left),
                ReadSchemaAsync(options.Right)
            ).ConfigureAwait(false);

            var left = schemas.First();
            var right = schemas.Last();

            File.WriteAllText($"left.{MyFileExtension}", JsonConvert.SerializeObject(left, SerializerSettings));
            File.WriteAllText($"right.{MyFileExtension}", JsonConvert.SerializeObject(right, SerializerSettings));

            ShowDiff(left, right);

        }

        private Task<GraphQlSchema> ReadSchemaAsync(string? schemaUriOrPath)
        {
            if (schemaUriOrPath is null)
                throw new ArgumentNullException(nameof(schemaUriOrPath));

            var file = new FileInfo(schemaUriOrPath);

            if (file.Exists)
            {
                return Task.FromResult(FromFile(file));
            }

            return GraphQlGenerator.RetrieveSchema(schemaUriOrPath);
        }

        private static GraphQlSchema FromFile(FileInfo file)
        {
            var json = File.ReadAllText(file.FullName);

            if (file.FullName.EndsWith(MyFileExtension, StringComparison.OrdinalIgnoreCase)) 
            {
                return JsonConvert.DeserializeObject<GraphQlSchema>(json);
            }
            return GraphQlGenerator.DeserializeGraphQlSchema(json);
        }

        private void ShowDiff(GraphQlSchema left, GraphQlSchema right)
        {
            var leftTypes = left.Types.ToDictionary(x => x.Name);
            var rightTypes = right.Types.ToDictionary(x => x.Name);

            CompareTypes(leftTypes, rightTypes);
            CompareFields(leftTypes, rightTypes);

            var comparisonReport = diff.ToMarkdown(options.Left, options.Right);

            if (!(options.ReportMarkdownPath is null))
            {
                File.WriteAllText(options.ReportMarkdownPath, comparisonReport);
            }

            if (!options.Silent)
                Console.WriteLine(comparisonReport);
        }

        private void CompareTypes(Dictionary<string, GraphQlType> leftTypes, Dictionary<string, GraphQlType> rightTypes)
        {
            var removedTypes = leftTypes.Keys.Except(rightTypes.Keys).ToArray();
            foreach (var type in removedTypes)
                diff.Removed(type);

            var addedTypes = rightTypes.Keys.Except(leftTypes.Keys).ToArray();
            foreach (var type in addedTypes)
                diff.Added(type);
        }

        private void CompareFields(Dictionary<string, GraphQlType> leftTypes, Dictionary<string, GraphQlType> rightTypes)
        {

            foreach (var right in rightTypes)
            {
                if (!leftTypes.TryGetValue(right.Key, out var left))
                {
                    return;
                }

                var leftFields = left.Fields?.ToDictionary(x => x.Name) ?? new Dictionary<string, GraphQlField>();
                var rightFields = right.Value.Fields?.ToDictionary(x => x.Name) ?? new Dictionary<string, GraphQlField>();

                var removedFieldNames = leftFields.Keys.Except(rightFields.Keys).ToArray();
                var removedFields = removedFieldNames.Select(x => leftFields[x]).ToArray();

                var addedFieldNames = rightFields.Keys.Except(leftFields.Keys).ToArray();
                var addedFields = addedFieldNames.Select(x => rightFields[x]).ToArray();

                if (!removedFieldNames.Any() && !addedFieldNames.Any())
                {
                    continue;
                }

                var modified = diff.Modified(right.Key);

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