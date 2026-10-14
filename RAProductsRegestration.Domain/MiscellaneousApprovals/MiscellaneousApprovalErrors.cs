using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.MiscellaneousApprovals
{
    public static class MiscellaneousApprovalErrors
    {
        public static readonly Error ProductIdRequired =
            Error.Validation(
                "MiscellaneousApproval.ProductIdRequired",
                "Product ID is required.");


        public static readonly Error FileTypeRequired =
            Error.Validation(
                "MiscellaneousApproval.FileTypeRequired",
                "File type is required.");


        public static readonly Error InitiatorRequired =
            Error.Validation(
                "MiscellaneousApproval.InitiatorRequired",
                "Initiator is required.");


        public static readonly Error ApprovalBeforeSubmission =
            Error.Validation(
                "MiscellaneousApproval.ApprovalBeforeSubmission",
                "Approval date cannot be before submission date.");


        public static readonly Error ApprovalExpiryBeforeApprovalDate =
            Error.Validation(
                "MiscellaneousApproval.ApprovalExpiryBeforeApprovalDate",
                "Approval expiry date cannot be before approval date.");


        public static Error NotFound(
            Guid id)
        {
            return Error.NotFound(
                "MiscellaneousApproval.NotFound",
                $"Miscellaneous approval '{id}' was not found.");
        }
    }
}
