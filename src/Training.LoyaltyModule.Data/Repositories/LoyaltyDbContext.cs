using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Training.LoyaltyModule.Data.Models;

namespace Training.LoyaltyModule.Data.Repositories
{
    public class LoyaltyDbContext : DbContextWithTriggers
    {
        public LoyaltyDbContext(DbContextOptions<LoyaltyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBalanceEntity>().ToTable("UserBalance").HasKey(x => x.Id);
            modelBuilder.Entity<UserBalanceEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserBalanceEntity>()
                .HasIndex(x => new { x.UserId, x.StoreId })
                .IsUnique();

            modelBuilder.Entity<PointsOperationEntity>().ToTable("PointsOperation").HasKey(x => x.Id);
            modelBuilder.Entity<PointsOperationEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<PointsOperationEntity>()
                .HasIndex(x => new { x.UserId, x.StoreId })
                .IsUnique(false);
        }
    }
}
