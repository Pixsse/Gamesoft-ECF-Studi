using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Gamesoft.Helpers;

namespace Gamesoft.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthenticatedOnlyAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.RequestServices.GetRequiredService<SessionHelper>();
            if (session.AccountId == 0)
            {
                session.BackToPage = context.HttpContext.Request.Path;
                context.Result = new RedirectToActionResult("Login", "Accounts", null);                
                
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}