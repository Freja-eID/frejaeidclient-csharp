using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttributeToReturn
    {
        BASIC_USER_INFO,
        CUSTOM_IDENTIFIER,
        INTEGRATOR_SPECIFIC_USER_ID,
        SSN,
        DATE_OF_BIRTH,
        RELYING_PARTY_USER_ID,
        EMAIL_ADDRESS,
        ORGANISATION_ID_IDENTIFIER
    }
}
