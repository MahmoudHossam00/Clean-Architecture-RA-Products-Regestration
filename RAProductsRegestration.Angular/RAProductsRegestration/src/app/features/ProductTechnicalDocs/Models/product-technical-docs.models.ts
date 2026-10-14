import { AttachmentReferenceInput, AttachmentResponse } from "../../product-basic-data/models/product-basic-data.models";


export interface DatedApprovalDocumentResponse {

  approvalDate: string;

  attachment: AttachmentResponse;
}


export interface DatedApprovalDocumentInput {

  approvalDate: string;

  attachment: AttachmentReferenceInput;
}


export interface ProductTechnicalDocsResponse {

  id: string;

  productId: string;


  // =========================================================
  // Specifications
  // =========================================================

  currentApprovedSpecifications:
    DatedApprovalDocumentResponse | null;

  approvedSpecificationsHistory:
    DatedApprovalDocumentResponse[];


  // =========================================================
  // Composition
  // =========================================================

  currentApprovedComposition:
    DatedApprovalDocumentResponse | null;

  approvedCompositionHistory:
    DatedApprovalDocumentResponse[];


  // =========================================================
  // MOA + Validation
  // =========================================================

  currentApprovedMoaValidation:
    DatedApprovalDocumentResponse | null;

  approvedMoaValidationHistory:
    DatedApprovalDocumentResponse[];
}