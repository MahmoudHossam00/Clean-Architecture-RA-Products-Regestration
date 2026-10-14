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
} from '../../../core/config/api.config';

import {
  SupplierLookupResponse,
  SupplierPagedResponse
} from '../models/supplier.models';


@Injectable({
  providedIn: 'root'
})
export class SupplierService {

  private readonly http =
    inject(HttpClient);

  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/Suppliers`;


  getAll(
    filter = '',
    pageNum = 1
  ): Observable<SupplierPagedResponse> {

    let params =
      new HttpParams()
        .set(
          'pageNum',
          pageNum
        );


    if (filter.trim()) {

      params =
        params.set(
          'filter',
          filter.trim()
        );
    }


    return this.http.get<SupplierPagedResponse>(
      this.baseUrl,
      {
        params
      }
    );
  }
}