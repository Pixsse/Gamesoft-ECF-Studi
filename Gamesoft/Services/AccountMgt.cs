using Microsoft.EntityFrameworkCore;
using Gamesoft.Contexts;
using Gamesoft.Extensions;
using Gamesoft.Models;
using System.Security;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Rendering;
using Gamesoft.ViewModels;
using NuGet.Common;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using static System.Net.WebRequestMethods;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Gamesoft.Services
{
    public class AccountMgt : IAccountMgt
    {
        private readonly Contexts.GamesoftContext _context;
        private readonly IEmailMgt _emailMgt;
        private readonly IUrlHelper _urlHelper;

        public enum ForgetPasswordStatus
        {
            Success,
            Failed,
            NotFound
        }
        public enum UserGroup
        {
            BasicUser,
            Admin,
            CommunityManager,
            Productor
        };

        public AccountMgt(Contexts.GamesoftContext context, IEmailMgt emailMgt, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _emailMgt = emailMgt;
            //_urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext());
            _urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext(httpContextAccessor.HttpContext, new RouteData(), new ControllerActionDescriptor()));
        }

        public Account? Login(string email, string password)
        {
            var account = _context.Accounts.FirstOrDefault(m => m.Email == email.DeleteHtml());
            if (account != null)
            {
                if (VerifyPassword(password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }

        public int CreateAccount(Account account)
        {
            if (account == null)
            {
                return 0;
            }
            account.Password = HashPassword(account.Password);
            _context.Add(account);
            _context.SaveChanges();

            return account.AccountId;
        }
        public Account? GetAccount(int accountId)
        {
            var account = _context.Accounts.Where(w => w.AccountId == accountId).FirstOrDefault();
            return account;
        }

        public UserGroup GetUserGroup(int accountId)
        {
            var account = _context.Accounts.Where(w => w.AccountId == accountId).FirstOrDefault();
            if (account != null)
            {
                return (UserGroup)account.GroupId;
            }
            return UserGroup.BasicUser;
        }
        public bool UpdateAccount(Account account)
        {
            if (account == null)
            {
                return false;
            }
            var accountToSave = new Account
            {
                AccountId = account.AccountId,
                Password = String.IsNullOrEmpty(account.Password) ? "" : account.Password,
                Email = account.Email,
                Username = account.Username,
                GroupId = account.GroupId
            };
            _context.Update(accountToSave);
            _context.SaveChanges();

            return true;
        }

        private static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

            return hashedPassword;
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return passwordMatch;
        }

        public ForgetPasswordStatus ForgetPassword(string email)
        {
            var account = _context.Accounts.FirstOrDefault(m => m.Email == email.DeleteHtml());
            if (account != null)
            {
                var resetLink = GenerateResetPasswordLink(email);

               if (_emailMgt.SendEmail(email, "Reset Password", GetResetPasswordEmailContent(resetLink)))
               {
                   return ForgetPasswordStatus.Success;
               }
               else
               {
                   return ForgetPasswordStatus.Failed;
               }

            }
            return ForgetPasswordStatus.NotFound;
        }

        public bool IsValidResetPasswordToken(string email, string token)
        {
            var resetPassword = _context.ResetPasswords
                .Include(rp => rp.Email)
                .SingleOrDefault(rp => rp.Email == email.DeleteHtml() && rp.Token == token);

            if (resetPassword != null && resetPassword.ExpiryDate > DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            var account = _context.Accounts.FirstOrDefault(m => m.Email == email.DeleteHtml());
            if (account != null)
            {
                account.Password = newPassword;
                return UpdateAccount(account);                
            }
            return false;
        }

        private string GenerateResetPasswordLink(string email)
        {

            string resetToken = Guid.NewGuid().ToString();


            var resetPasswordEntity = new ResetPassword
            {
                Email = email,
                Token = resetToken,
                ExpiryDate = DateTime.Now.AddHours(1)
            };

            var resetPassword = _context.ResetPasswords.SingleOrDefault(m => m.Email == email);
            if (resetPassword != null)
            {                
                _context.ResetPasswords.Remove(resetPassword);
            }

            _context.ResetPasswords.Add(resetPasswordEntity);            
            _context.SaveChanges();

            var resetLink = _urlHelper.Action("ResetPassword", "Accounts", new { email, resetToken });

            return resetLink;
        }

        private string GetResetPasswordEmailContent(string resetLink)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='fr'>
            <head>
                <meta charset='UTF-8'>
                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Réinitialisation de votre mot de passe</title>
            </head>
            <body>
                <p>Bonjour,</p>
                
                <p>Vous avez demandé la réinitialisation de votre mot de passe. Cliquez sur le lien ci-dessous pour procéder à la réinitialisation :</p>
                
                <p><a href='{resetLink}'>{resetLink}</a></p>
                
                <p>Ce lien expirera dans une heure. Si vous n'avez pas demandé de réinitialisation de mot de passe, veuillez ignorer ce courriel.</p>

                <p>Merci,</p>
                <p>Votre équipe Gamesoft</p>
            </body>
            </html>";
        }
    }
}
