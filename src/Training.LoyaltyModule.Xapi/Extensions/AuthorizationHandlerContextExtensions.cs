using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Platform.Core;

namespace Training.LoyaltyModule.Xapi.Extensions
{
    public static class AuthorizationHandlerContextExtensions
    {
        public static string GetUserId(this AuthorizationHandlerContext context)
        {
            return context.User.FindFirstValue("name");
        }

        public static bool CurrentUserIsAdministrator(this AuthorizationHandlerContext context)
        {
            return context.User.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);
        }

        public static void ApplyResult(this AuthorizationHandlerContext context, bool canAccess, IAuthorizationRequirement requirement)
        {
            if (canAccess)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
