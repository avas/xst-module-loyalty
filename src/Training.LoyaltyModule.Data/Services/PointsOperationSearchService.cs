using System;
using System.Collections.Generic;
using System.Linq;
using Training.LoyaltyModule.Core.Models;
using Training.LoyaltyModule.Core.Services;
using Training.LoyaltyModule.Data.Models;
using Training.LoyaltyModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace Training.LoyaltyModule.Data.Services
{
    public class PointsOperationSearchService : SearchService<PointsOperationSearchCriteria, PointsOperationSearchResult, PointsOperation, PointsOperationEntity>, IPointsOperationSearchService
    {
        public PointsOperationSearchService(Func<ILoyaltyRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IPointsOperationService crudService)
            : base(repositoryFactory, platformMemoryCache, crudService)
        {
        }

        protected override IQueryable<PointsOperationEntity> BuildQuery(IRepository repository, PointsOperationSearchCriteria criteria)
        {
            var query = ((ILoyaltyRepository)repository).PointsOperations;

            if (criteria.UserIds?.Any() == true)
            {
                query = criteria.UserIds.Count == 1
                    ? query.Where(x => x.UserId == criteria.UserIds.First())
                    : query.Where(x => criteria.UserIds.Contains(x.UserId));
            }

            if (criteria.StoreIds?.Any() == true)
            {
                query = criteria.StoreIds.Count == 1
                    ? query.Where(x => x.StoreId == criteria.StoreIds.First())
                    : query.Where(x => criteria.StoreIds.Contains(x.StoreId));
            }

            if (criteria.IsDeposit != null)
            {
                query = query.Where(x => x.IsDeposit == criteria.IsDeposit);
            }

            if (criteria.CreatedSince != null)
            {
                query = query.Where(x => x.CreatedDate >= criteria.CreatedSince);
            }

            if (criteria.CreatedTill != null)
            {
                query = query.Where(x => x.CreatedDate <= criteria.CreatedTill);
            }

            return query;
        }

        protected override IList<SortInfo> BuildSortExpression(PointsOperationSearchCriteria criteria)
        {
            var result = base.BuildSortExpression(criteria);

            if (result?.Any() != true)
            {
                result = new[] {
                    new SortInfo { SortColumn = nameof(PointsOperationEntity.UserId) },
                    new SortInfo { SortColumn = nameof(PointsOperationEntity.StoreId) },
                    new SortInfo { SortColumn = nameof(PointsOperationEntity.CreatedDate), SortDirection = SortDirection.Descending },
                };
            }

            return result;
        }
    }
}
