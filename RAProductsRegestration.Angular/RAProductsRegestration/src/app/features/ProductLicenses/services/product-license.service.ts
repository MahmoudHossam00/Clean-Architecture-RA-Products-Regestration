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

import {
  AddLicenseRequirementInput,
  AttachmentReferenceInput,
  CreateProductLicenseInput,
  DatedAttachmentInput,
  ProductLicenseResponse,
  UpdateLicenseRequirementInput
} from '../models/product-license.models';


@Injectable({
  providedIn: 'root'
})
export class ProductLicenseService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/product-licenses`;


  // =========================================================
  // Get By Product
  // =========================================================

  getByProductId(
    productId: string
  ): Observable<ProductLicenseResponse> {

    const context =
      new HttpContext()
        .set(
          SKIP_ERROR_TOAST,
          true
        );


    return this.http.get<ProductLicenseResponse>(
      `${this.baseUrl}/product/${productId}`,
      {
        context
      }
    );
  }


  // =========================================================
  // Create License
  // =========================================================

  create(
    input: CreateProductLicenseInput
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      input
    );
  }


  // =========================================================
  // License Category
  // =========================================================

  updateLicenseCategory(
    id: string,
    licenseCategoryId: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/license-category`,
      {
        licenseCategoryId
      }
    );
  }


  // =========================================================
  // Marketing Type
  // =========================================================

  updateMarketingType(
    id: string,
    marketingTypeId: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/marketing-type`,
      {
        marketingTypeId
      }
    );
  }


  // =========================================================
  // Dates
  // =========================================================

  updateDates(
    id: string,
    issueDate: string,
    validityDate: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/dates`,
      {
        issueDate,
        validityDate
      }
    );
  }


  // =========================================================
  // Registration Number
  // =========================================================

  updateRegistrationNumber(
    id: string,
    registrationNumberYear: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/registration-number`,
      {
        registrationNumberYear
      }
    );
  }


  // =========================================================
  // Last Approved Leaflet
  // =========================================================

  setLastApprovedLeaflet(
    id: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/last-approved-leaflet`,
      approval
    );
  }


  // =========================================================
  // Last Approved Layout
  // =========================================================

  setLastApprovedLayout(
    id: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/last-approved-layout`,
      approval
    );
  }


  // =========================================================
  // Requirements - Add
  // =========================================================

  addRequirement(
    id: string,
    input: AddLicenseRequirementInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/requirements`,
      input
    );
  }


  // =========================================================
  // Requirements - Update
  // =========================================================

  updateRequirement(
    id: string,
    requirementId: string,
    input: UpdateLicenseRequirementInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/requirements/${requirementId}`,
      input
    );
  }


  // =========================================================
  // Requirement Attachment - Add / Replace
  // =========================================================

  setRequirementAttachment(
    id: string,
    requirementId: string,
    attachment: AttachmentReferenceInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/requirements/${requirementId}/attachment`,
      attachment
    );
  }


  // =========================================================
  // Requirement Attachment - Remove
  // =========================================================

  removeRequirementAttachment(
    id: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/requirements/${requirementId}/attachment`
    );
  }


  // =========================================================
  // Requirement Approval - Add / Replace
  // =========================================================

  setRequirementApproval(
    id: string,
    requirementId: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/requirements/${requirementId}/approval`,
      approval
    );
  }


  // =========================================================
  // Requirement Approval - Remove
  // =========================================================

  removeRequirementApproval(
    id: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/requirements/${requirementId}/approval`
    );
  }


  // =========================================================
  // Requirement - Delete
  // =========================================================

  removeRequirement(
    id: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/requirements/${requirementId}`
    );
  }
}