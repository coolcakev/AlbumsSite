using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AlbumsSite.Models
{
    public class UserPageModel
    {
        
        public string UserLogin { get; set; }

        public List<Album> MyAlbums { get; set; }

        public int AuthorID { get; set; }
        public int CurrentUserId { get; set; }
    }
}
