using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class AuthenticationResultsRequest : RelyingPartyRequest, IEquatable<AuthenticationResultsRequest>
    {
        public const string INCLUDE_PREVIOUS = "ALL";

        public static AuthenticationResultsRequest Create()
        {
            return new AuthenticationResultsRequest(INCLUDE_PREVIOUS);
        }

        public static AuthenticationResultsRequest Create(string relyingPartyId)
        {
            return new AuthenticationResultsRequest(INCLUDE_PREVIOUS, relyingPartyId);
        }

        [JsonConstructor]
        private AuthenticationResultsRequest(string includePrevious) : this(INCLUDE_PREVIOUS, null) { }

        private AuthenticationResultsRequest(string includePrevious, string relyingPartyId)
        {
            this.IncludePrevious = includePrevious;
            this.RelyingPartyId = relyingPartyId;
        }

        [JsonProperty("includePrevious")]
        public string IncludePrevious { get; }

        [JsonIgnore]
        public string RelyingPartyId { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AuthenticationResultsRequest);
        }

        public bool Equals(AuthenticationResultsRequest other)
        {
            return other != null &&
                   IncludePrevious == other.IncludePrevious &&
                   RelyingPartyId == other.RelyingPartyId;
        }

        public override int GetHashCode()
        {
            var hashCode = -51539528;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IncludePrevious);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelyingPartyId);
            return hashCode;
        }

    }
}