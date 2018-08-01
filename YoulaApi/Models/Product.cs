using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoulaApi.Models
{
    [DataContract]
    public class Product : DataAbstract
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "owner")]
        public Owner Owner { get; set; }

        [DataMember(Name = "linked_id")]
        public string LinkedId { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "slug")]
        public string Slug { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "price")]
        public decimal? Price { get; set; }

        [DataMember(Name = "date_created")]
        public long? DateCtreate { get; set; }

        [DataMember(Name = "date_updated")]
        public long? DateUpdated { get; set; }

        [DataMember(Name = "date_published")]
        public long? DatePublished { get; set; }

        [DataMember(Name = "date_sold")]
        public long? DateSold { get; set; }

        [DataMember(Name = "date_blocked")]
        public long? DateBlockeg { get; set; }

        [DataMember(Name = "date_deleted")]
        public long? DateDeleted { get; set; }

        [DataMember(Name = "date_archivation")]
        public long? DateArchivation { get; set; }

        [DataMember(Name = "is_published")]
        public bool? IsPublished { get; set; }

        [DataMember(Name = "is_sold")]
        public bool? IsSold { get; set; }

        [DataMember(Name = "sold_mode")]
        public int? SoldMode { get; set; }

        [DataMember(Name = "is_deleted")]
        public bool? IsDeleted { get; set; }

        [DataMember(Name = "is_blocked")]
        public bool? IsBlocked { get; set; }

        [DataMember(Name = "is_archived")]
        public bool? IsArchived { get; set; }

        [DataMember(Name = "archive_mode")]
        public int? ArchiveMode { get; set; }

        [DataMember(Name = "is_expiring")]
        public bool? IsExpiring { get; set; }

        [DataMember(Name = "is_verified")]
        public bool? IsVerified { get; set; }

        [DataMember(Name = "is_promoted")]
        public bool? IsPromoted { get; set; }

        [DataMember(Name = "images")]
        public List<ImageResponse> Images { get; set; }

        [DataMember(Name = "location")]
        public Location Location { get; set; }

        [DataMember(Name = "category")]
        public int? Category { get; set; }

        [DataMember(Name = "subcategory")]
        public int? SubCategory { get; set; }

        [DataMember(Name = "is_favorite")]
        public bool? IsFavorite { get; set; }

        [DataMember(Name = "date_favorited")]
        public long? DateFavorited { get; set; }

        [DataMember(Name = "views")]
        public int? Views { get; set; }

        [DataMember(Name = "favorite_counter")]
        public int? FavoriteCounter { get; set; }

        [DataMember(Name = "group_id")]
        public int? GroupId { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "url_branch")]
        public string UrlBranch { get; set; }

        [DataMember(Name = "short_url")]
        public string ShortUrl { get; set; }

        [DataMember(Name = "contacts_visible")]
        public bool? ContactsVisible { get; set; }

        [DataMember(Name = "contractor")]
        public Сontractor Сontractor { get; set; }

    }
}
