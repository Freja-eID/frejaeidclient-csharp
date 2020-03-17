using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class AuthenticationResults : FrejaHttpResponse, IEquatable<AuthenticationResults>
    {
        public AuthenticationResults(List<AuthenticationResult> authenticationResults)
        {
            this.AllAuthenticationResults = authenticationResults;
        }

        [JsonProperty("authenticationResults")]
        public List<AuthenticationResult> AllAuthenticationResults { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AuthenticationResults);
        }

        public bool Equals(AuthenticationResults other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<List<AuthenticationResult>>.Default.Equals(AllAuthenticationResults, other.AllAuthenticationResults);
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            hashCode = hashCode * 1106319638 + EqualityComparer<List<AuthenticationResult>>.Default.GetHashCode(AllAuthenticationResults);
            return hashCode;
        }
    }
}