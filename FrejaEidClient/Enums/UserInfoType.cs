using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserInfoType
    {
        EMAIL,
        SSN,
        PHONE,
        INFERRED,
        ORG_ID
    }
}