using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using UnicornBot.Core.Attributes;

namespace UnicornBot.Core.Commands;

[Permissions(Permissions.PermissionType.User)]
public class Debugging : BaseCommandModule
{
    [Command("debug::permissions"), Description("Shows permissions")]
    public async Task DebugData(CommandContext context)
    {
        string rolesJoined = string.Empty;

        DiscordRole[] roles = context.Member != null ? context.Member.Roles.ToArray() : Array.Empty<DiscordRole>();

        foreach (var role in roles) rolesJoined += $"\r\n\r\nRole '{role.Name} ({role.Id})' has '{role.Permissions}'";

        await context.RespondAsync($"Permissions: {(context.Member != null ? context.Member.Permissions : "(none)")}{rolesJoined}");
    }

    [Command("debug::isAdmin")]
    public async Task IsAdmin(CommandContext context)
    {
        await context.RespondAsync(Permissions.IsAdministrator(context.Member).ToString());
    }
}
