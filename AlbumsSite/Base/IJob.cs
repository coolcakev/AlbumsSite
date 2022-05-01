using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Base
{
    public interface IJob
    {
        Task ExecuteAsync();
    }
}
