using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.General
{
    public class RequestedAttributes : IEquatable<RequestedAttributes>
    {
        public RequestedAttributes(BasicUserInfo basicUserInfo, string customIdentifier, SsnUserInfo ssn, string integratorSpecificUserId,
                                   string dateOfBirth, string relyingPartyUserId, string emailAddress, string organisationIdIdentifier)
        {
            this.BasicUserInfo = basicUserInfo;
            this.CustomIdentifier = customIdentifier;
            this.Ssn = ssn;
            this.IntegratorSpecificUserId = integratorSpecificUserId;
            this.DateOfBirth = dateOfBirth;
            this.RelyingPartyUserId = relyingPartyUserId;
            this.EmailAddress = emailAddress;
            this.OrganisationIdIdentifier = organisationIdIdentifier;
        }

        [JsonProperty("basicUserInfo")]
        public BasicUserInfo BasicUserInfo { get; }

        [JsonProperty("customIdentifier")]
        public string CustomIdentifier { get; }

        [JsonProperty("ssn")]
        public SsnUserInfo Ssn { get; }

        [JsonProperty("integratorSpecificUserId")]
        public string IntegratorSpecificUserId { get; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; }

        [JsonProperty("relyingPartyUserId")]
        public string RelyingPartyUserId { get; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; }

        [JsonProperty("organisationIdIdentifier")]
        public string OrganisationIdIdentifier { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RequestedAttributes);
        }

        public bool Equals(RequestedAttributes other)
        {
            return other != null &&
                   EqualityComparer<BasicUserInfo>.Default.Equals(BasicUserInfo, other.BasicUserInfo) &&
                   CustomIdentifier == other.CustomIdentifier &&
                   EqualityComparer<SsnUserInfo>.Default.Equals(Ssn, other.Ssn) &&
                   IntegratorSpecificUserId == other.IntegratorSpecificUserId &&
                   DateOfBirth == other.DateOfBirth &&
                   RelyingPartyUserId == other.RelyingPartyUserId &&
                   EmailAddress == other.EmailAddress &&
                   OrganisationIdIdentifier == other.OrganisationIdIdentifier;
        }

        public override int GetHashCode()
        {
            var hashCode = -2139740656;
            hashCode = hashCode * -1521134295 + EqualityComparer<BasicUserInfo>.Default.GetHashCode(BasicUserInfo);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomIdentifier);
            hashCode = hashCode * -1521134295 + EqualityComparer<SsnUserInfo>.Default.GetHashCode(Ssn);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IntegratorSpecificUserId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DateOfBirth);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelyingPartyUserId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EmailAddress);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OrganisationIdIdentifier);
            return hashCode;
        }
    }
}
