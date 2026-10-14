using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.Variations;
using RAProductsRegestration.Domain.Variations.Requirements;

namespace RAProductsRegestration.Application.Features.Features.Variations.Queries.GetByProductId
{
    internal static class VariationMapper
    {
        public static VariationResponse[] Map(
            IReadOnlyCollection<Variation> variations,
            IReadOnlyCollection<Department> departments)
        {
            var departmentMap =
                departments.ToDictionary(
                    x => x.Id);


            return variations
                .OrderBy(
                    x => x.Deadline)
                .Select(
                    variation =>
                        MapVariation(
                            variation,
                            departmentMap))
                .ToArray();
        }


        private static VariationResponse MapVariation(
            Variation variation,
            IReadOnlyDictionary<
                Guid,
                Department> departments)
        {
            var requirements =
                variation.Requirements
                    .OrderBy(
                        x => x.Deadline)
                    .Select(
                        requirement =>
                            MapRequirement(
                                requirement,
                                departments))
                    .ToArray();


            return new VariationResponse(
                variation.Id,
                variation.ProductId,
                variation.Initiator,
                variation.Cause,
                variation.Deadline,
                variation.DeadlineNote,
                MapDatedAttachment(
                    variation.Approval),
                requirements);
        }


        private static VariationRequirementResponse
            MapRequirement(
                VariationRequirement requirement,
                IReadOnlyDictionary<
                    Guid,
                    Department> departments)
        {
            if (
                !departments.TryGetValue(
                    requirement.ResponsibleDepartmentId,
                    out var department)
            )
            {
                throw new InvalidOperationException(
                    $"Department '{requirement.ResponsibleDepartmentId}' was not found.");
            }


            return new VariationRequirementResponse(
                requirement.Id,
                requirement.Name,
                requirement.Deadline,

                new LookupResponse(
                    department.Id,
                    department.Name),

                MapAttachment(
                    requirement.RequirementAttachment),

                MapDatedAttachment(
                    requirement.Approval),

                requirement.Notes);
        }


        private static AttachmentResponse?
            MapAttachment(
                AttachmentReference? attachment)
        {
            if (attachment is null)
            {
                return null;
            }


            return new AttachmentResponse(
                attachment.AttachmentId,
                attachment.FileName,
                attachment.Notes);
        }


        private static DatedAttachmentResponse?
            MapDatedAttachment(
                DatedAttachment? attachment)
        {
            if (attachment is null)
            {
                return null;
            }


            return new DatedAttachmentResponse(
                attachment.Date,

                new AttachmentResponse(
                    attachment.Attachment.AttachmentId,
                    attachment.Attachment.FileName,
                    attachment.Attachment.Notes));
        }
    }
}
