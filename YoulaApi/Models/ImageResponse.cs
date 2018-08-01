using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class ImageResponse : DataAbstract
    {
        [DataMember(Name = "height")]
        public int? Height { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "num")]
        public int? Num { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "width")]
        public int? Width { get; set; }
    }
}
