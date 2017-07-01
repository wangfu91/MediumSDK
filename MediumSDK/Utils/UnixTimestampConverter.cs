using System;
using Newtonsoft.Json;

namespace MediumSDK.Utils
{
    internal class UnixTimestampConverter : JsonConverter
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var timestamp = reader.Value as long?;
            if (!timestamp.HasValue) return null;
            return UnixEpoch.AddMilliseconds(timestamp.Value);
        }
    }
}
