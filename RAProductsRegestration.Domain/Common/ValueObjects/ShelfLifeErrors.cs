using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;


    namespace RAProductsRegestration.Domain.Common.ValueObjects;

    public static class ShelfLifeErrors
    {
        public static readonly Error
            NumberOfMonthsInvalid =
                Error.Validation(
                    "ShelfLife.NumberOfMonthsInvalid",
                    "Shelf life number of months must be greater than zero."
                );


        public static readonly Error
            AttachmentInvalid =
                Error.Validation(
                    "ShelfLife.AttachmentInvalid",
                    "Shelf life attachment is invalid."
                );


        public static Error AttachmentAlreadyExists(
            Guid attachmentId) =>
                Error.Conflict(
                    "ShelfLife.AttachmentAlreadyExists",
                    $"Attachment '{attachmentId}' already exists."
                );


        public static Error AttachmentNotFound(
            Guid attachmentId) =>
                Error.NotFound(
                    "ShelfLife.AttachmentNotFound",
                    $"Attachment '{attachmentId}' was not found."
                );
    }
