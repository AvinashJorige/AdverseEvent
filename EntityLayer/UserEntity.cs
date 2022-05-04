using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class UserEntity
    {
        public string UserCode { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDetail
    {
        public string UserCode { get; set; }
        public DateTime LastLogin { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class UserResetDetail
    {
        public string UserCode { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
