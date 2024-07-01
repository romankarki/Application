using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Contracts.Models.Response;

namespace API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (OfficerModel)context.HttpContext.Items["Officer"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized, Access Denied" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
