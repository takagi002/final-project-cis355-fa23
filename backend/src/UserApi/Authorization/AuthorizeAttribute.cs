namespace UserApi.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserApi.Entities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string? Role { get; set; }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Skip authorization if action is decorated with [AllowAnonymous] attribute
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            return;

        // Get user from context
        var user = (User?)context.HttpContext.Items["User"];

        // If user does not exist, return unauthorized
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // If controller enforcing role-based authorization, check if user has access  
        if (!string.IsNullOrEmpty(Role) && !user.Role.Equals(Role, StringComparison.InvariantCultureIgnoreCase))
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}

