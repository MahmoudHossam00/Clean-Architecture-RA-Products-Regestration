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
  switchMap
} from 'rxjs';

import {
  ProductTechnicalDocsService
} from '../Services/product-technical-docs.service';

import {
  DatedApprovalDocumentInput,
  ProductTechnicalDocsResponse
} from '../Models/product-technical-docs.models';

import {
  AttachmentService
} from '../../../core/services/attachment.service';


export type TechnicalDocsTab =
  | 'specifications'
  | 'composition'
  | 'moa-validation';


@Component({
  selector:
    'app-product-technical-docs',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './product-technical-docs.component.html',

  styleUrl:
    './product-technical-docs.component.css'
})
export class ProductTechnicalDocsComponent
  implements OnInit, OnDestroy {

  // =========================================================
  // Dependencies
  // =========================================================

  private readonly route =
    inject(ActivatedRoute);

  private readonly service =
    inject(ProductTechnicalDocsService);

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

  technicalDocs:
    ProductTechnicalDocsResponse | null =
      null;


  technicalDocsMissing =
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
  // Tabs
  // =========================================================

  activeTab:
    TechnicalDocsTab =
      'specifications';


  setActiveTab(
    tab: TechnicalDocsTab
  ): void {

    if (
      this.creationModalOpen ||
      this.saving
    ) {
      return;
    }

    this.activeTab =
      tab;
  }


  // =========================================================
  // Creation Modal
  // =========================================================

  creationModalOpen =
    false;


  openCreationModal(): void {

    if (
      !this.technicalDocs ||
      this.saving
    ) {
      return;
    }


    this.errorMessage =
      '';

    this.successMessage =
      '';


    /*
     * Start with a clean form every time
     * the modal is opened.
     */
    this.resetApprovalForms();


    this.creationModalOpen =
      true;
  }


  closeCreationModal(): void {

    if (this.saving) {
      return;
    }


    this.creationModalOpen =
      false;


    this.resetApprovalForms();
  }


  // =========================================================
  // Specifications Form
  // =========================================================

  specificationsApprovalDate =
    '';


  specificationsNotes =
    '';


  specificationsFile:
    File | null =
      null;


  // =========================================================
  // Composition Form
  // =========================================================

  compositionApprovalDate =
    '';


  compositionNotes =
    '';


  compositionFile:
    File | null =
      null;


  // =========================================================
  // MOA + Validation Form
  // =========================================================

  moaValidationApprovalDate =
    '';


  moaValidationNotes =
    '';


  moaValidationFile:
    File | null =
      null;


  // =========================================================
  // Lifecycle
  // =========================================================

  ngOnInit(): void {

    this.load();
  }


  ngOnDestroy(): void {

    this.closePdfViewer();
  }


  // =========================================================
  // Technical Docs Id
  // =========================================================

  get technicalDocsId():
    string | null {

    return (
      this.technicalDocs?.id ??
      null
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


    this.technicalDocsMissing =
      false;


    this.service
      .getByProductId(
        this.productId
      )
      .subscribe({

        next: response => {

          this.technicalDocs =
            response;


          this.technicalDocsMissing =
            false;


          this.loading =
            false;
        },


        error: (
          error: HttpErrorResponse
        ) => {

          console.error(
            'Unable to load Product Technical Docs',
            error
          );


          this.loading =
            false;


          if (error.status === 404) {

            this.technicalDocs =
              null;


            this.technicalDocsMissing =
              true;


            return;
          }


          this.errorMessage =
            'Unable to load Product Technical Docs.';
        }

      });
  }


  // =========================================================
  // Create Product Technical Docs
  // =========================================================

  createTechnicalDocs(): void {

    if (
      !this.productId ||
      this.saving
    ) {
      return;
    }


    this.saving =
      true;


    this.errorMessage =
      '';


    this.successMessage =
      '';


    this.service
      .create(
        this.productId
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.technicalDocsMissing =
            false;


          this.successMessage =
            'Product Technical Docs created successfully.';


          this.load();
        },


        error: (
          error: HttpErrorResponse
        ) => {

          console.error(
            'Unable to create Product Technical Docs',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to create Product Technical Docs.';
        }

      });
  }


  // =========================================================
  // Specifications File Selection
  // =========================================================

  onSpecificationsFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.specificationsFile =
      input.files?.[0] ??
      null;


    /*
     * Clear the native input value.
     *
     * This allows the same file to be selected
     * again later and still fire the change event.
     */
    input.value =
      '';
  }


  // =========================================================
  // Add Approved Specifications
  // =========================================================

  addApprovedSpecifications(): void {

    const id =
      this.technicalDocsId;


    const file =
      this.specificationsFile;


    if (
      !id ||
      !file ||
      !this.specificationsApprovalDate
    ) {

      this.errorMessage =
        'Approval date and specifications document are required.';

      return;
    }


    this.saving =
      true;


    this.errorMessage =
      '';


    this.successMessage =
      '';


    this.attachmentService
      .upload(
        file
      )
      .pipe(

        switchMap(
          uploaded => {

            const document:
              DatedApprovalDocumentInput = {

              approvalDate:
                this.specificationsApprovalDate,

              attachment: {

                attachmentId:
                  uploaded.attachmentId,

                fileName:
                  uploaded.fileName,

                notes:
                  this.specificationsNotes.trim()
                    ? this.specificationsNotes.trim()
                    : null
              }
            };


            return this.service
              .addApprovedSpecifications(
                id,
                document
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.creationModalOpen =
            false;


          this.resetApprovalForms();


          this.successMessage =
            'Approved specifications added successfully.';


          this.load();
        },


        error: (
          error: unknown
        ) => {

          console.error(
            'Unable to add approved specifications',
            error
          );


          this.saving =
            false;


          /*
           * Keep modal open so user does not
           * lose the selected values.
           */
          this.errorMessage =
            'Unable to add approved specifications.';
        }

      });
  }


  // =========================================================
  // Composition File Selection
  // =========================================================

  onCompositionFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.compositionFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  // =========================================================
  // Add Approved Composition
  // =========================================================

  addApprovedComposition(): void {

    const id =
      this.technicalDocsId;


    const file =
      this.compositionFile;


    if (
      !id ||
      !file ||
      !this.compositionApprovalDate
    ) {

      this.errorMessage =
        'Approval date and composition document are required.';

      return;
    }


    this.saving =
      true;


    this.errorMessage =
      '';


    this.successMessage =
      '';


    this.attachmentService
      .upload(
        file
      )
      .pipe(

        switchMap(
          uploaded => {

            const document:
              DatedApprovalDocumentInput = {

              approvalDate:
                this.compositionApprovalDate,

              attachment: {

                attachmentId:
                  uploaded.attachmentId,

                fileName:
                  uploaded.fileName,

                notes:
                  this.compositionNotes.trim()
                    ? this.compositionNotes.trim()
                    : null
              }
            };


            return this.service
              .addApprovedComposition(
                id,
                document
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.creationModalOpen =
            false;


          this.resetApprovalForms();


          this.successMessage =
            'Approved composition added successfully.';


          this.load();
        },


        error: (
          error: unknown
        ) => {

          console.error(
            'Unable to add approved composition',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to add approved composition.';
        }

      });
  }


  // =========================================================
  // MOA + Validation File Selection
  // =========================================================

  onMoaValidationFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.moaValidationFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  // =========================================================
  // Add Approved MOA + Validation
  // =========================================================

  addApprovedMoaValidation(): void {

    const id =
      this.technicalDocsId;


    const file =
      this.moaValidationFile;


    if (
      !id ||
      !file ||
      !this.moaValidationApprovalDate
    ) {

      this.errorMessage =
        'Approval date and MOA + Validation document are required.';

      return;
    }


    this.saving =
      true;


    this.errorMessage =
      '';


    this.successMessage =
      '';


    this.attachmentService
      .upload(
        file
      )
      .pipe(

        switchMap(
          uploaded => {

            const document:
              DatedApprovalDocumentInput = {

              approvalDate:
                this.moaValidationApprovalDate,

              attachment: {

                attachmentId:
                  uploaded.attachmentId,

                fileName:
                  uploaded.fileName,

                notes:
                  this.moaValidationNotes.trim()
                    ? this.moaValidationNotes.trim()
                    : null
              }
            };


            return this.service
              .addApprovedMoaValidation(
                id,
                document
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.creationModalOpen =
            false;


          this.resetApprovalForms();


          this.successMessage =
            'Approved MOA + Validation added successfully.';


          this.load();
        },


        error: (
          error: unknown
        ) => {

          console.error(
            'Unable to add approved MOA + Validation',
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to add approved MOA + Validation.';
        }

      });
  }


  // =========================================================
  // Reset Approval Forms
  // =========================================================

  private resetApprovalForms(): void {

    // =======================================================
    // Specifications
    // =======================================================

    this.specificationsApprovalDate =
      '';


    this.specificationsNotes =
      '';


    this.specificationsFile =
      null;


    // =======================================================
    // Composition
    // =======================================================

    this.compositionApprovalDate =
      '';


    this.compositionNotes =
      '';


    this.compositionFile =
      null;


    // =======================================================
    // MOA + Validation
    // =======================================================

    this.moaValidationApprovalDate =
      '';


    this.moaValidationNotes =
      '';


    this.moaValidationFile =
      null;
  }


  // =========================================================
  // Download Attachment
  // =========================================================

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


          document.body.appendChild(
            anchor
          );


          anchor.click();


          anchor.remove();


          URL.revokeObjectURL(
            url
          );
        },


        error: (
          error: unknown
        ) => {

          console.error(
            'Unable to download attachment',
            error
          );


          this.errorMessage =
            `Unable to download "${fileName}".`;
        }

      });
  }


  // =========================================================
  // Open Non-PDF Attachment
  // =========================================================

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


          /*
           * Keep the object URL alive long enough
           * for the newly opened browser tab.
           */
          setTimeout(
            () => {

              URL.revokeObjectURL(
                url
              );

            },
            60_000
          );
        },


        error: (
          error: unknown
        ) => {

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
  // PDF Viewer State
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
  // Is PDF
  // =========================================================

  isPdf(
    fileName: string
  ): boolean {

    return fileName
      .toLowerCase()
      .endsWith('.pdf');
  }


  // =========================================================
  // Preview PDF
  // =========================================================

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


        error: (
          error: unknown
        ) => {

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


  // =========================================================
  // Close PDF Viewer
  // =========================================================

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
}