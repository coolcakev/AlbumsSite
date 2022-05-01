using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Models.Admin
{
    public class AcountUserAdminModel
    {
        public IEnumerable<AcountUserAdminItem> AllUsers { get; set; }
    }
   public class AcountUserAdminItem
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string Email { get; set; }
        public bool IsVip { get; set; }
        public List<int> AlbumId { get; set; }
        public List<string> AlbumName { get; set; }
    }
}
