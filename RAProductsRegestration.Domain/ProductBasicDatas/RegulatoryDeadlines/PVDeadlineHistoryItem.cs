using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.ProductBasicDatas.RegulatoryDeadlines
{
    public sealed class PVDeadlineHistoryItem : Entity
    {
        private PVDeadlineHistoryItem()
        {
        }


        internal PVDeadlineHistoryItem(
            int versionNumber,
            FulfillmentDeadline deadline)
            : base(Guid.NewGuid())
        {
            VersionNumber =
                versionNumber;

            Deadline =
                deadline;
        }


        // =====================================================
        // Properties
        // =====================================================

        public int VersionNumber
        {
            get;
            private set;
        }


        public FulfillmentDeadline Deadline
        {
            get;
            private set;
        } = null!;


        // =====================================================
        // Fulfillment
        // =====================================================

        internal Result<Updated> Fulfill(
            DateOnly fulfillmentDate)
        {
            Deadline =
                Deadline.Fulfill(
                    fulfillmentDate);


            return Result.Updated;
        }


        internal Result<Updated>
            ResetFulfillment()
        {
            Deadline =
                Deadline.ResetFulfillment();


            return Result.Updated;
        }
    }
}
