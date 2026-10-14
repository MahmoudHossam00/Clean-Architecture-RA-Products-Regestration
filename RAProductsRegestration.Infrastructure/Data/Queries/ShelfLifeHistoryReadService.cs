using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Queries;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetShelfLifeHistory;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Data.Queries
{
    internal sealed class ShelfLifeHistoryReadService(
        ApplicationDbContext context)
        : IShelfLifeHistoryReadService
    {
        public async Task<ShelfLifeHistoryResponse[]?>
            GetByProductBasicDataIdAsync(
                Guid productBasicDataId,
                CancellationToken cancellationToken = default)
        {
            var basicData =
                await context.ProductBasicData
                    .AsNoTracking()

                    .Include(x =>
                        x.ShelfLifeHistory)

                    .FirstOrDefaultAsync(
                        x =>
                            x.Id ==
                            productBasicDataId,
                        cancellationToken);


            if (basicData is null)
            {
                return null;
            }


            var currentVersionNumber =
                basicData.ShelfLifeHistory.Count == 0
                    ? (int?)null
                    : basicData.ShelfLifeHistory
                        .Max(x =>
                            x.VersionNumber);


            return basicData.ShelfLifeHistory
                .OrderByDescending(x =>
                    x.VersionNumber)

                .Select(x =>
                    new ShelfLifeHistoryResponse(
                        x.Id,

                        x.VersionNumber,

                        x.VersionNumber ==
                        currentVersionNumber,

                        x.ShelfLife.NumberOfMonths,

                        x.ShelfLife.ApprovalDate,

                        x.ShelfLife.Attachments
                            .Select(attachment =>
                                new ShelfLifeHistoryAttachmentResponse(
                                    attachment.AttachmentId,
                                    attachment.FileName,
                                    attachment.Notes))
                            .ToArray()))

                .ToArray();
        }
    }
}
