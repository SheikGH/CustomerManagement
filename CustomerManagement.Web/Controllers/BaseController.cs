using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerManagement.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Request.Cookies["userName"] != null)
            {
                ViewBag.M_UserName = Request.Cookies["userName"];
            }
            else if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("userName")))
            {
                ViewBag.M_UserName = HttpContext.Session.GetString("userName");
            }
            else
            {
                ViewBag.M_UserName = "";
            }

            base.OnActionExecuting(context);
        }
    }
}
