using RAProductsRegestration.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Text;

namespace RAProductsRegestration.Domain.Products
{
    public sealed class Product : Entity, IAggregateRoot
    {
        private Product()
        {
        }

        public Product(
            int ctdId,
            string productCode,
            string productName,
            Guid companyId)
        {
            CTDId = ctdId;
            ProductCode = productCode;
            ProductName = productName;
            CompanyId = companyId;
        }

        public int CTDId { get; private set; }

        public string ProductCode { get; private set; } = null!;

        public string ProductName { get; private set; } = null!;

        public Guid CompanyId { get; private set; }

        public void UpdateFromCTD(
            string productName,
            Guid companyId)
        {
            ProductName = productName;
            CompanyId = companyId;
        }
    }
    
    public class ActiveIngredient : Entity, IAggregateRoot
    {
        private ActiveIngredient()
        {
        }

        public ActiveIngredient(
            int ctdId,
            string code,
            string name)
        {
            CTDId = ctdId;
            Code = code;
            Name = name;
        }

        public int CTDId { get; private set; }

        public string Code { get; private set; } = null!;

        public string Name { get; private set; } = null!;

        public void UpdateFromCTD(string name)
        {
            Name = name;
        }
    }
}
