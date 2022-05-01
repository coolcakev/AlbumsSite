using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Admin
{
    public class GetAlbumAdminItem
    {

        public string Path { get; set; }
        public string AlbumOwner { get; set; }
        public string AlbumName { get; set; }
        public int AlbumId { get; set; }
        public int UserId { get; set; }

    }
}
