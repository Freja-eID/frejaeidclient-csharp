using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using Com.Verisec.FrejaEid.FrejaEidClient.Util;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication.Tests
{
    [TestClass]
    public class InitiateAuthenticationRequestTests
    {
        private const string EMAIL = "email";
        private static SsnUserInfo SSN_USER_INFO = SsnUserInfo.Create(Country.SWEDEN, "123123123");
        private const string PHONE_NUMBER = "123123123";
        private const string INFERRED_USER_INFO = "N/A";
        private const MinRegistrationLevel REGISTRATION_STATE = MinRegistrationLevel.EXTENDED;
        private const string RELYING_PARTY_ID = "relyingPartyId";
        private const string ORGANISATION_ID = "orgId";
        private static SortedSet<AttributeToReturn> REQUESTED_ATTRIBUTES = new SortedSet<AttributeToReturn>();

        [TestInitialize]
        public void TestInitialize()
        {
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.SSN);
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.BASIC_USER_INFO);
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.CUSTOM_IDENTIFIER);
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.DATE_OF_BIRTH);
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.EMAIL_ADDRESS);
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID);
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.RELYING_PARTY_USER_ID);
            REQUESTED_ATTRIBUTES.Add(AttributeToReturn.ORGANISATION_ID_IDENTIFIER);
        }

        [TestMethod]
        public void CreateDefaultWithEmailTest()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.EMAIL, EMAIL, MinRegistrationLevel.BASIC, null, null);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateDefaultWithEmail(EMAIL);
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateDefaultWithSsnTest()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.SSN, UserInfoUtil.ConvertSsnUserInfo(SSN_USER_INFO), MinRegistrationLevel.BASIC, null, null);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateDefaultWithSsn(SSN_USER_INFO);
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateCustomRequest_userInfoTypeEmail()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.EMAIL, EMAIL, MinRegistrationLevel.EXTENDED, REQUESTED_ATTRIBUTES, RELYING_PARTY_ID);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom()
                    .SetEmail(EMAIL)
                    .SetMinRegistrationLevel(REGISTRATION_STATE)
                    .SetAttributesToReturn(AttributeToReturn.SSN, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .SetRelyingPartyId(RELYING_PARTY_ID)
                    .Build();
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateCustomRequest_userInfoTypeEmail_defaultRegistrationState()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.EMAIL, EMAIL, MinRegistrationLevel.BASIC, REQUESTED_ATTRIBUTES, RELYING_PARTY_ID);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom()
                    .SetEmail(EMAIL)
                    .SetAttributesToReturn(AttributeToReturn.SSN, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .SetRelyingPartyId(RELYING_PARTY_ID)
                    .Build();
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateCustomRequest_userInfoTypeSsn()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.SSN, UserInfoUtil.ConvertSsnUserInfo(SSN_USER_INFO), MinRegistrationLevel.EXTENDED, REQUESTED_ATTRIBUTES, RELYING_PARTY_ID);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom()
                    .SetSsn(SSN_USER_INFO)
                    .SetAttributesToReturn(AttributeToReturn.SSN, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .SetMinRegistrationLevel(REGISTRATION_STATE)
                    .SetRelyingPartyId(RELYING_PARTY_ID)
                    .Build();
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateCustomRequest_userInfoTypePhoneNumber()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.PHONE, PHONE_NUMBER, REGISTRATION_STATE, REQUESTED_ATTRIBUTES, RELYING_PARTY_ID);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom()
                    .SetPhoneNumber(PHONE_NUMBER)
                    .SetAttributesToReturn(AttributeToReturn.SSN, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .SetMinRegistrationLevel(REGISTRATION_STATE)
                    .SetRelyingPartyId(RELYING_PARTY_ID)
                    .Build();
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateCustomRequest_userInfoTypeInferred()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.INFERRED, INFERRED_USER_INFO, REGISTRATION_STATE, REQUESTED_ATTRIBUTES, RELYING_PARTY_ID);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom()
                    .SetInferred()
                    .SetAttributesToReturn(AttributeToReturn.SSN, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .SetMinRegistrationLevel(REGISTRATION_STATE)
                    .SetRelyingPartyId(RELYING_PARTY_ID)
                    .Build();
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateCustomRequest_RelyingPartyIdNull()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.INFERRED, INFERRED_USER_INFO, MinRegistrationLevel.BASIC, REQUESTED_ATTRIBUTES, null);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom()
                    .SetInferred()
                    .SetAttributesToReturn(AttributeToReturn.SSN, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .SetRelyingPartyId(null)
                    .Build();
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        [TestMethod]
        public void CreateCustomRequest_userInfoOrganisationId()
        {
            InitiateAuthenticationRequest expectedInitiateAuthenticationRequest = new InitiateAuthenticationRequest(UserInfoType.ORG_ID, ORGANISATION_ID, MinRegistrationLevel.EXTENDED, REQUESTED_ATTRIBUTES, RELYING_PARTY_ID);
            InitiateAuthenticationRequest initiateAuthenticationRequest = InitiateAuthenticationRequest.CreateCustom()
                    .SetOrganisationId(ORGANISATION_ID)
                    .SetAttributesToReturn(AttributeToReturn.SSN, AttributeToReturn.BASIC_USER_INFO, AttributeToReturn.CUSTOM_IDENTIFIER, AttributeToReturn.DATE_OF_BIRTH, AttributeToReturn.EMAIL_ADDRESS, AttributeToReturn.INTEGRATOR_SPECIFIC_USER_ID, AttributeToReturn.RELYING_PARTY_USER_ID, AttributeToReturn.ORGANISATION_ID_IDENTIFIER)
                    .SetMinRegistrationLevel(REGISTRATION_STATE)
                    .SetRelyingPartyId(RELYING_PARTY_ID)
                    .Build();
            AssertRequestsAreEqual(expectedInitiateAuthenticationRequest, initiateAuthenticationRequest);
        }

        private void AssertRequestsAreEqual(InitiateAuthenticationRequest expectedInitiateAuthenticationRequest, InitiateAuthenticationRequest initiateAuthenticationRequest)
        {
            Assert.AreEqual(expectedInitiateAuthenticationRequest.UserInfoType, initiateAuthenticationRequest.UserInfoType);
            Assert.AreEqual(expectedInitiateAuthenticationRequest.UserInfo, initiateAuthenticationRequest.UserInfo);
            Assert.AreEqual(expectedInitiateAuthenticationRequest.MinRegistrationLevel, initiateAuthenticationRequest.MinRegistrationLevel);
            SortedSet<AttributeToReturn>.CreateSetComparer().Equals(initiateAuthenticationRequest.AttributesToReturn, expectedInitiateAuthenticationRequest.AttributesToReturn);
            Assert.AreEqual(expectedInitiateAuthenticationRequest.RelyingPartyId, initiateAuthenticationRequest.RelyingPartyId);
        }
    }
}