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
} from '../models/promotional-material.models';


@Injectable({
  providedIn: 'root'
})
export class PromotionalMaterialTypeService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/promotional-material-types`;


  // =========================================================
  // Get All
  // =========================================================

  getAll(
    search = ''
  ): Observable<LookupResponse[]> {

    let params =
      new HttpParams();


    const normalized =
      search.trim();


    if (normalized) {

      params =
        params.set(
          'search',
          normalized
        );
    }


    return this.http.get<
      LookupResponse[]
    >(
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