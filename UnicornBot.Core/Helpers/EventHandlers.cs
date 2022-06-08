using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
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
        sender.Client.Logger.LogTrace(_botEventId, e.Exception, "A command error has occured.");

        IReadOnlyList<CheckBaseAttribute> failedChecks = ((ChecksFailedException)e.Exception).FailedChecks;

        foreach (CheckBaseAttribute failedCheck in failedChecks)
        {
            string msg = failedCheck switch
            {
                _ => $"An unknown error has occured:\r\n{e.Exception}",
            };

            await e.Context.RespondAsync(msg).ConfigureAwait(false);
        }
    }
}
