using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Training.LoyaltyModule.Xapi.Extensions
{
    public static class AuthorizationHandlerContextExtensions
    {
        public static string GetUserId(this AuthorizationHandlerContext context)
        {
            return context.User.FindFirstValue("name");
        }
    }
}
