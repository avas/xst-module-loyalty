using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace Training.LoyaltyModule.Core.Services;

public interface IPointsOperationSearchService : ISearchService<PointsOperationSearchCriteria, PointsOperationSearchResult, PointsOperation>
{
}
