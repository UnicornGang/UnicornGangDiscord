using System.Reflection;

namespace UnicornBot.Core.Helpers
{
    public class AppInfo
    {
        private readonly Assembly _assembly;

        public AppInfo(bool useCoreAssembly = false)
        {
            _assembly = useCoreAssembly ? Assembly.GetExecutingAssembly() : (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());
        }

        public string Name => Title.Split('.')[0];

        public string Title
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
        public string Version => (_assembly.GetName().Version ?? new Version("0")).ToString();
        public string VersionShort
        {
            get
            {
                string[] versionArray = Version.Split('.');
                versionArray = versionArray.Take(versionArray.Length - 1).ToArray();
                return string.Join(".", versionArray);
            }
        }

        public string AssemblyLocation => _assembly.Location;
    }
}
