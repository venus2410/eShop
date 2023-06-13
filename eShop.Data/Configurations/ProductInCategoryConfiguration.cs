using eShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Data.Configurations
{
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.ToTable("ProductInCategorys");
            builder.HasKey(m => new {m.ProductId, m.CategoryId});
            builder.HasOne(p=>p.Product)
                .WithMany(m=>m.ProductInCategories)
                .HasForeignKey(p=>p.ProductId);
            builder.HasOne(p => p.Category)
                .WithMany(m => m.ProductInCategories)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
