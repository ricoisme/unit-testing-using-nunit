using NUnit.Framework;
using System;

namespace BuildScripts.UnitTests
{
    [TestFixture]
    public class ShellUpdater_CanUpdate
    {
        private readonly ShellUpdater shellUpdater;
        public ShellUpdater_CanUpdate()
        {
            shellUpdater = new ShellUpdater();
        }

        [Test]
        public void CanUpdate()
        {
            var shContent = shellUpdater.ServiceUpdate("IntegrationTest", "aspnetwebapicoredemo");
            var secondLine = shContent.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries)[1];
            Assert.IsTrue(secondLine.Equals("service=IntegrationTest-aspnetwebapicoredemo.service"));
        }
    }
}
