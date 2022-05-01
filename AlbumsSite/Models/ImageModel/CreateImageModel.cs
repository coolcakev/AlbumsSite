using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Image
{
    public class CreateImageModel
    {
        public IEnumerable<IFormFile> Files { get; set; }
        public string AlbumName { get; set; }
        public int AlbumId { get; set; }
        public string  ValidationMessage { get; set; }
    }
}
