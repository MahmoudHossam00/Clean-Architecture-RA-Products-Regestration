export interface DosageFormLookupResponse {
  id: string;
  name: string;
}

export interface StorageConditionLookupResponse {
  id: string;
  name: string;
}

export interface ProductLookupResponse {
  id: string;
  ctdId: number;
  productCode: string;
  productName: string;
  companyId: string;
  companyName: string;
}
export interface ActiveIngredientLookupResponse {
  id: string;
  ctdId: number;
  code: string;
  name: string;
}