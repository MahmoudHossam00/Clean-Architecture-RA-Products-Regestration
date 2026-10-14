using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Common
{
    public static class ProductBasicDataApplicationErrors
    {
        public static Error ProductNotFound(Guid productId) =>
            Error.NotFound(
                "Product.NotFound",
                $"Product '{productId}' was not found.");

        public static Error ProductBasicDataNotFound(Guid productId) =>
            Error.NotFound(
                "ProductBasicData.NotFound",
                $"Product basic data for product '{productId}' was not found.");

        public static Error ProductBasicDataAlreadyExists(Guid productId) =>
            Error.Conflict(
                "ProductBasicData.AlreadyExists",
                $"Product basic data already exists for product '{productId}'.");

        public static Error ActiveIngredientNotFound(
            Guid activeIngredientId) =>
            Error.NotFound(
                "ActiveIngredient.NotFound",
                $"Active ingredient '{activeIngredientId}' was not found.");


        public static Error NotFound(
        Guid id) =>
        Error.NotFound(
            "ProductBasicData.NotFound",
            $"Product basic data '{id}' was not found.");

        public static Error AlreadyExistsForProduct(
            Guid productId) =>
            Error.Conflict(
                "ProductBasicData.AlreadyExists",
                $"Product basic data already exists for product '{productId}'.");
        public static Error SupplierNotFound(
            Guid supplierId) =>
            Error.NotFound(
                "Supplier.NotFound",
                $"Supplier '{supplierId}' was not found.");


        

        public static Error DosageFormNotFound(Guid id) =>
            Error.NotFound(
                "DosageForm.NotFound",
                $"Dosage form '{id}' was not found.");

        public static Error StorageConditionNotFound(Guid id) =>
            Error.NotFound(
                "StorageCondition.NotFound",
                $"Storage condition '{id}' was not found.");
    }
}
