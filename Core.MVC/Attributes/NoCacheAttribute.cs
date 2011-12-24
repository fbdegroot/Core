using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Attributes
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context) {
            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }
}