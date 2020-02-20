using QLeatherMan.Config;
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
            var config = new AutoRunSettings();

            var configFile = new FileInfo(".qlman");
            if (configFile.Exists)
                throw new NotSupportedException(Strings.OverwrtingSettingsFileNotSupported);

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(configFile.FullName, json)
                .ConfigureAwait(false);


        }
    }
}