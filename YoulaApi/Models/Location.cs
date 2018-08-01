using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "latitude")]
        public decimal Latitude { get; set; }

        [DataMember(Name = "longitude")]
        public decimal Longitude { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }
    }
}