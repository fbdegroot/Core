using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Localization
{
	public static class Extensions
	{
		public static MvcHtmlString Resource(this HtmlHelper htmlhelper, string expression, params object[] args)
		{
			string virtualPath = GetVirtualPath(htmlhelper);
			return MvcHtmlString.Create(GetResourceString(htmlhelper.ViewContext.HttpContext, expression, virtualPath, args));
		}

		public static string Resource(this Controller controller, string expression, params object[] args)
		{
			return GetResourceString(controller.HttpContext, expression, "~/", args);
		}

		public static string Resource(HttpContextBase httpContext, string expression, params object[] args)
		{
			return GetResourceString(httpContext, expression, null, args);
		}

		private static string GetResourceString(HttpContextBase httpContext, string expression, string virtualPath, object[] args)
		{
			string result;
			var identity = SqlResourceProvider.GetClassKey(expression);
			if (identity.ClassKey != null)
				result = (string)httpContext.GetGlobalResourceObject(identity.ClassKey, identity.ResourceKey, CultureInfo.CurrentUICulture);
			else 
				result = (string)httpContext.GetLocalResourceObject(virtualPath, identity.ResourceKey, CultureInfo.CurrentUICulture);

			return args.Length > 0
			    ? string.Format(result, args)
			    : result;
		}

		private static string GetVirtualPath(HtmlHelper htmlhelper)
		{
			RazorView view = htmlhelper.ViewContext.View as RazorView;

			if (view != null)
				return view.ViewPath;

			return null;
		}
	}
}
