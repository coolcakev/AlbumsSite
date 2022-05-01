using AlbumsSite.Models;
using AlbumsSite.Models.Admin;
using AlbumsSite.Models.AlbumModel;
using AlbumsSite.Models.Image;
using AlbumsSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Controllers
{
    public class AdminController : Controller
    {
        private readonly BaseAdminService _adminService;

        public AdminController(IServiceFactory serviceFactory)
        {
            _adminService = serviceFactory.GetInstance<IAdminService>() as BaseAdminService;
        }

        public async Task<IActionResult> DeleteImage(DeleteImageModel model)
        {
            await _adminService.DeleteImage(model);
            return Ok(1);
        }
        public async Task<IActionResult> DeleteAlbum(DeleteAlbumModel model)
        {
            await _adminService.DeleteAlbum(model);
            return Ok(1);
        }
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _adminService.DeleteUser(userId);
            return Ok(1);
        }
        public IActionResult Admin()
        {
            var model = new AcountUserAdminModel();
            _adminService.Admin(model);
            return View(model);
        }

        public IEnumerable<GetPhotosAdminItem> GetAllPhotos()
        {
            IEnumerable<GetPhotosAdminItem> allPhoto = _adminService.GetAllPhotos();
            return allPhoto;
        }
        public IEnumerable<GetAlbumAdminItem> GetAllAlbums()
        {
            IEnumerable<GetAlbumAdminItem> allAlbums = _adminService.GetAllAlbums();
            return allAlbums;
        }
        public IEnumerable<AcountUserAdminItem> GetUsers()
        {
            IEnumerable<AcountUserAdminItem> allUsers = _adminService.GetUsers();
            return allUsers;
        }

    }
}
