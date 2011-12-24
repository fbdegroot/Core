using System;
using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Extensions
{
    public static class CaptchaExtensions
    {
		internal const string SessionKeyPrefix = "__Captcha";
        private const string ImgFormat = @"<img src=""{0}"" class=""captcha"" /><input type=""hidden"" name=""{1}"" value=""{2}"" />";

        public static MvcHtmlString Captcha(this HtmlHelper html, string name) {
            var challengeGuid = Guid.NewGuid().ToString();
            html.ViewContext.HttpContext.Session[SessionKeyPrefix + challengeGuid] = MakeRandomSolution();

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string url = urlHelper.Action("Render", "Captcha", new { challengeGuid });
            string htmlToDisplay = string.Format(ImgFormat, url, name, challengeGuid);

            return MvcHtmlString.Create(htmlToDisplay);
        }

        private static string MakeRandomSolution() {
            var rdm = new Random();
            var length = rdm.Next(5, 7);
            var buffer = new char[length];

            for (var i = 0; i < length; i++)
                buffer[i] = (char)('a' + rdm.Next(26));

            return new string(buffer);
        }

        public static bool Verify(HttpContextBase context, string challengeGuid, string attemptedSolution) {
            var solution = (string)context.Session[SessionKeyPrefix + challengeGuid];
            context.Session.Remove(SessionKeyPrefix + challengeGuid);

            return solution != null && attemptedSolution == solution;
        }
    }
}