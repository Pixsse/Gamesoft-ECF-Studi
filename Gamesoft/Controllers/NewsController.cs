using Gamesoft.Attributes;
using Gamesoft.Contexts;
using Gamesoft.Models;
using Gamesoft.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Gamesoft.Controllers
{
    public class NewsController : BaseController
    {
        private readonly IHubContext<NewsHub> _hubContext;

        public NewsController(GamesoftContext context, IHubContext<NewsHub> hubContext) : base(context)
        {
            _hubContext = hubContext;
        }

        // GET: NewsController
        public ActionResult Index()
        {
            ViewData["PageBackground"] = "bg-img-Accounts-Details";
            return View(_context.News.ToList());
        }

        // GET: NewsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            ViewData["PageBackground"] = "bg-img-Accounts-Details";

            return View(news);            
        }

        // GET: NewsController/Create
        [AuthenticatedOnly]
        public ActionResult Create()
        {
            ViewData["PageBackground"] = "bg-img-Accounts-Details";
            return View();
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> Create([Bind("Title,Description,Author")] News news)
        {
            if (ModelState.IsValid)
            {
                _context.News.Add(news);
                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction(nameof(Index));
                    await _hubContext.Clients.All.SendAsync("ReceiveNews", _context.News.ToList());
                }
            }            
            return View(news);
        }

        // GET: NewsController/Edit/5
        [AuthenticatedOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            ViewData["PageBackground"] = "bg-img-Accounts-Details";
            ViewData["Title"] = "Edit";

            return View(news);
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,Title,Description,Author")] News news)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.News.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewstExists(news.NewsId))
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
            return View(news);
        }

        // GET: NewsController/Delete/5
        [AuthenticatedOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: NewsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthenticatedOnly]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool NewstExists(int id)
        {
            return _context.News.Any(e => e.NewsId == id);
        }

        public IActionResult NewsFeed()
        {
            return View(_context.News.ToList());
        }
    }
}
