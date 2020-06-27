using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Models;

namespace ProductAPI.Providers
{
    public class ProductConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            
            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.VendorCode).IsRequired();
            builder.Property(u => u.Vendor).IsRequired();
        }
    }
}