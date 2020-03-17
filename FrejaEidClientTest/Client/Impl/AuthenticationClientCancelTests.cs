using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClientTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Client.Impl.Tests
{
    [TestClass]
    public class AuthenticationClientCancelTests
    {
        private const string INVALID_REFERENCE_ERROR_MESSAGE = "Invalid reference (for example, nonexistent or expired).";
        private const int INVALID_REFERENCE_ERROR_CODE = 1100;

        [TestMethod]
        public void RelyingPartyIdNull_success()
        {
            CancelAuthenticationRequest cancelAuthenticationRequest = CancelAuthenticationRequest.Create(CommonTestData.REFERENCE);
            CancelAuthentication(cancelAuthenticationRequest, TransactionContext.PERSONAL, null);
        }

        [TestMethod]
        public void PersonalContext_success()
        {
            CancelAuthenticationRequest cancelAuthenticationRequest = CancelAuthenticationRequest.Create(CommonTestData.REFERENCE, CommonTestData.RELYING_PARTY_ID);
            CancelAuthentication(cancelAuthenticationRequest, TransactionContext.PERSONAL, CommonTestData.RELYING_PARTY_ID);
        }

        [TestMethod]
        public void OrganisationlContext_success()
        {
            CancelAuthenticationRequest cancelAuthenticationRequest = CancelAuthenticationRequest.Create(CommonTestData.REFERENCE, CommonTestData.RELYING_PARTY_ID);
            CancelAuthentication(cancelAuthenticationRequest, TransactionContext.ORGANISATIONAL, CommonTestData.RELYING_PARTY_ID);
        }

        private void CancelAuthentication(CancelAuthenticationRequest cancelAuthenticationRequest, TransactionContext transactionContext, string RelyingPartyId)
        {
            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                        .SetHttpService(CommonTestData.HttpServiceMock.Object)
                        .SetTransactionContext(transactionContext).Build<AuthenticationClient>();
            CommonTestData.HttpServiceMock.Setup(x => x.Send<EmptyFrejaResponse>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<CancelAuthenticationRequest>(), It.IsAny<string>())).Returns(new EmptyFrejaResponse());
            authenticationClient.Cancel(cancelAuthenticationRequest);
            if (transactionContext.Equals(TransactionContext.PERSONAL))
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<EmptyFrejaResponse>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_CANCEL), RequestTemplate.CANCEL_AUTHENTICATION_TEMPLATE, cancelAuthenticationRequest, RelyingPartyId));
            }
            else
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<EmptyFrejaResponse>(new Uri(FrejaEnvironment.TEST + MethodUrl.ORGANISATION_AUTHENTICATION_CANCEL), RequestTemplate.CANCEL_AUTHENTICATION_TEMPLATE, cancelAuthenticationRequest, RelyingPartyId));
            }

        }

        [TestMethod]
        public void InvalidReference_expectInvalidReferenceError()
        {
            CancelAuthenticationRequest cancelAuthenticationRequest = CancelAuthenticationRequest.Create(CommonTestData.REFERENCE, CommonTestData.RELYING_PARTY_ID);
            try
            {
                IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                            .SetHttpService(CommonTestData.HttpServiceMock.Object)
                            .SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();

                CommonTestData.HttpServiceMock.Setup(x => x.Send<EmptyFrejaResponse>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<CancelAuthenticationRequest>(), It.IsAny<string>())).Throws(new FrejaEidException(INVALID_REFERENCE_ERROR_MESSAGE, INVALID_REFERENCE_ERROR_CODE));
                authenticationClient.Cancel(cancelAuthenticationRequest);
                Assert.Fail("Test should throw exception!");
            }
            catch (FrejaEidException rpEx)
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<EmptyFrejaResponse>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_CANCEL), RequestTemplate.CANCEL_AUTHENTICATION_TEMPLATE, cancelAuthenticationRequest, CommonTestData.RELYING_PARTY_ID));
                Assert.AreEqual(INVALID_REFERENCE_ERROR_CODE, rpEx.ErrorCode);
                Assert.AreEqual(INVALID_REFERENCE_ERROR_MESSAGE, rpEx.Message);
            }
        }

    }
}
