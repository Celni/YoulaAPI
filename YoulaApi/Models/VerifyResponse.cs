using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace YoulaApi.Models
{
    public class VerifyResponse
    {
        [JsonProperty(PropertyName = "session_id")]
        public string SessionId { get; set; }

        [JsonProperty(PropertyName = "verification_url")]
        public string VerificationUrl { get; set; }

        [JsonProperty(PropertyName = "checks")]
        public List<string> Checks { get; set; }

        [JsonProperty(PropertyName = "ivr_timeout_sec")]
        public string IvrTimeoutSec { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "sms_template")]
        public string SmsTemplate { get; set; }

        [JsonProperty(PropertyName = "code_type")]
        public string CodeType { get; set; }

        [JsonProperty(PropertyName = "code_length")]
        public int? CodeLength { get; set; }

        [JsonProperty(PropertyName = "server_timestamp")]
        public long? ServerTimestamp { get; set; }

        [JsonProperty(PropertyName = "call_template")]
        public List<string> CallTemplate { get; set; }

        [JsonProperty(PropertyName = "modified_phone_number")]
        public string ModifiedPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "supported_ivr_languages")]
        public List<string> SupportedIvrLanguages { get; set; }

        [JsonProperty(PropertyName = "detail_status")]
        public string DetailStatus { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
