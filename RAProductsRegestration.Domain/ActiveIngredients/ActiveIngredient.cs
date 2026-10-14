using RAProductsRegestration.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.ActiveIngredients
{

    public sealed class ActiveIngredient : Entity, IAggregateRoot
    {
        private ActiveIngredient()
        {
        }

        public ActiveIngredient(
            int ctdId,
            string code,
            string name)
            : base(Guid.NewGuid())
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
