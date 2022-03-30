using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Training.LoyaltyModule.Core.Models;
using Training.LoyaltyModule.Core.Services;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.Platform.Core.Common;

namespace Training.LoyaltyModule.Xapi.Queries
{
    public class GetUserBalanceQueryHandler : IQueryHandler<GetUserBalanceQuery, GetUserBalanceResponse>
    {
        private const int _pointsOperationCountForBalance = 20;

        private readonly IUserBalanceService _userBalanceService;
        private readonly IPointsOperationSearchService _pointsOperationSearchService;

        public GetUserBalanceQueryHandler(IUserBalanceService userBalanceService, IPointsOperationSearchService pointsOperationSearchService)
        {
            _userBalanceService = userBalanceService;
            _pointsOperationSearchService = pointsOperationSearchService;
        }

        public async Task<GetUserBalanceResponse> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
        {
            var balance = await GetUserBalance(request.UserId, request.StoreId);

            if (balance != null && request.IncludeOperations)
            {
                balance.Operations = await GetPointsOperations(request.UserId, request.StoreId);
            }

            return new GetUserBalanceResponse
            {
                UserBalance = balance,
            };
        }


        private async Task<UserBalance> GetUserBalance(string userId, string storeId)
        {
            return await _userBalanceService.GetUserBalance(userId, storeId);
        }

        private async Task<IList<PointsOperation>> GetPointsOperations(string userId, string storeId)
        {
            var criteria = AbstractTypeFactory<PointsOperationSearchCriteria>.TryCreateInstance();

            criteria.UserIds = new[] { userId };
            criteria.StoreIds = new[] { storeId };
            criteria.Sort = "CreatedDate:desc";
            criteria.Take = _pointsOperationCountForBalance;

            var result = await _pointsOperationSearchService.SearchAsync(criteria);

            return result.Results;
        }
    }
}
