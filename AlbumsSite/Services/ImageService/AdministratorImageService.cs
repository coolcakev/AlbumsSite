using AlbumsSite.Command;
using AlbumsSite.Command.Image;
using AlbumsSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public class AdministratorImageService: BaseImageService
    {
        public AdministratorImageService(
            IDeleteImageCommand IDeleteImageCommand,
            IHttpContextAccessor contextAccessor,
            IParseMetaDataCommand parseMetaDataCommand,
            IWebHostEnvironment environment,
            IGetPlaceByGPSCommand getPlaceByGPSCommand,
            ApplicationContext repository
            ) : base(IDeleteImageCommand, contextAccessor, parseMetaDataCommand, environment, getPlaceByGPSCommand,
                repository,RoleName.Administrator)
        {
           
        }
    }
}
