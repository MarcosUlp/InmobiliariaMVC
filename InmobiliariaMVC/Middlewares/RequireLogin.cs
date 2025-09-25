using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InmobiliariaMVC.Middlewares
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
