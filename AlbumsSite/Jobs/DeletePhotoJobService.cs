using AlbumsSite.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Jobs
{
    public interface IDeletePhotoJobService : IBaseJobService
    {
    }
    public class DeletePhotoJobService : BaseJobService, IDeletePhotoJobService
    {

        public DeletePhotoJobService(IServiceProvider serviceProvider ,IDeletePhotoWhithoutAlbum job, IHttpContextAccessor httpContext) :base(job, serviceProvider)
        {
        }
    }
}
