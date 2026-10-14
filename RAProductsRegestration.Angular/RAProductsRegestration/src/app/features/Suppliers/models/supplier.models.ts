export interface SupplierLookupResponse {
  id: string;
  jdeCode: number;
  name: string;
}

export interface SupplierPagedResponse {
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  items: SupplierLookupResponse[];
}