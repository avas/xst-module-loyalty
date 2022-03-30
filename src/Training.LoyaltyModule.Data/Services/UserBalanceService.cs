using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Training.LoyaltyModule.Core.Events;
using Training.LoyaltyModule.Core.Models;
using Training.LoyaltyModule.Core.Services;
using Training.LoyaltyModule.Data.Models;
using Training.LoyaltyModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace Training.LoyaltyModule.Data.Services
{
    public class UserBalanceService : CrudService<UserBalance, UserBalanceEntity, UserBalanceChangeEvent, UserBalanceChangedEvent>, IUserBalanceService
    {
        public UserBalanceService(Func<ILoyaltyRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
            : base(repositoryFactory, platformMemoryCache, eventPublisher)
        {
        }

        public virtual async Task<UserBalance> GetUserBalance(string userId, string storeId)
        {
            using var repository = (ILoyaltyRepository)_repositoryFactory();

            var userBalanceEntity = await repository.UserBalances.FirstOrDefaultAsync(x => x.UserId == userId && x.StoreId == storeId);

            return userBalanceEntity?.ToModel(AbstractTypeFactory<UserBalance>.TryCreateInstance());
        }


        protected override async Task<IEnumerable<UserBalanceEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup)
        {
            return await ((ILoyaltyRepository)repository).UserBalances
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
