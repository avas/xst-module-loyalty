using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;

namespace Training.LoyaltyModule.Xapi.Commands
{
    public class RegisterPointsOperationCommand : ICommand<PointsOperation>
    {
        public RegisterPointsOperationCommand(string userId, string storeId, string reason, decimal amount)
        {
            UserId = userId;
            StoreId = storeId;
            Reason = reason;
            Amount = amount;
        }

        public string UserId { get; set; }
        public string StoreId { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
    }
}
