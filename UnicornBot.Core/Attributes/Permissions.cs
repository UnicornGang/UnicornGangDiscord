using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace UnicornBot.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class Permissions : CheckBaseAttribute
{
    public enum PermissionType : byte
    {
        Everyone,
        User,
        Admin,
        Owner
    }

    private readonly PermissionType _type;

    public Permissions(PermissionType permissionType)
    {
        _type = permissionType;
    }

    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
    {
        bool allow = _type switch
        {
            PermissionType.Everyone => true,
            PermissionType.User => !ctx.User.IsBot,
            PermissionType.Admin => IsAdministrator(ctx.Member),
            PermissionType.Owner => ctx.Member != null && ctx.Member.IsOwner,
            _ => false,
        };

        return Task.FromResult(allow);
    }

    public static bool IsAdministrator(DiscordMember? member)
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
