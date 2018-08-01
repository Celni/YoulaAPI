using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace YoulaApi.Models
{
    [DataContract]
    [KnownType(typeof(UserData))]
    [KnownType(typeof(CheckBonusResponse))]
    [KnownType(typeof(ImageResponse))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(Counters))]
    [KnownType(typeof(Categorys))]
    [KnownType(typeof(Chats))]
    public class DataAbstract
    {
        [DataMember]
        public virtual string Uri { get; }
    }

}
