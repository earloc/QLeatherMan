using QLeatherMan.Generate;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace QLeatherMan.Diff
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "used via DependencyInjection")]

    internal class ConfigCommand : ICommand
    {
        public async Task RunAsync()
        {
            var config = new Config()
            {
                Generate = new GenerateVerb(),
                Compare = new CompareVerb()
            };

            var configFile = new FileInfo(".qlman");
            if (configFile.Exists)
                throw new NotSupportedException("Overwriting an existing config is not supported. Delete the file '.qlman' and re-run the command.");

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(configFile.FullName, json)
                .ConfigureAwait(false);


        }
    }
}