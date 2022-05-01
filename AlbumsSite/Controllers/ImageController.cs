using AlbumsSite.Models;
using AlbumsSite.Models.AlbumModel;
using AlbumsSite.Models.Image;
using AlbumsSite.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Controllers
{
    [Route("[controller]/[action]")]
    public class ImageController : Controller
    {
        private readonly BaseImageService _baseImageService;
        public ImageController(IServiceFactory serviceFactory)
        {
            _baseImageService = serviceFactory.GetInstance<IBaseImageService>() as BaseImageService;
        }
        public async Task <IActionResult> SinglePhoto(int id)
        {
            
            SinglePhotoModel singlePhotoModel = new()
            {
                ImageId = id
            };
            await _baseImageService.SinglePhoto(singlePhotoModel);
            if (singlePhotoModel.Picture == null) return NotFound();

            return View(singlePhotoModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateImage( CreateImageModel model )
        {
          var checkForAnswer= await _baseImageService.CreateImage(model);
            if (!checkForAnswer)
                return NotFound(model.ValidationMessage);
            else
                return Json(new { redirectToUrl = $"/Albums/{model.AlbumName}?id={model.AlbumId}" });
        }
         
        public async Task<IActionResult> ChangeImageData(ChangeImageDataModel model)
        {
            await _baseImageService.ChangeImageData(model);
            return Json(new { redirectToUrl = Url.Action("SinglePhoto", "Image", new { id = model.Id }) });
        }

        public async Task<IActionResult> DeleteImage(DeleteImageModel model)
        {
            await _baseImageService.DeleteImage(model);
            return Json(new { redirectToUrl = $"/user-{model.UserName}?id={model.AuthorId}" } );
        }
        [HttpPost]
        public async Task<IActionResult> DeletePhotoFromAlbum(DeleteImageModel model)
        {
            await _baseImageService.DeletePhotoFromAlbum(model);
            return Ok();
        }

        [HttpPost]
        public async Task<IEnumerable<Album>> AddImageModalOpen( int id)
        {
            AddImageModalOpenModel addImageModalOpenModel = new();
            addImageModalOpenModel.ImageId = id;
            await _baseImageService.AddImageModalOpen(addImageModalOpenModel);
            
            return addImageModalOpenModel.UsersAlbums;
        }
        public async Task<IActionResult> AddImageToAlbumFromGalery(AddPhotoInAlbumModel model)
        {
            if (model.AlbumId == 0) return NotFound(); 
             var checkForAnswer = await _baseImageService.AddImageToAlbumFromGalery(model);

            if (!checkForAnswer)
                return NotFound(model.ValidationMessage);
            else
                return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoInAlbum(AddPhotoInAlbumModel model)
        {
            await _baseImageService.AddPhotoInAlbum(model);
            var checkForAnswer = await _baseImageService.AddImageToAlbumFromGalery(model);

            if (!checkForAnswer)
                return NotFound(model.ValidationMessage);
            else
                return Json(new { redirectToUrl = Url.Action("SinglePhoto", "Image", new { id = model.PhotoId }) });
        }
        [HttpPost("{photoId}")]
        public IActionResult Like(string photoId)
        {
            _baseImageService.Like(int.Parse(photoId));
            return Ok();
        }
        [HttpPost("{photoId}")]
        public IActionResult Dislike(string photoId)
        {
            _baseImageService.Dislike(int.Parse(photoId));
            return Ok();
        }
    }
}
