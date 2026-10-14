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
  VariationService
} from '../services/variation.service';

import {
  AttachmentService
} from '../../../core/services/attachment.service';

import {
  AddVariationRequirementInput,
  CreateVariationInput,
  UpdateVariationDeadlineInput,
  UpdateVariationDetailsInput,
  UpdateVariationRequirementInput,
  VariationRequirementResponse,
  VariationResponse
} from '../models/variation.models';

import {
  DepartmentService
} from '../../ProductLicenses/services/department.service';

import {
  DatedAttachmentInput,
  LookupResponse
} from '../../product-basic-data/models/product-basic-data.models';


export type RequirementFormMode =
  | 'add'
  | 'edit';


@Component({
  selector:
    'app-product-variations',

  standalone:
    true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './product-variations.component.html',

  styleUrl:
    './product-variations.component.css'
})
export class ProductVariationsComponent
  implements OnInit, OnDestroy {

  // =========================================================
  // Dependencies
  // =========================================================

  private readonly route =
    inject(ActivatedRoute);

  private readonly variationService =
    inject(VariationService);

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
  // Main State
  // =========================================================

  variations:
    VariationResponse[] =
      [];


  selectedVariationId:
    string | null =
      null;


  loading =
    false;


  saving =
    false;


  errorMessage =
    '';


  successMessage =
    '';


  // =========================================================
  // Expand / Collapse State
  // =========================================================

  variationInfoExpanded =
    true;


  deadlineExpanded =
    false;


  variationApprovalExpanded =
    false;


  requirementsExpanded =
    true;


  expandedRequirementIds =
    new Set<string>();


  // =========================================================
  // Selected Variation
  // =========================================================

  get selectedVariation():
    VariationResponse | null {

    if (!this.selectedVariationId) {
      return null;
    }


    return (
      this.variations.find(
        x =>
          x.id ===
          this.selectedVariationId
      ) ?? null
    );
  }


  // =========================================================
  // Requirement Statistics
  // =========================================================

  get approvedRequirementsCount():
    number {

    return (
      this.selectedVariation
        ?.requirements
        .filter(
          requirement =>
            !!requirement.approval
        )
        .length ?? 0
    );
  }


  get pendingRequirementsCount():
    number {

    const total =
      this.selectedVariation
        ?.requirements
        .length ?? 0;


    return (
      total -
      this.approvedRequirementsCount
    );
  }


  get requirementsWithAttachmentsCount():
    number {

    return (
      this.selectedVariation
        ?.requirements
        .filter(
          requirement =>
            !!requirement
              .requirementAttachment
        )
        .length ?? 0
    );
  }


  get allRequirementsExpanded():
    boolean {

    const requirements =
      this.selectedVariation
        ?.requirements ?? [];


    return (
      requirements.length > 0 &&
      requirements.every(
        requirement =>
          this.expandedRequirementIds
            .has(
              requirement.id
            )
      )
    );
  }


  // =========================================================
  // Create Variation Modal
  // =========================================================

  createModalOpen =
    false;


  createInitiator =
    '';


  createCause =
    '';


  createDeadline =
    '';


  createDeadlineNote =
    '';


  // =========================================================
  // Edit Details Modal
  // =========================================================

  editDetailsModalOpen =
    false;


  editInitiator =
    '';


  editCause =
    '';


  // =========================================================
  // Deadline Modal
  // =========================================================

  deadlineModalOpen =
    false;


  editDeadline =
    '';


  editDeadlineNote =
    '';


  // =========================================================
  // Variation Approval Modal
  // =========================================================

  variationApprovalModalOpen =
    false;


  variationApprovalDate =
    '';


  variationApprovalNotes =
    '';


  variationApprovalFile:
    File | null =
      null;


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
  // Requirement Attachment Modal
  // =========================================================

  requirementAttachmentModalOpen =
    false;


  requirementAttachmentTarget:
    VariationRequirementResponse | null =
      null;


  requirementAttachmentFile:
    File | null =
      null;


  requirementAttachmentNotes =
    '';


  // =========================================================
  // Requirement Approval Modal
  // =========================================================

  requirementApprovalModalOpen =
    false;


  requirementApprovalTarget:
    VariationRequirementResponse | null =
      null;


  requirementApprovalDate =
    '';


  requirementApprovalNotes =
    '';


  requirementApprovalFile:
    File | null =
      null;


  // =========================================================
  // PDF Viewer
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
  // Expand / Collapse Main Sections
  // =========================================================

  toggleVariationInfo(): void {

    this.variationInfoExpanded =
      !this.variationInfoExpanded;
  }


  toggleDeadline(): void {

    this.deadlineExpanded =
      !this.deadlineExpanded;
  }


  toggleVariationApproval(): void {

    this.variationApprovalExpanded =
      !this.variationApprovalExpanded;
  }


  toggleRequirementsSection(): void {

    this.requirementsExpanded =
      !this.requirementsExpanded;
  }


  // =========================================================
  // Expand / Collapse Requirements
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


  toggleAllRequirements(): void {

    const requirements =
      this.selectedVariation
        ?.requirements ?? [];


    if (!requirements.length) {
      return;
    }


    if (this.allRequirementsExpanded) {

      requirements.forEach(
        requirement => {

          this.expandedRequirementIds
            .delete(
              requirement.id
            );
        }
      );

      return;
    }


    requirements.forEach(
      requirement => {

        this.expandedRequirementIds
          .add(
            requirement.id
          );
      }
    );
  }


  private removeInvalidExpandedRequirements():
    void {

    const validIds =
      new Set(
        this.selectedVariation
          ?.requirements
          .map(
            requirement =>
              requirement.id
          ) ?? []
      );


    for (
      const id
      of this.expandedRequirementIds
    ) {

      if (!validIds.has(id)) {

        this.expandedRequirementIds
          .delete(id);
      }
    }
  }


  // =========================================================
  // Load Variations
  // =========================================================

  load(
    preferredVariationId?: string
  ): void {

    if (!this.productId) {

      this.errorMessage =
        'Product ID was not found.';

      return;
    }


    this.loading =
      true;


    this.errorMessage =
      '';


    this.variationService
      .getByProductId(
        this.productId
      )
      .subscribe({

        next: variations => {

          this.variations =
            variations.map(
              variation => ({
                ...variation,

                requirements:
                  variation.requirements ?? []
              })
            );


          if (
            preferredVariationId &&
            this.variations.some(
              x =>
                x.id ===
                preferredVariationId
            )
          ) {

            this.selectedVariationId =
              preferredVariationId;
          }
          else if (
            this.selectedVariationId &&
            this.variations.some(
              x =>
                x.id ===
                this.selectedVariationId
            )
          ) {
            // Keep current selection.
          }
          else {

            this.selectedVariationId =
              this.variations[0]?.id ??
              null;
          }


          this.removeInvalidExpandedRequirements();


          this.loading =
            false;
        },


        error: error => {

          console.error(
            'Unable to load variations',
            error
          );


          this.loading =
            false;


          this.errorMessage =
            'Unable to load variations.';
        }

      });
  }


  // =========================================================
  // Select Tab
  // =========================================================

  selectVariation(
    variationId: string
  ): void {

    if (this.saving) {
      return;
    }


    if (
      this.selectedVariationId ===
      variationId
    ) {
      return;
    }


    this.selectedVariationId =
      variationId;


    this.expandedRequirementIds
      .clear();


    this.variationInfoExpanded =
      true;


    this.deadlineExpanded =
      false;


    this.variationApprovalExpanded =
      false;


    this.requirementsExpanded =
      true;


    this.clearMessages();
  }


  // =========================================================
  // Create Variation
  // =========================================================

  openCreateModal(): void {

    this.resetCreateForm();


    this.clearMessages();


    this.createModalOpen =
      true;
  }


  closeCreateModal(): void {

    if (this.saving) {
      return;
    }


    this.createModalOpen =
      false;


    this.resetCreateForm();
  }


  createVariation(): void {

    if (
      !this.productId ||
      this.saving
    ) {
      return;
    }


    const initiator =
      this.createInitiator.trim();


    const cause =
      this.createCause.trim();


    if (!initiator) {

      this.errorMessage =
        'Variation initiator is required.';

      return;
    }


    if (!cause) {

      this.errorMessage =
        'Variation cause is required.';

      return;
    }


    if (!this.createDeadline) {

      this.errorMessage =
        'Variation deadline is required.';

      return;
    }


    const input:
      CreateVariationInput = {

      productId:
        this.productId,

      initiator,

      cause,

      deadline:
        this.createDeadline,

      deadlineNote:
        this.createDeadlineNote.trim() ||
        null
    };


    this.saving =
      true;


    this.clearMessages();


    this.variationService
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
            'Variation created successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to create variation',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to create variation.';
        }

      });
  }


  // =========================================================
  // Edit Variation Details
  // =========================================================

  openEditDetailsModal(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      this.saving
    ) {
      return;
    }


    this.editInitiator =
      variation.initiator;


    this.editCause =
      variation.cause;


    this.clearMessages();


    this.editDetailsModalOpen =
      true;
  }


  closeEditDetailsModal(): void {

    if (this.saving) {
      return;
    }


    this.editDetailsModalOpen =
      false;
  }


  saveVariationDetails(): void {

    const variation =
      this.selectedVariation;


    if (!variation) {
      return;
    }


    const initiator =
      this.editInitiator.trim();


    const cause =
      this.editCause.trim();


    if (!initiator) {

      this.errorMessage =
        'Variation initiator is required.';

      return;
    }


    if (!cause) {

      this.errorMessage =
        'Variation cause is required.';

      return;
    }


    const input:
      UpdateVariationDetailsInput = {

      initiator,

      cause
    };


    this.saving =
      true;


    this.clearMessages();


    this.variationService
      .updateDetails(
        variation.id,
        input
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.editDetailsModalOpen =
            false;


          this.variationInfoExpanded =
            true;


          this.successMessage =
            'Variation details updated successfully.';


          this.load(
            variation.id
          );
        },


        error: error => {

          console.error(
            'Unable to update variation details',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to update variation details.';
        }

      });
  }


  // =========================================================
  // Deadline
  // =========================================================

  openDeadlineModal(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      this.saving
    ) {
      return;
    }


    this.editDeadline =
      variation.deadline;


    this.editDeadlineNote =
      variation.deadlineNote ??
      '';


    this.clearMessages();


    this.deadlineModalOpen =
      true;
  }


  closeDeadlineModal(): void {

    if (this.saving) {
      return;
    }


    this.deadlineModalOpen =
      false;
  }


  saveDeadline(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      !this.editDeadline
    ) {

      this.errorMessage =
        'Deadline is required.';

      return;
    }


    const input:
      UpdateVariationDeadlineInput = {

      deadline:
        this.editDeadline,

      deadlineNote:
        this.editDeadlineNote.trim() ||
        null
    };


    this.saving =
      true;


    this.clearMessages();


    this.variationService
      .updateDeadline(
        variation.id,
        input
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.deadlineModalOpen =
            false;


          this.deadlineExpanded =
            true;


          this.successMessage =
            'Variation deadline updated successfully.';


          this.load(
            variation.id
          );
        },


        error: error => {

          console.error(
            'Unable to update variation deadline',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to update variation deadline.';
        }

      });
  }


  // =========================================================
  // Variation Approval
  // =========================================================

  openVariationApprovalModal(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      this.saving
    ) {
      return;
    }


    this.variationApprovalDate =
      variation.approval?.date ??
      '';


    this.variationApprovalNotes =
      variation.approval
        ?.attachment
        .notes ??
      '';


    this.variationApprovalFile =
      null;


    this.clearMessages();


    this.variationApprovalModalOpen =
      true;
  }


  closeVariationApprovalModal(): void {

    if (this.saving) {
      return;
    }


    this.variationApprovalModalOpen =
      false;


    this.resetVariationApprovalForm();
  }


  onVariationApprovalFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.variationApprovalFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  saveVariationApproval(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      !this.variationApprovalDate
    ) {

      this.errorMessage =
        'Approval date is required.';

      return;
    }


    if (
      !variation.approval &&
      !this.variationApprovalFile
    ) {

      this.errorMessage =
        'Approval attachment is required.';

      return;
    }


    this.saving =
      true;


    this.clearMessages();


    if (!this.variationApprovalFile) {

      const existing =
        variation.approval;


      if (!existing) {

        this.saving =
          false;

        return;
      }


      const approval:
        DatedAttachmentInput = {

        date:
          this.variationApprovalDate,

        attachmentId:
          existing.attachment.attachmentId,

        fileName:
          existing.attachment.fileName,

        notes:
          this.variationApprovalNotes.trim() ||
          null
      };


      this.variationService
        .setApproval(
          variation.id,
          approval
        )
        .subscribe({

          next: () => {

            this.variationApprovalSaved(
              variation.id
            );
          },


          error: error => {

            this.variationApprovalFailed(
              error
            );
          }

        });


      return;
    }


    this.attachmentService
      .upload(
        this.variationApprovalFile
      )
      .pipe(

        switchMap(
          uploaded => {

            const approval:
              DatedAttachmentInput = {

              date:
                this.variationApprovalDate,

              attachmentId:
                uploaded.attachmentId,

              fileName:
                uploaded.fileName,

              notes:
                this.variationApprovalNotes.trim() ||
                null
            };


            return this.variationService
              .setApproval(
                variation.id,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.variationApprovalSaved(
            variation.id
          );
        },


        error: error => {

          this.variationApprovalFailed(
            error
          );
        }

      });
  }


  private variationApprovalSaved(
    variationId: string
  ): void {

    this.saving =
      false;


    this.variationApprovalModalOpen =
      false;


    this.variationApprovalExpanded =
      true;


    this.resetVariationApprovalForm();


    this.successMessage =
      'Variation approval saved successfully.';


    this.load(
      variationId
    );
  }


  private variationApprovalFailed(
    error: unknown
  ): void {

    console.error(
      'Unable to save variation approval',
      error
    );


    this.saving =
      false;


    this.errorMessage =
      'Unable to save variation approval.';
  }


  removeVariationApproval(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      !variation.approval ||
      this.saving
    ) {
      return;
    }


    if (
      !window.confirm(
        'Remove this variation approval?'
      )
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.variationService
      .removeApproval(
        variation.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.variationApprovalExpanded =
            true;


          this.successMessage =
            'Variation approval removed successfully.';


          this.load(
            variation.id
          );
        },


        error: error => {

          console.error(
            'Unable to remove variation approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to remove variation approval.';
        }

      });
  }


  // =========================================================
  // Delete Variation
  // =========================================================

  deleteVariation(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      this.saving
    ) {
      return;
    }


    const confirmed =
      window.confirm(
        `Delete this variation?\n\nCause: ${variation.cause}\n\nAll requirements will also be deleted.`
      );


    if (!confirmed) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.variationService
      .remove(
        variation.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.selectedVariationId =
            null;


          this.expandedRequirementIds
            .clear();


          this.successMessage =
            'Variation deleted successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to delete variation',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to delete variation.';
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
      value.trim().toLowerCase();


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


  openDepartmentDropdown(): void {

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
    department: LookupResponse
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

  openAddRequirementModal(): void {

    if (
      !this.selectedVariation ||
      this.saving
    ) {
      return;
    }


    this.resetRequirementForm();


    this.requirementFormMode =
      'add';


    this.requirementModalOpen =
      true;


    this.requirementsExpanded =
      true;


    this.clearMessages();


    this.loadDepartments('');
  }


  // =========================================================
  // Edit Requirement
  // =========================================================

  openEditRequirementModal(
    requirement:
      VariationRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.resetRequirementForm();


    this.requirementFormMode =
      'edit';


    this.editingRequirementId =
      requirement.id;


    this.requirementName =
      requirement.name;


    this.requirementDeadline =
      requirement.deadline;


    this.requirementDepartmentId =
      requirement.responsibleDepartment.id;


    this.requirementDepartmentName =
      requirement.responsibleDepartment.name;


    this.departmentSearch =
      requirement.responsibleDepartment.name;


    this.requirementNotes =
      requirement.notes ??
      '';


    this.requirementModalOpen =
      true;


    this.clearMessages();


    this.loadDepartments(
      requirement.responsibleDepartment.name
    );
  }


  closeRequirementModal(): void {

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


  removeSelectedRequirementFile(): void {

    this.requirementFile =
      null;
  }


  // =========================================================
  // Save Requirement
  // =========================================================

  saveRequirement(): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      this.saving
    ) {
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
    // Edit Existing Requirement
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
        UpdateVariationRequirementInput = {

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


      this.variationService
        .updateRequirement(
          variation.id,
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


            this.requirementsExpanded =
              true;


            this.expandedRequirementIds
              .add(
                requirementId
              );


            this.successMessage =
              'Requirement updated successfully.';


            this.load(
              variation.id
            );
          },


          error: error => {

            console.error(
              'Unable to update requirement',
              error
            );


            this.saving =
              false;


            this.errorMessage =
              'Unable to update requirement.';
          }

        });


      return;
    }


    // =======================================================
    // Add Without Attachment
    // =======================================================

    if (!this.requirementFile) {

      const input:
        AddVariationRequirementInput = {

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


      this.variationService
        .addRequirement(
          variation.id,
          input
        )
        .subscribe({

          next: () => {

            this.requirementSaved(
              variation.id
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
    // Add With Attachment
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
              AddVariationRequirementInput = {

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


            return this.variationService
              .addRequirement(
                variation.id,
                input
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementSaved(
            variation.id
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
    variationId: string
  ): void {

    this.saving =
      false;


    this.requirementModalOpen =
      false;


    this.resetRequirementForm();


    this.requirementsExpanded =
      true;


    this.successMessage =
      'Requirement added successfully.';


    this.load(
      variationId
    );
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
      'Unable to save requirement.';
  }


  // =========================================================
  // Requirement Attachment
  // =========================================================

  openRequirementAttachmentModal(
    requirement:
      VariationRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.requirementAttachmentTarget =
      requirement;


    this.requirementAttachmentFile =
      null;


    this.requirementAttachmentNotes =
      requirement
        .requirementAttachment
        ?.notes ??
      '';


    this.requirementAttachmentModalOpen =
      true;


    this.clearMessages();
  }


  closeRequirementAttachmentModal(): void {

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


  saveRequirementAttachment(): void {

    const variation =
      this.selectedVariation;


    const requirement =
      this.requirementAttachmentTarget;


    const file =
      this.requirementAttachmentFile;


    if (
      !variation ||
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

            return this.variationService
              .setRequirementAttachment(
                variation.id,
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


          this.requirementsExpanded =
            true;


          this.expandedRequirementIds
            .add(
              requirement.id
            );


          this.successMessage =
            'Requirement attachment saved successfully.';


          this.load(
            variation.id
          );
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
    requirement:
      VariationRequirementResponse
  ): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
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


    this.variationService
      .removeRequirementAttachment(
        variation.id,
        requirement.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedRequirementIds
            .add(
              requirement.id
            );


          this.successMessage =
            'Requirement attachment removed successfully.';


          this.load(
            variation.id
          );
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
    requirement:
      VariationRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


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


    this.requirementApprovalModalOpen =
      true;


    this.clearMessages();
  }


  closeRequirementApprovalModal(): void {

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


  saveRequirementApproval(): void {

    const variation =
      this.selectedVariation;


    const requirement =
      this.requirementApprovalTarget;


    if (
      !variation ||
      !requirement ||
      !this.requirementApprovalDate
    ) {

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


    if (!this.requirementApprovalFile) {

      const existing =
        requirement.approval;


      if (!existing) {

        this.saving =
          false;

        return;
      }


      const approval:
        DatedAttachmentInput = {

        date:
          this.requirementApprovalDate,

        attachmentId:
          existing.attachment.attachmentId,

        fileName:
          existing.attachment.fileName,

        notes:
          this.requirementApprovalNotes
            .trim() ||
          null
      };


      this.variationService
        .setRequirementApproval(
          variation.id,
          requirement.id,
          approval
        )
        .subscribe({

          next: () => {

            this.requirementApprovalSaved(
              variation.id,
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


    this.attachmentService
      .upload(
        this.requirementApprovalFile
      )
      .pipe(

        switchMap(
          uploaded => {

            const approval:
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


            return this.variationService
              .setRequirementApproval(
                variation.id,
                requirement.id,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementApprovalSaved(
            variation.id,
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
    variationId: string,
    requirementId: string
  ): void {

    this.saving =
      false;


    this.requirementApprovalModalOpen =
      false;


    this.resetRequirementApprovalForm();


    this.requirementsExpanded =
      true;


    this.expandedRequirementIds
      .add(
        requirementId
      );


    this.successMessage =
      'Requirement approval saved successfully.';


    this.load(
      variationId
    );
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
    requirement:
      VariationRequirementResponse
  ): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
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


    this.variationService
      .removeRequirementApproval(
        variation.id,
        requirement.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedRequirementIds
            .add(
              requirement.id
            );


          this.successMessage =
            'Requirement approval removed successfully.';


          this.load(
            variation.id
          );
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
    requirement:
      VariationRequirementResponse
  ): void {

    const variation =
      this.selectedVariation;


    if (
      !variation ||
      this.saving
    ) {
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


    this.variationService
      .removeRequirement(
        variation.id,
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
            'Requirement deleted successfully.';


          this.load(
            variation.id
          );
        },


        error: error => {

          console.error(
            'Unable to delete requirement',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to delete requirement.';
        }

      });
  }


  // =========================================================
  // PDF / Attachments
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
            'Unable to preview file',
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


  closePdfViewer(): void {

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
  // Reset Helpers
  // =========================================================

  private resetCreateForm(): void {

    this.createInitiator =
      '';


    this.createCause =
      '';


    this.createDeadline =
      '';


    this.createDeadlineNote =
      '';
  }


  private resetVariationApprovalForm(): void {

    this.variationApprovalDate =
      '';


    this.variationApprovalNotes =
      '';


    this.variationApprovalFile =
      null;
  }


  private resetRequirementForm(): void {

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


  private resetRequirementAttachmentForm(): void {

    this.requirementAttachmentTarget =
      null;


    this.requirementAttachmentFile =
      null;


    this.requirementAttachmentNotes =
      '';
  }


  private resetRequirementApprovalForm(): void {

    this.requirementApprovalTarget =
      null;


    this.requirementApprovalDate =
      '';


    this.requirementApprovalNotes =
      '';


    this.requirementApprovalFile =
      null;
  }


  private clearMessages(): void {

    this.errorMessage =
      '';


    this.successMessage =
      '';
  }
}