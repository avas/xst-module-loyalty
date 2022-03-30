using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace Training.LoyaltyModule.Core.Services
{
    public interface IUserBalanceSearchService : ISearchService<UserBalanceSearchCriteria, UserBalanceSearchResult, UserBalance>
    {
    }
}
