using Discord.Commands;

namespace UnicornBot.Core.Commands
{
    public class TextModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Pings the bot")]
        public Task PingAsync()
        {
            return ReplyAsync($"Pong, {Context.Message.Author.Mention}!");
        }
    }
}
