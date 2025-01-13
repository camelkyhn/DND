using DND.Middleware.Constants;
using DND.Middleware.Extensions;
using DND.Middleware.System;
using DND.Web.Server.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DND.Web.Server.Attributes
{
    public class AppAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _permission;

        public AppAuthorizeAttribute(string permission)
        {
            _permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.GetUserId() == null || context.HttpContext.User.Identity == null)
            {
                context.HttpContext.SignOutAsync();
                var result = new ObjectResult(new Result().Error(Errors.UnauthorizedRequest));
                result.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = result;
            }
            else if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!context.HasAccess(_permission))
                {
                    var result = new ObjectResult(new Result().Error(Errors.ForbiddenRequest));
                    result.StatusCode = StatusCodes.Status403Forbidden;
                    context.Result = result;
                }
            }
        }
    }
}