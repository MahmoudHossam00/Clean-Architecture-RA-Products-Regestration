using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Infrastructure.Integrations.CTD.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Integrations.CTD
{
    internal sealed class CTDReadDbContext(
    DbContextOptions<CTDReadDbContext> options)
    : DbContext(options)
    {
        internal DbSet<CTDCompanyRow> Companies =>
            Set<CTDCompanyRow>();

        internal DbSet<CTDActiveIngredientRow> APIs =>
            Set<CTDActiveIngredientRow>();

        internal DbSet<CTDProductRow> Products =>
            Set<CTDProductRow>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            ConfigureCompanies(modelBuilder);
            ConfigureAPIs(modelBuilder);
            ConfigureProducts(modelBuilder);
        }

        private static void ConfigureCompanies(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CTDCompanyRow>(builder =>
            {
                builder.ToTable("Companies");

                builder.HasKey(x => x.Code);

                builder.Property(x => x.Code)
                    .HasColumnName("Code");

                builder.Property(x => x.Name)
                    .HasColumnName("Name");
            });
        }

        private static void ConfigureAPIs(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CTDActiveIngredientRow>(builder =>
            {
                builder.ToTable("APIs");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id)
                    .HasColumnName("Id");

                builder.Property(x => x.Code)
                    .HasColumnName("Code");

                builder.Property(x => x.Name)
                    .HasColumnName("Name");
            });
        }

        private static void ConfigureProducts(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CTDProductRow>(builder =>
            {
                builder.ToTable("Products");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedNever();

                builder.Property(x => x.ProductCode)
                    .HasColumnName("ProductCode");

                builder.Property(x => x.ProductName)
                    .HasColumnName("ProductName");

                builder.Property(x => x.CompanyCode)
                    .HasColumnName("CompanyCode");
            });
        }
    }
}
