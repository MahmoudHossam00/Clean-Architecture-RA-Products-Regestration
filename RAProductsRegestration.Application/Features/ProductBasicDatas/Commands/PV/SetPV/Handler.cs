//using RAProductsRegestration.Application.Common.Interfaces.Messaging;
//using RAProductsRegestration.Application.Common.Interfaces.Persistence;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateTradeName;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
//using RAProductsRegestration.Domain.Common.Results;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.SetPV
//{
//    internal sealed class Handler(
//    IProductBasicDataRepository repository,
//    IUnitOfWork unitOfWork)
//    : ICommandHandler<
//        SetPVCommand,
//        Updated>
//    {
//        public Task<Result<Updated>> Handle(
//            SetPVCommand request,
//            CancellationToken ct)
//        {
//            return ProductBasicDataCommandExecutor
//                .ExecuteAsync(
//                    request.ProductBasicDataId,
//                    repository,
//                    unitOfWork,
//                    x => x.SetPV(
//                     request.Description,
//                     request.Deadline,
//                     request.Notes),
//                    ct);
//        }
//    }
//}
