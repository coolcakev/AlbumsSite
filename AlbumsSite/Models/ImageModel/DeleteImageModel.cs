using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Image
{
    public class DeleteImageModel
    {
        public int ImageId { get; set; }
        public int AlbumId { get; set; }
        public string UserName { get; set; }
        public int AuthorId { get; set; }
    }
}
