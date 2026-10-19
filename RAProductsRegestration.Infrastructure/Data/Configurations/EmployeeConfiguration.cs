using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.Employees;

namespace RAProductsRegestration.Infrastructure
    .Data.Configurations;

internal sealed class EmployeeConfiguration
    : IEntityTypeConfiguration<Employee>
{
    public void Configure(
        EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable(
            "Employees");


        builder.HasKey(
            x => x.Id);


        // =====================================================
        // Zen Id
        // =====================================================

        builder.Property(
                x => x.ZenId)
            .IsRequired();


        builder.HasIndex(
                x => x.ZenId)
            .IsUnique();


        // =====================================================
        // Employee Number
        // =====================================================

        builder.Property(
                x => x.EmployeeNumber)
            .IsRequired();


        builder.HasIndex(
                x => x.EmployeeNumber)
            .IsUnique();


        // =====================================================
        // Name
        // =====================================================

        builder.Property(
                x => x.NameEn)
            .HasMaxLength(250)
            .IsRequired();


        builder.Property(
                x => x.NameAr)
            .HasMaxLength(250);


        // =====================================================
        // Email
        // =====================================================

        builder.Property(
                x => x.Email)
            .HasMaxLength(320);


        // =====================================================
        // Department
        // =====================================================

        builder.Property(
            x => x.DepartmentId);


        builder
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(
                x => x.DepartmentId)
            .OnDelete(
                DeleteBehavior.SetNull);
    }
}