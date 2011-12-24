using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Attributes
{
	/// <summary>
	/// Als de gebruiker een verouderde browser gebruikt krijgt hij eenmaal de waarschuwing te zien dat hij moet updaten
	/// </summary>
	public class OldBrowserWarningAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var request = filterContext.HttpContext.Request;
			var browser = request.Browser;
			if (request.Cookies["OldBrowserWarning"] != null)
				return;

			if ((browser.Browser == "Chrome" && browser.MajorVersion < 12) ||
				(browser.Browser == "Firefox" && browser.MajorVersion < 5) ||
				(browser.Browser == "IE" && browser.MajorVersion < 9) ||
				(browser.Browser == "Safari" && browser.MajorVersion < 5) ||
				(browser.Browser == "Opera" && browser.MajorVersion < 9))
			{
				filterContext.Result = new ViewResult { 
					ViewName = "OldBrowserWarning", 
					ViewData = {
						Model = browser 
					} 
				};

				filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			}
		}
	}
}