using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Chats : DataAbstract
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "product")]
        public Product Product { get; set; }

        [DataMember(Name = "date_last_active")]
        public long? DateLastActive { get; set; }

        [DataMember(Name = "date_created")]
        public long? DateCreated { get; set; }

        [DataMember(Name = "owner")]
        public Owner Owner { get; set; }

        [DataMember(Name = "recipient")]
        public Сontractor Recipient { get; set; }

        [DataMember(Name = "messages")]
        public List<Messages> Messages { get; set; }

        [DataMember(Name = "unread_count")]
        public int? UnreadCount { get; set; }

        [DataMember(Name = "is_deleted")]
        public bool? IsDeleted { get; set; }

        [DataMember(Name = "no_chat")]
        public bool? NoChat { get; set; }

        [DataMember(Name = "channel_name")]
        public string ChannelName { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
