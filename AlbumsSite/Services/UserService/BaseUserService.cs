using AlbumsSite.Command.Image;
using AlbumsSite.Helper;
using AlbumsSite.Models;
using AlbumsSite.Models.AlbumModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public interface IBaseUserService : IBaseService
    {
        
        
        Task SetVipStatus(int id);
        Task DeleteUser(int id);
        UserPageModel UserPage(int id);
    }
    public class BaseUserService : BaseService, IBaseUserService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationContext _repository;
        public BaseUserService(
            IHttpContextAccessor contextAccessor,
            ApplicationContext repository,
            IWebHostEnvironment environment,
            RoleName serviceType = RoleName.None) : base(serviceType, contextAccessor)
        {
            _repository = repository;
            _environment = environment;
        }

        
        public async Task SetVipStatus(int id)
        {
            var user = await _repository.Users.SingleOrDefaultAsync(x => x.Id == id);
            user.IsVip = !user.IsVip;
            await _repository.SaveChangesAsync();
        }

       

        public UserPageModel UserPage(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId")?.Value ?? "-1";

            var personFromDb = _repository.Users.Include(x => x.MyAlbums).SingleOrDefault(x => x.Id == id);
            if (personFromDb == null)
                return null;
            var user = new UserPageModel()
            {
                MyAlbums = personFromDb.MyAlbums.ToList(),
                UserLogin = personFromDb.UserLogin,
                AuthorID = id,
                CurrentUserId = int.Parse(userId),
            };
            return user;
        }

        public async Task DeleteUser(int id)
        {
            var person = _repository.Users.SingleOrDefault(x => x.Id == id);
            _repository.Users.Remove(person);
            await _repository.SaveChangesAsync();
        }
    }
}
