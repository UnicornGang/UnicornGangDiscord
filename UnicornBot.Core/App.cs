using Discord;
using Discord.WebSocket;
using UnicornBot.Core.Helpers;
using UnicornBot.Core.Model;

namespace UnicornBot.Core
{
    public class App
    {
        private readonly DiscordSocketClient _client;
        private readonly ConfigurationClient _configurationClient;
        private readonly Config _config;

        public App(string configLocation)
        {
            _client = new DiscordSocketClient();
            _configurationClient = new ConfigurationClient(configLocation);
            _config = _configurationClient.LoadFromFileAsync().Result;

            _client.Log += Log;
        }

        public async Task Run()
        {
            Console.WriteLine(_config.Token);
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
