﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.AlbumModel
{
    public class AddPhotoInAlbumModel
    {
        public int PhotoId { get; set; }
        public int AlbumId { get; set; }
        public string ValidationMessage { get; set; }
    }
}
