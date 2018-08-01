using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class OAuth
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn = 86400;

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        public DateTime TimeGrantToken = DateTime.Now;
    }
}
