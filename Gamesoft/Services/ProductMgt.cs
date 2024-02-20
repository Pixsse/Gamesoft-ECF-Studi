using Gamesoft.Contexts;
using Gamesoft.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.Identity.Client;
using Azure.Core;
using Gamesoft.Extensions;
using Newtonsoft.Json.Linq;

namespace Gamesoft.Services
{
    public class ProductMgt : IProductMgt
    {
        private readonly Contexts.GamesoftContext _context;
        private readonly IImageMgt _imageMgt;

        public ProductMgt(Contexts.GamesoftContext context, IImageMgt imageMgt)
        {
            _context = context;
            _imageMgt = imageMgt;
        }
        public int CreateProduct(Product product)
        {
            if (product == null)
            {
                return 0;
            }
            _context.Add(product);
            
            _context.SaveChanges();

            return product.ProductId;
        }
        public bool UpdateProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }
            _context.Update(product);

            _context.SaveChanges();

            return true;
        }

        public void UploadAndInsertImages(int productId, List<IFormFile> imageFiles, IFormFile featuredImage)
        {
            if (featuredImage != null && featuredImage.Length > 0) 
            {
                var filePath = BuildAndGetFilePath(productId, featuredImage.FileName, true);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    featuredImage.CopyTo(stream);
                    InsertImage(productId, featuredImage.FileName);
                }
            }

            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var filePath = BuildAndGetFilePath(productId, imageFile.FileName, false);                

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                        InsertImage(productId, imageFile.FileName);
                    }
                }
            }
            _context.SaveChanges();
        }

        private string BuildAndGetFilePath(int productId, string FileName, bool isFeaturedImage)
        {
            var baseDirectory = Path.Combine("wwwroot\\img\\products", productId.ToString(), (isFeaturedImage ? "featured" : ""));

            var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), baseDirectory);
            Directory.CreateDirectory(fileDirectory);

            return Path.Combine(fileDirectory, FileName);
        }

        private void InsertImage(int productId, string imageName)
        {
            if (productId == 0)
            {
                return;
            }
            ProductImage productImage = new ProductImage();
            productImage.ProductId = productId;
            productImage.Image = imageName;

            _context.Add(productImage);
        }

        public List<ProductImage> GetImages(int productId) 
        {
            return _context.ProductImages.Where(w => w.ProductId == productId).ToList();
        }

        public List<VProductsStore> GetProductsStore(IQueryCollection query)
        {
            var productFilterParameters = GetProductFilterParametersFromQuery(query);
            var request = BuildRequest(productFilterParameters);
            return request.ToList();            
        }

        public bool ExistFavorite(int accountId, int productId)
        {
            return(_context.ProductFavorites.Any(w => w.AccountId == accountId && w.ProductId == productId));
        }

        public int CountFavorite(int productId)
        {
            return (_context.ProductFavorites.Where(w => w.ProductId == productId).Count());
        }

        public bool AddToFavorite(int accountId, int productId)
        {            
            var productFavorite = new ProductFavorite();
            productFavorite.ProductId = productId;
            productFavorite.AccountId = accountId;
            _context.Add(productFavorite);         
            _context.SaveChanges();

            return true;
        }
        public bool DeleteFavorite(int accountId, int productId)
        {
            var productFavorite = _context.ProductFavorites.Where(w => w.AccountId == accountId && w.ProductId == productId).FirstOrDefault();
            if (productFavorite != null)
            {
                _context.Remove(productFavorite);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<ProductFavorite> GetProductFavorites(int accountId)
        {
            var productFavorites = _context.ProductFavorites.Where(w => w.AccountId == accountId).ToList();

            return(productFavorites);
        }

        public List<VProductsStore> GetProductStoreFavorites(int accountId)
        {
            var query = from product in _context.VProductsStores
                        join favorite in _context.ProductFavorites
                        on product.ProductId equals favorite.ProductId
                        where favorite.AccountId == accountId
                        select product;

            return query.ToList();
        }

        private IQueryable<VProductsStore> BuildRequest(ProductFilterParameters productFilterParameters)
        {
            var request = _context.VProductsStores.AsQueryable();
            if (!string.IsNullOrEmpty(productFilterParameters.SearchedText))
            {                
                var arrayText = productFilterParameters.SearchedText.Split(' ');
                foreach (var s in arrayText)
                {
                    request = request.Where(w => w.Title.Contains(s));
                }
            }
            if (productFilterParameters.Genre != null)
            {
                request = request.Where(w => productFilterParameters.Genre.Contains(w.GenreId));
            }
            if (productFilterParameters.Status != null)
            {
                request = request.Where(w => productFilterParameters.Status.Contains(w.StatusId));
            }
            return request;
        }

        private ProductFilterParameters GetProductFilterParametersFromQuery(IQueryCollection query)
        {
            int number;
            ProductFilterParameters productFilterParameters = new ProductFilterParameters();
            foreach (var q in query)
            {
                var value = q.Value.ToString().DeleteHtml().Trim();
                if (!String.IsNullOrEmpty(value))
                { 
                    switch (q.Key)
                    {
                        case "genre":
                            productFilterParameters.Genre = (from s in value.Split(",") select int.TryParse(s, out number) ? number : 0).ToList();                            
                            break;
                        case "status":
                            productFilterParameters.Status = (from s in value.Split(",") select int.TryParse(s, out number) ? number : 0).ToList();
                            break;
                        case "searchedtext":
                            productFilterParameters.SearchedText = value;
                            break;
                        }
                    }
            }
            return productFilterParameters;
        }

        public ProductImage GetLastProductImage()
        {
            var r = _context.Products
                    .OrderByDescending(ob => ob.DateCreated)
                    .Join(_context.ProductImages,
                          product => product.ProductId,
                          image => image.ProductId,
                          (product, image) => new { Product = product, Image = image })
                    .Select(result => new { ProductId = result.Product.ProductId, Image = result.Image.Image })
                    .FirstOrDefault();

            ProductImage lastProductImage = new ProductImage();
            lastProductImage.ProductId = r.ProductId;
            lastProductImage.Image = r.Image;
            return lastProductImage;
        }
    }
}
