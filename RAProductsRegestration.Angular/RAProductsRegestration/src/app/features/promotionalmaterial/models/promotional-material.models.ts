export interface LookupResponse {
  id: string;
  name: string;
}


export interface AttachmentResponse {
  attachmentId: string;
  fileName: string;
  notes?: string | null;
}


export interface DatedAttachmentResponse {
  date: string;
  attachment: AttachmentResponse;
}


export interface AttachmentReferenceInput {
  attachmentId: string;
  fileName: string;
  notes?: string | null;
}


export interface DatedAttachmentInput {
  date: string;
  attachmentId: string;
  fileName: string;
  notes?: string | null;
}


export interface PromotionalMaterialResponse {
  id: string;
  productId: string;

  type: LookupResponse;

  materialFile: AttachmentResponse;

  approval: DatedAttachmentResponse | null;

  approvalExpiryDate: string | null;
}


export interface CreatePromotionalMaterialInput {
  productId: string;

  promotionalMaterialTypeId: string;

  materialFile: AttachmentReferenceInput;
}


export interface ReplacePromotionalMaterialFileInput {
  materialFile: AttachmentReferenceInput;
}


export interface SetPromotionalMaterialApprovalInput {
  approval: DatedAttachmentInput;

  approvalExpiryDate: string;
}