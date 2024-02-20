using Gamesoft.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security;
using static Gamesoft.Services.AccountMgt;

namespace Gamesoft.Services
{
    public interface IAccountMgt
    {
        public Account? Login(string email, string password);
        public int CreateAccount(Account account);
        public Account? GetAccount(int accountId);
        public UserGroup GetUserGroup(int accountId);
        public ForgetPasswordStatus ForgetPassword(string email);
        public bool UpdateAccount(Account account);
        public bool IsValidResetPasswordToken(string email, string token);
        public bool UpdatePassword(string email, string newPassword);
    }
}
