using AlbumsSite.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public class AuthentificateUserHomeService : BaseHomeService
    {
        public AuthentificateUserHomeService(
            IHttpContextAccessor contextAccessor,
           ApplicationContext repository
         ) : base(contextAccessor, repository, RoleName.User)
        {

        }
    }
  
}
