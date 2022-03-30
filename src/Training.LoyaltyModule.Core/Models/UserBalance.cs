using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace Training.LoyaltyModule.Core.Models
{
    public class UserBalance : AuditableEntity, ICloneable
    {
        public string UserId { get; set; }
        public string StoreId { get; set; }
        public decimal Amount { get; set; }

        public IList<PointsOperation> Operations { get; set; }

        public object Clone()
        {
            var result = (UserBalance)MemberwiseClone();

            result.Operations = Operations
                ?.Select(x => x.Clone())
                .OfType<PointsOperation>()
                .ToList();

            return result;
        }
    }
}
