using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
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
    public class AuthenticationClientInitAuthenticationTests
    {
        private const string INVALID_USER_INFO_ERROR_MESSAGE = "Invalid or missing userInfo.";
        private const int INVALID_USER_INFO_ERROR_CODE = 1002;

        [TestMethod]
        public void UserInfoTypeEmail_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateDefaultWithEmail(CommonTestData.EMAIL);
            RelyingPartyNull_success(initiateAuthenticationRequest);
        }
        [TestMethod]
        public void UserInfoTypeSsn_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateDefaultWithSsn(SsnUserInfo.Create(CommonTestData.COUNTRY, CommonTestData.SSN));
            RelyingPartyNull_success(initiateAuthenticationRequest);
        }

        [TestMethod]
        public void UserInfoTypeEmail__requestedAttributes_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetEmail(CommonTestData.EMAIL).SetAttributesToReturn(AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.SSN, AttributeToReturn.ORGANISATION_ID_IDENTIFIER).SetRelyingPartyId(CommonTestData.RELYING_PARTY_ID).Build();
            PersonalContext_relyingPartyNotNull_success(initiateAuthenticationRequest);
        }

        [TestMethod]
        public void UserInfoTypeSsn_requestedAttributes_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetSsn(SsnUserInfo.Create(CommonTestData.COUNTRY, CommonTestData.SSN)).SetAttributesToReturn(AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.SSN, AttributeToReturn.ORGANISATION_ID_IDENTIFIER).SetRelyingPartyId(CommonTestData.RELYING_PARTY_ID).Build();
            PersonalContext_relyingPartyNotNull_success(initiateAuthenticationRequest);
        }

        [TestMethod]
        public void UserInfoTypePhoneNumber__requestedAttributes_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetPhoneNumber(CommonTestData.EMAIL).SetAttributesToReturn(AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.SSN, AttributeToReturn.ORGANISATION_ID_IDENTIFIER).SetRelyingPartyId(CommonTestData.RELYING_PARTY_ID).Build();
            PersonalContext_relyingPartyNotNull_success(initiateAuthenticationRequest);
        }

        [TestMethod]
        public void UserInfoTypeOrganisationId__requestedAttributes_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetOrganisationId(CommonTestData.ORGANISATION_ID).SetAttributesToReturn(AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.SSN, AttributeToReturn.ORGANISATION_ID_IDENTIFIER).SetRelyingPartyId(CommonTestData.RELYING_PARTY_ID).Build();
            RelyingPartyNotNull_success(initiateAuthenticationRequest, TransactionContext.ORGANISATIONAL);
        }

        [TestMethod]
        public void MinRegistrationLevelBasic_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetPhoneNumber(CommonTestData.EMAIL).SetAttributesToReturn(AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.SSN, AttributeToReturn.ORGANISATION_ID_IDENTIFIER).SetRelyingPartyId(CommonTestData.RELYING_PARTY_ID).Build();
            PersonalContext_relyingPartyNotNull_success(initiateAuthenticationRequest);
        }

        [TestMethod]
        public void MinRegistrationLevelPlus_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetEmail(CommonTestData.EMAIL).SetAttributesToReturn(AttributeToReturn.CUSTOM_IDENTIFIER).SetMinRegistrationLevel(MinRegistrationLevel.PLUS).Build();
            RelyingPartyNull_success(initiateAuthenticationRequest);
        }

        [TestMethod]
        public void UserInfoTypeInferred_requestedAttributes_success()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetInferred().Build();
            RelyingPartyNull_success(initiateAuthenticationRequest);
        }

        private void RelyingPartyNull_success(InitiateAuthenticationRequest initiateAuthenticationRequest)
        {
            InitiateAuthenticationResponse expectedResponse = new InitiateAuthenticationResponse(CommonTestData.REFERENCE);
            CommonTestData.HttpServiceMock.Setup(x => x.Send<InitiateAuthenticationResponse>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<InitiateAuthenticationRequest>(), null)).Returns(expectedResponse);

            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                                 .SetHttpService(CommonTestData.HttpServiceMock.Object)
                                 .SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();
            string reference = authenticationClient.Initiate(initiateAuthenticationRequest);
            CommonTestData.HttpServiceMock.Verify(x => x.Send<InitiateAuthenticationResponse>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_INIT), RequestTemplate.INIT_AUTHENTICATION_TEMPLATE, initiateAuthenticationRequest, null));
            Assert.AreEqual(CommonTestData.REFERENCE, reference);
        }

        private void PersonalContext_relyingPartyNotNull_success(InitiateAuthenticationRequest initiateAuthenticationRequest)
        {
            RelyingPartyNotNull_success(initiateAuthenticationRequest, TransactionContext.PERSONAL);
        }

        private void RelyingPartyNotNull_success(InitiateAuthenticationRequest initiateAuthenticationRequest, TransactionContext transactionContext)
        {
            InitiateAuthenticationResponse expectedResponse = new InitiateAuthenticationResponse(CommonTestData.REFERENCE);
            CommonTestData.HttpServiceMock.Setup(x => x.Send<InitiateAuthenticationResponse>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<InitiateAuthenticationRequest>(), It.IsAny<string>())).Returns(expectedResponse);

            IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                                 .SetHttpService(CommonTestData.HttpServiceMock.Object)
                                 .SetTransactionContext(transactionContext).Build<AuthenticationClient>();
            string reference = authenticationClient.Initiate(initiateAuthenticationRequest);
            if (transactionContext.Equals(TransactionContext.PERSONAL))
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<InitiateAuthenticationResponse>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_INIT), RequestTemplate.INIT_AUTHENTICATION_TEMPLATE, initiateAuthenticationRequest, CommonTestData.RELYING_PARTY_ID));
            }
            else
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<InitiateAuthenticationResponse>(new Uri(FrejaEnvironment.TEST + MethodUrl.ORGANISATION_AUTHENTICATION_INIT), RequestTemplate.INIT_AUTHENTICATION_TEMPLATE, initiateAuthenticationRequest, CommonTestData.RELYING_PARTY_ID));
            }

            Assert.AreEqual(CommonTestData.REFERENCE, reference);
        }

        [TestMethod]
        public void InvalidUserInfo_expectError()
        {
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom().SetPhoneNumber(CommonTestData.EMAIL).SetRelyingPartyId(CommonTestData.RELYING_PARTY_ID).Build();
            try
            {
                IAuthenticationClient authenticationClient = AuthenticationClient.Create(SslSettings.Create(TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.KEYSTORE_PATH_PKCS12), TestKeystoreUtil.KEYSTORE_PASSWORD, TestKeystoreUtil.GetKeystorePath(TestKeystoreUtil.CERTIFICATE_PATH)), FrejaEnvironment.TEST)
                                     .SetHttpService(CommonTestData.HttpServiceMock.Object)
                                     .SetTransactionContext(TransactionContext.PERSONAL).Build<AuthenticationClient>();

                CommonTestData.HttpServiceMock.Setup(x => x.Send<InitiateAuthenticationResponse>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<InitiateAuthenticationRequest>(), It.IsAny<string>())).Throws(new FrejaEidException(INVALID_USER_INFO_ERROR_MESSAGE, INVALID_USER_INFO_ERROR_CODE));
                authenticationClient.Initiate(initiateAuthenticationRequest);
                Assert.Fail("Test should throw exception!");
            }
            catch (FrejaEidException rpEx)
            {
                CommonTestData.HttpServiceMock.Verify(x => x.Send<InitiateAuthenticationResponse>(new Uri(FrejaEnvironment.TEST + MethodUrl.AUTHENTICATION_INIT), RequestTemplate.INIT_AUTHENTICATION_TEMPLATE, initiateAuthenticationRequest, CommonTestData.RELYING_PARTY_ID));
                Assert.AreEqual(1002, rpEx.ErrorCode);
                Assert.AreEqual("Invalid or missing userInfo.", rpEx.Message);
            }
        }

    }
}
