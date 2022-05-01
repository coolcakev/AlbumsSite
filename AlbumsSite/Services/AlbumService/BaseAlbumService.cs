using AlbumsSite.Command;
using AlbumsSite.Command.AlbumCommand;
using AlbumsSite.Command.Image;
using AlbumsSite.Helper;
using AlbumsSite.Models;
using AlbumsSite.Models.AlbumModel;
using AlbumsSite.Models.Image;
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
    public interface IBaseAlbumService : IBaseService
    {
        Task ChangeThumbail(AlbumThumbailModel model);
        Task SingleAlbum(SingleAlbumModel albumModel);
        Task<bool> CreateAlbum(CreateAlbumModel model);
        Task DeleteSelectedAlbum(DeleteSelectedAlbumModel model);
        Task<bool> EditAlbum(EditAlbumModel model);
        Task DeleteAlbum(DeleteAlbumModel model);
        List<Album> GetPopularAlbum();
        void FillIndex(AllAlbumsModel model);




    }
    public class BaseAlbumService : BaseService, IBaseAlbumService
    {
        private readonly IDeleteAlbumCommand _IDeleteAlbumCommand;
        private readonly IParseMetaDataCommand _parseMetaDataCommand;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationContext _repository;



        public BaseAlbumService(
            IDeleteAlbumCommand IDeleteAlbumCommand,
            IHttpContextAccessor contextAccessor,
            IParseMetaDataCommand parseMetaDataCommand,
            IWebHostEnvironment environment,
            ApplicationContext repository,
            RoleName serviceType = RoleName.None) : base(serviceType, contextAccessor)
        {
            _IDeleteAlbumCommand = IDeleteAlbumCommand;
            _environment = environment;
            _repository = repository;
            _parseMetaDataCommand = parseMetaDataCommand;
        }

        public async Task<bool> CreateAlbum(CreateAlbumModel model)
        {
            var albumName = _repository.Albums.SingleOrDefault(x => x.Name == model.Name);
            if (albumName != null)
                return false;
            if (model.Name == null) 
                return false;

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;
            var user = _repository.Users.Include(x => x.MyAlbums).SingleOrDefault(x => x.Id == int.Parse(userId));
            var userAlbums = _repository.Albums.Include(x => x.Author).Include(x => x.Pictures).Where(x => x.Author.Id == int.Parse(userId)).ToList();
            var AlbumCount = userAlbums.Count()+1;
            int needToDelete;
            if (!user.IsVip && AlbumCount > 10)
            {
                needToDelete = AlbumCount - 10;
                model.ValidationMessage = $"Вам нужно удалить {needToDelete} альбома  чтобы добавлять новый контент";
                return false;

            }

            var album = new Album();
            album.Description = model.Description;
            album.Name = model.Name;
            var author = _repository.Users.SingleOrDefault(x => x.Id == int.Parse(userId));
            album.Author = author;
            string webRootPath = _environment.WebRootPath;
            string dateTime = DateTime.Now.ToString().Replace(':', '_');
            if (model.File == null)
            {
                album.ThumbailImagePath = string.Empty;
                album.SmallThumbailImagePath = "\\Photo\\Plug\\240x240.jpeg";
                await _repository.Albums.AddAsync(album);
                await _repository.SaveChangesAsync();
                model.AlbumId = album.Id;
                return true;
            }

            string path = Path.Combine(webRootPath, "Photo", "AlbumThumbail", dateTime + model.File.FileName);
            await FileManagerHelper.CreateFile(model.File, path);
            Picture picture = new Picture();
            _parseMetaDataCommand.ExecuteAsync(path, picture);
            File.Delete(path);
            int widhThumbail = 240;
            int heightThumbail = (int)(picture.Height / ((double)picture.Width / (double)widhThumbail));
            var byteArrayThumbail = await PhotoManagerHelper.ResizePhoto(model.File, widhThumbail, heightThumbail);
            await FileManagerHelper.CreateFile(byteArrayThumbail, path);
            album.ThumbailImagePath = path;
            album.SmallThumbailImagePath = Path.Combine("/Photo", "AlbumThumbail", dateTime + model.File.FileName);

            await _repository.Albums.AddAsync(album);
            await _repository.SaveChangesAsync();
            model.AlbumId = album.Id;
            return true;
        }

        public async Task ChangeThumbail(AlbumThumbailModel model)
        {
            var album = _repository.Albums.SingleOrDefault(x => x.Id == model.AlbumId);
            if (album.SmallThumbailImagePath != "\\Photo\\Plug\\240x240.jpeg")
                File.Delete(album.ThumbailImagePath);
            string webRootPath = _environment.WebRootPath;
            string dateTime = DateTime.Now.ToString().Replace(':', '_');
            string path = Path.Combine(webRootPath, "Photo", "AlbumThumbail", dateTime + model.File.FileName);
            await FileManagerHelper.CreateFile(model.File, path);
            Picture picture = new Picture();
            _parseMetaDataCommand.ExecuteAsync(path, picture);
            File.Delete(path);
            int widhThumbail = 240;
            int heightThumbail = (int)(picture.Height / ((double)picture.Width / (double)widhThumbail));
            var byteArrayThumbail = await PhotoManagerHelper.ResizePhoto(model.File, widhThumbail, heightThumbail);
            await FileManagerHelper.CreateFile(byteArrayThumbail, path);
            album.ThumbailImagePath = path;
            album.SmallThumbailImagePath = Path.Combine("/Photo", "AlbumThumbail", dateTime + model.File.FileName);
            await _repository.SaveChangesAsync();

            model.SmallPathThumbail = album.SmallThumbailImagePath;

        }

        public async Task SingleAlbum(SingleAlbumModel albumModel)
        {

            albumModel.Album = await _repository.Albums.Include(x => x.Author).Include(x => x.Pictures).ThenInclude(x => x.Likes).Include(x => x.Pictures).ThenInclude(x => x.Dislikes).SingleOrDefaultAsync(x => x.Name == albumModel.AlbumName);
            if (albumModel.Album == null) 
                return;
            albumModel.AlbumId = albumModel.Album.Id;
            albumModel.AutorAlbumId = albumModel.Album.Author.Id;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId")?.Value ?? "-1";
            albumModel.CurrentUserId = int.Parse(userId);
        }
        public void FillIndex(AllAlbumsModel model)
        {
            model.AllAlbums = _repository.Albums.ToList();
        }


        public async Task DeleteSelectedAlbum(DeleteSelectedAlbumModel model)
        {
            var deleteAlbums = _repository.Albums.Where(x => model.SelectedIds.Contains(x.Id));
            foreach (var item in deleteAlbums)
            {

                if (item.SmallThumbailImagePath != "\\Photo\\Plug\\240x240.jpeg")
                    File.Delete(item.ThumbailImagePath);

                _repository.Albums.Remove(item);
            }

            await _repository.SaveChangesAsync();
        }

        public async Task<bool> EditAlbum(EditAlbumModel model)
        {
            var albumName = _repository.Albums.SingleOrDefault(x => x.Name == model.Name && x.Id != model.AlbumId);
            if (albumName != null)
                return false;

            var names = _repository.Albums.Select(x => x.Name).ToList();
            var album = await _repository.Albums.SingleOrDefaultAsync(x => x.Id == model.AlbumId);
            if (!names.Contains(model.Name))
            {
                album.Name = model.Name;
            }
            album.Description = model.Description;
            await _repository.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAlbum(DeleteAlbumModel model)
        {
            var album = await _repository.Albums.SingleOrDefaultAsync(x => x.Id == model.AlbumId);
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId")?.Value ?? "-1";
            var user = await _repository.Users.SingleOrDefaultAsync(x => x.Id == int.Parse(userId));
            model.UserId = album.AuthorId;
            model.UserName = user.UserLogin;
            await _IDeleteAlbumCommand.ExecuteAsync(album);
        }

        public List<Album> GetPopularAlbum()
        {
            var allAlbums = _repository.Albums.Include(x => x.Pictures).OrderByDescending(x => x.Pictures.Count).ToList();

            return allAlbums;
        }
    }
}
