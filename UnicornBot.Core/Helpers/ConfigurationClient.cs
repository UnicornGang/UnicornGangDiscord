﻿using System.Text;
using System.Text.Json;
using UnicornBot.Core.Model;

namespace UnicornBot.Core.Helpers
{
    internal class ConfigurationClient
    {
        private readonly string _filePath;

        internal ConfigurationClient(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath)) CreateConfig();
        }

        internal async Task<Config> LoadFromFileAsync()
        {
            string configData;

            using FileStream stream = File.OpenRead(_filePath);
            using StreamReader reader = new(stream, Encoding.UTF8);
            configData = await reader.ReadToEndAsync().ConfigureAwait(false);

            var data = JsonSerializer.Deserialize<Config>(configData);
            return data;
        }

        internal void CreateConfig()
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
}
