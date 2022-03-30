using System.Threading.Tasks;
using Training.LoyaltyModule.Core.Models;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace Training.LoyaltyModule.Core.Services
{
    public interface IUserBalanceService : ICrudService<UserBalance>
    {
        Task<UserBalance> GetUserBalance(string userId, string storeId);
    }
}
