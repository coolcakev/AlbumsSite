using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Image
{
    public class AddImageModalOpenModel
    {
        public IEnumerable<Album> UsersAlbums { get; set; }
        public int ImageId { get; set; }
    }
}
