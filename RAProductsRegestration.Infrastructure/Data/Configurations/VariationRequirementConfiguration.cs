using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.Variations.Requirements;

namespace RAProductsRegestration.Infrastructure.Configurations;

internal sealed class VariationRequirementConfiguration
    : IEntityTypeConfiguration<VariationRequirement>
{
    public void Configure(
        EntityTypeBuilder<VariationRequirement> builder)
    {
        builder.ToTable(
            "VariationRequirements");


        builder.HasKey(
            x => x.Id);


        // =====================================================
        // Name
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
        // Department
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
        // Requirement Approval
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