using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QLeatherMan
{
    public static class JsonDefaults
    {

        static JsonDefaults()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;

            Options = options;
        }

        public static JsonSerializerOptions Options { get; }

    }
}
