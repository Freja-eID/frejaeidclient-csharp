using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class InitiateAuthenticationResponse : FrejaHttpResponse, IEquatable<InitiateAuthenticationResponse>
    {
        public InitiateAuthenticationResponse(string authRef)
        {
            this.AuthRef = authRef;
        }

        [JsonProperty("authRef")]
        public string AuthRef { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as InitiateAuthenticationResponse);
        }

        public bool Equals(InitiateAuthenticationResponse other)
        {
            return other != null &&
                   base.Equals(other) &&
                   AuthRef == other.AuthRef;
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            hashCode = hashCode * 449170272 + EqualityComparer<string>.Default.GetHashCode(AuthRef);
            return hashCode;
        }
    }
}