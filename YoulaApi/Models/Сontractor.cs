using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Сontractor
    {
        [DataMember(Name = "display_phone_num")]
        public string DisplayPhoneNum { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "linked_id")]
        public string LinkedId { get; set; }

        [DataMember(Name = "date_registered")]
        public long? DateRegistered { get; set; }

        [DataMember(Name = "is_shop")]
        public bool? IsShop { get; set; }

        [DataMember(Name = "isOnline")]
        public bool? IsOnline { get; set; }

        [DataMember(Name = "image")]
        public Image Image { get; set; }
    }
}
