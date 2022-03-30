using System.Collections.Generic;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace Training.LoyaltyModule.Core.Events
{
    public class PointsOperationChangeEvent : GenericChangedEntryEvent<PointsOperation>
    {
        public PointsOperationChangeEvent(IEnumerable<GenericChangedEntry<PointsOperation>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
