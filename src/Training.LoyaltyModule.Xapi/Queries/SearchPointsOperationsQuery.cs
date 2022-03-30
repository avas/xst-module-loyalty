using System;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;

namespace Training.LoyaltyModule.Xapi.Queries
{
    public class SearchPointsOperationsQuery : IQuery<PointsOperationSearchResult>
    {
        public string UserId { get; set; }
        public string StoreId { get; set; }
        public bool? IsDeposit { get; set; }
        public DateTime? CreatedSince { get; set; }
        public DateTime? CreatedTill { get; set; }
        public string Sort { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
