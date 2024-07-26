using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.NET.Models;

namespace Wallet.NET.Data.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Ticker)
                .IsRequired(true)
                .HasColumnType("VARCHAR(10)");

            builder.Property(x => x.Exchange)
                .IsRequired(true)
                .HasColumnType("VARCHAR(20)");
        }
    }
}
