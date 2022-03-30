using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;

namespace Training.LoyaltyModule.Xapi.Queries
{
    public class GetUserBalanceQuery : IQuery<GetUserBalanceResponse>
    {
        public string UserId { get; set; }
        public string StoreId { get; set; }
        public bool IncludeOperations { get; set; }
    }
}
