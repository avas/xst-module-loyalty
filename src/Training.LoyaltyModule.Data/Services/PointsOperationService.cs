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
    public class PointsOperationService : CrudService<PointsOperation, PointsOperationEntity, PointsOperationChangeEvent, PointsOperationChangedEvent>, IPointsOperationService
    {
        private readonly IUserBalanceService _userBalanceService;
        private readonly IUserBalanceSearchService _userBalanceSearchService;

        public PointsOperationService(
            Func<ILoyaltyRepository> repositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            IEventPublisher eventPublisher,
            IUserBalanceService userBalanceService,
            IUserBalanceSearchService userBalanceSearchService)
            : base(repositoryFactory, platformMemoryCache, eventPublisher)
        {
            _userBalanceService = userBalanceService;
            _userBalanceSearchService = userBalanceSearchService;
        }

        protected override async Task<IEnumerable<PointsOperationEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup)
        {
            return await ((ILoyaltyRepository)repository).PointsOperations
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        protected override async Task BeforeSaveChanges(IEnumerable<PointsOperation> models)
        {
            await base.BeforeSaveChanges(models);

            var userBalances = await GetUserBalances(models);

            foreach (var model in models)
            {
                var userBalanceEntity = userBalances.FirstOrDefault(x => x.UserId.EqualsInvariant(model.UserId) && x.StoreId.EqualsInvariant(model.StoreId));
                var currentBalance = userBalanceEntity?.Amount ?? 0m;

                model.Balance = currentBalance + model.Amount;
            }
        }

        protected override async Task AfterSaveChangesAsync(IEnumerable<PointsOperation> models, IEnumerable<GenericChangedEntry<PointsOperation>> changedEntries)
        {
            await base.AfterSaveChangesAsync(models, changedEntries);

            var newModels = changedEntries
                .Where(x => x.EntryState == EntryState.Added)
                .Select(x => x.NewEntry)
                .Where(x => x.Amount != 0m)
                .ToList();

            var userBalances = await GetUserBalances(newModels);
            var changedUserBalances = new List<UserBalance>();

            foreach (var newModel in newModels)
            {
                var userId = newModel.UserId;
                var storeId = newModel.StoreId;

                var userBalance = userBalances.FirstOrDefault(x => x.UserId.EqualsInvariant(userId) && x.StoreId.EqualsInvariant(storeId)) ?? CreateUserBalance(userId, storeId);

                userBalance.Amount += newModel.Amount;

                changedUserBalances.Add(userBalance);
            }

            if (changedUserBalances.Any())
            {
                await _userBalanceService.SaveChangesAsync(changedUserBalances);
            }
        }

        protected virtual async Task<IList<UserBalance>> GetUserBalances(IEnumerable<PointsOperation> models)
        {
            var userIds = new HashSet<string>();
            var storeIds = new HashSet<string>();

            foreach (var model in models)
            {
                userIds.Add(model.UserId);
                storeIds.Add(model.StoreId);
            }

            var criteria = AbstractTypeFactory<UserBalanceSearchCriteria>.TryCreateInstance();

            criteria.UserIds = userIds.ToList();
            criteria.StoreIds = storeIds.ToList();
            criteria.Take = int.MaxValue;

            var searchResult = await _userBalanceSearchService.SearchAsync(criteria);

            return searchResult.Results;
        }

        protected virtual UserBalance CreateUserBalance(string userId, string storeId)
        {
            var result = AbstractTypeFactory<UserBalance>.TryCreateInstance();

            result.UserId = userId;
            result.StoreId = storeId;
            result.Amount = 0m;

            return result;
        }
    }
}
