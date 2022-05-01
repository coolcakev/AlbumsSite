using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.AlbumModel
{
    public class AlbumThumbailModel
    {
        public IFormFile File { get; set; }
        public int AlbumId { get; set; }
        public string SmallPathThumbail { get; set; }


    }
}
