using AlbumsSite.Command;
using AlbumsSite.Command.Image;
using AlbumsSite.Helper;
using AlbumsSite.Models;
using AlbumsSite.Models.AlbumModel;
using AlbumsSite.Models.Image;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public interface IBaseImageService : IBaseService
    {
        Task<Boolean> CreateImage(CreateImageModel model);
        Task SinglePhoto(SinglePhotoModel singlePhotoModel);
        Task ChangeImageData(ChangeImageDataModel model);
        Task DeleteImage(DeleteImageModel model);
        Task AddImageModalOpen(AddImageModalOpenModel model);
        Task<Boolean> AddImageToAlbumFromGalery(AddPhotoInAlbumModel model);
        Task<Boolean> AddPhotoInAlbum(AddPhotoInAlbumModel model);
        Task DeletePhotoFromAlbum(DeleteImageModel model);
        Task Like(int id);
        Task Dislike(int id);

    }
    public class BaseImageService : BaseService, IBaseImageService
    {
        private readonly IDeleteImageCommand _IDeleteImageCommand;
        private readonly IParseMetaDataCommand _parseMetaDataCommand;
        private readonly IWebHostEnvironment _environment;
        private readonly IGetPlaceByGPSCommand _getPlaceByGPSCommand;
        private readonly ApplicationContext _repository;

        public BaseImageService(
            IDeleteImageCommand IDeleteImageCommand,
            IHttpContextAccessor contextAccessor,
            IParseMetaDataCommand parseMetaDataCommand,
            IWebHostEnvironment environment,
            IGetPlaceByGPSCommand getPlaceByGPSCommand,
            ApplicationContext repository,
            RoleName serviceType = RoleName.User) : base(serviceType, contextAccessor)
        {
            _IDeleteImageCommand = IDeleteImageCommand;
            _parseMetaDataCommand = parseMetaDataCommand;
            _environment = environment;
            _repository = repository;
            _getPlaceByGPSCommand = getPlaceByGPSCommand;
        }



        public async Task<Boolean> CreateImage(CreateImageModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;
            var user = _repository.Users.Include(x => x.MyAlbums).SingleOrDefault(x => x.Id == int.Parse(userId));
            var userAlbums = _repository.Albums.Include(x => x.Author).Include(x => x.Pictures).Where(x => x.Author.Id == int.Parse(userId)).ToList();
            var picturesCount = userAlbums.Select(x => x.Pictures.Count).Sum();
            var countOfPictures = picturesCount + model.Files.Count();
            int needToDelete;
            if (!user.IsVip && countOfPictures > 100)
            {
                needToDelete = countOfPictures - 100;
                model.ValidationMessage = $"Вам нужно удалить {needToDelete} фото чтобы добавлять новый контент";
                return false;

            }
            foreach (var item in model.Files)
            {


                Picture picture = new Picture();
                string webRootPath = _environment.WebRootPath;
                string dateTime = DateTime.Now.ToString().Replace(':', '_');
                string path = Path.Combine(webRootPath, "Photo", "Original", dateTime + item.FileName);
                string smallPath = Path.Combine("\\Photo", "Original", dateTime + item.FileName);
                string pathThubail = Path.Combine(webRootPath, "Photo", "Thumbail", dateTime + "Thumbail" + item.FileName);
                string smalpathThubail = Path.Combine("\\Photo", "Thumbail", dateTime + "Thumbail" + item.FileName);
                string pathMedium = Path.Combine(webRootPath, "Photo", "Medium", dateTime + "Medium" + item.FileName);
                string smalpathMedium = Path.Combine("\\Photo", "Medium", dateTime + "Medium" + item.FileName);
                await FileManagerHelper.CreateFile(item, path);
                var GPS = _parseMetaDataCommand.ExecuteAsync(path, picture);
                int widhThumbail = 240;
                int heightThumbail = (int)(picture.Height / ((double)picture.Width / (double)widhThumbail));
                var byteArrayThumbail = await PhotoManagerHelper.ResizePhoto(item, widhThumbail, heightThumbail);
                await FileManagerHelper.CreateFile(byteArrayThumbail, pathThubail);
                int widthMedium = 1920;
                int heightMedium = (int)(picture.Height / ((double)picture.Width / (double)widthMedium));
                var byteArrayMedium = await PhotoManagerHelper.ResizePhoto(item, widthMedium, heightMedium);
                await FileManagerHelper.CreateFile(byteArrayMedium, pathMedium);
                picture.Name = item.FileName.Split('.')[0];
                picture.Width = widthMedium;
                picture.Height = heightMedium;
                picture.Path = path;
                picture.SmallPath = smallPath;
                picture.City = string.Empty;
                picture.PathMedium = pathMedium;
                picture.SmalpathMedium = smalpathMedium;
                picture.PathThubail = pathThubail;
                picture.SmalpathThubail = smalpathThubail;


                picture.Author = user;
                if (!double.IsNaN(GPS.GPSLatitude) && !double.IsNaN(GPS.GPSLongitude))
                    picture.City = _getPlaceByGPSCommand.ExecuteAsync(GPS.GPSLatitude, GPS.GPSLongitude);

                var album = _repository.Albums.Include(x => x.Pictures).SingleOrDefault(x => x.Id == model.AlbumId);
                model.AlbumName = album.Name;
                album.Pictures.Add(picture);
                await _repository.Pictures.AddAsync(picture);

            }

            await _repository.SaveChangesAsync();
            return true;
        }

        public virtual async Task SinglePhoto(SinglePhotoModel singlePhotoModel)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;
            singlePhotoModel.Picture = _repository.Pictures.Include(x => x.PictureAlbums).Include(x => x.Author).Include(x => x.Likes).Include(x => x.Dislikes).SingleOrDefault(x => x.Id == singlePhotoModel.ImageId);
            if (singlePhotoModel.Picture == null) return;
            var user = _repository.Users.Include(x => x.MyAlbums).SingleOrDefault(x => x.Id == int.Parse(userId));
            singlePhotoModel.AuthorId = singlePhotoModel.Picture.Author.Id;
            foreach (var item in singlePhotoModel.Picture.PictureAlbums)
            {
                user.MyAlbums.Remove(item);
            }
            singlePhotoModel.UsersAlbum = user.MyAlbums;
            
        }

        public async Task ChangeImageData(ChangeImageDataModel model)
        {

            var picture = await _repository.Pictures.SingleOrDefaultAsync(x => x.Id == model.Id);
            picture.Name = model.Name??string.Empty;
            if (model.Date != null)
                picture.Date = DateTime.Parse(model.Date);
            picture.City = model.City ?? string.Empty;
            picture.CameraModel = model.CameraModel ?? string.Empty;
            picture.FocalLength = model.FocalLength ?? string.Empty;
            picture.Aperture = model.Aperture ?? string.Empty;
            picture.ShutterSpeed = model.ShutterSpeed ?? string.Empty;
            picture.ISO = model.ISO ?? string.Empty;
            picture.Flash = model.Flash ?? string.Empty;
            await _repository.SaveChangesAsync();

        }
        public async Task DeleteImage(DeleteImageModel model)
        {
            var picture = await _repository.Pictures.Include(x => x.Author).SingleOrDefaultAsync(x => x.Id == model.ImageId);
            model.AuthorId = picture.Author.Id;
            model.UserName = picture.Author.UserLogin;
            await _IDeleteImageCommand.ExecuteAsync(picture);
        }
        public async Task AddImageModalOpen(AddImageModalOpenModel model)
        {
            var picture = await _repository.Pictures.Include(x => x.PictureAlbums).SingleOrDefaultAsync(x => x.Id == model.ImageId);

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;

            var user = _repository.Users.Include(x => x.MyAlbums).SingleOrDefault(x => x.Id == int.Parse(userId));
            foreach (var item in picture.PictureAlbums)
            {
                user.MyAlbums.Remove(item);
            }

            var albums = user.MyAlbums;
            model.UsersAlbums = albums;
            
        }

        public async Task<Boolean> AddImageToAlbumFromGalery(AddPhotoInAlbumModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;
            var user = _repository.Users.Include(x => x.MyAlbums).SingleOrDefault(x => x.Id == int.Parse(userId));
            var userAlbums = _repository.Albums.Include(x => x.Author).Include(x => x.Pictures).Where(x => x.Author.Id == int.Parse(userId)).ToList();
            var picturesCount = userAlbums.Select(x => x.Pictures.Count).Sum();
            var countOfPictures = picturesCount +1;
            int needToDelete;
            if (!user.IsVip && countOfPictures > 100)
            {
                needToDelete = countOfPictures - 100;
                model.ValidationMessage = $"Вам нужно удалить {needToDelete} фото чтобы добавлять новый контент";
                return false;

            }
            var album = await _repository.Albums.SingleOrDefaultAsync(x => x.Id == model.AlbumId);
            var picture = await _repository.Pictures.Include(x => x.PictureAlbums).SingleOrDefaultAsync(x => x.Id == model.PhotoId);
            picture.PictureAlbums.Add(album);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<Boolean> AddPhotoInAlbum(AddPhotoInAlbumModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;
            var user = _repository.Users.Include(x => x.MyAlbums).SingleOrDefault(x => x.Id == int.Parse(userId));
            var userAlbums = _repository.Albums.Include(x => x.Author).Include(x => x.Pictures).Where(x => x.Author.Id == int.Parse(userId)).ToList();
            var picturesCount = userAlbums.Select(x => x.Pictures.Count).Sum();
            var countOfPictures = picturesCount + 1;
            int needToDelete;
            if (!user.IsVip && countOfPictures > 100)
            {
                needToDelete = countOfPictures - 100;
                model.ValidationMessage = $"Вам нужно удалить {needToDelete} фото чтобы добавлять новый контент";
                return false;

            }
            var album = await _repository.Albums.SingleOrDefaultAsync(x => x.Id == model.AlbumId);
            var picture = await _repository.Pictures.Include(x => x.PictureAlbums).SingleOrDefaultAsync(x => x.Id == model.PhotoId);
            picture.PictureAlbums.Add(album);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task DeletePhotoFromAlbum(DeleteImageModel model)
        {
            var album = await _repository.Albums.SingleOrDefaultAsync(x => x.Id == model.AlbumId);
            var picture = await _repository.Pictures.Include(x => x.PictureAlbums).SingleOrDefaultAsync(x => x.Id == model.ImageId);
            album.Pictures.Remove(picture);
            await _repository.SaveChangesAsync();
        }
        public async Task Like(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;

            var user = _repository.Users.Include(x => x.Likes).Include(x => x.Dislikes).SingleOrDefault(x => x.Id == int.Parse(userId));
            var like = user.Likes.SingleOrDefault(x => x.PictureId == id);
            var dislike = user.Dislikes.SingleOrDefault(x => x.PictureId == id);
            if (like == null)
            {
                like = new Like();
                like.PictureId = id;
                like.UserId = int.Parse(userId);
                _repository.Likes.Add(like);
                if (dislike != null)
                    _repository.Dislikes.Remove(dislike);
                _repository.SaveChanges();
                return;
            }
            _repository.Likes.Remove(like);

            _repository.SaveChanges();
        }

        public async Task Dislike(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId").Value;

            var user = _repository.Users.Include(x => x.Dislikes).Include(x => x.Likes).SingleOrDefault(x => x.Id == int.Parse(userId));
            var dislike = user.Dislikes.SingleOrDefault(x => x.PictureId == id);
            var like = user.Likes.SingleOrDefault(x => x.PictureId == id);
            if (dislike == null)
            {
                dislike = new Dislike();
                dislike.PictureId = id;
                dislike.UserId = int.Parse(userId);
                _repository.Dislikes.Add(dislike);

                if (like != null)
                    _repository.Likes.Remove(like);

                _repository.SaveChanges();
                return;
            }
            _repository.Dislikes.Remove(dislike);

            _repository.SaveChanges();
        }
    }
}
