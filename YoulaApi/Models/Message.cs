using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Messages
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "type")]
        public int? Type { get; set; }

        [DataMember(Name = "date_created")]
        public long? DateCreated { get; set; }

        [DataMember(Name = "sender_id")]
        public string SenderId { get; set; }

        [DataMember(Name = "recipient_id")]
        public string RecipientId { get; set; }

        [DataMember(Name = "chat_id")]
        public string ChatId { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "images")]
        public List<Image> Images { get; set; }

        [DataMember(Name = "is_read")]
        public bool? IsRead { get; set; }

        [DataMember(Name = "is_spam")]
        public bool? IsSpam { get; set; }

        [DataMember(Name = "has_text")]
        public bool? HasText { get; set; }
    }
}
