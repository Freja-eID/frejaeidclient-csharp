using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.Verisec.FrejaEid.FrejaEidClient.Client;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using Com.Verisec.FrejaEid.FrejaEidClientTest.Utils;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Services.Tests
{
    [TestClass]
    public class RequestValidationServiceTests
    {

        private const string IDENTIFIER = "identifier";
        private const string EMAIL = "email";
        private const string REFERENCE = "reference";

        private const string TEST_URL = "test";

        private static IAuthenticationClient authenticationClient;


        [TestInitialize]
        public void TestInitialize()
        {
            authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                .SetTestModeCustomUrl(TEST_URL).SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();
        }

        [TestMethod]
        public void CancelRequest_missingReference_expectError()
        {
            CancelAuthenticationRequest cancelAuthenticationRequest = CancelAuthenticationRequest.Create("");
            ValidateRequest_expectError("Reference cannot be null or empty.", () => authenticationClient.Cancel(cancelAuthenticationRequest));

        }

        [TestMethod]
        public void CancelRequest_referenceNull_expectError()
        {
            CancelAuthenticationRequest cancelAuthenticationRequest = CancelAuthenticationRequest.Create(null);
            ValidateRequest_expectError("Reference cannot be null or empty.", () => authenticationClient.Cancel(cancelAuthenticationRequest));
        }

        [TestMethod]
        public void CancelRequest_relyingPartyIdEmpty_expectError()
        {
            CancelAuthenticationRequest cancelAuthenticationRequest = CancelAuthenticationRequest.Create(REFERENCE, "");
            ValidateRequest_expectError("RelyingPartyId cannot be empty.", () => authenticationClient.Cancel(cancelAuthenticationRequest));
        }

        [TestMethod]
        public void CancelRequest_requestNull_expectError()
        {
            ValidateRequest_expectError("Request cannot be null value.", () => authenticationClient.Cancel(null));
        }

        [TestMethod]
        public void GetResult_missingReference_expectError()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create("");
            ValidateRequest_expectError("Reference cannot be null or empty.", () => authenticationClient.GetResult(authenticationResultRequest));
        }

        [TestMethod]
        public void GetResult_referenceNull_expectError()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(null);
            ValidateRequest_expectError("Reference cannot be null or empty.", () => authenticationClient.GetResult(authenticationResultRequest));
        }

        [TestMethod]
        public void GetResult_requestNull_expectError()
        {
            ValidateRequest_expectError("Request cannot be null value.", () => authenticationClient.GetResult(null));
        }

        [TestMethod]
        public void GetResult_relyingPartyIdEmpty_expectError()
        {
            AuthenticationResultRequest authenticationResultRequest = AuthenticationResultRequest.Create(REFERENCE, "");
            ValidateRequest_expectError("RelyingPartyId cannot be empty.", () => authenticationClient.GetResult(authenticationResultRequest));
        }

        [TestMethod]
        public void GetResults_requestNull_expectError()
        {
            ValidateRequest_expectError("Request cannot be null value.", () => authenticationClient.GetResults(null));
        }

        [TestMethod]
        public void GetResults_relyingPartyIdEmpty_expectError()
        {
            AuthenticationResultsRequest authenticationResultsRequest = AuthenticationResultsRequest.Create("");
            ValidateRequest_expectError("RelyingPartyId cannot be empty.", () => authenticationClient.GetResults(authenticationResultsRequest));
        }

        [TestMethod]
        public void InitAuth_requestNull()
        {
            ValidateRequest_expectError("Request cannot be null value.", () => authenticationClient.Initiate(null));
        }

        [TestMethod]
        public void InitAuth_relyingPartyIdEmpty()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetEmail(EMAIL).SetRelyingPartyId("").Build();
            ValidateRequest_expectError("RelyingPartyId cannot be empty.", () => authenticationClient.Initiate(initiateAuthenticationRequest));
        }

        [TestMethod]
        public void InitAuth_userInfoEmpty()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetEmail("").Build();
            ValidateRequest_expectError("UserInfo cannot be null or empty.", () => authenticationClient.Initiate(initiateAuthenticationRequest));
        }

        [TestMethod]
        public void InitAuth_userInfoNull()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetEmail(null).Build();
            ValidateRequest_expectError("UserInfo cannot be null or empty.", () => authenticationClient.Initiate(initiateAuthenticationRequest));
        }

        [TestMethod]
        public void InitAuth_requestedAttributesEmpty()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetEmail(EMAIL).SetAttributesToReturn().Build();
            ValidateRequest_expectError("RequestedAttributes cannot be empty.", () => authenticationClient.Initiate(initiateAuthenticationRequest));
        }

        [TestMethod]
        public void InitAuth_perosnal_setOrgId()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetOrganisationId(IDENTIFIER).SetAttributesToReturn().Build();
            ValidateRequest_expectError("UserInfoType ORG ID cannot be used in personal context.", () => authenticationClient.Initiate(initiateAuthenticationRequest));
        }

        private void ValidateRequest_expectError(string errorMessage, Action authenticationMethod)
        {
            try
            {
                authenticationMethod();
                Assert.Fail("Test should throw exception!");
            }
            catch (FrejaEidClientInternalException ex)
            {
                Assert.AreEqual(errorMessage, ex.Message);
            }
        }
    }
}