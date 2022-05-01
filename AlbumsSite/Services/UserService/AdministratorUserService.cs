using AlbumsSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public class AdministratorUserService: BaseUserService
    {
        public AdministratorUserService(
           IHttpContextAccessor contextAccessor,
           ApplicationContext repository,
           IWebHostEnvironment environment
           ) : base(contextAccessor, repository, environment, RoleName.Administrator)
        {
          
        }
    }
}
