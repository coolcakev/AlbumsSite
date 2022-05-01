using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.AlbumModel
{
    public class CreateAlbumModel
    {
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AlbumId { get; set; }
        public string ValidationMessage { get; set; }
    }
}
