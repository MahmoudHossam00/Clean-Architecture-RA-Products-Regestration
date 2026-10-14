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
  AttachmentReferenceInput,
  DatedAttachmentInput,
  FulfillmentDeadlineInput,
  ProductBasicDataResponse,
  ShelfLifeHistoryResponse,
  ShelfLifeInput
} from '../models/product-basic-data.models';

import {
  SKIP_ERROR_TOAST
} from '../../../core/interceptors/error.interceptor';


@Injectable({
  providedIn: 'root'
})
export class ProductBasicDataService {

  private readonly http =
    inject(HttpClient);


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/product-basic-data`;


  // =========================================================
  // Query
  // =========================================================

  getByProductId(
    productId: string
  ): Observable<ProductBasicDataResponse> {

    const context =
      new HttpContext()
        .set(
          SKIP_ERROR_TOAST,
          true
        );


    return this.http.get<
      ProductBasicDataResponse
    >(
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
    productId: string,
    tradeName: string | null
  ): Observable<unknown> {

    return this.http.post(
      this.baseUrl,
      {
        productId,
        tradeName
      }
    );
  }


  // =========================================================
  // General
  // =========================================================

  updateTradeName(
    id: string,
    tradeName: string | null
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/trade-name`,
      {
        tradeName
      }
    );
  }


  updateDosageForm(
    id: string,
    dosageFormId: string | null
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/dosage-form`,
      {
        dosageFormId
      }
    );
  }


  updateStorageCondition(
    id: string,
    storageConditionId: string | null
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/storage-condition`,
      {
        storageConditionId
      }
    );
  }


  updatePhysicalDescription(
    id: string,
    physicalDescription: string | null
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/physical-description`,
      {
        physicalDescription
      }
    );
  }


  // =========================================================
  // Product Shelf Life
  // =========================================================

  updateShelfLife(
    id: string,
    shelfLife: ShelfLifeInput
  ): Observable<void> {

    return this.http.put<void>(
      `${this.baseUrl}/${id}/shelf-life`,
      {
        shelfLife
      }
    );
  }


  addShelfLifeAttachment(
    id: string,
    attachment: AttachmentReferenceInput
  ): Observable<void> {

    return this.http.post<void>(
      `${this.baseUrl}/${id}/shelf-life/attachments`,
      attachment
    );
  }


  removeShelfLifeAttachment(
    id: string,
    attachmentId: string
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.baseUrl}/${id}/shelf-life/attachments/${attachmentId}`
    );
  }


  // =========================================================
  // Active Ingredients
  // =========================================================

  addActiveIngredient(
    id: string,
    activeIngredientId: string,
    strength: string
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/active-ingredients`,
      {
        activeIngredientId,
        strength
      }
    );
  }


  updateActiveIngredientStrength(
    id: string,
    productActiveIngredientId: string,
    strength: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/strength`,
      {
        strength
      }
    );
  }


  updateActiveIngredientStorageCondition(
    id: string,
    productActiveIngredientId: string,
    storageConditionId: string | null
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/storage-condition`,
      {
        storageConditionId
      }
    );
  }


  updateActiveIngredientPhysicalDescription(
    id: string,
    productActiveIngredientId: string,
    physicalDescription: string | null
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/physical-description`,
      {
        physicalDescription
      }
    );
  }


  updateActiveIngredientShelfLife(
    id: string,
    productActiveIngredientId: string,
    shelfLife: ShelfLifeInput
  ): Observable<void> {

    return this.http.put<void>(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/shelf-life`,
      {
        shelfLife
      }
    );
  }


  addActiveIngredientShelfLifeAttachment(
    id: string,
    productActiveIngredientId: string,
    attachment: AttachmentReferenceInput
  ): Observable<void> {

    return this.http.post<void>(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/shelf-life/attachments`,
      attachment
    );
  }


  removeActiveIngredientShelfLifeAttachment(
    id: string,
    productActiveIngredientId: string,
    attachmentId: string
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/shelf-life/attachments/${attachmentId}`
    );
  }


  removeActiveIngredient(
    id: string,
    productActiveIngredientId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}`
    );
  }


  // =========================================================
  // Suppliers
  // =========================================================

  addApprovedSupplier(
    id: string,
    productActiveIngredientId: string,
    supplierId: string
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/suppliers`,
      {
        supplierId
      }
    );
  }


  activateApprovedSupplier(
    id: string,
    productActiveIngredientId: string,
    approvedSupplierId: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/suppliers/${approvedSupplierId}/activate`,
      {}
    );
  }


  deactivateApprovedSupplier(
    id: string,
    productActiveIngredientId: string,
    approvedSupplierId: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/suppliers/${approvedSupplierId}/deactivate`,
      {}
    );
  }


  // =========================================================
  // Product Approved Packs
  // =========================================================

  addApprovedPack(
    id: string,
    description: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/packs`,
      {
        description,
        approval
      }
    );
  }


  updateApprovedPack(
    id: string,
    approvedPackId: string,
    description: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/packs/${approvedPackId}`,
      {
        description,
        approval
      }
    );
  }


  // =========================================================
  // Active Ingredient Packs
  // =========================================================

  addActiveIngredientApprovedPack(
    id: string,
    productActiveIngredientId: string,
    description: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/packs`,
      {
        description,
        approval
      }
    );
  }


  updateActiveIngredientApprovedPack(
    id: string,
    productActiveIngredientId: string,
    approvedPackId: string,
    description: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/packs/${approvedPackId}`,
      {
        description,
        approval
      }
    );
  }


  removeActiveIngredientApprovedPack(
    id: string,
    productActiveIngredientId: string,
    approvedPackId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/active-ingredients/${productActiveIngredientId}/packs/${approvedPackId}`
    );
  }


  // =========================================================
  // Manufacturers
  // =========================================================

  addApprovedManufacturer(
    id: string,
    manufacturerName: string,
    approval: DatedAttachmentInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/manufacturers`,
      {
        manufacturerName,
        approval
      }
    );
  }


  activateApprovedManufacturer(
    id: string,
    approvedManufacturerId: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/manufacturers/${approvedManufacturerId}/activate`,
      {}
    );
  }


  deactivateApprovedManufacturer(
    id: string,
    approvedManufacturerId: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/manufacturers/${approvedManufacturerId}/deactivate`,
      {}
    );
  }


  removeApprovedManufacturer(
    id: string,
    approvedManufacturerId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/manufacturers/${approvedManufacturerId}`
    );
  }


  // =========================================================
  // Prices
  // =========================================================

  addApprovedPrice(
    id: string,
    price: number,
    pricePerDescription: string,
    issueDate: string,
    expiryDate: string
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/prices`,
      {
        price,
        pricePerDescription,
        issueDate,
        expiryDate
      }
    );
  }


  updateApprovedPrice(
    id: string,
    approvedPriceId: string,
    price: number,
    pricePerDescription: string,
    issueDate: string,
    expiryDate: string
  ): Observable<unknown> {

    return this.http.put(
      `${this.baseUrl}/${id}/prices/${approvedPriceId}`,
      {
        price,
        pricePerDescription,
        issueDate,
        expiryDate
      }
    );
  }


  removeApprovedPrice(
    id: string,
    approvedPriceId: string
  ): Observable<unknown> {

    return this.http.delete(
      `${this.baseUrl}/${id}/prices/${approvedPriceId}`
    );
  }


  // =========================================================
  // PV HISTORY
  // =========================================================

  addPV(
    id: string,
    value: FulfillmentDeadlineInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/pv`,
      value
    );
  }


  fulfillPV(
    id: string,
    fulfillmentDate: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/pv/fulfill`,
      {
        fulfillmentDate
      }
    );
  }


  resetPV(
    id: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/pv/reset`,
      {}
    );
  }


  // =========================================================
  // DECREE 600 HISTORY
  // =========================================================

  addDecree600(
    id: string,
    value: FulfillmentDeadlineInput
  ): Observable<unknown> {

    return this.http.post(
      `${this.baseUrl}/${id}/decree600`,
      value
    );
  }


  fulfillDecree600(
    id: string,
    fulfillmentDate: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/decree600/fulfill`,
      {
        fulfillmentDate
      }
    );
  }
getShelfLifeHistory(
  productBasicDataId: string
): Observable<ShelfLifeHistoryResponse[]> {

  return this.http.get<ShelfLifeHistoryResponse[]>(
    `${this.baseUrl}/${productBasicDataId}/shelf-life/history`
  );
}

  resetDecree600(
    id: string
  ): Observable<unknown> {

    return this.http.patch(
      `${this.baseUrl}/${id}/decree600/reset`,
      {}
    );
  }
}