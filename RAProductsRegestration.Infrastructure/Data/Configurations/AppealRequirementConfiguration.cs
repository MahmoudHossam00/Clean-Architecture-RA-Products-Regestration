using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Appeals.Requirements;
using RAProductsRegestration.Domain.Departments;

namespace RAProductsRegestration.Infrastructure.Configurations;

internal sealed class AppealRequirementConfiguration
    : IEntityTypeConfiguration<AppealRequirement>
{
    public void Configure(
        EntityTypeBuilder<AppealRequirement> builder)
    {
        builder.ToTable(
            "AppealRequirements");


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
            .HasMaxLength(4000);


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