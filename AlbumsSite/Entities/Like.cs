using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models
{
    public class Like
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Picture Picture { get; set; }
        public int PictureId { get; set; }
    }
}
