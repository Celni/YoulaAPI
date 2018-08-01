using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Categorys : DataAbstract
    {
        [DataMember(Name = "id")]
        public long? Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "slug")]
        public string Slug { get; set; }

        [DataMember(Name = "slug_seo")]
        public string SlugSeo { get; set; }

        [DataMember(Name = "order")]
        public long? Order { get; set; }

        [DataMember(Name = "payment_available")]
        public bool? PaymentAvailable { get; set; }

        [DataMember(Name = "subcategories")]
        public List<Categorys> SubCategoryes { get; set; }
    }
}
