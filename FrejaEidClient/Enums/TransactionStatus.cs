using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionStatus
    {
        STARTED,
        DELIVERED_TO_MOBILE,
        CANCELED,
        RP_CANCELED,
        EXPIRED,
        APPROVED,
        REJECTED
    }
}