using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Integrations.JDE.Suppliers
{
    internal sealed class JdeReadDbContext(
    DbContextOptions<JdeReadDbContext> options)
    : DbContext(options)
    {
        public DbSet<SupplierRow> Suppliers =>
            Set<SupplierRow>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierRow>(builder =>
            {
                builder.HasNoKey();

                builder.Property(x => x.Code)
                    .HasColumnName("CODE");

                builder.Property(x => x.Name)
                    .HasColumnName("NAME");
            });
        }
    }
}
