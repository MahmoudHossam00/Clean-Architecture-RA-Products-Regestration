import {
  Component,
  inject,
  OnDestroy,
  OnInit
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormsModule
} from '@angular/forms';

import {
  ActivatedRoute
} from '@angular/router';

import {
  DomSanitizer,
  SafeResourceUrl
} from '@angular/platform-browser';

import {
  Subject,
  debounceTime,
  distinctUntilChanged,
  switchMap
} from 'rxjs';

import {
  MiscellaneousApprovalService
} from '../services/miscellaneous-approval.service';

import {
  MiscellaneousApprovalFileTypeService
} from '../services/miscellaneous-approval-file-type.service';

import {
  DepartmentService
} from '../../ProductLicenses/services/department.service';

import {
  AttachmentService
} from '../../../core/services/attachment.service';

import {
  AddApprovalRequirementInput,
  ApprovalRequirementResponse,
  CreateMiscellaneousApprovalInput,
  DatedAttachmentInput,
  LookupResponse,
  MiscellaneousApprovalResponse,
  UpdateApprovalRequirementInput,
  UpdateMiscellaneousApprovalDetailsInput
} from '../models/miscellaneous-approval.models';

import {
  CreatableLookupComponent
} from '../../../core/components/creatable-lookup/creatable-lookup.component';


export type RequirementFormMode =
  | 'add'
  | 'edit';


export type MiscApprovalStatus =
  | 'Pending'
  | 'Approved'
  | 'Expired';


