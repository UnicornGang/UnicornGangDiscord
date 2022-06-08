using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace UnicornBot.Core;

internal class EventHandlers
{

    private static readonly EventId _botEventId = new(420, "UnicornBot");

    internal static Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
    {
        sender.Logger.LogInformation(_botEventId, "Bot started.");

        return Task.CompletedTask;
    }

    internal static Task Client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
    {
        sender.Logger.LogInformation(_botEventId, "Connected to '{Guild Name}'.", e.Guild.Name);

        return Task.CompletedTask;
    }

    internal static Task Client_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
    {
        sender.Logger.LogError(_botEventId, e.Exception, "An exception has occured.");

        return Task.CompletedTask;
    }

    internal static async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        sender.Client.Logger.LogTrace(_botEventId, "A command ({Command}) from {User} raised an exception: {Exception}.", e.Context.RawArgumentString, e.Context.User.Username, e.Exception.Message);

        await e.Context.RespondAsync(e.Exception.Message).ConfigureAwait(false);
    }
}
