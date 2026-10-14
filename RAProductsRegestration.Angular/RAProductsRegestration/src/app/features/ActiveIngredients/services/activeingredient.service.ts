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
  API_CONFIG
} from '../../../core/config/api.config'
import { PaginatedResponse } from '../../../core/models/paginated-list';
import { ActiveIngredientLookupResponse } from '../../product-basic-data/models/lookups.models';





@Injectable({
  providedIn: 'root'
})
export class ActiveIngredientService {

  private readonly http =
    inject(HttpClient);

  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/ActiveIngredients`;


  getAll(
    filter = '',
    pageNumber = 1,
    pageSize = 20
  ): Observable<
    PaginatedResponse<ActiveIngredientLookupResponse>
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
      PaginatedResponse<ActiveIngredientLookupResponse>
    >(
      this.baseUrl,
      {
        params
      }
    );
  }
}