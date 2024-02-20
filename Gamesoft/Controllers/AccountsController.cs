using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gamesoft.Contexts;
using Gamesoft.ViewModels;
using Gamesoft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using static Gamesoft.Services.AccountMgt;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using Gamesoft.Helpers;
using Microsoft.AspNetCore.HttpLogging;
using Gamesoft.Attributes;
using NuGet.Common;
using static System.Collections.Specialized.BitVector32;

namespace Gamesoft.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly ILogger<HomeController> _logger;        
        private readonly IAccountMgt _account;
        private readonly IUrlHelper _urlHelper;
        private readonly SessionHelper _sessionHelper;

        public AccountsController(ILogger<HomeController> logger, GamesoftContext context, IAccountMgt account, SessionHelper sessionHelper): base(context)
        {            
            _account = account;

            _logger = logger;
            _account = account;
            _sessionHelper = sessionHelper;
        }

        // GET: Accounts
        [AuthenticatedOnly]
        public async Task<IActionResult> Index()
        {
            var gamesoftContext = _context.Accounts;

            ViewData["PageBackground"] = "bg-img-Accounts-Index";

            return View(await gamesoftContext.ToListAsync());
        }

        // GET: Products/Details/5
        [AuthenticatedOnly]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            ViewData["PageBackground"] = "bg-img-Product-Details";

            return View(account);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.AccountGroups, "GroupId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Create([Bind("Email,Password,Username")] Models.Account account)
        {
            if (ModelState.IsValid)
            {
                var accountId = _account.CreateAccount(account);
                if (accountId != 0)
                    return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.AccountGroups, "GroupId", "Name", account.GroupId);
            return View(account);
        }

        // GET: Products/Edit/5
        [AuthenticatedOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            ViewData["GroupId"] = new SelectList(_context.AccountGroups, "GroupId", "Name", account.GroupId);
            ViewData["PageBackground"] = "bg-img-Accounts-Edit";
            ViewData["Title"] = "Edit";

            return View(account);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Email,Password,Username,GroupId")] Models.Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _account.UpdateAccount(account);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }         
            return View(account);
        }

        // GET: Products/Delete/5
        [AuthenticatedOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }

        //------------------------------ //
        // ----------- Login ----------- //

        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            ViewData["PageBackground"] = "bg-img-signup-login";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "UserName"),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            var account = _account.Login(loginViewModel.Email, loginViewModel.Password);
            if (account != null)
            {

                HttpContext.Session.SetInt32("AccountId", account.AccountId);
                HttpContext.Session.SetInt32("UserGroupId", account.GroupId);

                if (!String.IsNullOrEmpty(_sessionHelper.BackToPage))
                {
                    var backToPage = _sessionHelper.BackToPage;
                    _sessionHelper.BackToPage = "";
                    return Redirect(backToPage);
                }
                return RedirectToAction("Index", "Home");
            }
            SetTempDataMessage(MessageType.Error, "Incorrect login details.");

            return View(loginViewModel);
        }

        [AuthenticatedOnly]
        public IActionResult Logout()
        {
            HttpContext.Session.SetInt32("AccountId", 0);
            HttpContext.Session.SetInt32("UserGroupId", 0);

            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ForgetPassword()
        {
            ViewData["Title"] = "Forget password";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var status = _account.ForgetPassword(model.Email);
                switch (status)
                {
                    case ForgetPasswordStatus.Success:
                        SetTempDataMessage(MessageType.Success, "Email Send");                        
                        break;
                    case ForgetPasswordStatus.Failed:                        
                        SetTempDataMessage(MessageType.Error, "Error");
                        break;
                    case ForgetPasswordStatus.NotFound:
                        SetTempDataMessage(MessageType.Error, "Account not found");                        
                        break;
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {

            if (!_account.IsValidResetPasswordToken(email, token)) 
            {
                ViewBag["ResetPasswordTokenExpired"] = "Le lien a expiré";
                return View();
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        [HttpPost("resetpassword")]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_account.IsValidResetPasswordToken(model.Email, model.Token))
                {
                    if (_account.UpdatePassword(model.Email, model.NewPassword))
                    {
                        SetTempDataMessage(MessageType.Success, "Successful password reset");
                        return RedirectToAction("Login", "Accounts");
                    }
                    else 
                    {
                        SetTempDataMessage(MessageType.Error, "An error occurred while resetting the password");
                    }
                }
            }
            return View(model);
        }
    }
}
