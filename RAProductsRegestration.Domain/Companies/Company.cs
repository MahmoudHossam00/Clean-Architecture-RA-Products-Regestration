using RAProductsRegestration.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Companies
{
    public sealed class Company : Entity, IAggregateRoot
    {
        private Company()
        {
        }

        public Company(
            string externalCode,
            string name)
        {
            Code = externalCode;
            Name = name;
        }

        public string Code { get; private set; } = null!;

        public string Name { get; private set; } = null!;

        public void UpdateFromCTD(string name)
        {
            Name = name;
        }
    }
}
