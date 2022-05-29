using System.Reflection;
using UnicornBot.Core;

string configFile = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".json");

await new App(configFile).Run();