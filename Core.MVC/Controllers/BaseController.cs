using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace Core.MVC.Controllers
{
	public class BaseController : Controller
	{
		public void EnforceCanonicalURL(RouteValueDictionary routeValues)
		{
			string canonicalUrl = Url.RouteUrl(routeValues);
			if (HttpContext.Request.Url.PathAndQuery != canonicalUrl)
				Response.RedirectPermanent(canonicalUrl, true);
		}

		protected string RenderPartialViewToString(string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = ControllerContext.RouteData.GetRequiredString("action");

			ViewData.Model = model;

			using (var writer = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer);
				viewResult.View.Render(viewContext, writer);

				return writer.GetStringBuilder().ToString();
			}
		}

		protected bool iPad
		{
			get
			{
				return Request.UserAgent.ToLowerInvariant().Contains("ipad");
			}
		}
	}
}