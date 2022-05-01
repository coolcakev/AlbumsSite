using AlbumsSite.Command.AlbumCommand;
using AlbumsSite.Command.Image;
using AlbumsSite.Models;
using AlbumsSite.Models.Admin;
using AlbumsSite.Models.AlbumModel;
using AlbumsSite.Models.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public interface IAdminService:IBaseService
    {
        void Admin(AcountUserAdminModel model);
        IEnumerable<GetPhotosAdminItem> GetAllPhotos();
        IEnumerable<GetAlbumAdminItem> GetAllAlbums();
        IEnumerable<AcountUserAdminItem> GetUsers();
        Task DeleteImage(DeleteImageModel model);
        Task DeleteAlbum(DeleteAlbumModel model);
    }

    public class BaseAdminService :BaseService, IAdminService
    {
        private readonly IDeleteAlbumCommand _IDeleteAlbumCommand;
        private readonly ApplicationContext _baseRepository;
        private readonly IDeleteImageCommand _IDeleteImageCommand;

        public BaseAdminService(
            IHttpContextAccessor httpContextAccessor,
            ApplicationContext baseRepository,
            IDeleteImageCommand IDeleteImageCommand,
            IDeleteAlbumCommand IDeleteAlbumCommand,
            RoleName roleName=RoleName.None):base(roleName, httpContextAccessor)
        {
            _IDeleteAlbumCommand = IDeleteAlbumCommand;
             _IDeleteImageCommand = IDeleteImageCommand;
            _baseRepository = baseRepository;
        }
        public void Admin(AcountUserAdminModel model)
        {

            var users= _baseRepository.Users.Include(x => x.MyAlbums).ThenInclude(x => x.Author).Where(x => x.Role != RoleName.Administrator);
            var accountUsersAdminItems = new List<AcountUserAdminItem>();
            foreach (var item in users)
            {
                var accountUsersAdminItem = new AcountUserAdminItem()
                {
                    UserId = item.Id,
                    UserLogin = item.UserLogin,
                    Email = item.Email,
                    IsVip = item.IsVip,
                    AlbumId = item.MyAlbums.Select(x => x.Id).ToList(),
                    AlbumName = item.MyAlbums.Select(x => x.Name).ToList()
                };
                accountUsersAdminItems.Add(accountUsersAdminItem);
            }
            model.AllUsers = accountUsersAdminItems;

        }
        public IEnumerable<GetPhotosAdminItem> GetAllPhotos()
        {
            var allPhoto = _baseRepository.Pictures.Include(x => x.PictureAlbums).Include(x => x.Author).Where(x => x.PictureAlbums.Count > 0).ToList().Select(x => new GetPhotosAdminItem()
            {
                PhotoId=x.Id,
                Path=x?.SmalpathThubail,
                AlbumName = x.PictureAlbums.Select(x => x.Name).ToList(),
                AlbumId = x.PictureAlbums.Select(x => x.Id).ToList(),
                AuthorName = x.Author.UserLogin,
                UserId = x.Author.Id,
            });

            return allPhoto;
        }
        public async Task DeleteImage(DeleteImageModel model)
        {
            
            var picture = await _baseRepository.Pictures.Include(x => x.Author).SingleOrDefaultAsync(x => x.Id == model.ImageId);
            model.AuthorId = picture.Author.Id;
            await _IDeleteImageCommand.ExecuteAsync(picture);
        }
        public async Task DeleteAlbum(DeleteAlbumModel model)
        {
            var album = await _baseRepository.Albums.SingleOrDefaultAsync(x => x.Id == model.AlbumId);
            await _IDeleteAlbumCommand.ExecuteAsync(album);
        }
        public async Task DeleteUser(int userId)
        {
            var user = await _baseRepository.Users.Include(x => x.MyAlbums).Include(x => x.Dislikes).Include(x => x.Likes).Include(x => x.Pictures).SingleOrDefaultAsync(x => x.Id == userId);
            _baseRepository.Users.Remove(user);
            _baseRepository.SaveChanges();
        }

        public IEnumerable<GetAlbumAdminItem> GetAllAlbums()
        {
            var allAlbum = _baseRepository.Albums.Include(x => x.Pictures).Include(x => x.Author).ToList().Select(x => new GetAlbumAdminItem()
            {
                Path=x.SmallThumbailImagePath,
                AlbumOwner = x.Author.UserLogin,
                AlbumName = x.Name,
                AlbumId = x.Id,
                UserId = x.Author.Id,
            });
            return allAlbum;
        }
       public IEnumerable<AcountUserAdminItem> GetUsers()
        {
     
            var users = _baseRepository.Users.Include(x => x.MyAlbums).ThenInclude(x => x.Author).Where(x => x.Role != RoleName.Administrator);
            var accountUsersAdminItems = new List<AcountUserAdminItem>();
            foreach (var item in users)
            {
                var accountUsersAdminItem = new AcountUserAdminItem()
                {
                    UserId = item.Id,
                    UserLogin = item.UserLogin,
                    Email = item.Email,
                    IsVip = item.IsVip,
                    AlbumId = item.MyAlbums.Select(x => x.Id).ToList(),
                    AlbumName = item.MyAlbums.Select(x => x.Name).ToList()
                };
                accountUsersAdminItems.Add(accountUsersAdminItem);
            }


            return accountUsersAdminItems;
        }
    }
}
