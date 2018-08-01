using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Image
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "num")]
        public string Num { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
