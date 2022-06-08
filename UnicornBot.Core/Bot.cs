using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using System.Reflection;
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

        Client.Ready += EventHandlers.Client_Ready;
        Client.GuildAvailable += EventHandlers.Client_GuildAvailable;
        Client.ClientErrored += EventHandlers.Client_ClientErrored;
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

        commands.RegisterCommands(Assembly.GetExecutingAssembly());

        commands.CommandErrored += EventHandlers.Commands_CommandErrored;

        return Task.FromResult(commands);
    }
}
