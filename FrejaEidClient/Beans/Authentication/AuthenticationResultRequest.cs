using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class AuthenticationResultRequest : RelyingPartyRequest, IEquatable<AuthenticationResultRequest>
    {
        [JsonConstructor]
        private AuthenticationResultRequest(string authRef) : this(authRef, null) { }

        private AuthenticationResultRequest(string authRef, string relyingPartyId)
        {
            this.AuthRef = authRef;
            this.RelyingPartyId = relyingPartyId;
        }

        public static AuthenticationResultRequest Create(string authRef)
        {
            return new AuthenticationResultRequest(authRef);
        }

        public static AuthenticationResultRequest Create(string authRef, string relyingPartyId)
        {
            return new AuthenticationResultRequest(authRef, relyingPartyId);
        }

        [JsonProperty("authRef")]
        public string AuthRef { get; }

        [JsonIgnore]
        public string RelyingPartyId { get; }


        public override bool Equals(object obj)
        {
            return Equals(obj as AuthenticationResultRequest);
        }

        public bool Equals(AuthenticationResultRequest other)
        {
            return other != null &&
                   AuthRef == other.AuthRef &&
                   RelyingPartyId == other.RelyingPartyId;
        }

        public override int GetHashCode()
        {
            var hashCode = 1197659710;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AuthRef);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelyingPartyId);
            return hashCode;
        }
    }
}