using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShop.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.GetString("Token");
            if (session == null)
            {
                context.Result=new RedirectToActionResult("Login","Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
