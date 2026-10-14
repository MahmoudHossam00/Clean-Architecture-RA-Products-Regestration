using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;
using RAProductsRegestration.Domain.MiscellaneousApprovals;
using RAProductsRegestration.Domain.MiscellaneousApprovals.Requirements;
using RAProductsRegestration.Domain.Products;

namespace RAProductsRegestration.Infrastructure.Persistence.Configurations;

internal sealed class MiscellaneousApprovalConfiguration
    : IEntityTypeConfiguration<MiscellaneousApproval>
{
    public void Configure(
        EntityTypeBuilder<MiscellaneousApproval> builder)
    {
        // =====================================================
        // Table
        // =====================================================

        builder.ToTable(
            "MiscellaneousApprovals");


        // =====================================================
        // Key
        // =====================================================

        builder.HasKey(
            x => x.Id);


        // =====================================================
        // Product
        // =====================================================

        builder.Property(
                x => x.ProductId)
            .IsRequired();


        builder.HasIndex(
            x => x.ProductId);


        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(
                x => x.ProductId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // File Type
        // =====================================================

        builder.Property(
                x => x.FileTypeId)
            .IsRequired();


        builder.HasIndex(
            x => x.FileTypeId);


        builder.HasOne<MiscellaneousApprovalFileType>()
            .WithMany()
            .HasForeignKey(
                x => x.FileTypeId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Initiator
        // =====================================================

        builder.Property(
                x => x.Initiator)
            .HasMaxLength(250)
            .IsRequired();


        // =====================================================
        // Submission Date
        // =====================================================

        builder.Property(
                x => x.SubmissionDate)
            .IsRequired();


        // =====================================================
        // Main Approval
        // =====================================================

        builder.Property(
            x => x.ApprovalDate);


        builder.Property(
            x => x.ApprovalExpiryDate);


        builder.HasIndex(
            x => x.ApprovalExpiryDate);


        // =====================================================
        // Approval Requirements
        // =====================================================

        builder
    .HasMany(x => x.ApprovalRequirements)
    .WithOne()
    .HasForeignKey("MiscellaneousApprovalId")
    .OnDelete(DeleteBehavior.Cascade);


        builder.Navigation(
                x => x.ApprovalRequirements)
            .HasField("_approvalRequirements")
            .UsePropertyAccessMode(
                PropertyAccessMode.Field)
            .AutoInclude();

        // =====================================================
        // Use private List as backing field
        // =====================================================

        //builder.Navigation(
        //        x => x.ApprovalRequirements)
        //    .HasField(
        //        "_approvalRequirements")
        //    .UsePropertyAccessMode(
        //        PropertyAccessMode.Field);
    }
}