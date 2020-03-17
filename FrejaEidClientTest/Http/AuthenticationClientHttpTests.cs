using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Client;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Services;
using Com.Verisec.FrejaEid.FrejaEidClientTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Http.Tests
{
    [TestClass]
    public class AuthenticationClientHttpTests : CommonHttpTest
    {
        private static readonly string CUSTOM_URL = "http://localhost:" + MOCK_SERVICE_PORT;
        private static InitiateAuthenticationResponse initiateAuthenticationResponse;
        private static AuthenticationResult authenticationResult;
        private static AuthenticationResult authenticationResultWithRequestedAttributes;
        private static AuthenticationResults authenticationResults;
        private static IAuthenticationClient authenticationClient;

        [TestInitialize]
        public void TestInitialize()
        {
            jsonService = new JsonService();
            initiateAuthenticationResponse = new InitiateAuthenticationResponse(REFERENCE);
            authenticationResult = new AuthenticationResult(REFERENCE, TransactionStatus.STARTED, null, null);
            authenticationResultWithRequestedAttributes = new AuthenticationResult(REFERENCE, TransactionStatus.APPROVED, DETAILS, REQUESTED_ATTRIBUTES);
            authenticationResults = new AuthenticationResults(new List<AuthenticationResult> { authenticationResult });
            authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                    .SetTestModeCustomUrl(CUSTOM_URL).SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();
            StopServer();
        }

        [TestMethod]
        public void InitAuth_success()
        {
            String initAuthResponseString = jsonService.SerializeToJson(initiateAuthenticationResponse);

            InitiateAuthenticationRequest initiateAuthenticationRequestDefaultWithEmail = InitiateAuthenticationRequest.CreateDefaultWithEmail(EMAIL);
            SendInitAuthRequestAndAssertResponse(initiateAuthenticationRequestDefaultWithEmail, initAuthResponseString);

            InitiateAuthenticationRequest initiateAuthenticationRequestDefaultWithSsn = InitiateAuthenticationRequest.CreateDefaultWithSsn(SsnUserInfo.Create(Country.NORWAY, SSN));
            SendInitAuthRequestAndAssertResponse(initiateAuthenticationRequestDefaultWithSsn, initAuthResponseString);

            InitiateAuthenticationRequest initAuthenticationRequestWithRequestedAttributesUserInfoEmail = InitiateAuthenticationRequest.CreateCustom()
                    .SetEmail(EMAIL)
                    .SetAttributesToReturn(AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.SSN, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID)
                    .Build();
            SendInitAuthRequestAndAssertResponse(initAuthenticationRequestWithRequestedAttributesUserInfoEmail, initAuthResponseString);

            InitiateAuthenticationRequest initAuthenticationRequestWithRequestedAttributesUserInfoPhoneNum = InitiateAuthenticationRequest.CreateCustom()
                    .SetPhoneNumber(EMAIL)
                    .SetAttributesToReturn(AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.SSN, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID)
                    .Build();
            SendInitAuthRequestAndAssertResponse(initAuthenticationRequestWithRequestedAttributesUserInfoPhoneNum, initAuthResponseString);

            InitiateAuthenticationRequest initAuthenticationRequestWithRequestedAttributesUserInfoSsn = InitiateAuthenticationRequest.CreateCustom()
                    .SetSsn(SsnUserInfo.Create(Country.NORWAY, SSN))
                    .SetAttributesToReturn(AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.SSN, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID)
                    .Build();
            SendInitAuthRequestAndAssertResponse(initAuthenticationRequestWithRequestedAttributesUserInfoSsn, initAuthResponseString);

            InitiateAuthenticationRequest initAuthenticationRequestWithRequestedAttributesUserInfoInferred = InitiateAuthenticationRequest.CreateCustom()
                    .SetInferred()
                    .SetAttributesToReturn(AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.SSN, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID)
                    .Build();
            SendInitAuthRequestAndAssertResponse(initAuthenticationRequestWithRequestedAttributesUserInfoInferred, initAuthResponseString);

            InitiateAuthenticationRequest initAuthenticationRequestWithRegistrationStateAndRelyingPartyId = InitiateAuthenticationRequest.CreateCustom()
                    .SetEmail(EMAIL)
                    .SetMinRegistrationLevel(MinRegistrationLevel.EXTENDED)
                    .SetRelyingPartyId(RELYING_PARTY_ID)
                    .Build();
            InitiateAuthenticationRequest expectedInitAuthenticationRequestWithRegistrationStateAndRelyingPartyId = InitiateAuthenticationRequest.CreateCustom()
                    .SetEmail(EMAIL)
                    .SetMinRegistrationLevel(MinRegistrationLevel.EXTENDED)
                    .Build();

            SendInitAuthRequestAndAssertResponse(expectedInitAuthenticationRequestWithRegistrationStateAndRelyingPartyId, initAuthenticationRequestWithRegistrationStateAndRelyingPartyId, initAuthResponseString);

        }

        [TestMethod]
        public void InitAuth_organisationalTransaction_success()
        {
            AuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                    .SetTestModeCustomUrl(CUSTOM_URL).SetTransactionContext(TransactionContext.ORGANISATIONAL).Build<AuthenticationClient>();
            InitiateAuthenticationRequest initAuthenticationRequestWithRequestedAttributesUserInfoOrganisationId = InitiateAuthenticationRequest.CreateCustom()
                    .SetOrganisationId(ORGANISATION_ID)
                    .SetAttributesToReturn(AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.SSN, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .Build();

            String initAuthResponseString = jsonService.SerializeToJson(initiateAuthenticationResponse);
            StartMockServer(initAuthenticationRequestWithRequestedAttributesUserInfoOrganisationId, (int)HttpStatusCode.OK, initAuthResponseString);
            String reference = authenticationClient.Initiate(initAuthenticationRequestWithRequestedAttributesUserInfoOrganisationId);
            StopServer();
            Assert.AreEqual(REFERENCE, reference);

        }

        private void SendInitAuthRequestAndAssertResponse(InitiateAuthenticationRequest validRequest, String initAuthResponseString)
        {
            SendInitAuthRequestAndAssertResponse(validRequest, validRequest, initAuthResponseString);
        }

        private void SendInitAuthRequestAndAssertResponse(InitiateAuthenticationRequest expectedRequest, InitiateAuthenticationRequest validRequest, String initAuthResponseString)
        {
            StartMockServer(expectedRequest, (int)HttpStatusCode.OK, initAuthResponseString);
            String reference = authenticationClient.Initiate(validRequest);
            StopServer();
            Assert.AreEqual(REFERENCE, reference);
        }


        [TestMethod]
        public void GetAuthResult_sendRequestWithoutRelyingPartyId_success()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(REFERENCE);
            String authenticationResultResponseString = jsonService.SerializeToJson(authenticationResult);

            StartMockServer(authenticationResultRequest, (int)HttpStatusCode.OK, authenticationResultResponseString);

            AuthenticationResult response = authenticationClient.GetResult(authenticationResultRequest);
            Assert.AreEqual(REFERENCE, response.AuthRef);
        }

        [TestMethod]
        public void GetAuthResult_sendRequestWithRelyingPartyId_success()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(REFERENCE, RELYING_PARTY_ID);
            String authenticationResultResponseString = jsonService.SerializeToJson(authenticationResult);
            AuthenticationResultRequest expectedAuthenticationResultRequest = AuthenticationResultRequest.Create(REFERENCE);
            StartMockServer(expectedAuthenticationResultRequest, (int)HttpStatusCode.OK, authenticationResultResponseString);

            AuthenticationResult response = authenticationClient.GetResult(authenticationResultRequest);
            Assert.AreEqual(REFERENCE, response.AuthRef);
        }

        [TestMethod]
        public void GetAuthResult_sendRequestWithRelyingPartyId_withRequestedAttributes_success()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(REFERENCE, RELYING_PARTY_ID);
            String authenticationResultResponseString = jsonService.SerializeToJson(authenticationResultWithRequestedAttributes);
            AuthenticationResultRequest expectedAuthenticationResultRequest = AuthenticationResultRequest.Create(REFERENCE);
            StartMockServer(expectedAuthenticationResultRequest, (int)HttpStatusCode.OK, authenticationResultResponseString);

            AuthenticationResult response = authenticationClient.GetResult(authenticationResultRequest);
            Assert.AreEqual(REFERENCE, response.AuthRef);
        }

        [TestMethod]
        public void GetAuthResults_sendRequestWithoutRelyingPartyId_success()
        {
            AuthenticationResultsRequest authenticationResultsRequest = AuthenticationResultsRequest.Create();
            String authenticationResultsResponseString = jsonService.SerializeToJson(authenticationResults);

            StartMockServer(authenticationResultsRequest, (int)HttpStatusCode.OK, authenticationResultsResponseString);

            List<AuthenticationResult> response = authenticationClient.GetResults(authenticationResultsRequest);
            Assert.AreEqual(REFERENCE, response[0].AuthRef);
        }

        [TestMethod]
        public void GetAuthResults_sendRequestWithRelyingPartyId_success()
        {
            AuthenticationResultsRequest authenticationResultsRequest = AuthenticationResultsRequest.Create(RELYING_PARTY_ID);
            String authenticationResultsResponseString = jsonService.SerializeToJson(authenticationResults);
            AuthenticationResultsRequest expectedAuthenticationResultsRequest = AuthenticationResultsRequest.Create();
            StartMockServer(expectedAuthenticationResultsRequest, (int)HttpStatusCode.OK, authenticationResultsResponseString);
            List<AuthenticationResult> response = authenticationClient.GetResults(authenticationResultsRequest);
            Assert.AreEqual(REFERENCE, response[0].AuthRef);
        }

        [TestMethod]
        public void CancelAuth_sendRequestWithRelyingPartyId_success()
        {
            CancelAuthenticationRequest cancelRequest = CancelAuthenticationRequest.Create(REFERENCE, RELYING_PARTY_ID);
            SendAndValidateCancelRequest(cancelRequest);
        }

        [TestMethod]
        public void CancelAuth_sendRequestWithoutRelyingPartyId_success()
        {
            CancelAuthenticationRequest cancelRequest = CancelAuthenticationRequest.Create(REFERENCE);
            SendAndValidateCancelRequest(cancelRequest);
        }

        private void SendAndValidateCancelRequest(CancelAuthenticationRequest cancelAuthenticationRequest)
        {
            CancelAuthenticationRequest expectedCancelRequest = CancelAuthenticationRequest.Create(REFERENCE);
            String responseString = jsonService.SerializeToJson(new EmptyFrejaResponse());

            StartMockServer(expectedCancelRequest, (int)HttpStatusCode.OK, responseString);

            authenticationClient.Cancel(cancelAuthenticationRequest);
        }

        [TestCleanup]
        public void Cleanup()
        {
            StopServer();
        }

    }
}
