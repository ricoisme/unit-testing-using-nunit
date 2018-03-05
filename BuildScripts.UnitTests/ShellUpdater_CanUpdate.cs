using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BuildScripts.UnitTests
{
    [TestClass]
    public class ShellUpdater_CanUpdate
    {
        private readonly ShellUpdater shellUpdater;
        public ShellUpdater_CanUpdate()
        {
            shellUpdater = new ShellUpdater();
        }

        [TestMethod]
        public void CanUpdate()
        {
            var shContent = shellUpdater.ServiceUpdate("IntegrationTest", "aspnetwebapicoredemo");
            var secondLine = shContent.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries)[1];
            Assert.IsTrue(secondLine.Equals("cat /usr/lib/systemd/system/IntegrationTest-aspnetwebapicoredemo.service << EOF"));
        }
    }
}
