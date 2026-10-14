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
  AddApprovalRequirementInput,
  AttachmentReferenceInput,
  CreateMiscellaneousApprovalInput,
  DatedAttachmentInput,
  MiscellaneousApprovalResponse,
  SetMiscellaneousApprovalInput,
  UpdateApprovalRequirementInput,
  UpdateMiscellaneousApprovalDetailsInput
} from '../models/miscellaneous-approval.models';


@Injectable({
  providedIn: 'root'
})
export class MiscellaneousApprovalService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/miscellaneous-approvals`;


  // =========================================================
  // Get By Product
  // =========================================================

  getByProductId(
    productId: string
  ): Observable<MiscellaneousApprovalResponse[]> {

    return this.http.get<
      MiscellaneousApprovalResponse[]
    >(
      `${this.baseUrl}/product/${productId}`
    );
  }


  // =========================================================
  // Create
  // =========================================================

  create(
    input:
      CreateMiscellaneousApprovalInput
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      input
    );
  }


  // =========================================================
  // Update File Type
  // =========================================================

  updateFileType(
    id: string,
    fileTypeId: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/file-type`,
      {
        fileTypeId
      }
    );
  }


  // =========================================================
  // Update Details
  // =========================================================

  updateDetails(
    id: string,
    input:
      UpdateMiscellaneousApprovalDetailsInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/details`,
      input
    );
  }


  // =========================================================
  // Main Approval
  // =========================================================

  setApproval(
    id: string,
    input:
      SetMiscellaneousApprovalInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/approval`,
      input
    );
  }


  removeApproval(
    id: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/approval`
    );
  }


  // =========================================================
  // Delete Approval
  // =========================================================

  remove(
    id: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}`
    );
  }


  // =========================================================
  // Add Requirement
  // =========================================================

  addRequirement(
    approvalId: string,
    input:
      AddApprovalRequirementInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${approvalId}/requirements`,
      input
    );
  }


  // =========================================================
  // Update Requirement
  // =========================================================

  updateRequirement(
    approvalId: string,
    requirementId: string,
    input:
      UpdateApprovalRequirementInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${approvalId}/requirements/${requirementId}`,
      input
    );
  }


  // =========================================================
  // Requirement Attachment
  // =========================================================

  setRequirementAttachment(
    approvalId: string,
    requirementId: string,
    attachment:
      AttachmentReferenceInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${approvalId}/requirements/${requirementId}/attachment`,
      {
        attachment
      }
    );
  }


  removeRequirementAttachment(
    approvalId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${approvalId}/requirements/${requirementId}/attachment`
    );
  }


  // =========================================================
  // Requirement Approval
  // =========================================================

  approveRequirement(
    approvalId: string,
    requirementId: string,
    approval:
      DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${approvalId}/requirements/${requirementId}/approval`,
      {
        approval
      }
    );
  }


  removeRequirementApproval(
    approvalId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${approvalId}/requirements/${requirementId}/approval`
    );
  }


  // =========================================================
  // Delete Requirement
  // =========================================================

  removeRequirement(
    approvalId: string,
    requirementId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${approvalId}/requirements/${requirementId}`
    );
  }
}