using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.ProductLicenses.Requirements;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class LicenseRequirementConfiguration
    : IEntityTypeConfiguration<LicenseRequirement>
{
    public void Configure(
        EntityTypeBuilder<LicenseRequirement> builder)
    {
        builder.ToTable(
            "ProductLicenseRequirements");


        builder.HasKey(
            x => x.Id);


        // =====================================================
        // Requirement Name
        // =====================================================

        builder.Property(
                x => x.Name)
            .HasMaxLength(500)
            .IsRequired();


        // =====================================================
        // Deadline
        // =====================================================

        builder.Property(
                x => x.Deadline)
            .IsRequired();


        // =====================================================
        // Responsible Department
        // =====================================================

        builder.Property(
                x => x.ResponsibleDepartmentId)
            .IsRequired();


        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(
                x => x.ResponsibleDepartmentId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Notes
        // =====================================================

        builder.Property(
                x => x.Notes)
            .HasMaxLength(2000);


        // =====================================================
        // Requirement Attachment
        // =====================================================

        builder.ComplexProperty(
            x => x.RequirementAttachment,
            attachment =>
            {
                attachment.ToJson(
                    "RequirementAttachment");
            });


        // =====================================================
        // Approval
        // =====================================================

        builder.ComplexProperty(
            x => x.Approval,
            approval =>
            {
                approval.ToJson(
                    "Approval");

                approval.ComplexProperty(
                    x => x.Attachment);
            });
    }
}