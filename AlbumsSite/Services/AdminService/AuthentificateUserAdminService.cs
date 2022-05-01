using AlbumsSite.Command.AlbumCommand;
using AlbumsSite.Command.Image;
using AlbumsSite.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public class AuthentificateUserAdminService: BaseAdminService
    {
        public AuthentificateUserAdminService(
          IHttpContextAccessor httpContextAccessor,
          ApplicationContext baseRepository,
          IDeleteImageCommand IDeleteImageCommand,
          IDeleteAlbumCommand IDeleteAlbumCommand
          ) : base(httpContextAccessor, baseRepository, IDeleteImageCommand,
              IDeleteAlbumCommand, RoleName.User)
        {

        }
    }
   
}
