using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Admin
{
    public class GetPhotosAdminItem
    {
        public int PhotoId { get; set; }
        public string Path { get; set; }
        public string AuthorName { get; set; }
        public int UserId { get; set; }
        public List<string> AlbumName { get; set; }
        public List<int> AlbumId { get; set; }
        
    }
}
