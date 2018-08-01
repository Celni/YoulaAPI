using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class PushRequest
    {
        [DataMember(Name = "uid")]
        public string Uid { get; set; }

        [DataMember(Name = "device_type")]
        public string DeviceType { get; set; }

        [DataMember(Name = "push_token")]
        public string PushToken { get; set; }
    }
}
