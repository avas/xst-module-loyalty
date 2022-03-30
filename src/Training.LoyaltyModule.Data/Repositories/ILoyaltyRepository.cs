using System.Linq;
using Training.LoyaltyModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace Training.LoyaltyModule.Data.Repositories
{
    public interface ILoyaltyRepository : IRepository
    {
        IQueryable<UserBalanceEntity> UserBalances { get; }
        IQueryable<PointsOperationEntity> PointsOperations { get; }
    }
}
