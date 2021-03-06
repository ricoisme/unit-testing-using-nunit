//using NUnit.Framework;
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
            Assert.IsTrue(secondLine.Equals("service=IntegrationTest-aspnetwebapicoredemo.service"));
        }
    }

    #region using nunit

    //[TestFixture]
    //public class ShellUpdater_CanUpdate
    //{
    //    private readonly ShellUpdater shellUpdater;
    //    public ShellUpdater_CanUpdate()
    //    {
    //        shellUpdater = new ShellUpdater();
    //    }

    //    [Test]
    //    public void CanUpdate()
    //    {
    //        var shContent = shellUpdater.ServiceUpdate("IntegrationTest", "aspnetwebapicoredemo");
    //        var secondLine = shContent.Split(new string[] { Environment.NewLine },
    //            StringSplitOptions.RemoveEmptyEntries)[1];
    //        Assert.IsTrue(secondLine.Equals("service=IntegrationTest-aspnetwebapicoredemo.service"));
    //    }
    //}

    #endregion

}
