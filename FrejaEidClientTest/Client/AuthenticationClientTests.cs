using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClientTest.Utils;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Client.Tests
{
    [TestClass]
    public class AuthenticationClientTests
    {
        private const string INVALID_KEYSTORE_PATH = "x";
        private const string INVALID_CERTIFICATE_PATH = "x";
        private const string INVALID_PASSWORD = "1111111111";
        private const string EMPTY_PATH = " ";
        private const int INVALID_POLLING_TIMEOUT = 0;

        [TestMethod]
        public void AuthClientInit_invalidKeystorePath_expectKeystoreError()
        {
            AuthClientInit_assertError(INVALID_KEYSTORE_PATH, TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH), "Failed to initialize SSL Context with given client certificate path or password.");
        }

        [TestMethod]
        public void AuthClientInit_invalidCertificatePath_expectKeystoreError()
        {
            AuthClientInit_assertError(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, INVALID_CERTIFICATE_PATH, "Failed to initialize SSL Context with given server certificate path or password.");
        }

        [TestMethod]
        public void AuthClientInit_invalidKeystorePassword_expectKeystoreError()
        {
            AuthClientInit_assertError(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), INVALID_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH), "Failed to initialize SSL Context with given client certificate path or password.");
        }

        [TestMethod]
        public void AuthClientInit_invalidKeystoreFileAuthentication_expectKeystoreError()
        {

            AuthClientInit_assertError(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.INVALID_KEYSTORE_FILE), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH), "Failed to initialize SSL Context with given client certificate path or password.");
        }

        [TestMethod]
        public void AuthClientInit_invalidNullParameter_expectInternalError()
        {
            AuthClientInit_assertError(null, TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH), "KeyStore Path, keyStore password or server certificate path cannot be null or empty.");
        }

        [TestMethod]
        public void AuthClientInit_invalidEmptyParameter_expectInternalError()
        {
            AuthClientInit_assertError(EMPTY_PATH, TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH), "KeyStore Path, keyStore password or server certificate path cannot be null or empty.");
        }

        private void AuthClientInit_assertError(string keyStorePath, string password, string certificatePath, string errorMessage)
        {
            try
            {
                AuthenticationClient.Create(SslSettings.Create(keyStorePath, password, certificatePath), FrejaEnvironment.TEST)
                        .Build<AuthenticationClient>();
                Assert.Fail("Test should throw exception!");
            }
            catch (FrejaEidClientInternalException ex)
            {
                Assert.AreEqual(errorMessage, ex.Message);
            }
        }

        [TestMethod]
        public void AuthClientInit_invalidPoolingTime_expectError()
        {
            try
            {
                AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                        .SetPollingTimeout(INVALID_POLLING_TIMEOUT)
                        .Build<AuthenticationClient>();
                Assert.Fail("Test should throw exception!");
            }
            catch (FrejaEidClientInternalException ex)
            {
                Assert.AreEqual("Polling timeout must be between 1 and 30 seconds.", ex.Message);
            }
        }

        [TestMethod]
        public void AuthClientInit_success()
        {
            IAuthenticationClient authenticationClientPKCS12 = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                .SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();
        }
    }
}