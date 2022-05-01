using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Entities
{
    public class JobMessage
    {
        public int Id { get; set; }
        public JobName JobName { get; set; }
        public DateTime NextRun { get; set; }
    }

    public enum JobName
    {
        DeletePhotoWhithoutAlbum=1

    }
}
