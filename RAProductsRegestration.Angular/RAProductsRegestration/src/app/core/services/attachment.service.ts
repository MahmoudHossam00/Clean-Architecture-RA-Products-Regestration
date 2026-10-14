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
} from '../config/api.config';

import {
  AttachmentUploadResponse
} from '../../features/product-basic-data/models/product-basic-data.models';


@Injectable({
  providedIn: 'root'
})
export class AttachmentService {

  private readonly http =
    inject(HttpClient);

  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/attachments`;


  // =========================================================
  // Upload
  // =========================================================

  upload(
    file: File
  ): Observable<AttachmentUploadResponse> {

    const formData =
      new FormData();

    formData.append(
      'file',
      file,
      file.name
    );

    return this.http.post<
      AttachmentUploadResponse
    >(
      this.baseUrl,
      formData
    );
  }


  // =========================================================
  // Download
  // =========================================================

  download(
    attachmentId: string,
    fileName: string
  ): Observable<Blob> {

    const params =
      new HttpParams()
        .set(
          'fileName',
          fileName
        );

    return this.http.get(
      `${this.baseUrl}/${attachmentId}`,
      {
        params,
        responseType: 'blob'
      }
    );
  }
}