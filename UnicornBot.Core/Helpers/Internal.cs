using DSharpPlus.Entities;

namespace UnicornBot.Core.Helpers;

internal class Internal
{
    internal static bool IsAdministrator(DiscordMember? member)
    {
        if (member == null) return false;

        if (member.IsOwner) return true;

        DiscordRole[]? roles = member.Roles.ToArray();

        if (roles == null) return false;

        foreach (DiscordRole role in roles)
            if (role.Permissions.ToString().Contains("Administrator")) return true;

        return false;
    }
}
