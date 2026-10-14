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
  AddAppealRequirementInput,
  AppealResponse,
  CreateAppealInput,
  UpdateAppealDetailsInput,
  UpdateAppealRequirementInput
} from '../models/appeal.models';
import { AttachmentReferenceInput, DatedAttachmentInput } from '../../product-basic-data/models/product-basic-data.models';


@Injectable({
  providedIn: 'root'
})
export class AppealService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/appeals`;


  // =========================================================
  // Get Appeals By Product
  // =========================================================

  getByProductId(
    productId: string
  ): Observable<AppealResponse[]> {

    return this.http.get<AppealResponse[]>(
      `${this.baseUrl}/product/${productId}`
    );
  }


  // =========================================================
  // Create Appeal
  // =========================================================

  create(
    input: CreateAppealInput
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      input
    );
  }


  // =========================================================
  // Update Appeal Details
  // =========================================================

  updateDetails(
    appealId: string,
    input: UpdateAppealDetailsInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${appealId}/details`,
      input
    );
  }


  // =========================================================
  // Appeal Approval
  // =========================================================

  setApproval(
    appealId: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${appealId}/approval`,
      approval
    );
  }


  removeApproval(
    appealId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${appealId}/approval`
    );
  }


  // =========================================================
  // Delete Appeal
  // =========================================================

  remove(
    appealId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${appealId}`
    );
  }


  // =========================================================
  // Requirement - Add
  // =========================================================

  addRequirement(
    appealId: string,
    input: AddAppealRequirementInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${appealId}/requirements`,
      input
    );
  }


  // =========================================================
  // Requirement - Update
  // =========================================================

  updateRequirement(
    appealId: string,
    requirementId: string,
    input: UpdateAppealRequirementInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${appealId}/requirements/${requirementId}`,
      input
    );
  }


  // =========================================================
  // Requirement Attachment
  // =========================================================

  setRequirementAttachment(
    appealId: string,
    requirementId: string,
    attachment: AttachmentReferenceInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${appealId}/requirements/${requirementId}/attachment`,
      attachment
    );
  }


  removeRequirementAttachment(
    appealId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${appealId}/requirements/${requirementId}/attachment`
    );
  }


  // =========================================================
  // Requirement Approval
  // =========================================================

  setRequirementApproval(
    appealId: string,
    requirementId: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${appealId}/requirements/${requirementId}/approval`,
      approval
    );
  }


  removeRequirementApproval(
    appealId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${appealId}/requirements/${requirementId}/approval`
    );
  }


  // =========================================================
  // Delete Requirement
  // =========================================================

  removeRequirement(
    appealId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${appealId}/requirements/${requirementId}`
    );
  }
}