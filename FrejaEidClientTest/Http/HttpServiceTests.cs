using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Http.Tests
{
    [TestClass]
    public class HttpServiceTests
    {
        [TestMethod]
        public void MakeUserAgentHeaderTest()
        {
            string userAgentHeader = HttpService.MakeUserAgentHeader();
            Assert.AreEqual("FrejaEidClient/1.0.0.0 (.NET Framework/4.0.30319.42000)", userAgentHeader);
        }

        [TestMethod]
        public void GetLibVersionTest()
        {
            string libVersion = HttpService.GetLibVersion();
            Assert.AreEqual("1.0.0.0", libVersion);
        }
    }
}