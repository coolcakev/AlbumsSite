using AlbumsSite.Models;
using AlbumsSite.Models.HomeModel;
using AlbumsSite.Models.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public interface IBaseHomeService:IBaseService
    {
        List<Picture> FindPhoto(string inputText);
        List<Picture> FindPhotoByParameters(string inputText);
        Task FillIndex(FillIndexModel model);
    }
    public class BaseHomeService :BaseService, IBaseHomeService
    {

        private readonly ApplicationContext _repository;
        public BaseHomeService(
             IHttpContextAccessor contextAccessor,
            ApplicationContext repository,
            RoleName roleName=RoleName.None) : base(roleName, contextAccessor)
        {
            _repository = repository;
        }

        public async Task FillIndex(FillIndexModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId")?.Value??"-1";
            model.CurrentUserId = int.Parse(userId);
            var pictures = _repository.Pictures.Include(x=>x.PictureAlbums).Include(x=>x.Author).Include(x=>x.Likes).Include(x=>x.Dislikes).Where(x=>x.PictureAlbums.Count>0).ToList();
            pictures.OrderByDescending(x => x.Likes.Count);
            model.Pictures = pictures;
        }

        public List<Picture> FindPhoto(string inputText)
        {
            inputText ??= "";
            var obj = _repository.Pictures.Where(x => x.Name.Contains(inputText)).ToList();
            return obj;
        }
        public List<Picture> FindPhotoByParameters(string inputText)
        {
            inputText ??= "";
            var obj = _repository.Pictures.Include(x => x.Author).ToList().Where(x => x.Name.Contains(inputText) ||
              x.Aperture.Contains(inputText) || x.CameraModel.Contains(inputText) || x.City.Contains(inputText) ||
              x.Flash.Contains(inputText) || x.FocalLength.Contains(inputText)||x.ISO.Contains(inputText)||x.ShutterSpeed.Contains(inputText)||(x.Date.ToString().Contains(inputText))
            ).ToList();

            return obj;
        }

        

    }
}