using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.ProductsTechnicalDocs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.ProductsTechnicalDocs
{

    public sealed class ProductTechnicalDocs
        : AuditableEntity, IAggregateRoot
    {
        private readonly List<DatedApprovalDocument>
            _approvedSpecifications = [];

        private readonly List<DatedApprovalDocument>
            _approvedCompositions = [];

        private readonly List<DatedApprovalDocument>
            _approvedMoaValidations = [];


        private ProductTechnicalDocs()
        {
        }


        private ProductTechnicalDocs(
            Guid productId)
            : base(Guid.NewGuid())
        {
            ProductId = productId;
        }


        public Guid ProductId
        {
            get;
            private set;
        }


        // =========================================================
        // Histories
        // =========================================================

        public IReadOnlyCollection<DatedApprovalDocument>
            ApprovedSpecifications =>
                _approvedSpecifications.AsReadOnly();


        public IReadOnlyCollection<DatedApprovalDocument>
            ApprovedCompositions =>
                _approvedCompositions.AsReadOnly();


        public IReadOnlyCollection<DatedApprovalDocument>
            ApprovedMoaValidations =>
                _approvedMoaValidations.AsReadOnly();


        // =========================================================
        // Current Documents
        // =========================================================

        public DatedApprovalDocument?
            CurrentApprovedSpecifications =>
                _approvedSpecifications
                    .OrderByDescending(x => x.ApprovalDate)
                    .FirstOrDefault();


        public DatedApprovalDocument?
            CurrentApprovedComposition =>
                _approvedCompositions
                    .OrderByDescending(x => x.ApprovalDate)
                    .FirstOrDefault();


        public DatedApprovalDocument?
            CurrentApprovedMoaValidation =>
                _approvedMoaValidations
                    .OrderByDescending(x => x.ApprovalDate)
                    .FirstOrDefault();


        // =========================================================
        // Creation
        // =========================================================

        public static Result<ProductTechnicalDocs> Create(
            Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return ProductTechnicalDocsErrors
                    .ProductIdRequired;
            }

            return new ProductTechnicalDocs(
                productId);
        }


        // =========================================================
        // Specifications
        // =========================================================

        public Result<Created>
            AddApprovedSpecifications(
                DateOnly approvalDate,
                AttachmentReference attachment)
        {
            ArgumentNullException.ThrowIfNull(
                attachment);

            var document =
                new DatedApprovalDocument(
                    approvalDate,
                    attachment);

            _approvedSpecifications.Add(
                document);

            return Result.Created;
        }


        // =========================================================
        // Composition
        // =========================================================

        public Result<Created>
            AddApprovedComposition(
                DateOnly approvalDate,
                AttachmentReference attachment)
        {
            ArgumentNullException.ThrowIfNull(
                attachment);

            var document =
                new DatedApprovalDocument(
                    approvalDate,
                    attachment);

            _approvedCompositions.Add(
                document);

            return Result.Created;
        }


        // =========================================================
        // MOA + Validation
        // =========================================================

        public Result<Created>
            AddApprovedMoaValidation(
                DateOnly approvalDate,
                AttachmentReference attachment)
        {
            ArgumentNullException.ThrowIfNull(
                attachment);

            var document =
                new DatedApprovalDocument(
                    approvalDate,
                    attachment);

            _approvedMoaValidations.Add(
                document);

            return Result.Created;
        }
    }
}
