using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace Training.LoyaltyModule.Core.Models
{
    public class UserBalanceSearchCriteria : SearchCriteriaBase
    {
        public IList<string> UserIds { get; set; }
        public IList<string> StoreIds { get; set; }
        public DateTime? CreatedSince { get; set; }
        public DateTime? CreatedTill { get; set; }
    }
}
