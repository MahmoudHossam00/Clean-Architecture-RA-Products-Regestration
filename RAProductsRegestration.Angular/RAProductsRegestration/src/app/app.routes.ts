import {
  Routes
} from '@angular/router';

import {
  ProductsListComponent
} from './features/products/pages/products-list/products-list.component';

import {
  ProductDetailsComponent
} from './features/products/pages/product-details/product-details.component';

import {
  ProductBasicDataComponent
} from './features/product-basic-data/pages/product-basic-data/product-basic-data.component';
import { ProductTechnicalDocsComponent } from './features/ProductTechnicalDocs/Component/product-technical-docs.component';
import { ProductLicenseComponent } from './features/ProductLicenses/component/product-license.component';
import { ProductVariationsComponent } from './features/variations/component/product-variations.component';
import { ProductAppealsComponent } from './features/appeals/product-appeals/product-appeals.component';
import { ProductPromotionalMaterialsComponent } from './features/promotionalmaterial/component/product-promotional-materials.component';
import { ProductMiscellaneousApprovalsComponent } from './features/Miscellaneousapproval/component/product-miscellaneous-approvals.component';




export const routes: Routes = [

  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'products'
  },

  {
    path: 'products',
    component:
      ProductsListComponent
  },

  {
    path: 'products/:productId',
    component:
      ProductDetailsComponent,

    children: [

      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'basic-data'
      },

      {
        path: 'basic-data',
        component:
          ProductBasicDataComponent
      },
{
  path: 'appeals',
  component: ProductAppealsComponent
},
      {
        path: 'technical-docs',
        component:
          ProductTechnicalDocsComponent
      },
{
  path: 'variations',
  component: ProductVariationsComponent
},
{
  path: 'license',
  component:
    ProductLicenseComponent
},
{
  path: 'promotional-materials',
  component: ProductPromotionalMaterialsComponent
},
{
  path: 'miscellaneous-approvals',
  component: ProductMiscellaneousApprovalsComponent
}

    ]
  },

  {
    path: '**',
    redirectTo: 'products'
  }

];