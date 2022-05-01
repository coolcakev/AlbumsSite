using AlbumsSite.Command;
using AlbumsSite.Command.AlbumCommand;
using AlbumsSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public class AuthentificateUserAlbumService : BaseAlbumService
    {
        public AuthentificateUserAlbumService(
         IDeleteAlbumCommand IDeleteAlbumCommand,
         IHttpContextAccessor contextAccessor,
         IParseMetaDataCommand parseMetaDataCommand,
         IWebHostEnvironment environment,
         ApplicationContext repository
         ) : base(IDeleteAlbumCommand, contextAccessor, parseMetaDataCommand, environment, repository, RoleName.User)
        {

        }
    }
   
}
