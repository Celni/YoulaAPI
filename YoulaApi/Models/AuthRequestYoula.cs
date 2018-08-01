using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace YoulaApi.Models
{
    public class AuthRequestYoula
    {
        [JsonProperty(PropertyName = "uid")]
        public string Uid { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "session_id")]
        public string SessionId { get; set; }
    }
}
