import {
  AttachmentResponse
} from '../../product-basic-data/models/product-basic-data.models';

import {
  AttachmentReferenceInput,
  DatedAttachmentInput,
  DatedAttachmentResponse,
  LookupResponse
} from '../../ProductLicenses/models/product-license.models';



// =========================================================
// Variation Requirement
// =========================================================

export interface VariationRequirementResponse {

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
// Variation
// =========================================================

export interface VariationResponse {

  id: string;

  productId: string;

  initiator: string;

  cause: string;

  deadline: string;

  deadlineNote:
    string | null;

  approval:
    DatedAttachmentResponse | null;

  requirements:
    VariationRequirementResponse[];
}


// =========================================================
// Create Variation
// =========================================================

export interface CreateVariationInput {

  productId: string;

  initiator: string;

  cause: string;

  deadline: string;

  deadlineNote?:
    string | null;
}


// =========================================================
// Update Variation
// =========================================================

export interface UpdateVariationDetailsInput {

  initiator: string;

  cause: string;
}


export interface UpdateVariationDeadlineInput {

  deadline: string;

  deadlineNote?:
    string | null;
}


// =========================================================
// Requirements
// =========================================================

export interface AddVariationRequirementInput {

  name: string;

  deadline: string;

  responsibleDepartmentId:
    string;

  requirementAttachment:
    AttachmentReferenceInput | null;

  notes?:
    string | null;
}


export interface UpdateVariationRequirementInput {

  name: string;

  deadline: string;

  responsibleDepartmentId:
    string;

  notes?:
    string | null;
}