using System.Collections.Generic;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace Training.LoyaltyModule.Core.Events
{
    public class PointsOperationChangedEvent : GenericChangedEntryEvent<PointsOperation>
    {
        public PointsOperationChangedEvent(IEnumerable<GenericChangedEntry<PointsOperation>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
