using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class CheckBonusResponse : DataAbstract
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "day")]
        public int Day { get; set; }

        [DataMember(Name = "popup_id")]
        public string PopupId { get; set; }
    }
}
