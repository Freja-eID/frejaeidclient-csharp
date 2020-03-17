using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.General
{
    public class SsnUserInfo : IEquatable<SsnUserInfo>
    {
        [JsonConstructor]
        private SsnUserInfo(string country, string ssn)
        {
            this.Country = country;
            this.Ssn = ssn;
        }

        public static SsnUserInfo Create(string country, string ssn)
        {
            return new SsnUserInfo(country, ssn);
        }

        [JsonProperty("country")]
        public string Country { get; }

        [JsonProperty("ssn")]
        public string Ssn { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SsnUserInfo);
        }

        public bool Equals(SsnUserInfo other)
        {
            return other != null &&
                   Country == other.Country &&
                   Ssn == other.Ssn;
        }

        public override int GetHashCode()
        {
            var hashCode = -1761054160;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Country);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Ssn);
            return hashCode;
        }

    }
}
