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
  CreatePromotionalMaterialInput,
  PromotionalMaterialResponse,
  ReplacePromotionalMaterialFileInput,
  SetPromotionalMaterialApprovalInput
} from '../models/promotional-material.models';


@Injectable({
  providedIn: 'root'
})
export class PromotionalMaterialService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/promotional-materials`;


  // =========================================================
  // Get By Product
  // =========================================================

  getByProductId(
    productId: string
  ): Observable<PromotionalMaterialResponse[]> {

    return this.http.get<
      PromotionalMaterialResponse[]
    >(
      `${this.baseUrl}/product/${productId}`
    );
  }


  // =========================================================
  // Create
  // =========================================================

  create(
    input:
      CreatePromotionalMaterialInput
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      input
    );
  }


  // =========================================================
  // Update Type
  // =========================================================

  updateType(
    id: string,
    promotionalMaterialTypeId: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/type`,
      {
        promotionalMaterialTypeId
      }
    );
  }


  // =========================================================
  // Replace Material File
  // =========================================================

  replaceMaterialFile(
    id: string,
    input:
      ReplacePromotionalMaterialFileInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/material-file`,
      input
    );
  }


  // =========================================================
  // Set / Update Approval
  // =========================================================

  setApproval(
    id: string,
    input:
      SetPromotionalMaterialApprovalInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/approval`,
      input
    );
  }


  // =========================================================
  // Remove Approval
  // =========================================================

  removeApproval(
    id: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/approval`
    );
  }


  // =========================================================
  // Delete
  // =========================================================

  remove(
    id: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}`
    );
  }
}