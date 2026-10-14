import {
  Component,
  inject
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  ActivatedRoute,
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';

import {
  ProductLookupResponse
} from '../../models/product.models';

@Component({
  selector: 'app-product-details',
  standalone: true,

  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    RouterOutlet
  ],

  templateUrl:
    './product-details.component.html',

  styleUrl:
    './product-details.component.css'
})
export class ProductDetailsComponent {

  private readonly route =
    inject(ActivatedRoute);

  readonly productId =
    this.route.snapshot
      .paramMap
      .get('productId') ?? '';

  readonly product:
    ProductLookupResponse | undefined =
      history.state?.product;
}