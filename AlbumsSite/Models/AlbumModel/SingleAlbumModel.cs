using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.AlbumModel
{
    public class SingleAlbumModel
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public Album Album { get; set; }
        public int AutorAlbumId { get; set; }
        public int CurrentUserId { get; set; }
    }
}
