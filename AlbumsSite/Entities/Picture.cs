using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlbumsSite.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public int? AuthorId { get; set; }
        public User Author { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public string CameraModel { get; set; }
        public string FocalLength { get; set; }
        public string Aperture { get; set; }
        public string ShutterSpeed { get; set; }
        public string ISO { get; set; }
        public string Flash { get; set; }
        public string Path { get; set; }
        public string SmallPath { get; set; }
        public string PathThubail { get; set; }
        public string SmalpathThubail { get; set; }
        public string PathMedium { get; set; }
        public string SmalpathMedium { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<Album> PictureAlbums { get; set; }
        public List<Like> Likes { get; set; }
        public List<Dislike> Dislikes { get; set; }
    }
}           
