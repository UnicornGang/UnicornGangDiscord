using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Reflection;

namespace UnicornBot.Core.Commands;

public class Essentials : BaseCommandModule
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
    private readonly string _name = Path.GetFileNameWithoutExtension(_assembly.Location).Split('.')[0];
    private readonly string _version = (_assembly.GetName().Version ?? new Version("0")).ToString();

    [Command("ping")]
    [Description("Pings the bot")]
    public async Task Ping(CommandContext context)
    {
        string user = context.Member == null ? context.User.Mention : context.Member.Mention;

        await context.Channel.SendMessageAsync($"Pong, {user}!").ConfigureAwait(false);
    }

    [Command("about")]
    [Description("Displays information about the current build")]
    public async Task AboutAsync(CommandContext context)
    {
        string versionShort = string.Join(".", _version.Split('.').SkipLast(1));
        await context.Channel.SendMessageAsync($"{_name} v{versionShort}").ConfigureAwait(false);
    }
}
