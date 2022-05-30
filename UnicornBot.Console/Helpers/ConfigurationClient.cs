using System.Reflection;
using System.Text;
using System.Text.Json;
using UnicornBot.Core.Model;

namespace UnicornBot.Console.Helpers;

public class ConfigurationClient
{
    private readonly string _filePath;

    public ConfigurationClient()
    {
        _filePath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".json");
    }

    public ConfigurationClient(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<Config> LoadFromFileAsync()
    {
        if (!File.Exists(_filePath)) CreateConfig();

        string configData;

        using FileStream stream = File.OpenRead(_filePath);
        using StreamReader reader = new(stream, Encoding.UTF8);
        configData = await reader.ReadToEndAsync().ConfigureAwait(false);

        var data = JsonSerializer.Deserialize<Config>(configData);
        return data;
    }

    public void CreateConfig()
    {
        Config config = new()
        {
            Token = "<insert your token here>",
            Prefix = '$'
        };
        string configData = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

        using FileStream stream = File.OpenWrite(_filePath);
        using StreamWriter writer = new(stream, Encoding.UTF8);
        writer.Write(configData);
    }
}
