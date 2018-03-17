//using NUnit.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    [TestClass]
    public class PrimeService_IsPrimeShould
    {
        private readonly PrimeService _primeService;
        public PrimeService_IsPrimeShould()
        {
            _primeService = new PrimeService();
        }

        [TestMethod]
        public void ReturnFalseGivenValueOf1()
        {
            var result = _primeService.IsPrime(1);
            Assert.IsFalse(result, "1 should not be prime");
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        public void ReturnFalseGivenValuesLessThan2(int value)
        {
            var result = _primeService.IsPrime(value);

            Assert.IsFalse(result, $"{value} should not be prime");
        }
    }

    #region using Nunit

    //[TestFixture]
    //public class PrimeService_IsPrimeShould
    //{
    //    private readonly PrimeService _primeService;

    //    public PrimeService_IsPrimeShould()
    //    {
    //        _primeService = new PrimeService();
    //    }

    //    [Test]
    //    public void ReturnFalseGivenValueOf1()
    //    {
    //        var result = _primeService.IsPrime(1);
    //        //test
    //        Assert.IsFalse(result, "1 should not be prime");
    //    }

    //    #region Sample_TestCode
    //    [TestCase(-1)]
    //    [TestCase(0)]
    //    [TestCase(1)]
    //    public void ReturnFalseGivenValuesLessThan2(int value)
    //    {
    //        var result = _primeService.IsPrime(value);

    //        Assert.IsFalse(result, $"{value} should not be prime");
    //    }
    //    #endregion
    //}

    #endregion

}
