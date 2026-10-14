import {
  AttachmentResponse
} from '../../product-basic-data/models/product-basic-data.models';


export interface LookupResponse {

  id: string;

  name: string;

}

export interface AttachmentReferenceInput {

  attachmentId: string;

  fileName: string;

  notes?: string | null;

}

export interface AttachmentReferenceInput {
  attachmentId: string;
  fileName: string;
  notes?: string | null;
}

export interface LicenseRequirementResponse {
  id: string;
  name: string;
  deadline: string;
  responsibleDepartment: LookupResponse;
  requirementAttachment: AttachmentResponse | null;
  approval: DatedAttachmentResponse | null;
  notes: string | null;
}

export interface AddLicenseRequirementInput {
  name: string;
  deadline: string;
  responsibleDepartmentId: string;
  requirementAttachment: AttachmentReferenceInput | null;
  notes?: string | null;
}

export interface UpdateLicenseRequirementInput {
  name: string;
  deadline: string;
  responsibleDepartmentId: string;
  notes?: string | null;
}
export interface LicenseRequirementResponse {

  id: string;

  name: string;

  deadline: string;

  responsibleDepartment: LookupResponse;

  requirementAttachment:
    AttachmentResponse | null;

  approval:
    DatedAttachmentResponse | null;

  notes:
    string | null;

}


export interface AddLicenseRequirementInput {

  name: string;

  deadline: string;

  responsibleDepartmentId: string;

  requirementAttachment:
    AttachmentReferenceInput | null;

  notes?: string | null;

}
export interface DatedAttachmentResponse {

  date: string;

  attachment: AttachmentResponse;

}

export interface ProductLicenseResponse {

  id: string;

  productId: string;

  licenseCategory:
    LookupResponse;

  issueDate:
    string;

  validityDate:
    string;

  marketingType:
    LookupResponse;

  registrationNumberYear:
    string;

  lastApprovedLeaflet:
    DatedAttachmentResponse | null;

  lastApprovedLayout:
    DatedAttachmentResponse | null;

  requirements:
    LicenseRequirementResponse[];
}


/* =========================================================
   Create
   ========================================================= */

export interface CreateProductLicenseInput {

  productId: string;

  licenseCategoryId: string;

  issueDate: string;

  validityDate: string;

  marketingTypeId: string;

  registrationNumberYear: string;

}


/* =========================================================
   Dated Attachment
   ========================================================= */

export interface DatedAttachmentInput {

  date: string;

  attachmentId: string;

  fileName: string;

  notes?: string | null;

}