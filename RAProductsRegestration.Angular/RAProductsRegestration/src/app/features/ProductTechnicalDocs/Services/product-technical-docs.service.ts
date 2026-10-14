import {
  inject,
  Injectable
} from '@angular/core';

import {
  HttpClient,
  HttpContext
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  API_CONFIG
} from '../../../core/config/api.config';

import {
  SKIP_ERROR_TOAST
} from '../../../core/interceptors/error.interceptor';
import { DatedApprovalDocumentInput, ProductTechnicalDocsResponse } from '../Models/product-technical-docs.models';




@Injectable({
  providedIn: 'root'
})
export class ProductTechnicalDocsService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/product-technical-docs`;


  // =========================================================
  // Query
  // =========================================================

  getByProductId(
    productId: string
  ): Observable<ProductTechnicalDocsResponse> {

    /*
     * 404 is expected when ProductTechnicalDocs
     * has not been created yet.
     *
     * Don't show the global error toast.
     */
    const context =
      new HttpContext()
        .set(
          SKIP_ERROR_TOAST,
          true
        );


    return this.http
      .get<ProductTechnicalDocsResponse>(
        `${this.baseUrl}/product/${productId}`,
        {
          context
        }
      );
  }


  // =========================================================
  // Create
  // =========================================================

  create(
    productId: string
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      {
        productId
      }
    );
  }


  // =========================================================
  // Approved Specifications
  // =========================================================

  addApprovedSpecifications(
    id: string,
    document: DatedApprovalDocumentInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/specifications`,
      document
    );
  }


  // =========================================================
  // Approved Composition
  // =========================================================

  addApprovedComposition(
    id: string,
    document: DatedApprovalDocumentInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/compositions`,
      document
    );
  }


  // =========================================================
  // Approved MOA + Validation
  // =========================================================

  addApprovedMoaValidation(
    id: string,
    document: DatedApprovalDocumentInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/moa-validations`,
      document
    );
  }
}