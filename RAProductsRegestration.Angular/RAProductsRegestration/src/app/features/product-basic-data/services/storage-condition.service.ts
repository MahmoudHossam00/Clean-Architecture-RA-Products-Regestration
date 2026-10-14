import {
  inject,
  Injectable
} from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  StorageConditionLookupResponse
} from '../models/lookups.models';

import {
  API_CONFIG
} from '../../../core/config/api.config';


@Injectable({
  providedIn: 'root'
})
export class StorageConditionService {


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


  private readonly http =
    inject(HttpClient);

  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/StorageConditions`;


  getAll(
    activeOnly = true
  ): Observable<
    StorageConditionLookupResponse[]
  > {

    return this.http.get<
      StorageConditionLookupResponse[]
    >(
      this.baseUrl,
      {
        params: {
          activeOnly
        }
      }
    );
  }
}