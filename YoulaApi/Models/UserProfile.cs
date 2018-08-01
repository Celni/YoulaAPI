using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class UserProfile
    {
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "image")]
        public string ImageId { get; set; }
    }
}
