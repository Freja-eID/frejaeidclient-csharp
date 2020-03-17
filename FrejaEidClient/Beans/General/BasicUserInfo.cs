using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.General
{
    public class BasicUserInfo : IEquatable<BasicUserInfo>
    {
        public BasicUserInfo(string name, string surname)
        {
            this.Name = name;
            this.Surname = surname;
        }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("surname")]
        public string Surname { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BasicUserInfo);
        }

        public bool Equals(BasicUserInfo other)
        {
            return other != null &&
                   Name == other.Name &&
                   Surname == other.Surname;
        }

        public override int GetHashCode()
        {
            var hashCode = 305228700;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            return hashCode;
        }
    }
}
