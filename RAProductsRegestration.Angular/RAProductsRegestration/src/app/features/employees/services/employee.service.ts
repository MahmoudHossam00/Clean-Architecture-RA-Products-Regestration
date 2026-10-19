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
  API_CONFIG
} from '../../../core/config/api.config';

import {
  EmployeeImportResponse,
  EmployeeResponse
} from '../models/employee.models';


@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private readonly http =
    inject(
      HttpClient
    );


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/employees`;


  // =========================================================
  // Get All
  // =========================================================

  getAll():
    Observable<EmployeeResponse[]> {

    return this.http
      .get<EmployeeResponse[]>(
        this.baseUrl
      );
  }


  // =========================================================
  // Download Excel Template
  // =========================================================

  downloadImportTemplate():
    Observable<Blob> {

    return this.http
      .get(
        `${this.baseUrl}/import-template`,
        {
          responseType:
            'blob'
        }
      );
  }


  // =========================================================
  // Excel Upsert
  // =========================================================

  importEmployees(
    file: File
  ): Observable<EmployeeImportResponse> {

    const formData =
      new FormData();


    formData.append(
      'file',
      file
    );


    return this.http
      .post<EmployeeImportResponse>(
        `${this.baseUrl}/import`,
        formData
      );
  }
}