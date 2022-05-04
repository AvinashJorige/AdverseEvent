using EntityLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class LoginService
    {
        LoginRepo repo = null;
        public LoginService()
        {
            repo = new LoginRepo();
        }

        public UserLoginDetail ValidateUser(string UserCode, string Password)
        {
            return repo.ValidateUser(UserCode, Password);
        }

        public int SaveResetPasswordCode(string ResetCode, string CustId)
        {
            return repo.SaveResetPasswordCode(ResetCode, CustId);
        }

        public UserResetDetail GetCustomerByResetCode(string ResetCode)
        {
            return repo.GetCustomerByResetCode(ResetCode);
        }

        public string ResetPasswordFromMail(string UserCode, string Password, string ResetCode)
        {
            return repo.ResetPasswordFromMail(UserCode, Password, ResetCode);
        }

        public UserResetDetail GetCustomerByEmail(string EmailId)
        {
            return repo.GetCustomerByEmail(EmailId);
        }

        public string GetValidResetCode(string ResetCode)
        {
            return repo.GetValidResetCode(ResetCode);
        }

        public string ChangePassword(string UserCode, string Password)
        {
            return repo.ChangePassword(UserCode, Password);
        }
    }
}
