using AlbumsSite.Command;
using AlbumsSite.Command.Image;
using AlbumsSite.Models;
using AlbumsSite.Models.Image;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Services
{
    public class NoneAuthentificateUserImageService : BaseImageService
    {
        private readonly ApplicationContext _repository;
        public NoneAuthentificateUserImageService(
          IDeleteImageCommand IDeleteImageCommand,
          IHttpContextAccessor contextAccessor,
          IParseMetaDataCommand parseMetaDataCommand,
          IWebHostEnvironment environment,
          IGetPlaceByGPSCommand getPlaceByGPSCommand,
          ApplicationContext repository
          ) : base(IDeleteImageCommand, contextAccessor, parseMetaDataCommand, environment, getPlaceByGPSCommand,
              repository, RoleName.None)
        {
            _repository = repository;
        }

        public override async Task SinglePhoto(SinglePhotoModel singlePhotoModel)
        {

            singlePhotoModel.Picture = _repository.Pictures.Include(x => x.PictureAlbums).Include(x => x.Author).Include(x => x.Likes).Include(x => x.Dislikes).SingleOrDefault(x => x.Id == singlePhotoModel.ImageId);
            if (singlePhotoModel.Picture == null) return;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserId")?.Value ?? "-1";
            singlePhotoModel.AuthorId = singlePhotoModel.Picture.Author.Id;
           
        }

    }
}
