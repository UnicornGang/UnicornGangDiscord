using Discord.Commands;
using UnicornBot.Core.Helpers;

namespace UnicornBot.Core.Commands
{
    public class AboutModule : ModuleBase<SocketCommandContext>
    {
        [Command("about")]
        [Summary("Displays information about the current build")]
        public Task AboutAsync()
        {
            AppInfo appInfo = new(true);
            return ReplyAsync($"{appInfo.Name} v{appInfo.VersionShort}");
        }
    }
}
