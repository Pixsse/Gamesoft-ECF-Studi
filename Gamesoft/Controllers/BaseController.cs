using Gamesoft.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gamesoft.Models;
using Microsoft.AspNetCore.Http;
using Gamesoft.Helpers;
using Gamesoft.Contexts;

namespace Gamesoft.Controllers
{
    public class BaseController : Controller
    {
        public enum MessageType { Success, Error }
        protected readonly GamesoftContext _context;

        protected BaseController(GamesoftContext context)
        {
            _context = context;
        }
        protected void SetTempDataMessage(MessageType messageType, string message)
        {
            TempData["Message"] = message;
            TempData["TypeMessage"] = messageType == MessageType.Success ? "Success" : "Error";
        }
    }
}
