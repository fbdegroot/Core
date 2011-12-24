using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;

namespace Core.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ManualAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The key to the authentication token that should be submitted somewhere in the request.
        /// </summary>
        private const string TokenKey = "AuthenticationToken";

        /// <summary>
        /// This changes the behavior of AuthorizeCore so that it will only authorize
        /// users if a valid token is submitted with the request.
        /// </summary>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext) {
            var token = httpContext.Request.Params[TokenKey];
            if (token != null) {
                var ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null) {
                    var identity = new FormsIdentity(ticket);
                    var roles = System.Web.Security.Roles.GetRolesForUser(identity.Name);
                    var principal = new GenericPrincipal(identity, roles);
                    httpContext.User = principal;
                }
            }

            return base.AuthorizeCore(httpContext);
        }
    }
}