@Component({
  selector:
    'app-product-miscellaneous-approvals',

  standalone:
    true,

  imports: [
    CommonModule,
    FormsModule,
    CreatableLookupComponent
  ],

  templateUrl:
    './product-miscellaneous-approvals.component.html',

  styleUrl:
    './product-miscellaneous-approvals.component.css'
})
export class ProductMiscellaneousApprovalsComponent
  implements OnInit, OnDestroy {

  // =========================================================
  // Dependencies
  // =========================================================

  private readonly route =
    inject(ActivatedRoute);


  private readonly service =
    inject(MiscellaneousApprovalService);


  private readonly fileTypeService =
    inject(MiscellaneousApprovalFileTypeService);


  private readonly departmentService =
    inject(DepartmentService);


  private readonly attachmentService =
    inject(AttachmentService);


  private readonly sanitizer =
    inject(DomSanitizer);


  // =========================================================
  // Product
  // =========================================================

  readonly productId =
    this.route.parent?.snapshot
      .paramMap
      .get('productId') ?? '';


  // =========================================================
  // Data
  // =========================================================

  approvals:
    MiscellaneousApprovalResponse[] =
      [];


  loading =
    false;


  saving =
    false;


  errorMessage =
    '';


  successMessage =
    '';


  // =========================================================
  // Main Expand / Collapse
  // =========================================================

  approvalsSectionExpanded =
    true;


  expandedApprovalIds =
    new Set<string>();


  expandedRequirementSectionIds =
    new Set<string>();


  expandedRequirementIds =
    new Set<string>();


  // =========================================================
  // File Types
  // =========================================================

  fileTypes:
    LookupResponse[] =
      [];


  loadingFileTypes =
    false;


  creatingFileType =
    false;


  fileTypeSelections:
    Record<
      string,
      string | null
    > = {};


  // =========================================================
  // Create Approval
  // =========================================================

  createModalOpen =
    false;


  createFileTypeId:
    string | null =
      null;


  createInitiator =
    '';


  createSubmissionDate =
    '';


  // =========================================================
  // Edit Details
  // =========================================================

  editDetailsModalOpen =
    false;


  editDetailsTarget:
    MiscellaneousApprovalResponse | null =
      null;


  editInitiator =
    '';


  editSubmissionDate =
    '';


  // =========================================================
  // Main Approval Modal
  // =========================================================

  mainApprovalModalOpen =
    false;


  mainApprovalTarget:
    MiscellaneousApprovalResponse | null =
      null;


  mainApprovalDate =
    '';


  mainApprovalExpiryDate =
    '';


  // =========================================================
  // Departments
  // =========================================================

  departments:
    LookupResponse[] =
      [];


  departmentSearch =
    '';


  loadingDepartments =
    false;


  departmentDropdownOpen =
    false;


  private readonly departmentSearch$ =
    new Subject<string>();


  // =========================================================
  // Requirement Add / Edit
  // =========================================================

  requirementModalOpen =
    false;


  requirementFormMode:
    RequirementFormMode =
      'add';


  requirementParent:
    MiscellaneousApprovalResponse | null =
      null;


  editingRequirementId:
    string | null =
      null;


  requirementName =
    '';


  requirementDeadline =
    '';


  requirementDepartmentId:
    string | null =
      null;


  requirementDepartmentName =
    '';


  requirementNotes =
    '';


  requirementFile:
    File | null =
      null;


  // =========================================================
  // Requirement Attachment
  // =========================================================

  requirementAttachmentModalOpen =
    false;


  requirementAttachmentParent:
    MiscellaneousApprovalResponse | null =
      null;


  requirementAttachmentTarget:
    ApprovalRequirementResponse | null =
      null;


  requirementAttachmentFile:
    File | null =
      null;


  requirementAttachmentNotes =
    '';


  // =========================================================
  // Requirement Approval
  // =========================================================

  requirementApprovalModalOpen =
    false;


  requirementApprovalParent:
    MiscellaneousApprovalResponse | null =
      null;


  requirementApprovalTarget:
    ApprovalRequirementResponse | null =
      null;


  requirementApprovalDate =
    '';


  requirementApprovalFile:
    File | null =
      null;


  requirementApprovalNotes =
    '';


  // =========================================================
  // PDF
  // =========================================================

  pdfViewerOpen =
    false;


  pdfViewerLoading =
    false;


  pdfViewerFileName =
    '';


  pdfViewerUrl:
    SafeResourceUrl | null =
      null;


  private pdfObjectUrl:
    string | null =
      null;


  // =========================================================
  // Lifecycle
  // =========================================================

  ngOnInit(): void {

    this.loadFileTypes();


    this.load();


    this.departmentSearch$
      .pipe(
        debounceTime(250),
        distinctUntilChanged()
      )
      .subscribe(
        search => {

          this.loadDepartments(
            search
          );
        }
      );
  }


  ngOnDestroy(): void {

    this.departmentSearch$
      .complete();


    this.closePdfViewer();
  }


  // =========================================================
  // Counts
  // =========================================================

  get approvedCount():
    number {

    return this.approvals
      .filter(
        x =>
          x.approvalDate &&
          !this.isExpired(
            x.approvalExpiryDate
          )
      )
      .length;
  }


  get pendingCount():
    number {

    return this.approvals
      .filter(
        x =>
          !x.approvalDate
      )
      .length;
  }


  get expiredCount():
    number {

    return this.approvals
      .filter(
        x =>
          !!x.approvalDate &&
          this.isExpired(
            x.approvalExpiryDate
          )
      )
      .length;
  }


  get allApprovalsExpanded():
    boolean {

    return (
      this.approvals.length > 0 &&
      this.approvals.every(
        approval =>
          this.expandedApprovalIds
            .has(
              approval.id
            )
      )
    );
  }


  // =========================================================
  // Status
  // =========================================================

  getStatus(
    approval:
      MiscellaneousApprovalResponse
  ): MiscApprovalStatus {

    if (!approval.approvalDate) {

      return 'Pending';
    }


    if (
      this.isExpired(
        approval.approvalExpiryDate
      )
    ) {

      return 'Expired';
    }


    return 'Approved';
  }


  isExpired(
    expiryDate:
      string | null
  ): boolean {

    if (!expiryDate) {
      return false;
    }


    return (
      expiryDate <
      this.getToday()
    );
  }


  private getToday():
    string {

    const now =
      new Date();


    const year =
      now.getFullYear();


    const month =
      String(
        now.getMonth() + 1
      ).padStart(
        2,
        '0'
      );


    const day =
      String(
        now.getDate()
      ).padStart(
        2,
        '0'
      );


    return `${year}-${month}-${day}`;
  }


  // =========================================================
  // Requirement Counts
  // =========================================================

  approvedRequirementCount(
    approval:
      MiscellaneousApprovalResponse
  ): number {

    return approval
      .approvalRequirements
      .filter(
        requirement =>
          !!requirement.approval
      )
      .length;
  }


  pendingRequirementCount(
    approval:
      MiscellaneousApprovalResponse
  ): number {

    return (
      approval.approvalRequirements.length -
      this.approvedRequirementCount(
        approval
      )
    );
  }


  requirementFileCount(
    approval:
      MiscellaneousApprovalResponse
  ): number {

    return approval
      .approvalRequirements
      .filter(
        requirement =>
          !!requirement
            .requirementAttachment
      )
      .length;
  }


  // =========================================================
  // Expand Main Section
  // =========================================================

  toggleApprovalsSection():
    void {

    this.approvalsSectionExpanded =
      !this.approvalsSectionExpanded;
  }


  // =========================================================
  // Expand Approval
  // =========================================================

  isApprovalExpanded(
    approvalId: string
  ): boolean {

    return this.expandedApprovalIds
      .has(
        approvalId
      );
  }


  toggleApproval(
    approvalId: string
  ): void {

    if (
      this.expandedApprovalIds
        .has(
          approvalId
        )
    ) {

      this.expandedApprovalIds
        .delete(
          approvalId
        );

      return;
    }


    this.expandedApprovalIds
      .add(
        approvalId
      );
  }


  toggleAllApprovals():
    void {

    if (!this.approvals.length) {
      return;
    }


    if (this.allApprovalsExpanded) {

      this.expandedApprovalIds
        .clear();

      return;
    }


    this.approvals
      .forEach(
        approval => {

          this.expandedApprovalIds
            .add(
              approval.id
            );
        }
      );
  }


  // =========================================================
  // Requirement Section Expand
  // =========================================================

  isRequirementSectionExpanded(
    approvalId: string
  ): boolean {

    return this.expandedRequirementSectionIds
      .has(
        approvalId
      );
  }


  toggleRequirementSection(
    approvalId: string
  ): void {

    if (
      this.expandedRequirementSectionIds
        .has(
          approvalId
        )
    ) {

      this.expandedRequirementSectionIds
        .delete(
          approvalId
        );

      return;
    }


    this.expandedRequirementSectionIds
      .add(
        approvalId
      );
  }


  // =========================================================
  // Individual Requirement Expand
  // =========================================================

  isRequirementExpanded(
    requirementId: string
  ): boolean {

    return this.expandedRequirementIds
      .has(
        requirementId
      );
  }


  toggleRequirement(
    requirementId: string
  ): void {

    if (
      this.expandedRequirementIds
        .has(
          requirementId
        )
    ) {

      this.expandedRequirementIds
        .delete(
          requirementId
        );

      return;
    }


    this.expandedRequirementIds
      .add(
        requirementId
      );
  }


  allRequirementsExpanded(
    approval:
      MiscellaneousApprovalResponse
  ): boolean {

    return (
      approval.approvalRequirements.length > 0 &&
      approval.approvalRequirements.every(
        requirement =>
          this.expandedRequirementIds
            .has(
              requirement.id
            )
      )
    );
  }


  toggleAllRequirements(
    approval:
      MiscellaneousApprovalResponse
  ): void {

    if (
      !approval.approvalRequirements.length
    ) {
      return;
    }


    if (
      this.allRequirementsExpanded(
        approval
      )
    ) {

      approval.approvalRequirements
        .forEach(
          requirement => {

            this.expandedRequirementIds
              .delete(
                requirement.id
              );
          }
        );

      return;
    }


    approval.approvalRequirements
      .forEach(
        requirement => {

          this.expandedRequirementIds
            .add(
              requirement.id
            );
        }
      );
  }


  // =========================================================
  // Load
  // =========================================================

  load(): void {

    if (!this.productId) {

      this.errorMessage =
        'Product ID was not found.';

      return;
    }


    this.loading =
      true;


    this.errorMessage =
      '';


    this.service
      .getByProductId(
        this.productId
      )
      .subscribe({

        next: approvals => {

          this.approvals =
            (approvals ?? [])
              .map(
                approval => ({
                  ...approval,

                  approvalRequirements:
                    approval.approvalRequirements ??
                    []
                })
              );


          this.fileTypeSelections =
            {};


          this.approvals
            .forEach(
              approval => {

                this.fileTypeSelections[
                  approval.id
                ] =
                  approval.fileType.id;
              }
            );


          this.removeInvalidExpandState();


          this.loading =
            false;
        },


        error: error => {

          console.error(
            'Unable to load miscellaneous approvals',
            error
          );


          this.loading =
            false;


          this.errorMessage =
            'Unable to load miscellaneous approvals.';
        }

      });
  }


  private removeInvalidExpandState():
    void {

    const approvalIds =
      new Set(
        this.approvals.map(
          x => x.id
        )
      );


    for (
      const id
      of this.expandedApprovalIds
    ) {

      if (!approvalIds.has(id)) {

        this.expandedApprovalIds
          .delete(id);
      }
    }


    for (
      const id
      of this.expandedRequirementSectionIds
    ) {

      if (!approvalIds.has(id)) {

        this.expandedRequirementSectionIds
          .delete(id);
      }
    }


    const requirementIds =
      new Set(
        this.approvals
          .flatMap(
            x =>
              x.approvalRequirements
          )
          .map(
            x => x.id
          )
      );


    for (
      const id
      of this.expandedRequirementIds
    ) {

      if (!requirementIds.has(id)) {

        this.expandedRequirementIds
          .delete(id);
      }
    }
  }


  // =========================================================
  // File Types
  // =========================================================

  loadFileTypes():
    void {

    this.loadingFileTypes =
      true;


    this.fileTypeService
      .getAll('')
      .subscribe({

        next: items => {

          this.fileTypes =
            items;


          this.loadingFileTypes =
            false;
        },


        error: error => {

          console.error(
            'Unable to load miscellaneous approval file types',
            error
          );


          this.fileTypes =
            [];


          this.loadingFileTypes =
            false;
        }

      });
  }


  createFileType(
    name: string,
    approvalId?: string
  ): void {

    const normalized =
      name.trim();


    if (
      !normalized ||
      this.creatingFileType
    ) {
      return;
    }


    this.creatingFileType =
      true;


    this.clearMessages();


    this.fileTypeService
      .create(
        normalized
      )
      .subscribe({

        next: () => {

          this.fileTypeService
            .getAll('')
            .subscribe({

              next: items => {

                this.fileTypes =
                  items;


                this.creatingFileType =
                  false;


                const created =
                  items.find(
                    item =>
                      item.name
                        .trim()
                        .toLowerCase()
                      ===
                      normalized
                        .toLowerCase()
                  );


                if (!created) {

                  this.errorMessage =
                    'File type was created but could not be selected.';

                  return;
                }


                if (!approvalId) {

                  this.createFileTypeId =
                    created.id;

                  return;
                }


                this.fileTypeSelections[
                  approvalId
                ] =
                  created.id;


                this.saveFileType(
                  approvalId
                );
              },


              error: error => {

                console.error(
                  'Unable to reload file types',
                  error
                );


                this.creatingFileType =
                  false;


                this.errorMessage =
                  'File type was created but could not be reloaded.';
              }

            });
        },


        error: error => {

          console.error(
            'Unable to create file type',
            error
          );


          this.creatingFileType =
            false;


          this.errorMessage =
            `Unable to create file type "${normalized}".`;
        }

      });
  }


  saveFileType(
    approvalId: string
  ): void {

    const approval =
      this.approvals.find(
        x =>
          x.id ===
          approvalId
      );


    const fileTypeId =
      this.fileTypeSelections[
        approvalId
      ];


    if (
      !approval ||
      !fileTypeId ||
      fileTypeId ===
        approval.fileType.id ||
      this.saving
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .updateFileType(
        approvalId,
        fileTypeId
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedApprovalIds
            .add(
              approvalId
            );


          this.successMessage =
            'File type updated successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to update file type',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to update file type.';
        }

      });
  }


  // =========================================================
  // Create Miscellaneous Approval
  // =========================================================

  openCreateModal():
    void {

    if (this.saving) {
      return;
    }


    this.resetCreateForm();


    this.clearMessages();


    this.createModalOpen =
      true;


    this.approvalsSectionExpanded =
      true;
  }


  closeCreateModal():
    void {

    if (this.saving) {
      return;
    }


    this.createModalOpen =
      false;


    this.resetCreateForm();
  }


  createApproval():
    void {

    const initiator =
      this.createInitiator.trim();


    if (!this.createFileTypeId) {

      this.errorMessage =
        'File type is required.';

      return;
    }


    if (!initiator) {

      this.errorMessage =
        'Initiator is required.';

      return;
    }


    if (!this.createSubmissionDate) {

      this.errorMessage =
        'Submission date is required.';

      return;
    }


    const input:
      CreateMiscellaneousApprovalInput = {

      productId:
        this.productId,

      fileTypeId:
        this.createFileTypeId,

      initiator,

      submissionDate:
        this.createSubmissionDate
    };


    this.saving =
      true;


    this.clearMessages();


    this.service
      .create(
        input
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.createModalOpen =
            false;


          this.resetCreateForm();


          this.successMessage =
            'Miscellaneous approval created successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to create miscellaneous approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to create miscellaneous approval.';
        }

      });
  }


  // =========================================================
  // Edit Details
  // =========================================================

  openEditDetailsModal(
    approval:
      MiscellaneousApprovalResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.editDetailsTarget =
      approval;


    this.editInitiator =
      approval.initiator;


    this.editSubmissionDate =
      approval.submissionDate;


    this.clearMessages();


    this.editDetailsModalOpen =
      true;
  }


  closeEditDetailsModal():
    void {

    if (this.saving) {
      return;
    }


    this.editDetailsModalOpen =
      false;


    this.resetEditDetailsForm();
  }


  saveDetails():
    void {

    const target =
      this.editDetailsTarget;


    if (!target) {
      return;
    }


    const initiator =
      this.editInitiator.trim();


    if (!initiator) {

      this.errorMessage =
        'Initiator is required.';

      return;
    }


    if (!this.editSubmissionDate) {

      this.errorMessage =
        'Submission date is required.';

      return;
    }


    const input:
      UpdateMiscellaneousApprovalDetailsInput = {

      initiator,

      submissionDate:
        this.editSubmissionDate
    };


    this.saving =
      true;


    this.clearMessages();


    this.service
      .updateDetails(
        target.id,
        input
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.editDetailsModalOpen =
            false;


          this.resetEditDetailsForm();


          this.expandedApprovalIds
            .add(
              target.id
            );


          this.successMessage =
            'Miscellaneous approval details updated successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to update miscellaneous approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to update miscellaneous approval details.';
        }

      });
  }


  // =========================================================
  // Main Approval
  // =========================================================

  openMainApprovalModal(
    approval:
      MiscellaneousApprovalResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.mainApprovalTarget =
      approval;


    this.mainApprovalDate =
      approval.approvalDate ??
      '';


    this.mainApprovalExpiryDate =
      approval.approvalExpiryDate ??
      '';


    this.clearMessages();


    this.mainApprovalModalOpen =
      true;
  }


  closeMainApprovalModal():
    void {

    if (this.saving) {
      return;
    }


    this.mainApprovalModalOpen =
      false;


    this.resetMainApprovalForm();
  }


  saveMainApproval():
    void {

    const target =
      this.mainApprovalTarget;


    if (!target) {
      return;
    }


    if (!this.mainApprovalDate) {

      this.errorMessage =
        'Approval date is required.';

      return;
    }


    if (!this.mainApprovalExpiryDate) {

      this.errorMessage =
        'Approval expiry date is required.';

      return;
    }


    if (
      this.mainApprovalDate <
      target.submissionDate
    ) {

      this.errorMessage =
        'Approval date cannot be before submission date.';

      return;
    }


    if (
      this.mainApprovalExpiryDate <
      this.mainApprovalDate
    ) {

      this.errorMessage =
        'Approval expiry date cannot be before approval date.';

      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .setApproval(
        target.id,
        {
          approvalDate:
            this.mainApprovalDate,

          approvalExpiryDate:
            this.mainApprovalExpiryDate
        }
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.mainApprovalModalOpen =
            false;


          this.resetMainApprovalForm();


          this.expandedApprovalIds
            .add(
              target.id
            );


          this.successMessage =
            'Approval saved successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to save approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to save approval.';
        }

      });
  }


  removeMainApproval(
    approval:
      MiscellaneousApprovalResponse
  ): void {

    if (
      !approval.approvalDate ||
      this.saving
    ) {
      return;
    }


    if (
      !window.confirm(
        `Remove approval from "${approval.fileType.name}"?`
      )
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeApproval(
        approval.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedApprovalIds
            .add(
              approval.id
            );


          this.successMessage =
            'Approval removed successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to remove approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to remove approval.';
        }

      });
  }


  // =========================================================
  // Delete Parent
  // =========================================================

  deleteApproval(
    approval:
      MiscellaneousApprovalResponse
  ): void {

    if (this.saving) {
      return;
    }


    if (
      !window.confirm(
        `Delete miscellaneous approval "${approval.fileType.name}"?\n\nAll approval requirements will also be deleted.`
      )
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .remove(
        approval.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedApprovalIds
            .delete(
              approval.id
            );


          this.expandedRequirementSectionIds
            .delete(
              approval.id
            );


          this.successMessage =
            'Miscellaneous approval deleted successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to delete miscellaneous approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to delete miscellaneous approval.';
        }

      });
  }


  // =========================================================
  // Departments
  // =========================================================

  loadDepartments(
    search = ''
  ): void {

    this.loadingDepartments =
      true;


    this.departmentService
      .getAll(
        search
      )
      .subscribe({

        next: items => {

          this.departments =
            items;


          this.loadingDepartments =
            false;


          this.departmentDropdownOpen =
            true;
        },


        error: error => {

          console.error(
            'Unable to load departments',
            error
          );


          this.departments =
            [];


          this.loadingDepartments =
            false;
        }

      });
  }


  onDepartmentSearch(
    value: string
  ): void {

    this.departmentSearch =
      value;


    const typed =
      value
        .trim()
        .toLowerCase();


    const selected =
      this.requirementDepartmentName
        .trim()
        .toLowerCase();


    if (typed !== selected) {

      this.requirementDepartmentId =
        null;


      this.requirementDepartmentName =
        '';
    }


    this.departmentDropdownOpen =
      true;


    this.departmentSearch$
      .next(
        value
      );
  }


  openDepartmentDropdown():
    void {

    if (this.saving) {
      return;
    }


    this.departmentDropdownOpen =
      true;


    this.loadDepartments(
      this.departmentSearch
    );
  }


  selectDepartment(
    department:
      LookupResponse
  ): void {

    this.requirementDepartmentId =
      department.id;


    this.requirementDepartmentName =
      department.name;


    this.departmentSearch =
      department.name;


    this.departmentDropdownOpen =
      false;
  }


  // =========================================================
  // Add Requirement
  // =========================================================

  openAddRequirementModal(
    approval:
      MiscellaneousApprovalResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.resetRequirementForm();


    this.requirementParent =
      approval;


    this.requirementFormMode =
      'add';


    this.requirementModalOpen =
      true;


    this.expandedApprovalIds
      .add(
        approval.id
      );


    this.expandedRequirementSectionIds
      .add(
        approval.id
      );


    this.clearMessages();


    this.loadDepartments('');
  }


  // =========================================================
  // Edit Requirement
  // =========================================================

  openEditRequirementModal(
    approval:
      MiscellaneousApprovalResponse,
    requirement:
      ApprovalRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.resetRequirementForm();


    this.requirementParent =
      approval;


    this.requirementFormMode =
      'edit';


    this.editingRequirementId =
      requirement.id;


    this.requirementName =
      requirement.name;


    this.requirementDeadline =
      requirement.deadline;


    this.requirementDepartmentId =
      requirement
        .responsibleDepartment
        .id;


    this.requirementDepartmentName =
      requirement
        .responsibleDepartment
        .name;


    this.departmentSearch =
      requirement
        .responsibleDepartment
        .name;


    this.requirementNotes =
      requirement.notes ??
      '';


    this.requirementModalOpen =
      true;


    this.clearMessages();


    this.loadDepartments(
      requirement
        .responsibleDepartment
        .name
    );
  }


  closeRequirementModal():
    void {

    if (this.saving) {
      return;
    }


    this.requirementModalOpen =
      false;


    this.resetRequirementForm();
  }


  onRequirementFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.requirementFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  removeSelectedRequirementFile():
    void {

    this.requirementFile =
      null;
  }


  // =========================================================
  // Save Requirement
  // =========================================================

  saveRequirement():
    void {

    const parent =
      this.requirementParent;


    if (!parent) {
      return;
    }


    const name =
      this.requirementName.trim();


    const notes =
      this.requirementNotes.trim();


    if (!name) {

      this.errorMessage =
        'Requirement name is required.';

      return;
    }


    if (!this.requirementDeadline) {

      this.errorMessage =
        'Requirement deadline is required.';

      return;
    }


    if (!this.requirementDepartmentId) {

      this.errorMessage =
        'Responsible department is required.';

      return;
    }


    // =======================================================
    // Edit
    // =======================================================

    if (
      this.requirementFormMode ===
      'edit'
    ) {

      if (!this.editingRequirementId) {
        return;
      }


      const requirementId =
        this.editingRequirementId;


      const input:
        UpdateApprovalRequirementInput = {

        name,

        deadline:
          this.requirementDeadline,

        responsibleDepartmentId:
          this.requirementDepartmentId,

        notes:
          notes || null
      };


      this.saving =
        true;


      this.clearMessages();


      this.service
        .updateRequirement(
          parent.id,
          requirementId,
          input
        )
        .subscribe({

          next: () => {

            this.saving =
              false;


            this.requirementModalOpen =
              false;


            this.resetRequirementForm();


            this.expandedApprovalIds
              .add(
                parent.id
              );


            this.expandedRequirementSectionIds
              .add(
                parent.id
              );


            this.expandedRequirementIds
              .add(
                requirementId
              );


            this.successMessage =
              'Approval requirement updated successfully.';


            this.load();
          },


          error: error => {

            console.error(
              'Unable to update requirement',
              error
            );


            this.saving =
              false;


            this.errorMessage =
              'Unable to update approval requirement.';
          }

        });


      return;
    }


    // =======================================================
    // Add Without File
    // =======================================================

    if (!this.requirementFile) {

      const input:
        AddApprovalRequirementInput = {

        name,

        deadline:
          this.requirementDeadline,

        responsibleDepartmentId:
          this.requirementDepartmentId,

        requirementAttachment:
          null,

        notes:
          notes || null
      };


      this.saving =
        true;


      this.clearMessages();


      this.service
        .addRequirement(
          parent.id,
          input
        )
        .subscribe({

          next: () => {

            this.requirementSaved(
              parent.id
            );
          },


          error: error => {

            this.requirementSaveFailed(
              error
            );
          }

        });


      return;
    }


    // =======================================================
    // Add With File
    // =======================================================

    this.saving =
      true;


    this.clearMessages();


    this.attachmentService
      .upload(
        this.requirementFile
      )
      .pipe(

        switchMap(
          uploaded => {

            const input:
              AddApprovalRequirementInput = {

              name,

              deadline:
                this.requirementDeadline,

              responsibleDepartmentId:
                this.requirementDepartmentId!,

              requirementAttachment: {

                attachmentId:
                  uploaded.attachmentId,

                fileName:
                  uploaded.fileName,

                notes:
                  null
              },

              notes:
                notes || null
            };


            return this.service
              .addRequirement(
                parent.id,
                input
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementSaved(
            parent.id
          );
        },


        error: error => {

          this.requirementSaveFailed(
            error
          );
        }

      });
  }


  private requirementSaved(
    approvalId: string
  ): void {

    this.saving =
      false;


    this.requirementModalOpen =
      false;


    this.resetRequirementForm();


    this.expandedApprovalIds
      .add(
        approvalId
      );


    this.expandedRequirementSectionIds
      .add(
        approvalId
      );


    this.successMessage =
      'Approval requirement added successfully.';


    this.load();
  }


  private requirementSaveFailed(
    error: unknown
  ): void {

    console.error(
      'Unable to save requirement',
      error
    );


    this.saving =
      false;


    this.errorMessage =
      'Unable to save approval requirement.';
  }


  // =========================================================
  // Requirement Attachment
  // =========================================================

  openRequirementAttachmentModal(
    approval:
      MiscellaneousApprovalResponse,
    requirement:
      ApprovalRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.requirementAttachmentParent =
      approval;


    this.requirementAttachmentTarget =
      requirement;


    this.requirementAttachmentFile =
      null;


    this.requirementAttachmentNotes =
      requirement
        .requirementAttachment
        ?.notes ??
      '';


    this.clearMessages();


    this.requirementAttachmentModalOpen =
      true;
  }


  closeRequirementAttachmentModal():
    void {

    if (this.saving) {
      return;
    }


    this.requirementAttachmentModalOpen =
      false;


    this.resetRequirementAttachmentForm();
  }


  onRequirementAttachmentFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.requirementAttachmentFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  saveRequirementAttachment():
    void {

    const parent =
      this.requirementAttachmentParent;


    const requirement =
      this.requirementAttachmentTarget;


    const file =
      this.requirementAttachmentFile;


    if (
      !parent ||
      !requirement ||
      !file
    ) {

      this.errorMessage =
        'Please select an attachment.';

      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.attachmentService
      .upload(
        file
      )
      .pipe(

        switchMap(
          uploaded => {

            return this.service
              .setRequirementAttachment(
                parent.id,
                requirement.id,
                {
                  attachmentId:
                    uploaded.attachmentId,

                  fileName:
                    uploaded.fileName,

                  notes:
                    this.requirementAttachmentNotes
                      .trim() ||
                    null
                }
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.requirementAttachmentModalOpen =
            false;


          this.resetRequirementAttachmentForm();


          this.expandRequirementAfterSave(
            parent.id,
            requirement.id
          );


          this.successMessage =
            'Requirement attachment saved successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to save requirement attachment',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to save requirement attachment.';
        }

      });
  }


  removeRequirementAttachment(
    approval:
      MiscellaneousApprovalResponse,
    requirement:
      ApprovalRequirementResponse
  ): void {

    if (
      !requirement.requirementAttachment ||
      this.saving
    ) {
      return;
    }


    if (
      !window.confirm(
        `Remove attachment from "${requirement.name}"?`
      )
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeRequirementAttachment(
        approval.id,
        requirement.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandRequirementAfterSave(
            approval.id,
            requirement.id
          );


          this.successMessage =
            'Requirement attachment removed successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to remove requirement attachment',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to remove requirement attachment.';
        }

      });
  }


  // =========================================================
  // Requirement Approval
  // =========================================================

  openRequirementApprovalModal(
    approval:
      MiscellaneousApprovalResponse,
    requirement:
      ApprovalRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.requirementApprovalParent =
      approval;


    this.requirementApprovalTarget =
      requirement;


    this.requirementApprovalDate =
      requirement.approval?.date ??
      '';


    this.requirementApprovalNotes =
      requirement.approval
        ?.attachment
        .notes ??
      '';


    this.requirementApprovalFile =
      null;


    this.clearMessages();


    this.requirementApprovalModalOpen =
      true;
  }


  closeRequirementApprovalModal():
    void {

    if (this.saving) {
      return;
    }


    this.requirementApprovalModalOpen =
      false;


    this.resetRequirementApprovalForm();
  }


  onRequirementApprovalFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.requirementApprovalFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  saveRequirementApproval():
    void {

    const parent =
      this.requirementApprovalParent;


    const requirement =
      this.requirementApprovalTarget;


    if (
      !parent ||
      !requirement
    ) {
      return;
    }


    if (!this.requirementApprovalDate) {

      this.errorMessage =
        'Approval date is required.';

      return;
    }


    if (
      !requirement.approval &&
      !this.requirementApprovalFile
    ) {

      this.errorMessage =
        'Approval attachment is required.';

      return;
    }


    this.saving =
      true;


    this.clearMessages();


    // =======================================================
    // Keep existing approval file
    // =======================================================

    if (!this.requirementApprovalFile) {

      const existing =
        requirement.approval;


      if (!existing) {

        this.saving =
          false;

        return;
      }


      const input:
        DatedAttachmentInput = {

        date:
          this.requirementApprovalDate,

        attachmentId:
          existing
            .attachment
            .attachmentId,

        fileName:
          existing
            .attachment
            .fileName,

        notes:
          this.requirementApprovalNotes
            .trim() ||
          null
      };


      this.service
        .approveRequirement(
          parent.id,
          requirement.id,
          input
        )
        .subscribe({

          next: () => {

            this.requirementApprovalSaved(
              parent.id,
              requirement.id
            );
          },


          error: error => {

            this.requirementApprovalFailed(
              error
            );
          }

        });


      return;
    }


    // =======================================================
    // Upload new approval file
    // =======================================================

    this.attachmentService
      .upload(
        this.requirementApprovalFile
      )
      .pipe(

        switchMap(
          uploaded => {

            const input:
              DatedAttachmentInput = {

              date:
                this.requirementApprovalDate,

              attachmentId:
                uploaded.attachmentId,

              fileName:
                uploaded.fileName,

              notes:
                this.requirementApprovalNotes
                  .trim() ||
                null
            };


            return this.service
              .approveRequirement(
                parent.id,
                requirement.id,
                input
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementApprovalSaved(
            parent.id,
            requirement.id
          );
        },


        error: error => {

          this.requirementApprovalFailed(
            error
          );
        }

      });
  }


  private requirementApprovalSaved(
    approvalId: string,
    requirementId: string
  ): void {

    this.saving =
      false;


    this.requirementApprovalModalOpen =
      false;


    this.resetRequirementApprovalForm();


    this.expandRequirementAfterSave(
      approvalId,
      requirementId
    );


    this.successMessage =
      'Requirement approval saved successfully.';


    this.load();
  }


  private requirementApprovalFailed(
    error: unknown
  ): void {

    console.error(
      'Unable to save requirement approval',
      error
    );


    this.saving =
      false;


    this.errorMessage =
      'Unable to save requirement approval.';
  }


  removeRequirementApproval(
    approval:
      MiscellaneousApprovalResponse,
    requirement:
      ApprovalRequirementResponse
  ): void {

    if (
      !requirement.approval ||
      this.saving
    ) {
      return;
    }


    if (
      !window.confirm(
        `Remove approval from "${requirement.name}"?`
      )
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeRequirementApproval(
        approval.id,
        requirement.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandRequirementAfterSave(
            approval.id,
            requirement.id
          );


          this.successMessage =
            'Requirement approval removed successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to remove requirement approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to remove requirement approval.';
        }

      });
  }


  // =========================================================
  // Delete Requirement
  // =========================================================

  deleteRequirement(
    approval:
      MiscellaneousApprovalResponse,
    requirement:
      ApprovalRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


    if (
      !window.confirm(
        `Delete requirement "${requirement.name}"?`
      )
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeRequirement(
        approval.id,
        requirement.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedRequirementIds
            .delete(
              requirement.id
            );


          this.successMessage =
            'Approval requirement deleted successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to delete approval requirement',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to delete approval requirement.';
        }

      });
  }


  private expandRequirementAfterSave(
    approvalId: string,
    requirementId: string
  ): void {

    this.expandedApprovalIds
      .add(
        approvalId
      );


    this.expandedRequirementSectionIds
      .add(
        approvalId
      );


    this.expandedRequirementIds
      .add(
        requirementId
      );
  }


  // =========================================================
  // Attachment Helpers
  // =========================================================

  isPdf(
    fileName: string
  ): boolean {

    return fileName
      .toLowerCase()
      .endsWith('.pdf');
  }


  previewPdf(
    attachmentId: string,
    fileName: string
  ): void {

    if (!this.isPdf(fileName)) {
      return;
    }


    this.closePdfViewer();


    this.pdfViewerOpen =
      true;


    this.pdfViewerLoading =
      true;


    this.pdfViewerFileName =
      fileName;


    this.attachmentService
      .download(
        attachmentId,
        fileName
      )
      .subscribe({

        next: blob => {

          this.pdfObjectUrl =
            URL.createObjectURL(
              blob
            );


          this.pdfViewerUrl =
            this.sanitizer
              .bypassSecurityTrustResourceUrl(
                this.pdfObjectUrl
              );


          this.pdfViewerLoading =
            false;
        },


        error: error => {

          console.error(
            'Unable to preview PDF',
            error
          );


          this.pdfViewerOpen =
            false;


          this.pdfViewerLoading =
            false;


          this.errorMessage =
            `Unable to preview "${fileName}".`;
        }

      });
  }


  closePdfViewer():
    void {

    this.pdfViewerOpen =
      false;


    this.pdfViewerLoading =
      false;


    this.pdfViewerFileName =
      '';


    this.pdfViewerUrl =
      null;


    if (this.pdfObjectUrl) {

      URL.revokeObjectURL(
        this.pdfObjectUrl
      );


      this.pdfObjectUrl =
        null;
    }
  }


  downloadAttachment(
    attachmentId: string,
    fileName: string
  ): void {

    this.attachmentService
      .download(
        attachmentId,
        fileName
      )
      .subscribe({

        next: blob => {

          const url =
            URL.createObjectURL(
              blob
            );


          const anchor =
            document.createElement(
              'a'
            );


          anchor.href =
            url;


          anchor.download =
            fileName;


          document.body
            .appendChild(
              anchor
            );


          anchor.click();


          anchor.remove();


          URL.revokeObjectURL(
            url
          );
        },


        error: error => {

          console.error(
            'Unable to download file',
            error
          );


          this.errorMessage =
            `Unable to download "${fileName}".`;
        }

      });
  }


  openAttachment(
    attachmentId: string,
    fileName: string
  ): void {

    this.attachmentService
      .download(
        attachmentId,
        fileName
      )
      .subscribe({

        next: blob => {

          const url =
            URL.createObjectURL(
              blob
            );


          window.open(
            url,
            '_blank'
          );


          setTimeout(
            () =>
              URL.revokeObjectURL(
                url
              ),
            60_000
          );
        },


        error: error => {

          console.error(
            'Unable to open file',
            error
          );


          this.errorMessage =
            `Unable to open "${fileName}".`;
        }

      });
  }


  // =========================================================
  // Reset
  // =========================================================

  private resetCreateForm():
    void {

    this.createFileTypeId =
      null;


    this.createInitiator =
      '';


    this.createSubmissionDate =
      '';
  }


  private resetEditDetailsForm():
    void {

    this.editDetailsTarget =
      null;


    this.editInitiator =
      '';


    this.editSubmissionDate =
      '';
  }


  private resetMainApprovalForm():
    void {

    this.mainApprovalTarget =
      null;


    this.mainApprovalDate =
      '';


    this.mainApprovalExpiryDate =
      '';
  }


  private resetRequirementForm():
    void {

    this.requirementParent =
      null;


    this.requirementFormMode =
      'add';


    this.editingRequirementId =
      null;


    this.requirementName =
      '';


    this.requirementDeadline =
      '';


    this.requirementDepartmentId =
      null;


    this.requirementDepartmentName =
      '';


    this.departmentSearch =
      '';


    this.departments =
      [];


    this.departmentDropdownOpen =
      false;


    this.requirementNotes =
      '';


    this.requirementFile =
      null;
  }


  private resetRequirementAttachmentForm():
    void {

    this.requirementAttachmentParent =
      null;


    this.requirementAttachmentTarget =
      null;


    this.requirementAttachmentFile =
      null;


    this.requirementAttachmentNotes =
      '';
  }


  private resetRequirementApprovalForm():
    void {

    this.requirementApprovalParent =
      null;


    this.requirementApprovalTarget =
      null;


    this.requirementApprovalDate =
      '';


    this.requirementApprovalFile =
      null;


    this.requirementApprovalNotes =
      '';
  }


  private clearMessages():
    void {

    this.errorMessage =
      '';


    this.successMessage =
      '';
  }
}