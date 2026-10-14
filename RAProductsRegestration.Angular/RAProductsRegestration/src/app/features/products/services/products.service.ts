import {
  inject,
  Injectable
} from '@angular/core';

import {
  HttpClient,
  HttpParams
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  ProductLookupResponse
} from '../../product-basic-data/models/lookups.models';

import {
  PaginatedResponse
} from '../../../core/models/paginated-list';

import {
  API_CONFIG
} from '../../../core/config/api.config';


@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  private readonly http =
    inject(HttpClient);

  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/Products`;


  getAll(
    filter = '',
    pageNumber = 1,
    pageSize = 20
  ): Observable<
    PaginatedResponse<ProductLookupResponse>
  > {

    let params =
      new HttpParams()
        .set(
          'pageNumber',
          pageNumber
        )
        .set(
          'pageSize',
          pageSize
        );

    if (filter.trim()) {
      params =
        params.set(
          'filter',
          filter.trim()
        );
    }

    return this.http.get<
      PaginatedResponse<ProductLookupResponse>
    >(
      this.baseUrl,
      {
        params
      }
    );
  }
}