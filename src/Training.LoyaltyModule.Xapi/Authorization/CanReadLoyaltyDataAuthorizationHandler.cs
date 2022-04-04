using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Training.LoyaltyModule.Xapi.Extensions;
using Training.LoyaltyModule.Xapi.Queries;
using VirtoCommerce.Platform.Security.Authorization;

namespace Training.LoyaltyModule.Xapi.Authorization
{
    public class CanReadLoyaltyDataAuthorizationRequirement : PermissionAuthorizationRequirement
    {
        public CanReadLoyaltyDataAuthorizationRequirement()
            : base("CanReadLoyaltyData")
        {
        }
    }

    public class CanReadLoyaltyDataAuthorizationHandler : PermissionAuthorizationHandlerBase<CanReadLoyaltyDataAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanReadLoyaltyDataAuthorizationRequirement requirement)
        {
            var canAccess = context.CurrentUserIsAdministrator();

            if (!canAccess)
            {
                if (context.Resource is GetUserBalanceQuery getUserBalanceQuery)
                {
                    getUserBalanceQuery.UserId = context.GetUserId();
                    canAccess = getUserBalanceQuery.UserId != null;
                }
                else if (context.Resource is SearchPointsOperationsQuery searchPointsOperationsQuery)
                {
                    searchPointsOperationsQuery.UserId = context.GetUserId();
                    canAccess = searchPointsOperationsQuery.UserId != null;
                }
            }

            context.ApplyResult(canAccess, requirement);

            return Task.CompletedTask;
        }
    }
}
