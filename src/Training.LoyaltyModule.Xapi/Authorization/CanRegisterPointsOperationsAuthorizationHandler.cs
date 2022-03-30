using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Security.Authorization;

namespace Training.LoyaltyModule.Xapi.Authorization
{
    public class CanRegisterPointsOperationsAuthorizationRequirement : PermissionAuthorizationRequirement
    {
        public CanRegisterPointsOperationsAuthorizationRequirement()
            : base("CanRegisterPointsOperations")
        {
        }
    }

    public class CanRegisterPointsOperationsAuthorizationHandler : PermissionAuthorizationHandlerBase<CanRegisterPointsOperationsAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanRegisterPointsOperationsAuthorizationRequirement requirement)
        {
            // TODO: customers should not be able to use the registerPointsOperation mutation (obviously), but who should have access to it?..

            var canAccess = context.User.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);

            if (canAccess)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
