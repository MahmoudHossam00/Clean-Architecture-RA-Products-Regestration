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


// =========================================================
// Requirement
// =========================================================

export interface ApprovalRequirementResponse {
  id: string;

  name: string;

  deadline: string;

  responsibleDepartment: LookupResponse;

  requirementAttachment:
    AttachmentResponse | null;

  approval:
    DatedAttachmentResponse | null;

  notes?: string | null;
}


// =========================================================
// Miscellaneous Approval
// =========================================================

export interface MiscellaneousApprovalResponse {
  id: string;

  productId: string;

  fileType: LookupResponse;

  initiator: string;

  submissionDate: string;

  approvalDate:
    string | null;

  approvalExpiryDate:
    string | null;

  approvalRequirements:
    ApprovalRequirementResponse[];
}


// =========================================================
// Create
// =========================================================

export interface CreateMiscellaneousApprovalInput {
  productId: string;

  fileTypeId: string;

  initiator: string;

  submissionDate: string;
}


// =========================================================
// Update Details
// =========================================================

export interface UpdateMiscellaneousApprovalDetailsInput {
  initiator: string;

  submissionDate: string;
}


// =========================================================
// Set Main Approval
// =========================================================

export interface SetMiscellaneousApprovalInput {
  approvalDate: string;

  approvalExpiryDate: string;
}


// =========================================================
// Add Requirement
// =========================================================

export interface AddApprovalRequirementInput {
  name: string;

  deadline: string;

  responsibleDepartmentId: string;

  requirementAttachment:
    AttachmentReferenceInput | null;

  notes?: string | null;
}


// =========================================================
// Update Requirement
// =========================================================

export interface UpdateApprovalRequirementInput {
  name: string;

  deadline: string;

  responsibleDepartmentId: string;

  notes?: string | null;
}