using GraphQlClientGenerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

    internal class DiffCommand : ICommand
    {
        private readonly DiffVerb options;
        private readonly SchemaDiffBuilder diff;

        public DiffCommand(DiffVerb options, SchemaDiffBuilder diff)
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
                ReadSchemaAsync(options.LeftUri),
                ReadSchemaAsync(options.RightUri)
            ).ConfigureAwait(false);

            var left = schemas.First();
            var right = schemas.Last();

            File.WriteAllText("left.json", JsonConvert.SerializeObject(left, SerializerSettings));
            File.WriteAllText("right.json", JsonConvert.SerializeObject(right, SerializerSettings));

            ShowDiff(left, right);

        }

        private Task<GraphQlSchema> ReadSchemaAsync(Uri? schemaUri)
            => GraphQlGenerator.RetrieveSchema(schemaUri?.AbsoluteUri);

        private void ShowDiff(GraphQlSchema left, GraphQlSchema right)
        {
            var leftTypes = left.Types.ToDictionary(x => x.Name);
            var rightTypes = right.Types.ToDictionary(x => x.Name);

            CompareTypes(leftTypes, rightTypes);
            CompareFields(leftTypes, rightTypes);

            File.WriteAllText("diff.md", diff.ToString());
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

                var removedFields = leftFields.Keys.Except(rightFields.Keys).ToArray();
                var addedFields = rightFields.Keys.Except(leftFields.Keys).ToArray();

                if (!removedFields.Any() && !addedFields.Any())
                {
                    continue;
                }

                var modified = diff.Modified(right.Key);

                modified.Removed(removedFields);
                modified.Added(addedFields);
            }
        }
    }
}