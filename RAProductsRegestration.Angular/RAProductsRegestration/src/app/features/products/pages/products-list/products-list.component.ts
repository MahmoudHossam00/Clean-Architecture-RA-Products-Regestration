import {
  Component,
  inject,
  OnInit
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormsModule
} from '@angular/forms';

import {
  Router
} from '@angular/router';

import {
  ProductsService
} from '../../services/products.service';

import {
  ProductLookupResponse
} from '../../models/product.models';

@Component({
  selector: 'app-products-list',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './products-list.component.html',

  styleUrl:
    './products-list.component.css'
})
export class ProductsListComponent
  implements OnInit {

  private readonly productsService =
    inject(ProductsService);

  private readonly router =
    inject(Router);

  products:
    ProductLookupResponse[] = [];

  filter = '';

  pageNumber:number = 1;

  totalPages = 0;

  totalCount = 0;

  loading = false;

  errorMessage = '';

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {

    this.loading = true;

    this.errorMessage = '';

    this.productsService
      .getAll(
        this.filter,
        this.pageNumber,
      )
      .subscribe({

        next: response => {

          this.products =
            response.items;

          this.pageNumber =
            response.pageNumber;

          this.totalPages =
            response.totalPages;

          this.totalCount =
            response.totalCount;

          this.loading = false;
        },

        error: error => {

          console.error(error);

          this.errorMessage =
            'Unable to load products.';

          this.loading = false;
        }
      });
  }

  search(): void {

    this.pageNumber = 1;

    this.loadProducts();
  }

  clearSearch(): void {

    this.filter = '';

    this.pageNumber = 1;

    this.loadProducts();
  }

  openProduct(
    product: ProductLookupResponse
  ): void {

    this.router.navigate(
      [
        '/products',
        product.id
      ],
      {
        state: {
          product
        }
      }
    );
  }

  previousPage(): void {

    if (this.pageNumber <= 1) {
      return;
    }

    this.pageNumber--;

    this.loadProducts();
  }

  nextPage(): void {

    if (
      this.pageNumber >=
      this.totalPages
    ) {
      return;
    }

    this.pageNumber++;

    this.loadProducts();
  }
}