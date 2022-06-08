using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Reflection;

namespace UnicornBot.Core.Commands;

[RequireUserPermissions(Permissions.SendMessages)]
public class Essentials : BaseCommandModule
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
    private readonly string _name = Path.GetFileNameWithoutExtension(_assembly.Location).Split('.')[0];
    private readonly string _version = (_assembly.GetName().Version ?? new Version("0")).ToString();

    [Command("ping"), Description("Pings the bot")]
    public async Task Ping(CommandContext context)
    {
        string user = context.Member == null ? context.User.Mention : context.Member.Mention;

        await context.Channel.SendMessageAsync($"Pong, {user}!").ConfigureAwait(false);
    }

    [Command("about"), Description("Displays information about the current build")]
    public async Task About(CommandContext context)
    {
        string versionShort = string.Join(".", _version.Split('.').SkipLast(1));

        await context.Channel.SendMessageAsync($"{_name} v{versionShort}").ConfigureAwait(false);
    }

    [Command("invite"), Description("Displays the current invite link"), RequireUserPermissions(Permissions.CreateInstantInvite)]
    public async Task GetInviteLink(CommandContext context)
    {
        DiscordChannel welcomeRoom = context.Guild.GetChannel(895214523068342283) ?? context.Channel;
        DiscordInvite invite = await welcomeRoom.CreateInviteAsync(max_age: 0, max_uses: 0, temporary: true, unique: false);

        await context.Channel.SendMessageAsync($"Invite link: https://discord.gg/{invite.Code}").ConfigureAwait(false);
    }
}
