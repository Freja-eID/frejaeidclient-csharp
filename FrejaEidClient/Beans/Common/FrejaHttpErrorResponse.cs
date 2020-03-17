using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common
{
    internal class FrejaHttpErrorResponse : FrejaHttpResponse, IEquatable<FrejaHttpErrorResponse>
    {

        public FrejaHttpErrorResponse(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        [JsonProperty("message")]
        public string Message { get; }

        [JsonProperty("code")]
        public int Code { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as FrejaHttpErrorResponse);
        }

        public bool Equals(FrejaHttpErrorResponse other)
        {
            return other != null &&
                   Message == other.Message &&
                   Code == other.Code;
        }

        public override int GetHashCode()
        {
            var hashCode = -1798610120;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            hashCode = hashCode * -1521134295 + Code.GetHashCode();
            return hashCode;
        }
    }
}