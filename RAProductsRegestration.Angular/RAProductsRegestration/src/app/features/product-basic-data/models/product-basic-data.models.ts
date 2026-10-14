// ==========================================================
// Common read models
// ==========================================================

export interface LookupResponse {
  id: string;
  name: string;
}

export interface AttachmentResponse {
  attachmentId: string;
  fileName: string;
  notes?: string | null;
}

export interface AttachmentReferenceInput {
  attachmentId: string;
  fileName: string;
  notes?: string | null;
}

export interface ShelfLifeResponse {
  numberOfMonths: number;
  approvalDate: string;
  attachments: AttachmentResponse[];
}

export interface ShelfLifeInput {
  numberOfMonths: number;
  approvalDate: string;
  attachments: AttachmentReferenceInput[];
}

export interface DatedAttachmentResponse {
  date: string;
  attachment: AttachmentResponse;
}

// ==========================================================
// Regulatory Deadline History
// ==========================================================

export interface FulfillmentDeadlineHistoryResponse {

  id: string;

  versionNumber: number;

  description: string;

  deadline: string;

  notes: string | null;

  fulfillmentDate: string | null;
}


// ==========================================================
// Approved Prices
// ==========================================================
export interface ApprovedPriceResponse {
  id: string;

  price: number;

  pricePerDescription: string;

  issueDate: string;

  expiryDate: string;
}


// ==========================================================
// Approved Packs
// ==========================================================

export interface ApprovedPackResponse {
  id: string;
  description: string;
  approval: DatedAttachmentResponse;
}


// ==========================================================
// Approved Manufacturers
// ==========================================================

export interface ApprovedManufacturerResponse {
  id: string;
  manufacturerName: string;
  isActive: boolean;
  approval: DatedAttachmentResponse;
}


// ==========================================================
// Approved Suppliers
// ==========================================================

export interface ApprovedSupplierResponse {
  id: string;
  supplierId: string;
  jdeCode: number;
  supplierName: string;
  isActive: boolean;
}


// ==========================================================
// Active Ingredients
// ==========================================================

export interface ProductActiveIngredientResponse {
  id: string;

  activeIngredient: LookupResponse;

  strength: string;

  storageCondition: LookupResponse | null;

  physicalDescription: string | null;

  shelfLife: ShelfLifeResponse | null;

  approvedSuppliers: ApprovedSupplierResponse[];

  approvedPacks: ApprovedPackResponse[];
}


// ==========================================================
// Main Product Basic Data response
// ==========================================================

export interface ProductBasicDataResponse {

  id: string;

  productId: string;

  productCode: string;

  productName: string;

  tradeName: string | null;

  dosageForm: LookupResponse | null;

  storageCondition: LookupResponse | null;

  physicalDescription: string | null;

  shelfLife?: ShelfLifeResponse | null;


  // ========================================================
  // PV
  // ========================================================

  currentPV:
    FulfillmentDeadlineHistoryResponse | null;

  pvHistory:
    FulfillmentDeadlineHistoryResponse[];


  // ========================================================
  // Decree 600
  // ========================================================

  currentDecree600:
    FulfillmentDeadlineHistoryResponse | null;

  decree600History:
    FulfillmentDeadlineHistoryResponse[];


  activeIngredients:
    ProductActiveIngredientResponse[];

  approvedPacks:
    ApprovedPackResponse[];

  approvedPrices:
    ApprovedPriceResponse[];

  approvedManufacturers:
    ApprovedManufacturerResponse[];
}
export interface ShelfLifeHistoryAttachmentResponse {
  attachmentId: string;
  fileName: string;
  notes: string | null;
}


export interface ShelfLifeHistoryResponse {
  id: string;
  versionNumber: number;
  isCurrent: boolean;
  numberOfMonths: number;
  approvalDate: string;
  attachments: ShelfLifeHistoryAttachmentResponse[];
}

// ==========================================================
// Command inputs
// ==========================================================

export interface DatedAttachmentInput {
  date: string;
  attachmentId: string;
  fileName: string;
  notes: string | null;
}

export interface FulfillmentDeadlineInput {
  description: string;
  deadline: string;
  notes: string | null;
}


// ==========================================================
// Upload response
// ==========================================================

export interface AttachmentUploadResponse {
  attachmentId: string;
  fileName: string;
  contentType: string;
  length: number;
}