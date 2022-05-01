using AlbumsSite.Command;
using AlbumsSite.Helper;
using AlbumsSite.Models;
using AlbumsSite.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlbumsSite.Services.Account
{
    public interface IAccountAuthenticateService
    {
        Task<ClaimsIdentity> Login(string login, string password);
        Task<bool> RegisterPerson(RegisterAccountModel user);   
    }

    public class AccountAuthenticateService : IAccountAuthenticateService
    {
        private readonly IAccountAuthenticateCommand _accountAuthenticateCommand;
        private readonly ApplicationContext _baseRepository;

        public AccountAuthenticateService(
            IAccountAuthenticateCommand accountAuthenticateCommand,
            ApplicationContext baseRepository)

        {
            _accountAuthenticateCommand = accountAuthenticateCommand;
            _baseRepository = baseRepository;
        }

        public async Task<ClaimsIdentity> Login(string login, string password)
        {
            var user =  _baseRepository.Users.SingleOrDefault(x => x.UserLogin == login);
            if (user == null)
                return null;
            if ((string.IsNullOrWhiteSpace(password)) || user.Password != SecurityHelper.ComputeSha256Hash(password))
                return null;
            return await _accountAuthenticateCommand.ExecuteAsync(user); ;
        }
        public async Task<bool> RegisterPerson(RegisterAccountModel user)
        {
            user.ValidationMessage =  new Dictionary<string, string>();
            Regex loginFormat = new Regex(@"^[^0-9][a-zA-Z0-9_]{3,}$");
            var mailFormat = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");
            var isValidLoginFormat = loginFormat.IsMatch(user.UserLogin);
            var isValidEmailFormat = mailFormat.IsMatch(user.Email);
            if (!isValidLoginFormat)
            {
                user.ValidationMessage.Add("UserLogin", "Введите только цифры и буквы и/или начинайте логин только с буквы");
            }
            if (user.Password.Length < 8)
            {
                user.ValidationMessage.Add("Password", "Пароль должен быть длиннее 8 символов");
            }
            if (!isValidEmailFormat)
            {
                user.ValidationMessage.Add("Email", "Неправильный формат электронной почты");
            }
            if (user.Password != user.ConfirmPassword)
            {
                user.ValidationMessage.Add("ConfirmPassword", "Пароли не совпадают");
            }
            if (user.ValidationMessage.Count >= 1)
            {
                return false;
            }
            var newPerson = new User()
            {
                UserLogin = user.UserLogin,
                Email = user.Email,
                Password = SecurityHelper.ComputeSha256Hash(user.Password),
                Role = RoleName.User,
                
            };
          await  _baseRepository.Users.AddAsync(newPerson);
          await   _baseRepository.SaveChangesAsync();
            return true;
        }

       
     

    }

}

