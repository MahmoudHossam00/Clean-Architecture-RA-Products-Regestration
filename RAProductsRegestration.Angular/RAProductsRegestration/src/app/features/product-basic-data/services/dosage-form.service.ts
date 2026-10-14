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
  DosageFormLookupResponse
} from '../models/lookups.models';

import {
  API_CONFIG
} from '../../../core/config/api.config';


@Injectable({
  providedIn: 'root'
})
export class DosageFormService {

  private readonly http =
    inject(HttpClient);

  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/DosageForms`;

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
  getAll(
    activeOnly = true
  ): Observable<DosageFormLookupResponse[]> {

    return this.http.get<
      DosageFormLookupResponse[]
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