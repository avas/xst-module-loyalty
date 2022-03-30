using System.Collections.Generic;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace Training.LoyaltyModule.Core.Events
{
    public class UserBalanceChangedEvent : GenericChangedEntryEvent<UserBalance>
    {
        public UserBalanceChangedEvent(IEnumerable<GenericChangedEntry<UserBalance>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
