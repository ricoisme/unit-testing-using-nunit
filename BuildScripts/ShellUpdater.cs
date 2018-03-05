using System.IO;

namespace BuildScripts
{
    public class ShellUpdater
    {
        public ShellUpdater()
        {
            ServiceUpdate("IntegrationTest", "aspnetwebapicoredemo");
        }

        public string ServiceUpdate(string environment, string serviceName)
        {
            var mainPath = Directory.GetCurrentDirectory();
            var createServiceShellPath = $"{mainPath}/ShellScripts/CreateService.sh";
            var createServiceShellContent = File.ReadAllText(createServiceShellPath);
            createServiceShellContent = createServiceShellContent.Replace("${serviceName}$", $"{environment}-{serviceName}");
            createServiceShellContent = createServiceShellContent.Replace("${Description}$", "Example ASP.NET Web API running on CentOS 7");
            createServiceShellContent = createServiceShellContent.Replace("${WorkingDirectory}$", "/home/PrimeService/AssemblyOutput");
            createServiceShellContent = createServiceShellContent.Replace("${assembly}$", "MyAPI.dll");
            createServiceShellContent = createServiceShellContent.Replace("${Environment}$", $"{environment}");
            File.WriteAllText(createServiceShellPath, createServiceShellContent);
            return createServiceShellContent;
        }
    }

}
