using System;
using System.Linq;
using JwtLib.Entities;
using JwtLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JwtLib.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var success = (bool?)context.HttpContext.Items["Success"];
         
            if (success == null) // not middleware
            {                
                context.Result = new JsonResult(new { message = "Not Apply Middleware" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if(success == false) // not logged in
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
