using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Account
{
    public class RegisterAccountModel
    {
        public string Email { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Dictionary<string, string> ValidationMessage { get; set; }
    }
}
