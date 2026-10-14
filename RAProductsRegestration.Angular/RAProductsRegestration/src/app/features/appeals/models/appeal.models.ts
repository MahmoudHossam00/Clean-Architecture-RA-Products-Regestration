import {
    AttachmentReferenceInput,
  AttachmentResponse,
  DatedAttachmentResponse,
  LookupResponse
} from '../../product-basic-data/models/product-basic-data.models';






// =========================================================
// Appeal Requirement
// =========================================================

export interface AppealRequirementResponse {

  id: string;

  name: string;

  deadline: string;

  responsibleDepartment:
    LookupResponse;

  requirementAttachment:
    AttachmentResponse | null;

  approval:
    DatedAttachmentResponse | null;

  notes:
    string | null;
}


// =========================================================
// Appeal
// =========================================================

export interface AppealResponse {

  id: string;

  productId: string;

  initiator: string;

  appealType:
    LookupResponse;

  appealDate:
    string;

  approval:
    DatedAttachmentResponse | null;

  notes:
    string | null;

  requirements:
    AppealRequirementResponse[];
}


// =========================================================
// Create
// =========================================================

export interface CreateAppealInput {

  productId: string;

  initiator: string;

  appealTypeId: string;

  appealDate: string;

  notes?: string | null;
}


// =========================================================
// Update
// =========================================================

export interface UpdateAppealDetailsInput {

  initiator: string;

  appealTypeId: string;

  appealDate: string;

  notes?: string | null;
}


// =========================================================
// Requirement Inputs
// =========================================================

export interface AddAppealRequirementInput {

  name: string;

  deadline: string;

  responsibleDepartmentId:
    string;

  requirementAttachment:
    AttachmentReferenceInput | null;

  notes?: string | null;
}


export interface UpdateAppealRequirementInput {

  name: string;

  deadline: string;

  responsibleDepartmentId:
    string;

  notes?: string | null;
}