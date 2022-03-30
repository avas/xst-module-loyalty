using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Training.LoyaltyModule.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LoyaltyDbContext>
    {
        public LoyaltyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LoyaltyDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new LoyaltyDbContext(builder.Options);
        }
    }
}
