using System.IO;

namespace BuildScripts
{
    public class ShellUpdater
    {
        public ShellUpdater()
        {
            ProcessService("IntegrationTest", "aspnetwebapicoredemo");
        }

        public string ProcessService(string environment, string serviceName)
        {
            var mainPath = Directory.GetCurrentDirectory();
            var shellScriptsPath = $"{mainPath}/ShellScripts/CreateService.sh";
            var scriptsContent = File.ReadAllText(shellScriptsPath);
            scriptsContent = scriptsContent.Replace("${serviceName}$", $"{environment}-{serviceName}");
            scriptsContent = scriptsContent.Replace("${WorkingDirectory}$", "/home/PrimeService/AssemblyOutput");
            scriptsContent = scriptsContent.Replace("${assembly}$", "MyAPI.dll");
            scriptsContent = scriptsContent.Replace("${Environment}$", $"{environment}");
            return scriptsContent;
        }
    }

}
