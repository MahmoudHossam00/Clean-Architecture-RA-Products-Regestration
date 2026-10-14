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
  AppealService
} from '../services/appeal.service';

import {
  AppealTypeService
} from '../services/appeal-type.service';

import {
  DepartmentService
} from '../../ProductLicenses/services/department.service';

import {
  AttachmentService
} from '../../../core/services/attachment.service';

import {
  AddAppealRequirementInput,
  AppealRequirementResponse,
  AppealResponse,
  CreateAppealInput,
  UpdateAppealDetailsInput,
  UpdateAppealRequirementInput
} from '../models/appeal.models';

import {
  CreatableLookupComponent
} from '../../../core/components/creatable-lookup/creatable-lookup.component';
import { DatedAttachmentInput, LookupResponse } from '../../product-basic-data/models/product-basic-data.models';


export type RequirementFormMode =
  | 'add'
  | 'edit';


@Component({
  selector: 'app-product-appeals',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    CreatableLookupComponent
  ],

  templateUrl:
    './product-appeals.component.html',

  styleUrl:
    './product-appeals.component.css'
})
export class ProductAppealsComponent
  implements OnInit, OnDestroy {

  // =========================================================
  // Dependencies
  // =========================================================

  private readonly route =
    inject(ActivatedRoute);


  private readonly appealService =
    inject(AppealService);


  private readonly appealTypeService =
    inject(AppealTypeService);


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

  appeals:
    AppealResponse[] =
      [];


  selectedAppealId:
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
  // Expand / Collapse
  // =========================================================

  appealDetailsExpanded =
    true;


  appealApprovalExpanded =
    false;


  requirementsExpanded =
    true;


  expandedRequirementIds =
    new Set<string>();


  // =========================================================
  // Selected Appeal
  // =========================================================

  get selectedAppeal():
    AppealResponse | null {

    if (!this.selectedAppealId) {
      return null;
    }


    return (
      this.appeals.find(
        x =>
          x.id ===
          this.selectedAppealId
      ) ?? null
    );
  }


  // =========================================================
  // Requirement Statistics
  // =========================================================

  get approvedRequirementsCount():
    number {

    return (
      this.selectedAppeal
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
      this.selectedAppeal
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
      this.selectedAppeal
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
      this.selectedAppeal
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
  // Appeal Types
  // =========================================================

  appealTypes:
    LookupResponse[] =
      [];


  loadingAppealTypes =
    false;


  creatingAppealType =
    false;


  // =========================================================
  // Create Appeal
  // =========================================================

  createModalOpen =
    false;


  createInitiator =
    '';


  createAppealTypeId:
    string | null =
      null;


  createAppealDate =
    '';


  createNotes =
    '';


  // =========================================================
  // Edit Appeal
  // =========================================================

  editModalOpen =
    false;


  editInitiator =
    '';


  editAppealTypeId:
    string | null =
      null;


  editAppealDate =
    '';


  editNotes =
    '';


  // =========================================================
  // Main Appeal Approval
  // =========================================================

  appealApprovalModalOpen =
    false;


  appealApprovalDate =
    '';


  appealApprovalNotes =
    '';


  appealApprovalFile:
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
  // Requirement Attachment
  // =========================================================

  requirementAttachmentModalOpen =
    false;


  requirementAttachmentTarget:
    AppealRequirementResponse | null =
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


  requirementApprovalTarget:
    AppealRequirementResponse | null =
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

    this.loadAppealTypes();


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
  // Expand / Collapse - Main Sections
  // =========================================================

  toggleAppealDetails(): void {

    this.appealDetailsExpanded =
      !this.appealDetailsExpanded;
  }


  toggleAppealApproval(): void {

    this.appealApprovalExpanded =
      !this.appealApprovalExpanded;
  }


  toggleRequirementsSection(): void {

    this.requirementsExpanded =
      !this.requirementsExpanded;
  }


  // =========================================================
  // Expand / Collapse - Requirements
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
      this.selectedAppeal
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


  expandRequirement(
    requirementId: string
  ): void {

    this.expandedRequirementIds
      .add(
        requirementId
      );
  }


  // =========================================================
  // Load Appeals
  // =========================================================

  load(
    preferredAppealId?: string
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


    this.appealService
      .getByProductId(
        this.productId
      )
      .subscribe({

        next: appeals => {

          this.appeals =
            appeals.map(
              appeal => ({
                ...appeal,

                requirements:
                  appeal.requirements ??
                  []
              })
            );


          if (
            preferredAppealId &&
            this.appeals.some(
              x =>
                x.id ===
                preferredAppealId
            )
          ) {

            this.selectedAppealId =
              preferredAppealId;
          }
          else if (
            this.selectedAppealId &&
            this.appeals.some(
              x =>
                x.id ===
                this.selectedAppealId
            )
          ) {
            // Keep current selection.
          }
          else {

            this.selectedAppealId =
              this.appeals[0]?.id ??
              null;
          }


          this.removeInvalidExpandedRequirements();


          this.loading =
            false;
        },


        error: error => {

          console.error(
            'Unable to load appeals',
            error
          );


          this.loading =
            false;


          this.errorMessage =
            'Unable to load appeals.';
        }

      });
  }


  private removeInvalidExpandedRequirements():
    void {

    const validIds =
      new Set(
        this.selectedAppeal
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
  // Appeal Types
  // =========================================================

  loadAppealTypes(): void {

    this.loadingAppealTypes =
      true;


    this.appealTypeService
      .getAll('')
      .subscribe({

        next: items => {

          this.appealTypes =
            items;


          this.loadingAppealTypes =
            false;
        },


        error: error => {

          console.error(
            'Unable to load appeal types',
            error
          );


          this.appealTypes =
            [];


          this.loadingAppealTypes =
            false;
        }

      });
  }


  createAppealType(
    name: string
  ): void {

    const normalized =
      name.trim();


    if (
      !normalized ||
      this.creatingAppealType
    ) {
      return;
    }


    this.creatingAppealType =
      true;


    this.clearMessages();


    this.appealTypeService
      .create(
        normalized
      )
      .subscribe({

        next: () => {

          this.appealTypeService
            .getAll('')
            .subscribe({

              next: items => {

                this.appealTypes =
                  items;


                this.creatingAppealType =
                  false;


                const created =
                  items.find(
                    x =>
                      x.name
                        .trim()
                        .toLowerCase()
                      ===
                      normalized
                        .toLowerCase()
                  );


                if (!created) {

                  this.errorMessage =
                    'Appeal type was created but could not be selected.';

                  return;
                }


                if (this.createModalOpen) {

                  this.createAppealTypeId =
                    created.id;
                }


                if (this.editModalOpen) {

                  this.editAppealTypeId =
                    created.id;
                }
              },


              error: error => {

                console.error(
                  'Unable to reload appeal types',
                  error
                );


                this.creatingAppealType =
                  false;


                this.errorMessage =
                  'Appeal type was created but could not be reloaded.';
              }

            });
        },


        error: error => {

          console.error(
            'Unable to create appeal type',
            error
          );


          this.creatingAppealType =
            false;


          this.errorMessage =
            `Unable to create appeal type "${normalized}".`;
        }

      });
  }


  // =========================================================
  // Select Appeal
  // =========================================================

  selectAppeal(
    appealId: string
  ): void {

    if (this.saving) {
      return;
    }


    if (
      this.selectedAppealId ===
      appealId
    ) {
      return;
    }


    this.selectedAppealId =
      appealId;


    this.expandedRequirementIds
      .clear();


    this.appealDetailsExpanded =
      true;


    this.appealApprovalExpanded =
      false;


    this.requirementsExpanded =
      true;


    this.clearMessages();
  }


  // =========================================================
  // Create Appeal
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


  createAppeal(): void {

    if (
      !this.productId ||
      this.saving
    ) {
      return;
    }


    const initiator =
      this.createInitiator.trim();


    if (!initiator) {

      this.errorMessage =
        'Appeal initiator is required.';

      return;
    }


    if (!this.createAppealTypeId) {

      this.errorMessage =
        'Appeal type is required.';

      return;
    }


    if (!this.createAppealDate) {

      this.errorMessage =
        'Appeal date is required.';

      return;
    }


    const input:
      CreateAppealInput = {

      productId:
        this.productId,

      initiator,

      appealTypeId:
        this.createAppealTypeId,

      appealDate:
        this.createAppealDate,

      notes:
        this.createNotes.trim() ||
        null
    };


    this.saving =
      true;


    this.clearMessages();


    this.appealService
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
            'Appeal created successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to create appeal',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to create appeal.';
        }

      });
  }


  // =========================================================
  // Edit Appeal
  // =========================================================

  openEditModal(): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
      this.saving
    ) {
      return;
    }


    this.editInitiator =
      appeal.initiator;


    this.editAppealTypeId =
      appeal.appealType.id;


    this.editAppealDate =
      appeal.appealDate;


    this.editNotes =
      appeal.notes ?? '';


    this.clearMessages();


    this.editModalOpen =
      true;
  }


  closeEditModal(): void {

    if (this.saving) {
      return;
    }


    this.editModalOpen =
      false;
  }


  saveAppealDetails(): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
      this.saving
    ) {
      return;
    }


    const initiator =
      this.editInitiator.trim();


    if (!initiator) {

      this.errorMessage =
        'Appeal initiator is required.';

      return;
    }


    if (!this.editAppealTypeId) {

      this.errorMessage =
        'Appeal type is required.';

      return;
    }


    if (!this.editAppealDate) {

      this.errorMessage =
        'Appeal date is required.';

      return;
    }


    const input:
      UpdateAppealDetailsInput = {

      initiator,

      appealTypeId:
        this.editAppealTypeId,

      appealDate:
        this.editAppealDate,

      notes:
        this.editNotes.trim() ||
        null
    };


    this.saving =
      true;


    this.clearMessages();


    this.appealService
      .updateDetails(
        appeal.id,
        input
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.editModalOpen =
            false;


          this.successMessage =
            'Appeal updated successfully.';


          this.load(
            appeal.id
          );
        },


        error: error => {

          console.error(
            'Unable to update appeal',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to update appeal.';
        }

      });
  }


  // =========================================================
  // Main Appeal Approval
  // =========================================================

  openAppealApprovalModal(): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
      this.saving
    ) {
      return;
    }


    this.appealApprovalDate =
      appeal.approval?.date ??
      '';


    this.appealApprovalNotes =
      appeal.approval
        ?.attachment
        .notes ??
      '';


    this.appealApprovalFile =
      null;


    this.clearMessages();


    this.appealApprovalModalOpen =
      true;
  }


  closeAppealApprovalModal(): void {

    if (this.saving) {
      return;
    }


    this.appealApprovalModalOpen =
      false;


    this.resetAppealApprovalForm();
  }


  onAppealApprovalFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.appealApprovalFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  saveAppealApproval(): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
      !this.appealApprovalDate
    ) {

      this.errorMessage =
        'Approval date is required.';

      return;
    }


    if (
      !appeal.approval &&
      !this.appealApprovalFile
    ) {

      this.errorMessage =
        'Approval attachment is required.';

      return;
    }


    this.saving =
      true;


    this.clearMessages();


    if (!this.appealApprovalFile) {

      const existing =
        appeal.approval;


      if (!existing) {

        this.saving =
          false;

        return;
      }


      const approval:
        DatedAttachmentInput = {

        date:
          this.appealApprovalDate,

        attachmentId:
          existing
            .attachment
            .attachmentId,

        fileName:
          existing
            .attachment
            .fileName,

        notes:
          this.appealApprovalNotes
            .trim() ||
          null
      };


      this.appealService
        .setApproval(
          appeal.id,
          approval
        )
        .subscribe({

          next: () => {

            this.appealApprovalSaved(
              appeal.id
            );
          },


          error: error => {

            this.appealApprovalFailed(
              error
            );
          }

        });


      return;
    }


    this.attachmentService
      .upload(
        this.appealApprovalFile
      )
      .pipe(

        switchMap(
          uploaded => {

            const approval:
              DatedAttachmentInput = {

              date:
                this.appealApprovalDate,

              attachmentId:
                uploaded.attachmentId,

              fileName:
                uploaded.fileName,

              notes:
                this.appealApprovalNotes
                  .trim() ||
                null
            };


            return this.appealService
              .setApproval(
                appeal.id,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.appealApprovalSaved(
            appeal.id
          );
        },


        error: error => {

          this.appealApprovalFailed(
            error
          );
        }

      });
  }


  private appealApprovalSaved(
    appealId: string
  ): void {

    this.saving =
      false;


    this.appealApprovalModalOpen =
      false;


    this.appealApprovalExpanded =
      true;


    this.resetAppealApprovalForm();


    this.successMessage =
      'Appeal approval saved successfully.';


    this.load(
      appealId
    );
  }


  private appealApprovalFailed(
    error: unknown
  ): void {

    console.error(
      'Unable to save appeal approval',
      error
    );


    this.saving =
      false;


    this.errorMessage =
      'Unable to save appeal approval.';
  }


  removeAppealApproval(): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
      !appeal.approval ||
      this.saving
    ) {
      return;
    }


    if (
      !window.confirm(
        'Remove this appeal approval?'
      )
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.appealService
      .removeApproval(
        appeal.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.successMessage =
            'Appeal approval removed successfully.';


          this.load(
            appeal.id
          );
        },


        error: error => {

          console.error(
            'Unable to remove appeal approval',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to remove appeal approval.';
        }

      });
  }


  // =========================================================
  // Delete Appeal
  // =========================================================

  deleteAppeal(): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
      this.saving
    ) {
      return;
    }


    const confirmed =
      window.confirm(
        `Delete this appeal?\n\nType: ${appeal.appealType.name}\n\nAll requirements will also be deleted.`
      );


    if (!confirmed) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.appealService
      .remove(
        appeal.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.selectedAppealId =
            null;


          this.expandedRequirementIds
            .clear();


          this.successMessage =
            'Appeal deleted successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to delete appeal',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to delete appeal.';
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
  // Requirements Add
  // =========================================================

  openAddRequirementModal(): void {

    if (
      !this.selectedAppeal ||
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
  // Requirements Edit
  // =========================================================

  openEditRequirementModal(
    requirement:
      AppealRequirementResponse
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


  removeSelectedRequirementFile():
    void {

    this.requirementFile =
      null;
  }


  saveRequirement(): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
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
    // Edit Existing
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
        UpdateAppealRequirementInput = {

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


      this.appealService
        .updateRequirement(
          appeal.id,
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


            this.expandedRequirementIds
              .add(
                requirementId
              );


            this.successMessage =
              'Requirement updated successfully.';


            this.load(
              appeal.id
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
    // Add Without File
    // =======================================================

    if (!this.requirementFile) {

      const input:
        AddAppealRequirementInput = {

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


      this.appealService
        .addRequirement(
          appeal.id,
          input
        )
        .subscribe({

          next: () => {

            this.requirementSaved(
              appeal.id
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
              AddAppealRequirementInput = {

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


            return this.appealService
              .addRequirement(
                appeal.id,
                input
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementSaved(
            appeal.id
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
    appealId: string
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
      appealId
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
      AppealRequirementResponse
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


  saveRequirementAttachment(): void {

    const appeal =
      this.selectedAppeal;


    const requirement =
      this.requirementAttachmentTarget;


    const file =
      this.requirementAttachmentFile;


    if (
      !appeal ||
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

            return this.appealService
              .setRequirementAttachment(
                appeal.id,
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


          this.expandedRequirementIds
            .add(
              requirement.id
            );


          this.successMessage =
            'Requirement attachment saved successfully.';


          this.load(
            appeal.id
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
      AppealRequirementResponse
  ): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
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


    this.appealService
      .removeRequirementAttachment(
        appeal.id,
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
            appeal.id
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
      AppealRequirementResponse
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


  saveRequirementApproval(): void {

    const appeal =
      this.selectedAppeal;


    const requirement =
      this.requirementApprovalTarget;


    if (
      !appeal ||
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


      this.appealService
        .setRequirementApproval(
          appeal.id,
          requirement.id,
          approval
        )
        .subscribe({

          next: () => {

            this.requirementApprovalSaved(
              appeal.id,
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


            return this.appealService
              .setRequirementApproval(
                appeal.id,
                requirement.id,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementApprovalSaved(
            appeal.id,
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
    appealId: string,
    requirementId: string
  ): void {

    this.saving =
      false;


    this.requirementApprovalModalOpen =
      false;


    this.resetRequirementApprovalForm();


    this.expandedRequirementIds
      .add(
        requirementId
      );


    this.successMessage =
      'Requirement approval saved successfully.';


    this.load(
      appealId
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
      AppealRequirementResponse
  ): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
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


    this.appealService
      .removeRequirementApproval(
        appeal.id,
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
            appeal.id
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
      AppealRequirementResponse
  ): void {

    const appeal =
      this.selectedAppeal;


    if (
      !appeal ||
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


    this.appealService
      .removeRequirement(
        appeal.id,
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
            appeal.id
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
  // Attachments
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
            'Unable to open attachment',
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

  private resetCreateForm(): void {

    this.createInitiator =
      '';


    this.createAppealTypeId =
      null;


    this.createAppealDate =
      '';


    this.createNotes =
      '';
  }


  private resetAppealApprovalForm():
    void {

    this.appealApprovalDate =
      '';


    this.appealApprovalNotes =
      '';


    this.appealApprovalFile =
      null;
  }


  private resetRequirementForm():
    void {

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

    this.requirementAttachmentTarget =
      null;


    this.requirementAttachmentFile =
      null;


    this.requirementAttachmentNotes =
      '';
  }


  private resetRequirementApprovalForm():
    void {

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