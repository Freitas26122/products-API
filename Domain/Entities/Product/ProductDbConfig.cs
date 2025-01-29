using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsAPI.Domain.Entities.Class;

namespace ProductsAPI.Domain.Entities.Product
{
    public class ProductDbConfig : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("products", schema: "dbo");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("CHAR(8)")
                .HasDefaultValueSql("[dbo].[new_id]()")
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("description")
                .HasColumnType("NVARCHAR(max)")
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnName("price")
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            builder.Property(p => p.StockQuantity)
                .HasColumnName("stock_quantity")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasQueryFilter(p => !p.Removed);
        }
    }
}
