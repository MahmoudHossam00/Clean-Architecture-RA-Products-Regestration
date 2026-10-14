using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Common.ValueObjects
{
    public sealed class ShelfLife
    {
        private ShelfLife()
        {
        }

        private ShelfLife(
            int numberOfMonths,
            DateOnly approvalDate,
            List<AttachmentReference> attachments)
        {
            NumberOfMonths = numberOfMonths;
            ApprovalDate = approvalDate;
            Attachments = attachments;
        }


        public int NumberOfMonths { get; private set; }

        public DateOnly ApprovalDate { get; private set; }

        public List<AttachmentReference> Attachments
        {
            get;
            private set;
        } = [];


        public static Result<ShelfLife> Create(
            int numberOfMonths,
            DateOnly approvalDate,
            IEnumerable<AttachmentReference> attachments)
        {
            if (numberOfMonths <= 0)
            {
                return ShelfLifeErrors
                    .NumberOfMonthsInvalid;
            }

            ArgumentNullException.ThrowIfNull(
                attachments
            );

            var attachmentList =
                attachments.ToList();

            return new ShelfLife(
                numberOfMonths,
                approvalDate,
                attachmentList
            );
        }


        public Result<Created> AddAttachment(
            AttachmentReference attachment)
        {
            ArgumentNullException.ThrowIfNull(
                attachment
            );

            if (
                Attachments.Any(
                    x =>
                        x.AttachmentId ==
                        attachment.AttachmentId
                )
            )
            {
                return ShelfLifeErrors
                    .AttachmentAlreadyExists(
                        attachment.AttachmentId
                    );
            }

            Attachments.Add(
                attachment
            );

            return Result.Created;
        }


        public Result<Deleted> RemoveAttachment(
            Guid attachmentId)
        {
            var attachment =
                Attachments.FirstOrDefault(
                    x =>
                        x.AttachmentId ==
                        attachmentId
                );

            if (attachment is null)
            {
                return ShelfLifeErrors
                    .AttachmentNotFound(
                        attachmentId
                    );
            }

            Attachments.Remove(
                attachment
            );

            return Result.Deleted;
        }
    }
}
