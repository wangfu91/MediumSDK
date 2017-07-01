using Newtonsoft.Json;
using System;
using MediumSDK.Utils;

namespace MediumSDK.Authentication
{
    public class Token
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public Scope[] Scope { get; set; }

        [JsonProperty("expires_at")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime ExpiresAt { get; set; }
    }
}
