using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.NET.Models;

namespace Wallet.NET.Data.Configurations
{
    public class UserStockConfiguration : IEntityTypeConfiguration<UserStock>
    {
        public void Configure(EntityTypeBuilder<UserStock> builder)
        {
            builder.ToTable("UserStocks");

            builder.HasKey(x => new { x.UserId, x.StockId });

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.UserStocks)
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Stock)
                .WithMany(x => x.UserStocks)
                .HasForeignKey(x => x.StockId);
        }
    }
}
