using Gamesoft.Services;
using Gamesoft.Extensions;
using Gamesoft.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security;
using Gamesoft.ViewModels;
using static Gamesoft.Services.AccountMgt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using Gamesoft.Contexts;
using Gamesoft.Attributes;
using Gamesoft.Models;

namespace Gamesoft.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountMgt _account;
        private readonly IProductMgt _product;

        public HomeController(ILogger<HomeController> logger, GamesoftContext context, IProductMgt product, IAccountMgt account): base(context)
        {
            _logger = logger;
            _account = account;
            _product = product;
        }

        public IActionResult Index()
        {

            ViewData["Title"] = "Home Page";
            ViewData["AccountId"] = HttpContext.Session.GetInt32("AccountId");

            var productImage = _product.GetLastProductImage();

            ViewData["LastProductImg"] = string.Format("products/{0}/{1}", productImage.ProductId, productImage.Image);
            
            // Get Cookies

            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            Debug.Print(userId);

            return View();
        }

        public IActionResult News()
        {
            ViewData["Title"] = "News";

            return View(_context.News.ToList());
        }

        public IActionResult About()
        {
            ViewData["Title"] = "About";
            ViewData["PageBackground"] = "bg-img-About";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact";

            return View();
        }

        public IActionResult FAQ()
        {
            ViewData["Title"] = "FAQ";

            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy";
            ViewData["PageBackground"] = "bg-img-privacy";

            return View();
        }
    }
}
