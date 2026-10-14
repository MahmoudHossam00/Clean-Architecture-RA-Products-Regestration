using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Appeals.Requirements;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.Departments;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Queries.GetByProductId
{
    internal static class AppealMapper
    {
        public static AppealResponse[] Map(
            IReadOnlyCollection<Appeal> appeals,
            IReadOnlyCollection<AppealType> appealTypes,
            IReadOnlyCollection<Department> departments)
        {
            var appealTypeMap =
                appealTypes.ToDictionary(
                    x => x.Id);


            var departmentMap =
                departments.ToDictionary(
                    x => x.Id);


            return appeals
                .OrderBy(
                    x => x.AppealDate)
                .Select(
                    appeal =>
                        MapAppeal(
                            appeal,
                            appealTypeMap,
                            departmentMap))
                .ToArray();
        }


        // =========================================================
        // Appeal
        // =========================================================

        private static AppealResponse MapAppeal(
            Appeal appeal,
            IReadOnlyDictionary<
                Guid,
                AppealType> appealTypes,
            IReadOnlyDictionary<
                Guid,
                Department> departments)
        {
            if (
                !appealTypes.TryGetValue(
                    appeal.AppealTypeId,
                    out var appealType)
            )
            {
                throw new InvalidOperationException(
                    $"Appeal type '{appeal.AppealTypeId}' was not found.");
            }


            var requirements =
                appeal.Requirements
                    .OrderBy(
                        x => x.Deadline)
                    .Select(
                        requirement =>
                            MapRequirement(
                                requirement,
                                departments))
                    .ToArray();


            return new AppealResponse(
                appeal.Id,
                appeal.ProductId,

                appeal.Initiator,

                new LookupResponse(
                    appealType.Id,
                    appealType.Name),

                appeal.AppealDate,

                MapDatedAttachment(
                    appeal.Approval),

                appeal.Notes,

                requirements);
        }


        // =========================================================
        // Requirement
        // =========================================================

        private static AppealRequirementResponse
            MapRequirement(
                AppealRequirement requirement,
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


            return new AppealRequirementResponse(
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


        // =========================================================
        // Attachment
        // =========================================================

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


        // =========================================================
        // Dated Attachment
        // =========================================================

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
                    attachment.Attachment
                        .AttachmentId,

                    attachment.Attachment
                        .FileName,

                    attachment.Attachment
                        .Notes));
        }
    }
}
