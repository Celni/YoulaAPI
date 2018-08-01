using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace YoulaApi.Models
{
    public class AttempResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "app_check_id")]
        public string AppCheckId { get; set; }

        [JsonProperty(PropertyName = "server_timestamp")]
        public long? ServerTimestamp { get; set; }

        [JsonProperty(PropertyName = "modified_phone_number")]
        public string ModifiedPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "token_expiration_time")]
        public int TokenExpirationTime { get; set; }

        [JsonProperty(PropertyName = "detail_status")]
        public string DetailStatus { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
