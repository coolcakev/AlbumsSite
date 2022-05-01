using AlbumsSite.Models;
using AlbumsSite.Models.HomeModel;
using AlbumsSite.Models.Image;
using AlbumsSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly BaseHomeService _homeService;

        public HomeController(IServiceFactory serviceFactory)
        {
            _homeService = serviceFactory.GetInstance<IBaseHomeService>() as BaseHomeService;
        }
        
        public IActionResult Index(FillIndexModel model)
        {
            _homeService.FillIndex(model);
            return View(model);
        }
        public List<Picture> FindPhoto( string inputText )
        {
            return _homeService.FindPhoto(inputText);
        }
        public List<Picture> FindPhotoByParameters(string inputText)
        {
            return _homeService.FindPhotoByParameters(inputText);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}
