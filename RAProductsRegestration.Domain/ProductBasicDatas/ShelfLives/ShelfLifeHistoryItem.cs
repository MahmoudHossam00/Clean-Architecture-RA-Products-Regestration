using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.ProductBasicDatas.ShelfLives
{
    public sealed class ShelfLifeHistoryItem : Entity
    {
        private ShelfLifeHistoryItem()
        {
        }


        internal ShelfLifeHistoryItem(
            int versionNumber,
            ShelfLife shelfLife)
            : base(Guid.NewGuid())
        {
            VersionNumber =
                versionNumber;

            ShelfLife =
                shelfLife;
        }


        public int VersionNumber
        {
            get;
            private set;
        }


        public ShelfLife ShelfLife
        {
            get;
            private set;
        } = null!;
    }
}
