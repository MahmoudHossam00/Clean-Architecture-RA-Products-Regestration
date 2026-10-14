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
  HttpErrorResponse
} from '@angular/common/http';

import {
  DomSanitizer,
  SafeResourceUrl
} from '@angular/platform-browser';

import {
  Observable,
  Subject,
  debounceTime,
  distinctUntilChanged,
  switchMap
} from 'rxjs';

import {
  ProductLicenseService
} from '../services/product-license.service';

import {
  LicenseCategoryService
} from '../services/license-category.service';

import {
  MarketingTypeService
} from '../services/marketing-type.service';

import {
  DepartmentService
} from '../services/department.service';

import {
  AttachmentService
} from '../../../core/services/attachment.service';

import {
  AddLicenseRequirementInput,
  CreateProductLicenseInput,
  DatedAttachmentInput,
  LicenseRequirementResponse,
  LookupResponse,
  ProductLicenseResponse,
  UpdateLicenseRequirementInput
} from '../models/product-license.models';

import {
  CreatableLookupComponent
} from '../../../core/components/creatable-lookup/creatable-lookup.component';


export type LicenseDocumentType =
  | 'leaflet'
  | 'layout';


export type RequirementFormMode =
  | 'add'
  | 'edit';


@Component({
  selector: 'app-product-license',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    CreatableLookupComponent
  ],

  templateUrl:
    './product-license.component.html',

  styleUrl:
    './product-license.component.css'
})
export class ProductLicenseComponent
  implements OnInit, OnDestroy {

  // =========================================================
  // Dependencies
  // =========================================================

  private readonly route =
    inject(ActivatedRoute);


  private readonly service =
    inject(ProductLicenseService);


  private readonly licenseCategoryService =
    inject(LicenseCategoryService);


  private readonly marketingTypeService =
    inject(MarketingTypeService);


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

  license:
    ProductLicenseResponse | null =
      null;


  licenseMissing =
    false;


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

  licenseInformationExpanded =
    true;


  approvedDocumentsExpanded =
    false;


  leafletExpanded =
    false;


  layoutExpanded =
    false;


  requirementsExpanded =
    true;


  expandedRequirementIds =
    new Set<string>();


  // =========================================================
  // Requirement Statistics
  // =========================================================

  get approvedRequirementsCount():
    number {

    return (
      this.license
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
      this.license
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
      this.license
        ?.requirements
        .filter(
          requirement =>
            !!requirement
              .requirementAttachment
        )
        .length ?? 0
    );
  }


  get approvedDocumentsCount():
    number {

    if (!this.license) {
      return 0;
    }


    let count =
      0;


    if (
      this.license
        .lastApprovedLeaflet
    ) {
      count++;
    }


    if (
      this.license
        .lastApprovedLayout
    ) {
      count++;
    }


    return count;
  }


  get allRequirementsExpanded():
    boolean {

    const requirements =
      this.license
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
  // License Categories
  // =========================================================

  licenseCategories:
    LookupResponse[] =
      [];


  loadingLicenseCategories =
    false;


  creatingLicenseCategory =
    false;


  // =========================================================
  // Marketing Types
  // =========================================================

  marketingTypes:
    LookupResponse[] =
      [];


  loadingMarketingTypes =
    false;


  creatingMarketingType =
    false;


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
  // Create License
  // =========================================================

  createLicenseCategoryId:
    string | null =
      null;


  createMarketingTypeId:
    string | null =
      null;


  createIssueDate =
    '';


  createValidityDate =
    '';


  createRegistrationNumberYear =
    '';


  // =========================================================
  // Edit License
  // =========================================================

  licenseCategoryId:
    string | null =
      null;


  marketingTypeId:
    string | null =
      null;


  issueDate =
    '';


  validityDate =
    '';


  registrationNumberYear =
    '';


  // =========================================================
  // Requirement Add / Edit Modal
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
    LicenseRequirementResponse | null =
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
    LicenseRequirementResponse | null =
      null;


  requirementApprovalDate =
    '';


  requirementApprovalNotes =
    '';


  requirementApprovalFile:
    File | null =
      null;


  // =========================================================
  // Leaflet / Layout Modal
  // =========================================================

  attachmentModalOpen =
    false;


  attachmentModalType:
    LicenseDocumentType | null =
      null;


  attachmentApprovalDate =
    '';


  attachmentNotes =
    '';


  attachmentFile:
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

    this.loadLookups();


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
  // License Id
  // =========================================================

  get licenseId():
    string | null {

    return (
      this.license?.id ??
      null
    );
  }


  // =========================================================
  // Expand / Collapse Main Sections
  // =========================================================

  toggleLicenseInformation():
    void {

    this.licenseInformationExpanded =
      !this.licenseInformationExpanded;
  }


  toggleApprovedDocuments():
    void {

    this.approvedDocumentsExpanded =
      !this.approvedDocumentsExpanded;
  }


  toggleLeaflet():
    void {

    this.leafletExpanded =
      !this.leafletExpanded;
  }


  toggleLayout():
    void {

    this.layoutExpanded =
      !this.layoutExpanded;
  }


  toggleRequirementsSection():
    void {

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


  toggleAllRequirements():
    void {

    const requirements =
      this.license
        ?.requirements ?? [];


    if (!requirements.length) {
      return;
    }


    if (
      this.allRequirementsExpanded
    ) {

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
        this.license
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

      if (
        !validIds.has(id)
      ) {

        this.expandedRequirementIds
          .delete(id);
      }
    }
  }


  // =========================================================
  // Load License
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


    this.licenseMissing =
      false;


    this.service
      .getByProductId(
        this.productId
      )
      .subscribe({

        next: response => {

          response.requirements =
            response.requirements ??
            [];


          this.license =
            response;


          this.licenseMissing =
            false;


          this.populateForm(
            response
          );


          this.removeInvalidExpandedRequirements();


          this.loading =
            false;
        },


        error: (
          error:
            HttpErrorResponse
        ) => {

          console.error(
            'Unable to load Product License',
            error
          );


          this.loading =
            false;


          if (
            error.status ===
            404
          ) {

            this.license =
              null;


            this.licenseMissing =
              true;


            this.expandedRequirementIds
              .clear();


            this.resetEditForm();

            return;
          }


          this.errorMessage =
            'Unable to load Product License.';
        }

      });
  }


  // =========================================================
  // Lookups
  // =========================================================

  loadLookups(): void {

    this.loadLicenseCategories();


    this.loadMarketingTypes();
  }


  // =========================================================
  // License Categories
  // =========================================================

  loadLicenseCategories():
    void {

    this.loadingLicenseCategories =
      true;


    this.licenseCategoryService
      .getAll('')
      .subscribe({

        next: items => {

          this.licenseCategories =
            items;


          this.loadingLicenseCategories =
            false;
        },


        error: error => {

          console.error(
            'Unable to load license categories',
            error
          );


          this.licenseCategories =
            [];


          this.loadingLicenseCategories =
            false;
        }

      });
  }


  createLicenseCategory(
    name: string
  ): void {

    const normalizedName =
      name.trim();


    if (
      !normalizedName ||
      this.creatingLicenseCategory
    ) {
      return;
    }


    this.creatingLicenseCategory =
      true;


    this.clearMessages();


    this.licenseCategoryService
      .create(
        normalizedName
      )
      .subscribe({

        next: () => {

          this.licenseCategoryService
            .getAll('')
            .subscribe({

              next: items => {

                this.licenseCategories =
                  items;


                const created =
                  items.find(
                    x =>
                      x.name
                        .trim()
                        .toLowerCase()
                      ===
                      normalizedName
                        .toLowerCase()
                  );


                this.creatingLicenseCategory =
                  false;


                if (!created) {

                  this.errorMessage =
                    'License category was created but could not be selected.';

                  return;
                }


                if (this.license) {

                  this.licenseCategoryId =
                    created.id;


                  this.saveLicenseCategory();

                  return;
                }


                this.createLicenseCategoryId =
                  created.id;
              },


              error: error => {

                console.error(
                  'Unable to reload license categories',
                  error
                );


                this.creatingLicenseCategory =
                  false;


                this.errorMessage =
                  'License category was created, but categories could not be reloaded.';
              }

            });
        },


        error: error => {

          console.error(
            'Unable to create license category',
            error
          );


          this.creatingLicenseCategory =
            false;


          this.errorMessage =
            `Unable to create license category "${normalizedName}".`;
        }

      });
  }


  // =========================================================
  // Marketing Types
  // =========================================================

  loadMarketingTypes():
    void {

    this.loadingMarketingTypes =
      true;


    this.marketingTypeService
      .getAll('')
      .subscribe({

        next: items => {

          this.marketingTypes =
            items;


          this.loadingMarketingTypes =
            false;
        },


        error: error => {

          console.error(
            'Unable to load marketing types',
            error
          );


          this.marketingTypes =
            [];


          this.loadingMarketingTypes =
            false;
        }

      });
  }


  createMarketingType(
    name: string
  ): void {

    const normalizedName =
      name.trim();


    if (
      !normalizedName ||
      this.creatingMarketingType
    ) {
      return;
    }


    this.creatingMarketingType =
      true;


    this.clearMessages();


    this.marketingTypeService
      .create(
        normalizedName
      )
      .subscribe({

        next: () => {

          this.marketingTypeService
            .getAll('')
            .subscribe({

              next: items => {

                this.marketingTypes =
                  items;


                const created =
                  items.find(
                    x =>
                      x.name
                        .trim()
                        .toLowerCase()
                      ===
                      normalizedName
                        .toLowerCase()
                  );


                this.creatingMarketingType =
                  false;


                if (!created) {

                  this.errorMessage =
                    'Marketing type was created but could not be selected.';

                  return;
                }


                if (this.license) {

                  this.marketingTypeId =
                    created.id;


                  this.saveMarketingType();

                  return;
                }


                this.createMarketingTypeId =
                  created.id;
              },


              error: error => {

                console.error(
                  'Unable to reload marketing types',
                  error
                );


                this.creatingMarketingType =
                  false;


                this.errorMessage =
                  'Marketing type was created, but marketing types could not be reloaded.';
              }

            });
        },


        error: error => {

          console.error(
            'Unable to create marketing type',
            error
          );


          this.creatingMarketingType =
            false;


          this.errorMessage =
            `Unable to create marketing type "${normalizedName}".`;
        }

      });
  }


  // =========================================================
  // Create License
  // =========================================================

  createLicense(): void {

    if (
      !this.productId ||
      this.saving
    ) {
      return;
    }


    if (
      !this.createLicenseCategoryId
    ) {

      this.errorMessage =
        'License category is required.';

      return;
    }


    if (
      !this.createMarketingTypeId
    ) {

      this.errorMessage =
        'Marketing type is required.';

      return;
    }


    if (
      !this.createIssueDate
    ) {

      this.errorMessage =
        'Issue date is required.';

      return;
    }


    if (
      !this.createValidityDate
    ) {

      this.errorMessage =
        'Validity date is required.';

      return;
    }


    if (
      this.createValidityDate <
      this.createIssueDate
    ) {

      this.errorMessage =
        'Validity date cannot be before issue date.';

      return;
    }


    if (
      !this.isRegistrationNumberValid(
        this.createRegistrationNumberYear
      )
    ) {

      this.errorMessage =
        'Registration Number/Year must contain two numbers separated by "/".';

      return;
    }


    const input:
      CreateProductLicenseInput = {

      productId:
        this.productId,

      licenseCategoryId:
        this.createLicenseCategoryId,

      issueDate:
        this.createIssueDate,

      validityDate:
        this.createValidityDate,

      marketingTypeId:
        this.createMarketingTypeId,

      registrationNumberYear:
        this.createRegistrationNumberYear
          .trim()
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


          this.licenseMissing =
            false;


          this.licenseInformationExpanded =
            true;


          this.successMessage =
            'Product License created successfully.';


          this.resetCreateForm();


          this.load();
        },


        error: error => {

          console.error(
            'Unable to create Product License',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to create Product License.';
        }

      });
  }


  // =========================================================
  // License Fields
  // =========================================================

  isRegistrationNumberValid(
    value: string
  ): boolean {

    return /^\d+\/\d+$/
      .test(
        value.trim()
      );
  }


  saveLicenseCategory():
    void {

    const id =
      this.licenseId;


    if (
      !id ||
      !this.licenseCategoryId ||
      this.license
        ?.licenseCategory
        .id ===
        this.licenseCategoryId
    ) {
      return;
    }


    this.execute(
      this.service
        .updateLicenseCategory(
          id,
          this.licenseCategoryId
        )
    );
  }


  saveMarketingType():
    void {

    const id =
      this.licenseId;


    if (
      !id ||
      !this.marketingTypeId ||
      this.license
        ?.marketingType
        .id ===
        this.marketingTypeId
    ) {
      return;
    }


    this.execute(
      this.service
        .updateMarketingType(
          id,
          this.marketingTypeId
        )
    );
  }


  saveDates(): void {

    const id =
      this.licenseId;


    if (
      !id ||
      !this.issueDate ||
      !this.validityDate
    ) {

      this.errorMessage =
        'Issue date and validity date are required.';

      return;
    }


    if (
      this.validityDate <
      this.issueDate
    ) {

      this.errorMessage =
        'Validity date cannot be before issue date.';

      return;
    }


    this.execute(
      this.service
        .updateDates(
          id,
          this.issueDate,
          this.validityDate
        )
    );
  }


  saveRegistrationNumber():
    void {

    const id =
      this.licenseId;


    const value =
      this.registrationNumberYear
        .trim();


    if (!id) {
      return;
    }


    if (
      !this.isRegistrationNumberValid(
        value
      )
    ) {

      this.errorMessage =
        'Registration Number/Year must contain two numbers separated by "/".';

      return;
    }


    if (
      value ===
      this.license
        ?.registrationNumberYear
    ) {
      return;
    }


    this.execute(
      this.service
        .updateRegistrationNumber(
          id,
          value
        )
    );
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


    if (
      typed !==
      selected
    ) {

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
  // Requirement Add
  // =========================================================

  openAddRequirementModal():
    void {

    if (
      !this.license ||
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
  // Requirement Edit
  // =========================================================

  openEditRequirementModal(
    requirement:
      LicenseRequirementResponse
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


  closeRequirementModal():
    void {

    if (this.saving) {
      return;
    }


    this.requirementModalOpen =
      false;


    this.resetRequirementForm();
  }


  // =========================================================
  // Requirement Initial Attachment
  // =========================================================

  onRequirementFileSelected(
    event: Event
  ): void {

  const input = event.target as HTMLInputElement;

  this.requirementFile = input.files?.[0] ?? null;

  input.value = '';
  }


  removeSelectedRequirementFile():
    void {

    this.requirementFile =
      null;
  }


  // =========================================================
  // Save Add / Edit Requirement
  // =========================================================

  saveRequirement(): void {

    const licenseId =
      this.licenseId;


    if (
      !licenseId ||
      this.saving
    ) {
      return;
    }


    const name =
      this.requirementName
        .trim();


    const notes =
      this.requirementNotes
        .trim();


    if (!name) {

      this.errorMessage =
        'Requirement name is required.';

      return;
    }


    if (
      !this.requirementDeadline
    ) {

      this.errorMessage =
        'Requirement deadline is required.';

      return;
    }


    if (
      !this.requirementDepartmentId
    ) {

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

      if (
        !this.editingRequirementId
      ) {
        return;
      }


      const requirementId =
        this.editingRequirementId;


      const input:
        UpdateLicenseRequirementInput = {

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
          licenseId,
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
              'Unable to update requirement.';
          }

        });


      return;
    }


    // =======================================================
    // Add Without Attachment
    // =======================================================

    if (
      !this.requirementFile
    ) {

      const input:
        AddLicenseRequirementInput = {

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
          licenseId,
          input
        )
        .subscribe({

          next: () => {

            this.requirementSaved();
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
              AddLicenseRequirementInput = {

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
                licenseId,
                input
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementSaved();
        },


        error: error => {

          this.requirementSaveFailed(
            error
          );
        }

      });
  }


  private requirementSaved():
    void {

    this.saving =
      false;


    this.requirementModalOpen =
      false;


    this.resetRequirementForm();


    this.requirementsExpanded =
      true;


    this.successMessage =
      'License requirement added successfully.';


    this.load();
  }


  private requirementSaveFailed(
    error: unknown
  ): void {

    console.error(
      'Unable to add license requirement',
      error
    );


    this.saving =
      false;


    this.errorMessage =
      'Unable to add license requirement.';
  }


  // =========================================================
  // Requirement Attachment Modal
  // =========================================================

  openRequirementAttachmentModal(
    requirement:
      LicenseRequirementResponse
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


  saveRequirementAttachment():
    void {

    const licenseId =
      this.licenseId;


    const requirement =
      this.requirementAttachmentTarget;


    const file =
      this.requirementAttachmentFile;


    if (
      !licenseId ||
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
                licenseId,
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
    requirement:
      LicenseRequirementResponse
  ): void {

    const licenseId =
      this.licenseId;


    if (
      !licenseId ||
      !requirement
        .requirementAttachment ||
      this.saving
    ) {
      return;
    }


    const confirmed =
      window.confirm(
        `Remove the attachment from "${requirement.name}"?`
      );


    if (!confirmed) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeRequirementAttachment(
        licenseId,
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
    requirement:
      LicenseRequirementResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.requirementApprovalTarget =
      requirement;


    this.requirementApprovalDate =
      requirement.approval
        ?.date ??
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


  saveRequirementApproval():
    void {

    const licenseId =
      this.licenseId;


    const requirement =
      this.requirementApprovalTarget;


    if (
      !licenseId ||
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


    // =======================================================
    // Reuse existing approval file
    // =======================================================

    if (
      !this.requirementApprovalFile
    ) {

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


      this.service
        .setRequirementApproval(
          licenseId,
          requirement.id,
          approval
        )
        .subscribe({

          next: () => {

            this.requirementApprovalSaved(
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


            return this.service
              .setRequirementApproval(
                licenseId,
                requirement.id,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.requirementApprovalSaved(
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
    requirement:
      LicenseRequirementResponse
  ): void {

    const licenseId =
      this.licenseId;


    if (
      !licenseId ||
      !requirement.approval ||
      this.saving
    ) {
      return;
    }


    const confirmed =
      window.confirm(
        `Remove the approval from "${requirement.name}"?`
      );


    if (!confirmed) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeRequirementApproval(
        licenseId,
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
            'Unable to remove requirement approval.';
        }

      });
  }


  // =========================================================
  // Delete Requirement
  // =========================================================

  deleteRequirement(
    requirement:
      LicenseRequirementResponse
  ): void {

    const licenseId =
      this.licenseId;


    if (
      !licenseId ||
      this.saving
    ) {
      return;
    }


    const confirmed =
      window.confirm(
        `Delete requirement "${requirement.name}"? This cannot be undone.`
      );


    if (!confirmed) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeRequirement(
        licenseId,
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


          this.load();
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
  // Leaflet / Layout
  // =========================================================

  openAttachmentModal(
    type:
      LicenseDocumentType
  ): void {

    if (
      !this.license ||
      this.saving
    ) {
      return;
    }


    this.approvedDocumentsExpanded =
      true;


    this.attachmentModalType =
      type;


    this.attachmentApprovalDate =
      '';


    this.attachmentNotes =
      '';


    this.attachmentFile =
      null;


    this.clearMessages();


    this.attachmentModalOpen =
      true;
  }


  closeAttachmentModal():
    void {

    if (this.saving) {
      return;
    }


    this.attachmentModalOpen =
      false;


    this.resetAttachmentForm();
  }


  onAttachmentFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.attachmentFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  saveApprovedDocument():
    void {

    const id =
      this.licenseId;


    const file =
      this.attachmentFile;


    const type =
      this.attachmentModalType;


    if (
      !id ||
      !file ||
      !type ||
      !this.attachmentApprovalDate
    ) {

      this.errorMessage =
        'Approval date and attachment are required.';

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

            const approval:
              DatedAttachmentInput = {

              date:
                this.attachmentApprovalDate,

              attachmentId:
                uploaded.attachmentId,

              fileName:
                uploaded.fileName,

              notes:
                this.attachmentNotes
                  .trim() ||
                null
            };


            if (
              type ===
              'leaflet'
            ) {

              return this.service
                .setLastApprovedLeaflet(
                  id,
                  approval
                );
            }


            return this.service
              .setLastApprovedLayout(
                id,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.attachmentModalOpen =
            false;


          this.approvedDocumentsExpanded =
            true;


          if (
            type ===
            'leaflet'
          ) {

            this.leafletExpanded =
              true;
          }
          else {

            this.layoutExpanded =
              true;
          }


          this.resetAttachmentForm();


          this.successMessage =
            type ===
            'leaflet'
              ? 'Last approved leaflet saved successfully.'
              : 'Last approved layout saved successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to save approved document',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to save approved document.';
        }

      });
  }


  // =========================================================
  // Populate / Reset
  // =========================================================

  private populateForm(
    license:
      ProductLicenseResponse
  ): void {

    this.licenseCategoryId =
      license
        .licenseCategory
        .id;


    this.marketingTypeId =
      license
        .marketingType
        .id;


    this.issueDate =
      license.issueDate;


    this.validityDate =
      license.validityDate;


    this.registrationNumberYear =
      license
        .registrationNumberYear;
  }


  private resetCreateForm():
    void {

    this.createLicenseCategoryId =
      null;


    this.createMarketingTypeId =
      null;


    this.createIssueDate =
      '';


    this.createValidityDate =
      '';


    this.createRegistrationNumberYear =
      '';
  }


  private resetEditForm():
    void {

    this.licenseCategoryId =
      null;


    this.marketingTypeId =
      null;


    this.issueDate =
      '';


    this.validityDate =
      '';


    this.registrationNumberYear =
      '';
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


    this.loadingDepartments =
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


  private resetAttachmentForm():
    void {

    this.attachmentModalType =
      null;


    this.attachmentApprovalDate =
      '';


    this.attachmentNotes =
      '';


    this.attachmentFile =
      null;
  }


  private clearMessages():
    void {

    this.errorMessage =
      '';


    this.successMessage =
      '';
  }


  // =========================================================
  // Shared Mutation
  // =========================================================

  private execute(
    request$:
      Observable<unknown>
  ): void {

    this.saving =
      true;


    this.clearMessages();


    request$
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.licenseInformationExpanded =
            true;


          this.successMessage =
            'Saved successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to save Product License',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to save changes.';
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

    if (
      !this.isPdf(fileName)
    ) {
      return;
    }


    this.closePdfViewer();


    this.pdfViewerOpen =
      true;


    this.pdfViewerLoading =
      true;


    this.pdfViewerFileName =
      fileName;


    this.errorMessage =
      '';


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


          this.pdfViewerLoading =
            false;


          this.pdfViewerOpen =
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


    if (
      this.pdfObjectUrl
    ) {

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
            'Unable to download attachment',
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
            () => {

              URL.revokeObjectURL(
                url
              );

            },
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
}