using System.Reflection;
using UnicornBot.Core;
using UnicornBot.Core.Helpers;

AppInfo appInfo = new();
Console.Title = $"{appInfo.Title} v{appInfo.VersionShort}";

string configFile = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".json");

await new App(configFile).RunAsync();