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
  switchMap
} from 'rxjs';

import {
  PromotionalMaterialService
} from '../services/promotional-material.service';

import {
  PromotionalMaterialTypeService
} from '../services/promotional-material-type.service';

import {
  AttachmentService
} from '../../../core/services/attachment.service';

import {
  CreatePromotionalMaterialInput,
  LookupResponse,
  PromotionalMaterialResponse,
  SetPromotionalMaterialApprovalInput
} from '../models/promotional-material.models';

import {
  CreatableLookupComponent
} from '../../../core/components/creatable-lookup/creatable-lookup.component';


export type PromotionalMaterialStatus =
  | 'Approved'
  | 'Pending'
  | 'Expired';


@Component({
  selector:
    'app-product-promotional-materials',

  standalone:
    true,

  imports: [
    CommonModule,
    FormsModule,
    CreatableLookupComponent
  ],

  templateUrl:
    './product-promotional-materials.component.html',

  styleUrl:
    './product-promotional-materials.component.css'
})
export class ProductPromotionalMaterialsComponent
  implements OnInit, OnDestroy {

  // =========================================================
  // Dependencies
  // =========================================================

  private readonly route =
    inject(ActivatedRoute);


  private readonly service =
    inject(PromotionalMaterialService);


  private readonly typeService =
    inject(PromotionalMaterialTypeService);


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

  materials:
    PromotionalMaterialResponse[] =
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
  // Expand / Collapse
  // =========================================================

  materialsSectionExpanded =
    true;


  expandedMaterialIds =
    new Set<string>();


  // =========================================================
  // Types
  // =========================================================

  promotionalMaterialTypes:
    LookupResponse[] =
      [];


  loadingTypes =
    false;


  creatingType =
    false;


  materialTypeSelections:
    Record<
      string,
      string | null
    > = {};


  // =========================================================
  // Create Material
  // =========================================================

  createModalOpen =
    false;


  createTypeId:
    string | null =
      null;


  createFile:
    File | null =
      null;


  createFileNotes =
    '';


  // =========================================================
  // Replace Material File
  // =========================================================

  replaceFileModalOpen =
    false;


  replaceFileTarget:
    PromotionalMaterialResponse | null =
      null;


  replaceFile:
    File | null =
      null;


  replaceFileNotes =
    '';


  // =========================================================
  // Approval
  // =========================================================

  approvalModalOpen =
    false;


  approvalTarget:
    PromotionalMaterialResponse | null =
      null;


  approvalDate =
    '';


  approvalExpiryDate =
    '';


  approvalFile:
    File | null =
      null;


  approvalNotes =
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

    this.loadTypes();

    this.load();
  }


  ngOnDestroy(): void {

    this.closePdfViewer();
  }


  // =========================================================
  // Counts
  // =========================================================

  get pendingCount():
    number {

    return this.materials
      .filter(
        material =>
          !material.approval
      )
      .length;
  }


  get expiredCount():
    number {

    return this.materials
      .filter(
        material =>
          material.approval &&
          this.isExpired(
            material.approvalExpiryDate
          )
      )
      .length;
  }


  get approvedCount():
    number {

    return this.materials
      .filter(
        material =>
          material.approval &&
          !this.isExpired(
            material.approvalExpiryDate
          )
      )
      .length;
  }


  get allMaterialsExpanded():
    boolean {

    return (
      this.materials.length > 0 &&
      this.materials.every(
        material =>
          this.expandedMaterialIds
            .has(
              material.id
            )
      )
    );
  }


  // =========================================================
  // Status
  // =========================================================

  getStatus(
    material:
      PromotionalMaterialResponse
  ): PromotionalMaterialStatus {

    if (!material.approval) {

      return 'Pending';
    }


    if (
      this.isExpired(
        material.approvalExpiryDate
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
  // Expand / Collapse
  // =========================================================

  toggleMaterialsSection():
    void {

    this.materialsSectionExpanded =
      !this.materialsSectionExpanded;
  }


  isMaterialExpanded(
    materialId: string
  ): boolean {

    return this.expandedMaterialIds
      .has(
        materialId
      );
  }


  toggleMaterial(
    materialId: string
  ): void {

    if (
      this.expandedMaterialIds
        .has(
          materialId
        )
    ) {

      this.expandedMaterialIds
        .delete(
          materialId
        );

      return;
    }


    this.expandedMaterialIds
      .add(
        materialId
      );
  }


  toggleAllMaterials():
    void {

    if (
      !this.materials.length
    ) {
      return;
    }


    if (
      this.allMaterialsExpanded
    ) {

      this.materials
        .forEach(
          material => {

            this.expandedMaterialIds
              .delete(
                material.id
              );
          }
        );

      return;
    }


    this.materials
      .forEach(
        material => {

          this.expandedMaterialIds
            .add(
              material.id
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

        next: materials => {

          this.materials =
            materials ?? [];


          this.materialTypeSelections =
            {};


          this.materials
            .forEach(
              material => {

                this.materialTypeSelections[
                  material.id
                ] =
                  material.type.id;
              }
            );


          this.removeInvalidExpandedMaterials();


          this.loading =
            false;
        },


        error: error => {

          console.error(
            'Unable to load promotional materials',
            error
          );


          this.loading =
            false;


          this.errorMessage =
            'Unable to load promotional materials.';
        }

      });
  }


  private removeInvalidExpandedMaterials():
    void {

    const validIds =
      new Set(
        this.materials
          .map(
            x => x.id
          )
      );


    for (
      const id
      of this.expandedMaterialIds
    ) {

      if (!validIds.has(id)) {

        this.expandedMaterialIds
          .delete(id);
      }
    }
  }


  // =========================================================
  // Types
  // =========================================================

  loadTypes(): void {

    this.loadingTypes =
      true;


    this.typeService
      .getAll('')
      .subscribe({

        next: items => {

          this.promotionalMaterialTypes =
            items;


          this.loadingTypes =
            false;
        },


        error: error => {

          console.error(
            'Unable to load promotional material types',
            error
          );


          this.promotionalMaterialTypes =
            [];


          this.loadingTypes =
            false;
        }

      });
  }


  createPromotionalMaterialType(
    name: string,
    materialId?: string
  ): void {

    const normalized =
      name.trim();


    if (
      !normalized ||
      this.creatingType
    ) {
      return;
    }


    this.creatingType =
      true;


    this.clearMessages();


    this.typeService
      .create(
        normalized
      )
      .subscribe({

        next: () => {

          this.typeService
            .getAll('')
            .subscribe({

              next: items => {

                this.promotionalMaterialTypes =
                  items;


                this.creatingType =
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
                    'Promotional material type was created but could not be selected.';

                  return;
                }


                // Creating new material

                if (!materialId) {

                  this.createTypeId =
                    created.id;

                  return;
                }


                // Existing material:
                // select + immediately save new type

                this.materialTypeSelections[
                  materialId
                ] =
                  created.id;


                this.saveMaterialType(
                  materialId
                );
              },


              error: error => {

                console.error(
                  'Unable to reload promotional material types',
                  error
                );


                this.creatingType =
                  false;


                this.errorMessage =
                  'Type was created but could not be reloaded.';
              }

            });
        },


        error: error => {

          console.error(
            'Unable to create promotional material type',
            error
          );


          this.creatingType =
            false;


          this.errorMessage =
            `Unable to create promotional material type "${normalized}".`;
        }

      });
  }


  // =========================================================
  // Update Existing Type
  // =========================================================

  saveMaterialType(
    materialId: string
  ): void {

    const material =
      this.materials.find(
        x =>
          x.id ===
          materialId
      );


    const selectedId =
      this.materialTypeSelections[
        materialId
      ];


    if (
      !material ||
      !selectedId ||
      selectedId ===
        material.type.id ||
      this.saving
    ) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .updateType(
        materialId,
        selectedId
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedMaterialIds
            .add(
              materialId
            );


          this.successMessage =
            'Promotional material type updated successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to update promotional material type',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to update promotional material type.';
        }

      });
  }


  // =========================================================
  // Create Material Modal
  // =========================================================

  openCreateModal(): void {

    if (this.saving) {
      return;
    }


    this.resetCreateForm();


    this.clearMessages();


    this.createModalOpen =
      true;


    this.materialsSectionExpanded =
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


  onCreateFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.createFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  removeCreateFile():
    void {

    this.createFile =
      null;
  }


  createMaterial(): void {

    if (
      !this.productId ||
      this.saving
    ) {
      return;
    }


    if (!this.createTypeId) {

      this.errorMessage =
        'Promotional material type is required.';

      return;
    }


    if (!this.createFile) {

      this.errorMessage =
        'Promotional material file is required.';

      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.attachmentService
      .upload(
        this.createFile
      )
      .pipe(

        switchMap(
          uploaded => {

            const input:
              CreatePromotionalMaterialInput = {

              productId:
                this.productId,

              promotionalMaterialTypeId:
                this.createTypeId!,

              materialFile: {

                attachmentId:
                  uploaded.attachmentId,

                fileName:
                  uploaded.fileName,

                notes:
                  this.createFileNotes
                    .trim() ||
                  null
              }
            };


            return this.service
              .create(
                input
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.createModalOpen =
            false;


          this.resetCreateForm();


          this.materialsSectionExpanded =
            true;


          this.successMessage =
            'Promotional material created successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to create promotional material',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to create promotional material.';
        }

      });
  }


  // =========================================================
  // Replace Material File
  // =========================================================

  openReplaceFileModal(
    material:
      PromotionalMaterialResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.replaceFileTarget =
      material;


    this.replaceFile =
      null;


    this.replaceFileNotes =
      material
        .materialFile
        .notes ??
      '';


    this.clearMessages();


    this.replaceFileModalOpen =
      true;
  }


  closeReplaceFileModal():
    void {

    if (this.saving) {
      return;
    }


    this.replaceFileModalOpen =
      false;


    this.resetReplaceFileForm();
  }


  onReplaceFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.replaceFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  saveReplacementFile():
    void {

    const material =
      this.replaceFileTarget;


    const file =
      this.replaceFile;


    if (
      !material ||
      !file
    ) {

      this.errorMessage =
        'Please select a material file.';

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
              .replaceMaterialFile(
                material.id,
                {
                  materialFile: {

                    attachmentId:
                      uploaded.attachmentId,

                    fileName:
                      uploaded.fileName,

                    notes:
                      this.replaceFileNotes
                        .trim() ||
                      null
                  }
                }
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.replaceFileModalOpen =
            false;


          this.resetReplaceFileForm();


          this.expandedMaterialIds
            .add(
              material.id
            );


          this.successMessage =
            'Promotional material file replaced successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to replace promotional material file',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to replace promotional material file.';
        }

      });
  }


  // =========================================================
  // Approval Modal
  // =========================================================

  openApprovalModal(
    material:
      PromotionalMaterialResponse
  ): void {

    if (this.saving) {
      return;
    }


    this.approvalTarget =
      material;


    this.approvalDate =
      material.approval?.date ??
      '';


    this.approvalExpiryDate =
      material.approvalExpiryDate ??
      '';


    this.approvalNotes =
      material.approval
        ?.attachment
        .notes ??
      '';


    this.approvalFile =
      null;


    this.clearMessages();


    this.approvalModalOpen =
      true;
  }


  closeApprovalModal():
    void {

    if (this.saving) {
      return;
    }


    this.approvalModalOpen =
      false;


    this.resetApprovalForm();
  }


  onApprovalFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.approvalFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  saveApproval(): void {

    const material =
      this.approvalTarget;


    if (!material) {
      return;
    }


    if (!this.approvalDate) {

      this.errorMessage =
        'Approval date is required.';

      return;
    }


    if (
      !this.approvalExpiryDate
    ) {

      this.errorMessage =
        'Approval expiry date is required.';

      return;
    }


    if (
      this.approvalExpiryDate <
      this.approvalDate
    ) {

      this.errorMessage =
        'Approval expiry date cannot be before approval date.';

      return;
    }


    if (
      !material.approval &&
      !this.approvalFile
    ) {

      this.errorMessage =
        'Approval attachment is required.';

      return;
    }


    this.saving =
      true;


    this.clearMessages();


    // =======================================================
    // Keep Current Approval File
    // =======================================================

    if (!this.approvalFile) {

      const existing =
        material.approval;


      if (!existing) {

        this.saving =
          false;

        return;
      }


      const input:
        SetPromotionalMaterialApprovalInput = {

        approval: {

          date:
            this.approvalDate,

          attachmentId:
            existing
              .attachment
              .attachmentId,

          fileName:
            existing
              .attachment
              .fileName,

          notes:
            this.approvalNotes
              .trim() ||
            null
        },

        approvalExpiryDate:
          this.approvalExpiryDate
      };


      this.service
        .setApproval(
          material.id,
          input
        )
        .subscribe({

          next: () => {

            this.approvalSaved(
              material.id
            );
          },


          error: error => {

            this.approvalFailed(
              error
            );
          }

        });


      return;
    }


    // =======================================================
    // Upload New Approval File
    // =======================================================

    this.attachmentService
      .upload(
        this.approvalFile
      )
      .pipe(

        switchMap(
          uploaded => {

            const input:
              SetPromotionalMaterialApprovalInput = {

              approval: {

                date:
                  this.approvalDate,

                attachmentId:
                  uploaded.attachmentId,

                fileName:
                  uploaded.fileName,

                notes:
                  this.approvalNotes
                    .trim() ||
                  null
              },

              approvalExpiryDate:
                this.approvalExpiryDate
            };


            return this.service
              .setApproval(
                material.id,
                input
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.approvalSaved(
            material.id
          );
        },


        error: error => {

          this.approvalFailed(
            error
          );
        }

      });
  }


  private approvalSaved(
    materialId: string
  ): void {

    this.saving =
      false;


    this.approvalModalOpen =
      false;


    this.resetApprovalForm();


    this.expandedMaterialIds
      .add(
        materialId
      );


    this.successMessage =
      'Promotional material approval saved successfully.';


    this.load();
  }


  private approvalFailed(
    error: unknown
  ): void {

    console.error(
      'Unable to save promotional material approval',
      error
    );


    this.saving =
      false;


    this.errorMessage =
      'Unable to save promotional material approval.';
  }


  // =========================================================
  // Remove Approval
  // =========================================================

  removeApproval(
    material:
      PromotionalMaterialResponse
  ): void {

    if (
      !material.approval ||
      this.saving
    ) {
      return;
    }


    const confirmed =
      window.confirm(
        `Remove approval from "${material.type.name}"?`
      );


    if (!confirmed) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .removeApproval(
        material.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedMaterialIds
            .add(
              material.id
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
  // Delete
  // =========================================================

  deleteMaterial(
    material:
      PromotionalMaterialResponse
  ): void {

    if (this.saving) {
      return;
    }


    const confirmed =
      window.confirm(
        `Delete promotional material "${material.type.name}"?\n\nThis cannot be undone.`
      );


    if (!confirmed) {
      return;
    }


    this.saving =
      true;


    this.clearMessages();


    this.service
      .remove(
        material.id
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.expandedMaterialIds
            .delete(
              material.id
            );


          this.successMessage =
            'Promotional material deleted successfully.';


          this.load();
        },


        error: error => {

          console.error(
            'Unable to delete promotional material',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to delete promotional material.';
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

  private resetCreateForm():
    void {

    this.createTypeId =
      null;


    this.createFile =
      null;


    this.createFileNotes =
      '';
  }


  private resetReplaceFileForm():
    void {

    this.replaceFileTarget =
      null;


    this.replaceFile =
      null;


    this.replaceFileNotes =
      '';
  }


  private resetApprovalForm():
    void {

    this.approvalTarget =
      null;


    this.approvalDate =
      '';


    this.approvalExpiryDate =
      '';


    this.approvalFile =
      null;


    this.approvalNotes =
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