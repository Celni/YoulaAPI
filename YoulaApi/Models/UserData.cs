using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class UserData : DataAbstract
    {
        [DataMember]
        public override string Uri => "/auth/validate";

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "push_token")]
        public string PushToken { get; set; }

        [DataMember(Name = "account")]
        public Account Account { get; set; }

        [DataMember(Name = "display_phone_num")]
        public string DisplayPhoneNum { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "is_blocked")]
        public bool IsBlocked { get; set; }

        [DataMember(Name = "image")]
        public Image Image { get; set; }
    }

    [DataContract]
    public class Account
    {
        [DataMember(Name = "bonus_cnt")]
        public int BonusCount { get; set; }

        [DataMember(Name = "bonus_code")]
        public string BunusCode { get; set; }
    }
}
