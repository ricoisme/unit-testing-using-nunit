﻿using System.IO;

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
            var shellScriptsPath = $"{mainPath}/ShellScripts/CreateService.sh";
            var scriptsContent = File.ReadAllText(shellScriptsPath);
            scriptsContent = scriptsContent.Replace("${serviceName}$", $"{environment}-{serviceName}");
            scriptsContent = scriptsContent.Replace("${Description}$", "Example ASP.NET Web API running on CentOS 7");
            scriptsContent = scriptsContent.Replace("${WorkingDirectory}$", "/home/PrimeService/AssemblyOutput");
            scriptsContent = scriptsContent.Replace("${assembly}$", "MyAPI.dll");
            scriptsContent = scriptsContent.Replace("${Environment}$", $"{environment}");
            File.WriteAllText(shellScriptsPath, scriptsContent);
            return scriptsContent;
        }
    }

}
