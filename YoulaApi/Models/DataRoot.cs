using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace YoulaApi.Models
{
    [DataContract]
    public class DataRoot
    {
        [DataMember(Name = "data")]
        public List<DataAbstract> Data { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "detail")]
        public string Detail { get; set; }

        [DataMember(Name = "uri")]
        public string Uri { get; set; }
    }
}
