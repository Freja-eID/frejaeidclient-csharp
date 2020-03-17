using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClientTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Client.Impl.Tests
{
    [TestClass]
    public class AuthenticationClientGetResultTests
    {
        private const string POLLING_TIMEOUT_ERROR_MESSAGE = "A timeout of 2s was reached while sending request.";
        private const int WAITING_TIME_IN_SEC = 10000;
        private const string INVALID_REFERENCE_ERROR_MESSAGE = "Invalid reference (for example, nonexistent or expired).";
        private const int INVALID_REFERENCE_ERROR_CODE = 1100;

        [TestMethod]
        public void GetAuthenticationResultRelyignPartyIdNull_success()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(CommonTestData.REFERENCE);
            GetAuthenticationResult(authenticationResultRequest, TransactionContext.PERSONAL, null);
        }

        [TestMethod]
        public void GetAuthenticationResultPersonal_success()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(CommonTestData.REFERENCE, CommonTestData.RELYING_PARTY_ID);
            GetAuthenticationResult(authenticationResultRequest, TransactionContext.PERSONAL, CommonTestData.RELYING_PARTY_ID);
        }

        [TestMethod]
        public void GetAuthenticationResultOrganisational_success()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(CommonTestData.REFERENCE, CommonTestData.RELYING_PARTY_ID);
            GetAuthenticationResult(authenticationResultRequest, TransactionContext.ORGANISATIONAL, CommonTestData.RELYING_PARTY_ID);

        }
        private void GetAuthenticationResult(AuthenticationResultRequest authenticationResultRequest, TransactionContext transactionContext, string relyingPartyId)
        {
            AuthenticationResult expectedResponse = new AuthenticationResult(CommonTestData.REFERENCE, TransactionStatus.STARTED, CommonTestData.DETAILS, CommonTestData.REQUESTED_ATTRIBUTES);
            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                                         .SetHttpService(CommonTestData.HttpServiceMock.Object)
                                         .SetTransactionContext(transactionContext).Build<AuthenticationClient>();
            CommonTestData.HttpServiceMock.Setup(x => x.Send<AuthenticationResult>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<AuthenticationResultRequest>(), It.IsAny<string>())).Returns(expectedResponse);
            AuthenticationResult response = authenticationClient.GetResult(authenticationResultRequest);

            if (transactionContext.Equals(TransactionContext.PERSONAL))
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResult>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_GET_RESULT), RequestTemplate.AUTHENTICATION_RESULT_TEMPLATE, authenticationResultRequest, relyingPartyId));
            }
            else
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResult>(new Uri(FrejaEnvironment.TEST + MethodUrl.ORGANISATION_AUTHENTICATION_GET_ONE_RESULT), RequestTemplate.AUTHENTICATION_RESULT_TEMPLATE, authenticationResultRequest, relyingPartyId));
            }
            Assert.AreEqual(CommonTestData.REFERENCE, response.AuthRef);
            Assert.AreEqual(TransactionStatus.STARTED, response.Status);
            Assert.AreEqual(CommonTestData.DETAILS, response.Details);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES, response.RequestedAttributes);
        }

        [TestMethod]
        public void GetAuthenticationResultInvalidReference_expectInvalidReferenceError()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(CommonTestData.REFERENCE, CommonTestData.RELYING_PARTY_ID);
            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                                         .SetHttpService(CommonTestData.HttpServiceMock.Object)
                                         .SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();
            try
            {
                CommonTestData.HttpServiceMock.Setup(x => x.Send<AuthenticationResult>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<AuthenticationResultRequest>(), It.IsAny<string>())).Throws(new FrejaEidException(INVALID_REFERENCE_ERROR_MESSAGE, INVALID_REFERENCE_ERROR_CODE));
                AuthenticationResult response = authenticationClient.GetResult(authenticationResultRequest);
                Assert.Fail("Test should throw exception!");
            }
            catch (FrejaEidException rpEx)
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResult>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_GET_RESULT), RequestTemplate.AUTHENTICATION_RESULT_TEMPLATE, authenticationResultRequest, CommonTestData.RELYING_PARTY_ID));
                Assert.AreEqual(INVALID_REFERENCE_ERROR_CODE, rpEx.ErrorCode);
                Assert.AreEqual(INVALID_REFERENCE_ERROR_MESSAGE, rpEx.Message);
            }
        }

        [TestMethod]
        public void PollForResultRelyingPartyIdNull_finalResponseRejected_success()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(CommonTestData.REFERENCE);
            PollForResultFinalResponseRejected(authenticationResultRequest, TransactionContext.PERSONAL, null);

        }

        [TestMethod]
        public void PollForResultRelyingPartyIdNotNull_finalResponseRejected_success()
        {

            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(CommonTestData.REFERENCE, CommonTestData.RELYING_PARTY_ID);
            PollForResultFinalResponseRejected(authenticationResultRequest, TransactionContext.ORGANISATIONAL, CommonTestData.RELYING_PARTY_ID);

        }

        private void PollForResultFinalResponseRejected(AuthenticationResultRequest authenticationResultRequest, TransactionContext transactionContext, string relyingPartyId)
        {

            AuthenticationResult expectedResponse = new AuthenticationResult(CommonTestData.DETAILS, TransactionStatus.REJECTED, CommonTestData.DETAILS, null);
            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                                .SetHttpService(CommonTestData.HttpServiceMock.Object)
                                .SetTransactionContext(transactionContext).Build<AuthenticationClient>();

            CommonTestData.HttpServiceMock.Setup(x => x.Send<AuthenticationResult>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<AuthenticationResultRequest>(), It.IsAny<string>())).Returns(expectedResponse);
            AuthenticationResult response = authenticationClient.PollForResult(authenticationResultRequest, WAITING_TIME_IN_SEC);
            if (transactionContext.Equals(TransactionContext.PERSONAL))
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResult>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_GET_RESULT), RequestTemplate.AUTHENTICATION_RESULT_TEMPLATE, authenticationResultRequest, relyingPartyId));
            }
            else
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResult>(new Uri(FrejaEnvironment.TEST + MethodUrl.ORGANISATION_AUTHENTICATION_GET_ONE_RESULT), RequestTemplate.AUTHENTICATION_RESULT_TEMPLATE, authenticationResultRequest, relyingPartyId));
            }
            Assert.AreEqual(expectedResponse.Status, response.Status);
        }

        [TestMethod]
        public void PollForResultRequestTimeout_expectTimeoutError()
        {
            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                                .SetHttpService(CommonTestData.HttpServiceMock.Object)
                                .SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(CommonTestData.REFERENCE);
            try
            {
                CommonTestData.HttpServiceMock.Setup(x => x.Send<AuthenticationResult>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<AuthenticationResultRequest>(), It.IsAny<string>())).Throws(new FrejaEidClientPollingException(POLLING_TIMEOUT_ERROR_MESSAGE));
                AuthenticationResult response = authenticationClient.PollForResult(authenticationResultRequest, WAITING_TIME_IN_SEC);
                Assert.Fail("Test should throw exception!");
            }
            catch (FrejaEidClientPollingException ex)
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResult>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_GET_RESULT), RequestTemplate.AUTHENTICATION_RESULT_TEMPLATE, authenticationResultRequest, null));
                Assert.AreEqual(POLLING_TIMEOUT_ERROR_MESSAGE, ex.Message);
            }
        }

        [TestMethod]
        public void GetAuthenticationResultsRelyingPartyIdNull_success()
        {
            AuthenticationResultsRequest authenticationResultsRequest = AuthenticationResultsRequest.Create();
            GetAuthenticationResults(authenticationResultsRequest, TransactionContext.PERSONAL, null);

        }

        [TestMethod]
        public void GetAuthenticationResultsRelyingPartyIdNotNull_success()
        {
            AuthenticationResultsRequest authenticationResultsRequest = AuthenticationResultsRequest.Create(CommonTestData.RELYING_PARTY_ID);
            GetAuthenticationResults(authenticationResultsRequest, TransactionContext.ORGANISATIONAL, CommonTestData.RELYING_PARTY_ID);

        }
        private void GetAuthenticationResults(AuthenticationResultsRequest getAuthenticationResultsRequest, TransactionContext transactionContext, string relyingPartyId)
        {
            AuthenticationResults expectedResponse = PrepareAuthenticationResultsResponse();
            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                            .SetHttpService(CommonTestData.HttpServiceMock.Object)
                            .SetTransactionContext(transactionContext).Build<AuthenticationClient>();

            CommonTestData.HttpServiceMock.Setup(x => x.Send<AuthenticationResults>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<AuthenticationResultsRequest>(), It.IsAny<string>())).Returns(expectedResponse);
            List<AuthenticationResult> response = authenticationClient.GetResults(getAuthenticationResultsRequest);

            if (transactionContext.Equals(TransactionContext.PERSONAL))
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResults>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_GET_RESULTS), RequestTemplate.AUTHENTICATION_RESULTS_TEMPLATE, getAuthenticationResultsRequest, relyingPartyId));
            }
            else
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<AuthenticationResults>(new Uri(FrejaEnvironment.TEST + MethodUrl.ORGANISATION_AUTHENTICATION_GET_RESULTS), RequestTemplate.AUTHENTICATION_RESULTS_TEMPLATE, getAuthenticationResultsRequest, relyingPartyId));
            }
            AuthenticationResult first = response[0];
            Assert.AreEqual(CommonTestData.REFERENCE, first.AuthRef);
            Assert.AreEqual(TransactionStatus.STARTED, first.Status);
            Assert.AreEqual(CommonTestData.DETAILS, first.Details);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.BasicUserInfo.Name, first.RequestedAttributes.BasicUserInfo.Name);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.BasicUserInfo.Surname, first.RequestedAttributes.BasicUserInfo.Surname);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.DateOfBirth, first.RequestedAttributes.DateOfBirth);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.EmailAddress, first.RequestedAttributes.EmailAddress);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.CustomIdentifier, first.RequestedAttributes.CustomIdentifier);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.Ssn.Country, first.RequestedAttributes.Ssn.Country);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.Ssn.Ssn, first.RequestedAttributes.Ssn.Ssn);
            Assert.AreEqual(CommonTestData.REQUESTED_ATTRIBUTES.OrganisationIdIdentifier, first.RequestedAttributes.OrganisationIdIdentifier);
            AuthenticationResult second = response[1];
            Assert.AreEqual(CommonTestData.REFERENCE, second.AuthRef);
            Assert.AreEqual(TransactionStatus.DELIVERED_TO_MOBILE, second.Status);
            Assert.AreEqual(CommonTestData.DETAILS, second.Details);
            Assert.AreEqual(CommonTestData.CUSTOM_IDENTIFIER, second.RequestedAttributes.CustomIdentifier);
        }

        private AuthenticationResults PrepareAuthenticationResultsResponse()
        {
            RequestedAttributes attributes1 = new RequestedAttributes(CommonTestData.BASIC_USER_INFO, CommonTestData.CUSTOM_IDENTIFIER, CommonTestData.SSN_USER_INFO, null, CommonTestData.DATE_OF_BIRTH, CommonTestData.RELYING_PARTY_USER_ID, CommonTestData.EMAIL, CommonTestData.ORGANISATION_ID);
            AuthenticationResult firstResponse = new AuthenticationResult(CommonTestData.REFERENCE, TransactionStatus.STARTED, CommonTestData.DETAILS, attributes1);
            RequestedAttributes attributes2 = new RequestedAttributes(null, CommonTestData.CUSTOM_IDENTIFIER, null, null, null, null, null, null);
            AuthenticationResult secondResponse = new AuthenticationResult(CommonTestData.REFERENCE, TransactionStatus.DELIVERED_TO_MOBILE, CommonTestData.DETAILS, attributes2);
            List<AuthenticationResult> responses = new List<AuthenticationResult> { firstResponse, secondResponse };
            return new AuthenticationResults(responses);
        }
    }
}
