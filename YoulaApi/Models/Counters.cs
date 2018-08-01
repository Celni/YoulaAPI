using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Counters : DataAbstract
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "chats")]
        public long? Chats { get; set; }
    }
}
