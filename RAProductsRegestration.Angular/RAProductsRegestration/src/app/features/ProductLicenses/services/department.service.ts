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
import { LookupResponse } from '../../product-basic-data/models/product-basic-data.models';




@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/departments`;


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


    return this.http.get<LookupResponse[]>(
      this.baseUrl,
      {
        params
      }
    );
  }
}