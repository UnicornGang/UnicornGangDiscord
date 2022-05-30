using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using UnicornBot.Core.Model;

namespace UnicornBot.Core;

public class Bot
{
    public DiscordClient Client { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    private readonly Config _config;

    public Bot(Config config)
    {
        _config = config;

        Client = new DiscordClient(Configure().Result);

        Commands = RegisterCommands().Result;

        Client.Ready += Client_Ready;
    }

    public async Task ConnectAsync()
    {
        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    public Task<DiscordConfiguration> Configure()
    {
        DiscordConfiguration botConfig = new()
        {
            Token = _config.Token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Information
        };

#if DEBUG
        botConfig.MinimumLogLevel = LogLevel.Debug;
#endif

        return Task.FromResult(botConfig);
    }

    public Task<CommandsNextExtension> RegisterCommands()
    {
        CommandsNextConfiguration commandsConfig = new()
        {
            StringPrefixes = new[] { _config.Prefix.ToString() },
            EnableDms = false,
            EnableMentionPrefix = false
        };

        CommandsNextExtension commands = Client.UseCommandsNext(commandsConfig);

        commands.RegisterCommands<Commands.Essentials>();

        return Task.FromResult(commands);
    }

    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
    {
        return Task.CompletedTask;
    }
}
