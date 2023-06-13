using eShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace eShop.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(x => new {x.OrderId,x.ProductId});
            builder.HasOne(x => x.Order).WithMany(n=>n.OrderDetails).HasForeignKey(p=>p.OrderId);
            builder.HasOne(x => x.Product).WithMany(n => n.OrderDetails).HasForeignKey(p => p.ProductId);
        }
    }
}
