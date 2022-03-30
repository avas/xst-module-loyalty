using System.Collections.Generic;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace Training.LoyaltyModule.Core.Events
{
    public class UserBalanceChangeEvent : GenericChangedEntryEvent<UserBalance>
    {
        public UserBalanceChangeEvent(IEnumerable<GenericChangedEntry<UserBalance>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
