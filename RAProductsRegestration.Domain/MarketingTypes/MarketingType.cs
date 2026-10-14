using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.MarketingTypes
{
    public sealed class MarketingType
    : AuditableEntity, IAggregateRoot
    {
        private MarketingType()
        {
        }

        private MarketingType(
            string name)
            : base(Guid.NewGuid())
        {
            Name = name;
        }


        public string Name
        {
            get;
            private set;
        } = null!;


        public static Result<MarketingType> Create(
            string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return MarketingTypeErrors
                    .NameRequired;
            }

            return new MarketingType(
                name.Trim());
        }


        public Result<Updated> Rename(
            string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return MarketingTypeErrors
                    .NameRequired;
            }

            Name =
                name.Trim();

            return Result.Updated;
        }
    }
}
