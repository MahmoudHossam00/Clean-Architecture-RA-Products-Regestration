using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Domain.ProductsTechnicalDocs.Common;
using System;
using System.Collections.Generic;
using System.Text;
using RAProductsRegestration.Application.Features.ProductsTechnicalDocs;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Queries.GetByProductId
{
    internal static class ProductTechnicalDocsMapper
    {
        public static ProductTechnicalDocsResponse Map(
            Domain.ProductsTechnicalDocs.ProductTechnicalDocs
                technicalDocs)
        {
            var specifications =
                MapHistory(
                    technicalDocs.ApprovedSpecifications);

            var compositions =
                MapHistory(
                    technicalDocs.ApprovedCompositions);

            var moaValidations =
                MapHistory(
                    technicalDocs.ApprovedMoaValidations);


            return new ProductTechnicalDocsResponse(
                technicalDocs.Id,
                technicalDocs.ProductId,

                specifications.FirstOrDefault(),
                specifications,

                compositions.FirstOrDefault(),
                compositions,

                moaValidations.FirstOrDefault(),
                moaValidations);
        }


        private static IReadOnlyCollection
            <DatedApprovalDocumentResponse>
            MapHistory(
                IEnumerable<DatedApprovalDocument> documents)
        {
            return documents
                .OrderByDescending(
                    x => x.ApprovalDate)
                .Select(MapDocument)
                .ToArray();
        }


        private static DatedApprovalDocumentResponse
            MapDocument(
                DatedApprovalDocument document)
        {
            return new DatedApprovalDocumentResponse(
                document.ApprovalDate,

                new AttachmentResponse(
                    document.Attachment.AttachmentId,
                    document.Attachment.FileName,
                    document.Attachment.Notes));
        }
    }
}
