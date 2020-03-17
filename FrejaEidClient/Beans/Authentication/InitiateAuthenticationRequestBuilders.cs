using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using System.Collections.Generic;
using Com.Verisec.FrejaEid.FrejaEidClient.Util;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class InitiateAuthenticationRequestBuilders
    {
        public const string NOT_APPLICABLE = "N/A";
        public class UserInfoBuilder
        {

            public SetOptionalParamsBuilder SetEmail(string email)
            {
                return new SetOptionalParamsBuilder(UserInfoType.EMAIL, email);
            }

            public SetOptionalParamsBuilder SetSsn(SsnUserInfo ssnUserInfo)
            {
                return new SetOptionalParamsBuilder(UserInfoType.SSN, UserInfoUtil.ConvertSsnUserInfo(ssnUserInfo));
            }
            public SetOptionalParamsBuilder SetPhoneNumber(string phoneNumber)
            {
                return new SetOptionalParamsBuilder(UserInfoType.PHONE, phoneNumber);
            }

            public SetOptionalParamsBuilder SetInferred()
            {
                return new SetOptionalParamsBuilder(UserInfoType.INFERRED, NOT_APPLICABLE);
            }

            public SetOptionalParamsBuilder SetOrganisationId(string identifier)
            {
                return new SetOptionalParamsBuilder(UserInfoType.ORG_ID, identifier);
            }

        }

        public class SetOptionalParamsBuilder
        {

            private readonly UserInfoType userInfoType;
            private readonly string userInfo;
            private MinRegistrationLevel minRegistrationLevel = MinRegistrationLevel.BASIC;
            private SortedSet<AttributeToReturn> attributesToReturn = null;
            private string relyingPartyId = null;

            internal SetOptionalParamsBuilder(UserInfoType userInfoType, string userInfo)
            {
                this.userInfoType = userInfoType;
                this.userInfo = userInfo;
            }

            public SetOptionalParamsBuilder SetMinRegistrationLevel(MinRegistrationLevel minRegistrationLevel)
            {
                this.minRegistrationLevel = minRegistrationLevel;
                return this;
            }

            public SetOptionalParamsBuilder SetAttributesToReturn(params AttributeToReturn[] attributesToReturn)
            {
                this.attributesToReturn = new SortedSet<AttributeToReturn>();
                foreach (AttributeToReturn attributeToReturn in attributesToReturn) {
                    this.attributesToReturn.Add(attributeToReturn);
                }  
                return this;
            }

            public SetOptionalParamsBuilder SetRelyingPartyId(string relyingPartyId)
            {
                this.relyingPartyId = relyingPartyId;
                return this;
            }

            public InitiateAuthenticationRequest Build()
            {
                return new InitiateAuthenticationRequest(userInfoType, userInfo, minRegistrationLevel, attributesToReturn, relyingPartyId);
            }

        }
    }
}

