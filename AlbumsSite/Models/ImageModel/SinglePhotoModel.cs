using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Image
{
    public class SinglePhotoModel
    {
        public Picture Picture { get; set; }

        public IEnumerable<Album> UsersAlbum { get; set; }

        public int AlbumId { get; set; }

        public int ImageId { get; set; }
        public int AuthorId { get; set; }




        
    }
}
