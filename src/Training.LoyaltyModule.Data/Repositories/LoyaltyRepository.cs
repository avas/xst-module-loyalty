using System.Linq;
using Training.LoyaltyModule.Data.Models;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace Training.LoyaltyModule.Data.Repositories
{
    public class LoyaltyRepository : DbContextRepositoryBase<LoyaltyDbContext>, ILoyaltyRepository
    {
        public LoyaltyRepository(LoyaltyDbContext dbContext, IUnitOfWork unitOfWork = null)
            : base(dbContext, unitOfWork)
        {
        }

        public IQueryable<UserBalanceEntity> UserBalances => DbContext.Set<UserBalanceEntity>();
        public IQueryable<PointsOperationEntity> PointsOperations => DbContext.Set<PointsOperationEntity>();
    }
}
