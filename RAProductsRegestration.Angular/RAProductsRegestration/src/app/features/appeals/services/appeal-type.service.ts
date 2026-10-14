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
export class AppealTypeService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/appeal-types`;


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


    return this.http.get<LookupResponse[]>(
      this.baseUrl,
      {
        params
      }
    );
  }


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