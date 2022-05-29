using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using UnicornBot.Core.Helpers;
using UnicornBot.Core.Model;

namespace UnicornBot.Core
{
    public class App
    {
        private readonly DiscordSocketClient _client;
        private readonly ConfigurationClient _configurationClient;
        private readonly Config _config;
        private readonly CommandService _commands;

        public App(string configLocation)
        {
            _client = new DiscordSocketClient();
            _configurationClient = new ConfigurationClient(configLocation);
            _config = _configurationClient.LoadFromFileAsync().Result;
            _commands = new CommandService();

            _client.Log += Log;
        }

        public async Task RunAsync()
        {
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (messageParam is not SocketUserMessage message) return;

            int argPos = 0;

            if (!(message.HasCharPrefix(_config.Prefix, ref argPos)
                  || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
                  || message.Author.IsBot) return;

            SocketCommandContext context = new(_client, message);

            await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
