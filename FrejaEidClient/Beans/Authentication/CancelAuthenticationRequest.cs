using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class CancelAuthenticationRequest : RelyingPartyRequest, IEquatable<CancelAuthenticationRequest>
    {
        [JsonConstructor]
        private CancelAuthenticationRequest(string authRef)
        {
            this.AuthRef = authRef;
            this.RelyingPartyId = null;
        }

        private CancelAuthenticationRequest(string authRef, string relyingPartyId)
        {
            this.AuthRef = authRef;
            this.RelyingPartyId = relyingPartyId;
        }

        public static CancelAuthenticationRequest Create(string authRef)
        {
            return new CancelAuthenticationRequest(authRef);
        }

        public static CancelAuthenticationRequest Create(string authRef, string relyingPartyId)
        {
            return new CancelAuthenticationRequest(authRef, relyingPartyId);
        }

        [JsonProperty("authRef")]
        public string AuthRef { get; }

        [JsonIgnore]
        public string RelyingPartyId { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as CancelAuthenticationRequest);
        }

        public bool Equals(CancelAuthenticationRequest other)
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