using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication
{
    public class AuthenticationResult : FrejaHttpResponse, IEquatable<AuthenticationResult>
    {
        public AuthenticationResult(string authRef, TransactionStatus status, string details, RequestedAttributes requestedAttributes)
        {
            this.AuthRef = authRef;
            this.Status = status;
            this.Details = details;
            this.RequestedAttributes = requestedAttributes;
        }

        [JsonProperty("authRef")]
        public string AuthRef { get; }

        [JsonProperty("status")]
        public TransactionStatus Status { get; }

        [JsonProperty("details")]
        public string Details { get; }

        [JsonProperty("requestedAttributes")]
        public RequestedAttributes RequestedAttributes { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AuthenticationResult);
        }

        public bool Equals(AuthenticationResult other)
        {
            return other != null &&
                   base.Equals(other) &&
                   AuthRef == other.AuthRef &&
                   Status == other.Status &&
                   Details == other.Details &&
                   EqualityComparer<RequestedAttributes>.Default.Equals(RequestedAttributes, other.RequestedAttributes);
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AuthRef);
            hashCode = hashCode * -1521134295 + Status.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Details);
            hashCode = hashCode * -1521134295 + EqualityComparer<RequestedAttributes>.Default.GetHashCode(RequestedAttributes);
            return hashCode;
        }
    }
}