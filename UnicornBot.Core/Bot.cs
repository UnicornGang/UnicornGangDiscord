using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System.Reflection;
using UnicornBot.Core.Model;

namespace UnicornBot.Core;

public class Bot
{
    public DiscordClient Client { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    private readonly Config _config;
    private readonly EventId _botEventId = new(420, "UnicornBot");

    public Bot(Config config)
    {
        _config = config;

        Client = new DiscordClient(Configure().Result);

        Commands = RegisterCommands().Result;

        Client.Ready += Client_Ready;
        Client.GuildAvailable += Client_GuildAvailable;
        Client.ClientErrored += Client_ClientErrored;
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

        commands.CommandErrored += Commands_CommandErrored;

        return Task.FromResult(commands);
    }

    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
    {
        sender.Logger.LogInformation(_botEventId, "Bot started.");

        return Task.CompletedTask;
    }

    private Task Client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
    {
        sender.Logger.LogInformation(_botEventId, "Connected to '{Guild Name}'.", e.Guild.Name);

        return Task.CompletedTask;
    }

    private Task Client_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
    {
        sender.Logger.LogError(_botEventId, e.Exception, "An exception has occured.");

        return Task.CompletedTask;
    }

    private Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        Client.Logger.LogTrace(_botEventId, e.Exception, "A command error has occured.");

        e.Context.RespondAsync("Permission denied.");

        return Task.CompletedTask;
    }
}
