using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Newtonsoft.Json;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Services
{
    public class JsonService
    {
        public JsonService()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

        }

        public string SerializeToJson<T>(T jsonSerializable)
        {
            try
            {
                return JsonConvert.SerializeObject(jsonSerializable);
            }
            catch (Exception ex)
            {
                throw new FrejaEidClientInternalException(message: $"Error while serializing {jsonSerializable}", cause: ex);
            }
        }

        public T DeserializeFromJson<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new FrejaEidClientInternalException(message: $"Failed to deserialize value {json}", cause: ex);
            }


        }

    }
}