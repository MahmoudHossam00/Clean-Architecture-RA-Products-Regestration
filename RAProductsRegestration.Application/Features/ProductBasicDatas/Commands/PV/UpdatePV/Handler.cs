//using RAProductsRegestration.Application.Common.Interfaces.Messaging;
//using RAProductsRegestration.Application.Common.Interfaces.Persistence;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.SetPV;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
//using RAProductsRegestration.Domain.Common.Results;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.UpdatePV
//{
//    internal sealed class Handler(
//    IProductBasicDataRepository repository,
//    IUnitOfWork unitOfWork)
//    : ICommandHandler<
//    UpdatePVCommand,
//    Updated>
//    {
//        public Task<Result<Updated>> Handle(
//            UpdatePVCommand request,
//            CancellationToken ct)
//        {
//            return ProductBasicDataCommandExecutor
//                .ExecuteAsync(
//                    request.ProductBasicDataId,
//                    repository,
//                    unitOfWork,
//                    x => x.UpdatePV(
//                     request.Description,
//                     request.Deadline,
//                     request.Notes),
//                    ct);
//        }
//    }
//}
