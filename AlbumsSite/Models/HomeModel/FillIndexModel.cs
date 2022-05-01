using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.HomeModel
{
    public class FillIndexModel
    {
        public IEnumerable<Picture> Pictures { get; set; }
        public int CurrentUserId { get; set; }
    }
}
