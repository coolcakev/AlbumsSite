using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlbumsSite.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<Album> MyAlbums { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<Like> Likes { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<Dislike> Dislikes { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<Picture> Pictures { get; set; }
        public bool IsVip { get; set; }
        public RoleName Role { get; set; }
    }
    public enum RoleName
    {
        None = 0,
        User = 1,      
        Administrator = 3
    }
}
