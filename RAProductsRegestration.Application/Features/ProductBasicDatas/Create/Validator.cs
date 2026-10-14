using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Create
{
    internal class Validator
    {
    }
    public sealed class CreateProductBasicDataValidator
    : AbstractValidator<CreateProductBasicDataCommand>
    {
        public CreateProductBasicDataValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithErrorCode(
                    "ProductBasicData.ProductId.Required")
                .WithMessage(
                    "Product ID is required.");
        }
    }
}
