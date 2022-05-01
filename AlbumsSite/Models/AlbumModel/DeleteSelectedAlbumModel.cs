using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.AlbumModel
{
    public class DeleteSelectedAlbumModel
    {
        public IEnumerable<int> SelectedIds { get; set; }
    }
}
