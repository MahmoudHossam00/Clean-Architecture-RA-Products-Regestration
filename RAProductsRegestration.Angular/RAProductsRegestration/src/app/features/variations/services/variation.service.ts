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
  AddVariationRequirementInput,
  CreateVariationInput,
  UpdateVariationDeadlineInput,
  UpdateVariationDetailsInput,
  UpdateVariationRequirementInput,
  VariationResponse
} from '../models/variation.models';
import { AttachmentReferenceInput, DatedAttachmentInput } from '../../product-basic-data/models/product-basic-data.models';


@Injectable({
  providedIn: 'root'
})
export class VariationService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/variations`;


  // =========================================================
  // Get All Variations For Product
  // =========================================================

  getByProductId(
    productId: string
  ): Observable<VariationResponse[]> {

    return this.http.get<VariationResponse[]>(
      `${this.baseUrl}/product/${productId}`
    );
  }


  // =========================================================
  // Create Variation
  // =========================================================

  create(
    input: CreateVariationInput
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      input
    );
  }


  // =========================================================
  // Update Initiator / Cause
  // =========================================================

  updateDetails(
    variationId: string,
    input: UpdateVariationDetailsInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${variationId}/details`,
      input
    );
  }


  // =========================================================
  // Update Deadline
  // =========================================================

  updateDeadline(
    variationId: string,
    input: UpdateVariationDeadlineInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${variationId}/deadline`,
      input
    );
  }


  // =========================================================
  // Variation Approval - Add / Replace
  // =========================================================

  setApproval(
    variationId: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${variationId}/approval`,
      approval
    );
  }


  // =========================================================
  // Variation Approval - Remove
  // =========================================================

  removeApproval(
    variationId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${variationId}/approval`
    );
  }


  // =========================================================
  // Delete Variation
  // =========================================================

  remove(
    variationId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${variationId}`
    );
  }


  // =========================================================
  // Requirement - Add
  // =========================================================

  addRequirement(
    variationId: string,
    input: AddVariationRequirementInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${variationId}/requirements`,
      input
    );
  }


  // =========================================================
  // Requirement - Update
  // =========================================================

  updateRequirement(
    variationId: string,
    requirementId: string,
    input: UpdateVariationRequirementInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${variationId}/requirements/${requirementId}`,
      input
    );
  }


  // =========================================================
  // Requirement Attachment - Add / Replace
  // =========================================================

  setRequirementAttachment(
    variationId: string,
    requirementId: string,
    attachment: AttachmentReferenceInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${variationId}/requirements/${requirementId}/attachment`,
      attachment
    );
  }


  // =========================================================
  // Requirement Attachment - Remove
  // =========================================================

  removeRequirementAttachment(
    variationId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${variationId}/requirements/${requirementId}/attachment`
    );
  }


  // =========================================================
  // Requirement Approval - Add / Replace
  // =========================================================

  setRequirementApproval(
    variationId: string,
    requirementId: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${variationId}/requirements/${requirementId}/approval`,
      approval
    );
  }


  // =========================================================
  // Requirement Approval - Remove
  // =========================================================

  removeRequirementApproval(
    variationId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${variationId}/requirements/${requirementId}/approval`
    );
  }


  // =========================================================
  // Requirement - Delete
  // =========================================================

  removeRequirement(
    variationId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${variationId}/requirements/${requirementId}`
    );
  }
}