﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.AlbumModel
{
    public class DeleteAlbumModel
    {
        public int AlbumId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
