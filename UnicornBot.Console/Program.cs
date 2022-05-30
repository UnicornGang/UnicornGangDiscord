using UnicornBot.Console.Helpers;
using UnicornBot.Core;
using UnicornBot.Core.Model;

Console.Title = $"{AppInfo.Title} v{AppInfo.VersionShort}";

Config config = await new ConfigurationClient().LoadFromFileAsync();

Bot bot = new(config);

new Task(async () => await bot.ConnectAsync()).Start();

do Console.ReadLine(); while (true);
