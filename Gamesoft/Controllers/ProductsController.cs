using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gamesoft.Contexts;
using Gamesoft.Models;
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
using Microsoft.CodeAnalysis;

namespace Gamesoft.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ILogger<HomeController> _logger;        
        private readonly IProductMgt _product;
        private readonly IUrlHelper _urlHelper;
        private readonly SessionHelper _sessionHelper;

        public ProductsController(ILogger<HomeController> logger, GamesoftContext context, IProductMgt product, SessionHelper sessionHelper): base(context)
        {            
            _product = product;
            _logger = logger;
            _sessionHelper = sessionHelper;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var gamesoftContext = _context.Products;

            ViewData["PageBackground"] = "bg-img-dark-3";

            return View(await gamesoftContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProductImages"] = _product.GetImages(product.ProductId);

            ViewData["PageBackground"] = "bg-img-dark-3";

            return View(product);
        }

        // GET: Products/Create
        [AuthenticatedOnly]
        public IActionResult Create()
        {
            ViewData["ProductGenres"] = _context.ProductGenres.ToList();
            ViewData["ProductStatuses"] = _context.ProductStatuses.ToList();
            ViewData["ProductEngines"] = _context.ProductEngines.ToList();
            ViewData["ProductSupports"] = _context.ProductSupports.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> Create(Product product, IFormFile featuredPicture, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                product.FeaturedImage = (featuredPicture != null ? featuredPicture.FileName : "");
                var productId = _product.CreateProduct(product);
                if (productId != 0)
                {
                    _product.UploadAndInsertImages(productId, imageFiles, featuredPicture);
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ProductGenres"] = _context.ProductGenres.ToList();
            ViewData["ProductStatuses"] = _context.ProductStatuses.ToList();
            ViewData["ProductEngines"] = _context.ProductEngines.ToList();
            ViewData["ProductSupports"] = _context.ProductSupports.ToList();


            return View(imageFiles);
        }

        // GET: Products/Edit/5
        [AuthenticatedOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProductGenres"] = _context.ProductGenres.ToList();
            ViewData["ProductStatuses"] = _context.ProductStatuses.ToList();
            ViewData["ProductEngines"] = _context.ProductEngines.ToList();
            ViewData["ProductSupports"] = _context.ProductSupports.ToList();
            ViewData["ProductImages"] = _product.GetImages(product.ProductId);

            ViewData["PageBackground"] = "bg-img-Accounts-Edit";
            ViewData["Title"] = "Edit";

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile featuredPicture, List<IFormFile> imageFiles)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.FeaturedImage = (featuredPicture != null ? featuredPicture.FileName : "");
                    if (_product.UpdateProduct(product))
                    {
                        _product.UploadAndInsertImages(product.ProductId, imageFiles, featuredPicture);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            SetTempDataMessage(MessageType.Error, "An error has occurred.");

            ViewData["ProductGenres"] = _context.ProductGenres.ToList();
            ViewData["ProductStatuses"] = _context.ProductStatuses.ToList();
            ViewData["ProductEngines"] = _context.ProductEngines.ToList();
            ViewData["ProductSupports"] = _context.ProductSupports.ToList();
            ViewData["ProductImages"] = _product.GetImages(product.ProductId);

            return View(product);
        }

        // GET: Products/Delete/5
        [AuthenticatedOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Store()
        {
            ViewData["PageBackground"] = "bg-img-dark-3";
            ViewData["ProductFavorites"] = _product.GetProductFavorites(_sessionHelper.AccountId);
            ViewData["ProductGenres"] = _context.ProductGenres.ToList();
            ViewData["ProductStatuses"] = _context.ProductStatuses.ToList();
            return View(_product.GetProductsStore(HttpContext.Request.Query));
        }

        [HttpGet]
        public IActionResult Search()
        {            
            ViewData["ProductFavorites"] = _product.GetProductFavorites(_sessionHelper.AccountId);
            return PartialView("_SearchResultsPartial", _product.GetProductsStore(HttpContext.Request.Query));
        }

        [HttpPost]
        [AuthenticatedOnly]
        public IActionResult AddToFavorite([FromBody] AddToFavoriteViewModel model)
        {
            if (model != null)
            {
                bool success;
                string message;
                if (_product.ExistFavorite(_sessionHelper.AccountId, model.ProductId))
                {
                    success = _product.DeleteFavorite(_sessionHelper.AccountId, model.ProductId);
                    message = "deleted";
                }
                else
                {
                    success = _product.AddToFavorite(_sessionHelper.AccountId, model.ProductId);
                    message = "added";
                }
                return Json(new { success, message });
            }            
            return NotFound();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        [AuthenticatedOnly]
        public async Task<IActionResult> WishList()
        {
            ViewData["PageBackground"] = "bg-img-dark-3";

            return View(_product.GetProductStoreFavorites(_sessionHelper.AccountId));
        }
    }
}
