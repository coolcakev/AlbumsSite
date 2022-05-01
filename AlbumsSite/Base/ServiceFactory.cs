using AlbumsSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public interface IServiceFactory
    {
        T GetInstance<T>() where T : IBaseService;
    }

    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _services;
        private readonly IHttpContextAccessor _httpContextAccessor;
    

        public ServiceFactory(
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider services
           )
        {
            _services = services;
            _httpContextAccessor = httpContextAccessor;
        }

        public T GetInstance<T>() where T : IBaseService
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(x=>x.Type== ClaimsIdentity.DefaultRoleClaimType)?.Value??"None";
            var roleEnum = (RoleName)Enum.Parse(typeof(RoleName), role);
            var servicesWithCurrentType = GetUserServices<T>().Where(x => x.ServiceType == roleEnum).ToList();
            if (servicesWithCurrentType.Count == 0)
            {
                throw new Exception("AccessDenied");
            }

            if (servicesWithCurrentType.Count > 1)
            {
                throw new Exception("You can't add more than 1 specialized service for one role!");
            }

            var service = (T)servicesWithCurrentType.FirstOrDefault();

            return service;
        }

        public IEnumerable<IBaseService> GetUserServices<T>() where T : IBaseService
        {
            IEnumerable<IBaseService> services = _services.GetServices<T>() as IEnumerable<IBaseService>;
            return services;
        }
    }
}
