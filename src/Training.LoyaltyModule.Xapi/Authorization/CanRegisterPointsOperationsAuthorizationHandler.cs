using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Training.LoyaltyModule.Xapi.Commands;
using Training.LoyaltyModule.Xapi.Extensions;
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
            var canAccess = context.CurrentUserIsAdministrator();

            if (!canAccess && context.Resource is RegisterPointsOperationCommand registerPointsOperationCommand)
            {
                registerPointsOperationCommand.UserId = context.GetUserId();
                canAccess = registerPointsOperationCommand.UserId != null;
            }

            context.ApplyResult(canAccess, requirement);

            return Task.CompletedTask;
        }
    }
}
