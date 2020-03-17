using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using Com.Verisec.FrejaEid.FrejaEidClient.Http;
using Moq;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Client.Impl.Tests
{
    public static class CommonTestData
    {
        internal static Mock<IHttpService> HttpServiceMock = new Mock<IHttpService>(MockBehavior.Strict);

        internal const string EMAIL = "eid.demo.verisec@gmail.com";
        internal const string SSN = "199207295578";
        internal const string REFERENCE = "123456789012345678";
        internal const string RELYING_PARTY_ID = "relyingPartyId";
        internal const string COUNTRY = Country.SWEDEN;
        internal static SsnUserInfo SSN_USER_INFO = SsnUserInfo.Create(COUNTRY, SSN);
        internal static BasicUserInfo BASIC_USER_INFO = new BasicUserInfo("John", "Fante");
        internal const string CUSTOM_IDENTIFIER = "vejofan";
        internal const string DETAILS = "Ask the dust";
        internal const string RELYING_PARTY_USER_ID = "relyingPartyUserId";
        internal const string DATE_OF_BIRTH = "1987-10-18";
        internal const string ORGANISATION_ID = "vealrad";
        internal static RequestedAttributes REQUESTED_ATTRIBUTES = new RequestedAttributes(BASIC_USER_INFO, CUSTOM_IDENTIFIER, SSN_USER_INFO, null, DATE_OF_BIRTH, RELYING_PARTY_USER_ID, EMAIL, ORGANISATION_ID);
    }
}
