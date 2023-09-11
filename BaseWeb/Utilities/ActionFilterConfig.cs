using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;

using System.Web;

namespace BaseWeb.Utilities
{
    public class ActionFilterConfig
    {
        public class Authentication : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {


                if (filterContext.HttpContext.Session.GetString("UserName") == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "Auth" },
                                { "Action", "Login" }
                                });
                }

            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var httpContext = filterContext.HttpContext.Request.Headers["Referer"].ToString();

                if (filterContext.HttpContext.Request.GetTypedHeaders().Referer == null ||
                    filterContext.HttpContext.Request.GetTypedHeaders().Referer.Host != filterContext.HttpContext.Request.Host.Host)
                {
                    filterContext.HttpContext.Session.Clear();

                    filterContext.Result = new RedirectToRouteResult(new
                                   RouteValueDictionary(new { controller = "Auth", action = "Login", area = "" }));
                }
            }
        }
    }
}
