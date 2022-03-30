using System.Threading;
using System.Threading.Tasks;
using Training.LoyaltyModule.Core.Models;
using Training.LoyaltyModule.Core.Services;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.Platform.Core.Common;

namespace Training.LoyaltyModule.Xapi.Queries
{
    public class SearchPointsOperationsQueryHandler : IQueryHandler<SearchPointsOperationsQuery, PointsOperationSearchResult>
    {
        private readonly IPointsOperationSearchService _pointsOperationSearchService;

        public SearchPointsOperationsQueryHandler(IPointsOperationSearchService pointsOperationSearchService)
        {
            _pointsOperationSearchService = pointsOperationSearchService;
        }

        public async Task<PointsOperationSearchResult> Handle(SearchPointsOperationsQuery request, CancellationToken cancellationToken)
        {
            var criteria = AbstractTypeFactory<PointsOperationSearchCriteria>.TryCreateInstance();

            criteria.UserIds = new[] { request.UserId };
            criteria.StoreIds = new[] { request.StoreId };
            criteria.IsDeposit = request.IsDeposit;
            criteria.CreatedSince = request.CreatedSince;
            criteria.CreatedTill = request.CreatedTill;
            criteria.Sort = request.Sort;
            criteria.Skip = request.Skip;
            criteria.Take = request.Take;

            var result = await _pointsOperationSearchService.SearchAsync(criteria);

            return result;
        }
    }
}
