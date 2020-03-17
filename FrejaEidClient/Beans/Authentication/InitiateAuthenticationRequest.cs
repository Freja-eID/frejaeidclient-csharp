using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using Com.Verisec.FrejaEid.FrejaEidClient.Util;
using static Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication.InitiateAuthenticationRequestBuilders;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class InitiateAuthenticationRequest : RelyingPartyRequest, IEquatable<InitiateAuthenticationRequest>
    {
        public static InitiateAuthenticationRequest CreateDefaultWithEmail(string email)
        {
            return new InitiateAuthenticationRequest(UserInfoType.EMAIL, email, MinRegistrationLevel.BASIC, null, null);
        }

        public static InitiateAuthenticationRequest CreateDefaultWithSsn(SsnUserInfo ssnUserInfo)
        {
            return new InitiateAuthenticationRequest(UserInfoType.SSN, UserInfoUtil.ConvertSsnUserInfo(ssnUserInfo), MinRegistrationLevel.BASIC, null, null);
        }

        public static UserInfoBuilder CreateCustom()
        {
            return new UserInfoBuilder();
        }

        [JsonConstructor]
        internal InitiateAuthenticationRequest(UserInfoType userInfoType, string userInfo, MinRegistrationLevel minRegistrationLevel, SortedSet<AttributeToReturn> attributesToReturn)
            : this(userInfoType, userInfo, minRegistrationLevel, attributesToReturn, null) { }

        internal InitiateAuthenticationRequest(UserInfoType userInfoType, string userInfo, MinRegistrationLevel minRegistrationLevel, SortedSet<AttributeToReturn> attributesToReturn, string relyingPartyId)
        {
            this.UserInfoType = userInfoType;
            this.UserInfo = userInfo;
            this.MinRegistrationLevel = minRegistrationLevel;
            this.AttributesToReturn = attributesToReturn;
            this.RelyingPartyId = relyingPartyId;
        }

        [JsonProperty("userInfoType")]
        public UserInfoType UserInfoType { get; }

        [JsonProperty("userInfo")]
        public string UserInfo { get; }

        [JsonProperty("minRegistrationLevel")]

        public MinRegistrationLevel MinRegistrationLevel { get; }

        [JsonProperty("attributesToReturn")]
        public SortedSet<AttributeToReturn> AttributesToReturn { get; }

        [JsonIgnore]
        public string RelyingPartyId { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as InitiateAuthenticationRequest);
        }

        public bool Equals(InitiateAuthenticationRequest other)
        {
            return other != null &&
                   UserInfoType == other.UserInfoType &&
                   UserInfo == other.UserInfo &&
                   MinRegistrationLevel == other.MinRegistrationLevel &&
                   SortedSet<AttributeToReturn>.CreateSetComparer().Equals(AttributesToReturn, other.AttributesToReturn) &&
                   RelyingPartyId == other.RelyingPartyId;
        }

        public override int GetHashCode()
        {
            var hashCode = 1528838401;
            hashCode = hashCode * -1521134295 + UserInfoType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserInfo);
            hashCode = hashCode * -1521134295 + MinRegistrationLevel.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<SortedSet<AttributeToReturn>>.Default.GetHashCode(AttributesToReturn);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelyingPartyId);
            return hashCode;
        }
    }
}