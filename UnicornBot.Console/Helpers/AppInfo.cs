using System.Reflection;

namespace UnicornBot.Console.Helpers;

public class AppInfo
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    public static string Name => Title.Split('.')[0];

    public static string Title
    {
        get
        {
            object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            string title = (attributes.Length > 0 && ((AssemblyTitleAttribute)attributes[0]).Title != "")
                ? ((AssemblyTitleAttribute)attributes[0]).Title
                : Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);

            return title;
        }
    }
    public static string Version => (_assembly.GetName().Version ?? new Version("0")).ToString();
    public static string VersionShort
    {
        get
        {
            string[] versionArray = Version.Split('.');
            versionArray = versionArray.Take(versionArray.Length - 1).ToArray();
            return string.Join(".", versionArray);
        }
    }

    public static string Location => _assembly.Location;
}
