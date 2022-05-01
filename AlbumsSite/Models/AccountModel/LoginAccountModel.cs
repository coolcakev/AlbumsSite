using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Account
{
    public class LoginAccountModel
    {
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string ValidationMessage { get; set; }
    }
}
