using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlbumsSite.Models
{
    public class Album
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public User Author { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<Picture> Pictures { get; set; }
        public string ThumbailImagePath { get; set; }
        public string SmallThumbailImagePath { get; set; }

       
    }
}
