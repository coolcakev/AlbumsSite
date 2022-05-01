using AlbumsSite.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public interface IBaseService
    {
        public RoleName ServiceType { get; }
    }

    public abstract class BaseService : IBaseService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public RoleName ServiceType { get; } = RoleName.None;

        protected BaseService(RoleName serviceType, IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
            ServiceType = serviceType;
        }

        protected BaseService(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }

    }
}
