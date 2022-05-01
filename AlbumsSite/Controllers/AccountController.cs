using AlbumsSite.Models;
using AlbumsSite.Models.Account;
using AlbumsSite.Services.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlbumsSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountAuthenticateService _accountAuthenticateService;

        public AccountController(IAccountAuthenticateService accountAuthenticateService) 
        {
            _accountAuthenticateService = accountAuthenticateService;
        }
        [HttpGet]
        public ActionResult Login()
        {
            var model = new LoginAccountModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous] 
        public async Task<ActionResult> Login(string userLogin,  string password,
            string returnUrl)
        {
            var model = new LoginAccountModel()
            {
                UserLogin = userLogin,
                Password = password
            };
            
            
            if (!ModelState.IsValid)
                return View(model);

            var claimsIdentity = await _accountAuthenticateService.Login(userLogin, password);
            if (claimsIdentity == null || (string.IsNullOrWhiteSpace(userLogin)  && string.IsNullOrWhiteSpace(password))|| (string.IsNullOrWhiteSpace(password)))
            {
                model.ValidationMessage= "Password or login is invalid";
                return View(model);
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            // аутентификация
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }   
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterAccountModel person)
        {
            var isValid = await _accountAuthenticateService.RegisterPerson(person);
            if (!isValid)
                return Json(person);
            return Json(new { redirectToUrl = Url.Action("Login", "Account") });
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
            
        
    }
}
