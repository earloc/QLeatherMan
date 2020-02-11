using GraphQlClientGenerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QLeatherMan
{
    internal class SchemaConverter
    {
        private const string MyFileExtension = ".qlman.json";

        internal static readonly JsonSerializerSettings SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

        public async Task<GraphQlSchema> ReadAsync(string? schemaUriOrPath)
        {
            if (schemaUriOrPath is null)
                throw new ArgumentNullException(nameof(schemaUriOrPath));

            var schema = default(GraphQlSchema?);

            var file = new FileInfo(schemaUriOrPath);
            if (file.Exists)
            {
                schema = FromFile(file);
            }
            else
            {
                schema = await GraphQlGenerator.RetrieveSchema(schemaUriOrPath).ConfigureAwait(false);
            }

            if (schema is null)
            {
                throw new InvalidOperationException($"failed to deserialize {file.FullName} to a GraphQlSchema");
            }

            return schema;
        }

        private static GraphQlSchema? FromFile(FileInfo file)
        {
            var json = File.ReadAllText(file.FullName);

            if (file.FullName.EndsWith(MyFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                return JsonConvert.DeserializeObject<GraphQlSchema>(json, SerializerSettings);
            }
            return GraphQlGenerator.DeserializeGraphQlSchema(json);
        }

        internal void WriteFileAsync(string simpleName, GraphQlSchema schema)
        {
            File.WriteAllText($"{simpleName}.{MyFileExtension}", JsonConvert.SerializeObject(schema, SerializerSettings));
        }
    }
}
