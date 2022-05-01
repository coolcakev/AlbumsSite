using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Image
{
    public class ChangeImageDataModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Date { get; set; }
        public string City { get; set; }
        public string CameraModel { get; set; }
        public string FocalLength { get; set; }
        public string Aperture { get; set; }
        public string ShutterSpeed { get; set; }
        public string ISO { get; set; }
        public string Flash { get; set; }
        
        
    }
}
