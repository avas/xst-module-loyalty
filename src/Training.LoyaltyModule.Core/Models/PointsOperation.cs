using System;
using VirtoCommerce.Platform.Core.Common;

namespace Training.LoyaltyModule.Core.Models
{
    public class PointsOperation : AuditableEntity, ICloneable
    {
        public string UserId { get; set; }
        public string StoreId { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public bool IsDeposit { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
