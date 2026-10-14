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
  LookupResponse
} from '../models/product-license.models';


@Injectable({
  providedIn: 'root'
})
export class MarketingTypeService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/marketing-types`;


  // =========================================================
  // Get All / Search
  // =========================================================

  getAll(
    search = ''
  ): Observable<LookupResponse[]> {

    let params =
      new HttpParams();


    const normalizedSearch =
      search.trim();


    if (normalizedSearch) {

      params =
        params.set(
          'search',
          normalizedSearch
        );
    }


    return this.http
      .get<LookupResponse[]>(
        this.baseUrl,
        {
          params
        }
      );
  }


  // =========================================================
  // Create
  // =========================================================

  create(
    name: string
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      {
        name
      }
    );
  }
}