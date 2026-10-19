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

import {
  ProductTechnicalDocsComponent
} from './features/ProductTechnicalDocs/Component/product-technical-docs.component';

import {
  ProductLicenseComponent
} from './features/ProductLicenses/component/product-license.component';

import {
  ProductVariationsComponent
} from './features/variations/component/product-variations.component';

import {
  ProductAppealsComponent
} from './features/appeals/product-appeals/product-appeals.component';

import {
  ProductPromotionalMaterialsComponent
} from './features/promotionalmaterial/component/product-promotional-materials.component';

import {
  ProductMiscellaneousApprovalsComponent
} from './features/Miscellaneousapproval/component/product-miscellaneous-approvals.component';


import {
  LoginComponent
} from './features/authentication/login/login.component';

import {
  FirstLoginComponent
} from './features/authentication/first-login/first-login.component';


import {
  EmployeeImportComponent
} from './features/employees/pages/employee-import/employee-import.component';

import {
  CreateUserComponent
} from './features/users/pages/create-user/create-user.component';


import {
  authGuard
} from './core/auth/guards/auth.guard';

import {
  guestGuard
} from './core/auth/guards/guest.guard';

import {
  firstLoginGuard
} from './core/auth/guards/first-login.guard';

import {
  firstLoginPageGuard
} from './core/auth/guards/first-login-page.guard';

import {
  roleGuard
} from './core/auth/guards/role.guard';


export const routes: Routes = [

  // =========================================================
  // Public Authentication Routes
  // =========================================================

  {
    path: 'login',

    component:
      LoginComponent,

    canActivate: [
      guestGuard
    ]
  },


  // =========================================================
  // Mandatory First Login
  // =========================================================

  {
    path: 'first-login',

    component:
      FirstLoginComponent,

    canActivate: [
      authGuard,
      firstLoginPageGuard
    ]
  },


  // =========================================================
  // Protected Application
  // =========================================================

  {
    path: '',

    canActivate: [
      authGuard
    ],

    canActivateChild: [
      authGuard,
      firstLoginGuard
    ],

    children: [

      // =====================================================
      // Default
      // =====================================================

      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'products'
      },


      // =====================================================
      // Products
      // =====================================================

      {
        path: 'products',

        component:
          ProductsListComponent
      },


      // =====================================================
      // Product Details
      // =====================================================

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
            path: 'technical-docs',

            component:
              ProductTechnicalDocsComponent
          },


          {
            path: 'license',

            component:
              ProductLicenseComponent
          },


          {
            path: 'variations',

            component:
              ProductVariationsComponent
          },


          {
            path: 'appeals',

            component:
              ProductAppealsComponent
          },


          {
            path: 'promotional-materials',

            component:
              ProductPromotionalMaterialsComponent
          },


          {
            path: 'miscellaneous-approvals',

            component:
              ProductMiscellaneousApprovalsComponent
          }
        ]
      },


      // =====================================================
      // ADMIN - Employee Import
      // =====================================================

      {
        path: 'admin/employees',

        component:
          EmployeeImportComponent,

        canActivate: [
          roleGuard
        ],

        data: {
          roles: [
            'ADMIN'
          ]
        }
      },


      // =====================================================
      // ADMIN - Create User
      // =====================================================

      {
        path: 'admin/users/create',

        component:
          CreateUserComponent,

        canActivate: [
          roleGuard
        ],

        data: {
          roles: [
            'ADMIN'
          ]
        }
      }

    ]
  },


  // =========================================================
  // Unknown Route
  // =========================================================

  {
    path: '**',

    redirectTo:
      'products'
  }

];