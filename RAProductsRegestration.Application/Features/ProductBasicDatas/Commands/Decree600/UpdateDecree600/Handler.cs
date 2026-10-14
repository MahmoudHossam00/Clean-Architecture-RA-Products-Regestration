//using RAProductsRegestration.Application.Common.Interfaces.Messaging;
//using RAProductsRegestration.Application.Common.Interfaces.Persistence;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
//using RAProductsRegestration.Domain.Common.Results;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.UpdateDecree600
//{
//    internal sealed class Handler(
//    IProductBasicDataRepository repository,
//    IUnitOfWork unitOfWork)
//    : ICommandHandler<
//    UpdateDecree600Command,
//    Updated>
//    {
//        public Task<Result<Updated>> Handle(
//            UpdateDecree600Command request,
//            CancellationToken ct)
//        {
//            return ProductBasicDataCommandExecutor
//                .ExecuteAsync(
//                    request.ProductBasicDataId,
//                    repository,
//                    unitOfWork,
//                    x => x.UpdateDecree600(
//                     request.Description,
//                     request.Deadline,
//                     request.Notes),
//                    ct);
//        }
//    }
//}
