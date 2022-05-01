using AlbumsSite.Models.AlbumModel;
using AlbumsSite.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Controllers
{
    public class UserController : Controller
    {
        private readonly IBaseUserService _baseUserService;

        public UserController(IBaseUserService baseUserService)
        {
            _baseUserService = baseUserService;
        }
        [HttpGet("user-{login}")]
        public IActionResult UserPage(int id)
        {
            var user = _baseUserService.UserPage(id);
            if(user==null)
                return NotFound();

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> SetVipStatus(int id)
        {
            await _baseUserService.SetVipStatus(id);
            return Ok();
        }
        [HttpPost]
        public async Task DeleteUser(int id)
        {
            await _baseUserService.DeleteUser(id);
        }

    }
}
