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
    [Route("Albums")]
    public class AlbumController : Controller
    {
        private readonly BaseAlbumService _baseAlbumService;

        public AlbumController(IServiceFactory serviceFactory)
        {
            _baseAlbumService = serviceFactory.GetInstance<IBaseAlbumService>() as BaseAlbumService;
        }
        [Route("{albumName}")]
        public async Task<IActionResult> SingleAlbum(string albumName)
        {
            SingleAlbumModel albumModel = new SingleAlbumModel();
            albumModel.AlbumName = albumName;
            await _baseAlbumService.SingleAlbum(albumModel);
            if (albumModel.Album == null)
                return NotFound();
            return View(albumModel);
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            var model = new AllAlbumsModel();
            _baseAlbumService.FillIndex(model);
            return View(model);
        }
        [HttpPost("ChangeThumbail")]
        public async Task<IActionResult> ChangeThumbail(AlbumThumbailModel model)
        {
            await _baseAlbumService.ChangeThumbail(model);

            return Json(model.SmallPathThumbail);
        }
        [HttpPost("CreateAlbum")]
        public async Task<IActionResult> CreateAlbum(CreateAlbumModel model)
        {
            var isValid = await _baseAlbumService.CreateAlbum(model);
            if (!isValid)
                return Json(model);

            return Json(new { redirectToUrl = Url.Action(model.Name, "Albums"), AlbumId = model.AlbumId });

        }
        [HttpGet("GetPopularAlbum")]
        public List<Album> GetPopularAlbum()
        {
            return _baseAlbumService.GetPopularAlbum();
        }
        [HttpPost("DeleteSelectedAlbum")]
        public async Task<IActionResult> DeleteSelectedAlbum(DeleteSelectedAlbumModel model)
        {
            await _baseAlbumService.DeleteSelectedAlbum(model);
            return Ok();

        }
        [HttpPost("EditAlbum")]
        public async Task<IActionResult> EditAlbum(EditAlbumModel model)
        {
            var isValid = await _baseAlbumService.EditAlbum(model);
            if (!isValid)
                return NotFound();
            return Ok();
        }
        [HttpPost("DeleteAlbum")]
        public async Task<IActionResult> DeleteAlbum(DeleteAlbumModel model)
        {
            await _baseAlbumService.DeleteAlbum(model);
            
            return Json(new { redirectToUrl = $"/user-{model.UserName}?id={model.UserId}" });
        }

    }
}
