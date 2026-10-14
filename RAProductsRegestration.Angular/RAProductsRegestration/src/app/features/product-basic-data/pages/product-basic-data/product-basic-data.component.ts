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
  HttpErrorResponse
} from '@angular/common/http';

import {
  Observable,
  of,
  switchMap
} from 'rxjs';

import {
  ProductBasicDataService
} from '../../services/product-basic-data.service';

import {
  AttachmentService
} from '../../../../core/services/attachment.service';

import {
  DosageFormService
} from '../../services/dosage-form.service';

import {
  StorageConditionService
} from '../../services/storage-condition.service';

import {
  ActiveIngredientService
} from '../../../ActiveIngredients/services/activeingredient.service';

import {
  SupplierService
} from '../../../Suppliers/services/supplier.service';

import {
  SupplierLookupResponse
} from '../../../Suppliers/models/supplier.models';

import {
  ActiveIngredientLookupResponse,
  DosageFormLookupResponse,
  StorageConditionLookupResponse
} from '../../models/lookups.models';

import {
  ApprovedPriceResponse,
  AttachmentReferenceInput,
  DatedAttachmentInput,
  FulfillmentDeadlineHistoryResponse,
  FulfillmentDeadlineInput,
  ProductActiveIngredientResponse,
  ProductBasicDataResponse,
  ShelfLifeHistoryResponse,
  ShelfLifeInput
} from '../../models/product-basic-data.models';

import {
  CreatableLookupComponent
} from '../../../../core/components/creatable-lookup/creatable-lookup.component';


export type ProductBasicDataTab =
  | 'general'
  | 'shelf-life'
  | 'prices'
  | 'active-ingredients'
  | 'approvals'
  | 'deadlines';


export type ProductBasicDataExpandableSection =
  | 'general-information'
  | 'shelf-life-summary'
  | 'shelf-life-details'
  | 'shelf-life-documents'
  | 'shelf-life-add-document'
  | 'prices-current'
  | 'prices-history'
  | 'prices-add'
  | 'active-ingredients-list'
  | 'approvals-summary'
  | 'approvals-packs'
  | 'approvals-add-pack'
  | 'approvals-manufacturers'
  | 'approvals-add-manufacturer'
  | 'deadlines-pv'
  | 'deadlines-pv-add'
  | 'deadlines-pv-history'
  | 'deadlines-decree600'
  | 'deadlines-decree600-add'
  | 'deadlines-decree600-history';


@Component({
  selector: 'app-product-basic-data',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    CreatableLookupComponent
  ],

  templateUrl:
    './product-basic-data.component.html',

  styleUrl:
    './product-basic-data.component.css'
})
export class ProductBasicDataComponent
  implements OnInit, OnDestroy {

  // =========================================================
  // Dependencies
  // =========================================================

  private readonly route =
    inject(ActivatedRoute);

  private readonly service =
    inject(ProductBasicDataService);

  private readonly attachmentService =
    inject(AttachmentService);

  private readonly dosageFormService =
    inject(DosageFormService);

  private readonly storageConditionService =
    inject(StorageConditionService);

  private readonly activeIngredientService =
    inject(ActiveIngredientService);

  private readonly supplierService =
    inject(SupplierService);

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
  // Page State
  // =========================================================

  activeTab:
    ProductBasicDataTab =
      'general';

  readonly tabOrder: ProductBasicDataTab[] = [
    'general', 'shelf-life', 'prices',
    'active-ingredients', 'approvals', 'deadlines'
  ];

  readonly tabInformation: Record<ProductBasicDataTab, { title: string; description: string }> = {
    general: {
      title: 'Product identity',
      description: 'Review the product name, dosage form and storage details. Save each field after editing.'
    },
    'shelf-life': {
      title: 'Shelf life & supporting documents',
      description: 'Review the current shelf life, update its details and manage supporting attachments.'
    },
    prices: {
      title: 'Approved prices',
      description: 'See the current approval, compare previous prices or add a new price record.'
    },
    'active-ingredients': {
      title: 'Active ingredients',
      description: 'Review the product composition. Open an ingredient to manage its details and suppliers.'
    },
    approvals: {
      title: 'Packs & manufacturers',
      description: 'Keep approved packaging, manufacturers and their supporting documents together.'
    },
    deadlines: {
      title: 'Regulatory deadlines',
      description: 'Track PV and Decree 600 requirements, record fulfillment and review previous versions.'
    }
  };

  get activeTabInformation(): { title: string; description: string } {
    return this.tabInformation[this.activeTab];
  }

  get activeTabNumber(): number {
    return this.tabOrder.indexOf(this.activeTab) + 1;
  }

  onTabKeydown(event: KeyboardEvent): void {
    if (!['ArrowRight', 'ArrowLeft', 'ArrowDown', 'ArrowUp', 'Home', 'End'].includes(event.key)) {
      return;
    }

    event.preventDefault();
    const current = this.tabOrder.indexOf(this.activeTab);
    const direction = ['ArrowRight', 'ArrowDown'].includes(event.key) ? 1 : -1;
    const next = event.key === 'Home' ? 0
      : event.key === 'End' ? this.tabOrder.length - 1
      : (current + direction + this.tabOrder.length) % this.tabOrder.length;

    this.setActiveTab(this.tabOrder[next]);
    (event.currentTarget as HTMLElement)
      .querySelectorAll<HTMLButtonElement>('[role="tab"]')[next]?.focus();
  }

  loading =
    false;

  saving =
    false;

  errorMessage =
    '';

  successMessage =
    '';

  basicDataMissing =
    false;

  createTradeName =
    '';

  basicData:
    ProductBasicDataResponse | null =
      null;


  // =========================================================
  // Expansion / Orientation State
  // =========================================================

  readonly expandedSections =
    new Set<ProductBasicDataExpandableSection>([
      'general-information',
      'shelf-life-summary',
      'shelf-life-details',
      'prices-current',
      'active-ingredients-list',
      'approvals-summary',
      'deadlines-pv',
      'deadlines-decree600'
    ]);


  private readonly sectionsByTab:
    Record<
      ProductBasicDataTab,
      ProductBasicDataExpandableSection[]
    > = {

    general: [
      'general-information'
    ],

    'shelf-life': [
      'shelf-life-summary',
      'shelf-life-details',
      'shelf-life-documents',
      'shelf-life-add-document'
    ],

    prices: [
      'prices-current',
      'prices-history',
      'prices-add'
    ],

    'active-ingredients': [
      'active-ingredients-list'
    ],

    approvals: [
      'approvals-summary',
      'approvals-packs',
      'approvals-add-pack',
      'approvals-manufacturers',
      'approvals-add-manufacturer'
    ],

    deadlines: [
      'deadlines-pv',
      'deadlines-pv-add',
      'deadlines-pv-history',
      'deadlines-decree600',
      'deadlines-decree600-add',
      'deadlines-decree600-history'
    ]
  };


  readonly expandedIngredientIds =
    new Set<string>();

  readonly expandedApprovedPackIds =
    new Set<string>();

  readonly expandedManufacturerIds =
    new Set<string>();


  // =========================================================
  // General
  // =========================================================

  tradeName =
    '';

  physicalDescription =
    '';

  dosageFormId:
    string | null =
      null;

  storageConditionId:
    string | null =
      null;


  dosageForms:
    DosageFormLookupResponse[] =
      [];

  storageConditions:
    StorageConditionLookupResponse[] =
      [];

  creatingDosageForm =
    false;

  creatingStorageCondition =
    false;

  creatingIngredientStorageCondition =
    false;


  // =========================================================
  // Shelf Life
  // =========================================================

  shelfLifeNumberOfMonths:
    number | null =
      null;

  shelfLifeApprovalDate =
    '';

  shelfLifeAttachments:
    AttachmentReferenceInput[] =
      [];

  shelfLifeFile:
    File | null =
      null;

  shelfLifeNotes =
    '';

  shelfLifeUploading =
    false;


  // =========================================================
  // Prices
  // =========================================================

  newPrice:
    number | null =
      null;

  newPricePerDescription =
    '';

  newPriceIssueDate =
    '';

  newPriceExpiryDate =
    '';


  // =========================================================
  // Product Approved Pack
  // =========================================================

  newApprovedPackDescription =
    '';

  newApprovedPackApprovalDate =
    '';

  newApprovedPackNotes =
    '';

  newApprovedPackFile:
    File | null =
      null;


  // =========================================================
  // Approved Manufacturer
  // =========================================================

  newManufacturerName =
    '';

  newManufacturerApprovalDate =
    '';

  newManufacturerNotes =
    '';

  newManufacturerFile:
    File | null =
      null;


  // =========================================================
  // PV - New Version
  // =========================================================

  pvDescription =
    '';

  pvDeadline =
    '';

  pvNotes =
    '';

  pvFulfillmentDate =
    '';



  // =========================================================
  // Decree 600 - New Version
  // =========================================================

  decree600Description =
    '';

  decree600Deadline =
    '';

  decree600Notes =
    '';

  decree600FulfillmentDate =
    '';



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
  // Add Active Ingredient
  // =========================================================

  addIngredientModalOpen =
    false;

  activeIngredientOptions:
    ActiveIngredientLookupResponse[] =
      [];

  activeIngredientSearch =
    '';

  activeIngredientFilter:
    'available' |
    'all' |
    'added' =
      'available';

  activeIngredientSort:
    'asc' |
    'desc' =
      'asc';

  selectedActiveIngredientId:
    string | null =
      null;

  newActiveIngredientStrength =
    '';

  loadingActiveIngredients =
    false;

  activeIngredientTotalCount =
    0;

  activeIngredientPageNumber =
    1;

  activeIngredientPageSize =
    30;


  // =========================================================
  // Active Ingredient Details
  // =========================================================

  ingredientDetailsModalOpen =
    false;

  ingredientDetailsTab:
    'general' |
    'shelf-life' |
    'packs' |
    'suppliers' =
      'general';

  selectedIngredientDetails:
    ProductActiveIngredientResponse | null =
      null;


  // =========================================================
  // Ingredient General
  // =========================================================

  ingredientEditStrength =
    '';

  ingredientEditStorageConditionId:
    string | null =
      null;

  ingredientEditPhysicalDescription =
    '';


  // =========================================================
  // Ingredient Shelf Life
  // =========================================================

  ingredientShelfLifeNumberOfMonths:
    number | null =
      null;

  ingredientShelfLifeApprovalDate =
    '';

  ingredientShelfLifeAttachments:
    AttachmentReferenceInput[] =
      [];

  ingredientShelfLifeNotes =
    '';

  ingredientShelfLifeFile:
    File | null =
      null;

  ingredientShelfLifeUploading =
    false;


  // =========================================================
  // Ingredient Approved Pack
  // =========================================================

  ingredientPackDescription =
    '';

  ingredientPackApprovalDate =
    '';

  ingredientPackNotes =
    '';

  ingredientPackFile:
    File | null =
      null;


  // =========================================================
  // Supplier Lookup
  // =========================================================

  supplierOptions:
    SupplierLookupResponse[] =
      [];

  supplierSearch =
    '';

  supplierLoading =
    false;

  supplierPageNumber =
    1;

  supplierTotalCount =
    0;

  selectedSupplierId:
    string | null =
      null;


  // =========================================================
  // Lifecycle
  // =========================================================

  ngOnInit(): void {

    this.loadLookups();

    this.load();
  }


  ngOnDestroy(): void {

    this.closePdfViewer();
  }


  // =========================================================
  // Basic Helpers
  // =========================================================

  get basicDataId():
    string | null {

    return this.basicData?.id ??
      null;
  }


  setActiveTab(
    tab: ProductBasicDataTab
  ): void {

    this.activeTab =
      tab;
  }


  // =========================================================
  // Current Approved Price
  // =========================================================

  get currentApprovedPrice():
    ApprovedPriceResponse | null {

    const prices =
      this.basicData?.approvedPrices ??
      [];


    if (
      prices.length === 0
    ) {
      return null;
    }


    return prices.reduce(
      (
        latest,
        current
      ) =>
        current.issueDate >
        latest.issueDate
          ? current
          : latest
    );
  }


  // =========================================================
  // PV / Decree History Helpers
  // =========================================================

  get sortedPVHistory():
    FulfillmentDeadlineHistoryResponse[] {

    return [
      ...(this.basicData?.pvHistory ?? [])
    ].sort(
      (
        a,
        b
      ) =>
        b.versionNumber -
        a.versionNumber
    );
  }


  get sortedDecree600History():
    FulfillmentDeadlineHistoryResponse[] {

    return [
      ...(this.basicData?.decree600History ?? [])
    ].sort(
      (
        a,
        b
      ) =>
        b.versionNumber -
        a.versionNumber
    );
  }


  isDeadlineFulfilled(
    item:
      FulfillmentDeadlineHistoryResponse | null
  ): boolean {

    return !!item?.fulfillmentDate;
  }


  isCurrentPV(
    item:
      FulfillmentDeadlineHistoryResponse
  ): boolean {

    return (
      item.id ===
      this.basicData?.currentPV?.id
    );
  }


  isCurrentDecree600(
    item:
      FulfillmentDeadlineHistoryResponse
  ): boolean {

    return (
      item.id ===
      this.basicData
        ?.currentDecree600
        ?.id
    );
  }


  isSectionExpanded(
    section:
      ProductBasicDataExpandableSection
  ): boolean {

    return this.expandedSections
      .has(section);
  }


  toggleSection(
    section:
      ProductBasicDataExpandableSection
  ): void {

    if (
      this.expandedSections
        .has(section)
    ) {

      this.expandedSections
        .delete(section);

      return;
    }


    this.expandedSections
      .add(section);
  }


  expandCurrentTab(): void {

    for (
      const section
      of this.sectionsByTab[this.activeTab]
    ) {

      this.expandedSections
        .add(section);
    }
  }


  collapseCurrentTab(): void {

    for (
      const section
      of this.sectionsByTab[this.activeTab]
    ) {

      this.expandedSections
        .delete(section);
    }
  }


  get currentTabAllExpanded():
    boolean {

    return this.sectionsByTab[
      this.activeTab
    ].every(
      section =>
        this.expandedSections
          .has(section)
    );
  }

  get currentTabAllCollapsed(): boolean {
    return this.sectionsByTab[this.activeTab].every(section => !this.expandedSections.has(section));
  }


  toggleIngredientCard(
    ingredientId: string
  ): void {

    this.toggleIdInSet(
      this.expandedIngredientIds,
      ingredientId
    );
  }


  isIngredientCardExpanded(
    ingredientId: string
  ): boolean {

    return this.expandedIngredientIds
      .has(ingredientId);
  }


  toggleApprovedPackCard(
    approvedPackId: string
  ): void {

    this.toggleIdInSet(
      this.expandedApprovedPackIds,
      approvedPackId
    );
  }


  isApprovedPackCardExpanded(
    approvedPackId: string
  ): boolean {

    return this.expandedApprovedPackIds
      .has(approvedPackId);
  }


  toggleManufacturerCard(
    manufacturerId: string
  ): void {

    this.toggleIdInSet(
      this.expandedManufacturerIds,
      manufacturerId
    );
  }


  isManufacturerCardExpanded(
    manufacturerId: string
  ): boolean {

    return this.expandedManufacturerIds
      .has(manufacturerId);
  }


  private toggleIdInSet(
    target:
      Set<string>,
    id:
      string
  ): void {

    if (
      target.has(id)
    ) {

      target.delete(id);

      return;
    }


    target.add(id);
  }


  // =========================================================
  // Load
  // =========================================================

  load(): void {

    if (!this.productId) {
      return;
    }


    this.loading =
      true;

    this.errorMessage =
      '';

    this.successMessage =
      '';

    this.basicDataMissing =
      false;


    this.service
      .getByProductId(
        this.productId
      )
      .subscribe({

        next: response => {

          this.basicData =
            response;


          if (
            this.selectedIngredientDetails
          ) {

            const refreshedIngredient =
              response.activeIngredients
                .find(
                  x =>
                    x.id ===
                    this.selectedIngredientDetails!.id
                );


            if (refreshedIngredient) {

              this.selectedIngredientDetails =
                refreshedIngredient;


              this.populateIngredientDetailsForm(
                refreshedIngredient
              );

            } else {

              this.closeIngredientDetails();
            }
          }


          this.basicDataMissing =
            false;


          this.populateForm(
            response
          );


          this.loading =
            false;
        },


        error: (
          error:
            HttpErrorResponse
        ) => {

          console.error(
            error
          );


          this.loading =
            false;


          if (
            error.status ===
            404
          ) {

            this.basicData =
              null;

            this.basicDataMissing =
              true;

            return;
          }


          this.errorMessage =
            'Unable to load Product Basic Data.';
        }

      });
  }


  private populateForm(
    data:
      ProductBasicDataResponse
  ): void {

    this.tradeName =
      data.tradeName ??
      '';


    this.physicalDescription =
      data.physicalDescription ??
      '';


    this.dosageFormId =
      data.dosageForm?.id ??
      null;


    this.storageConditionId =
      data.storageCondition?.id ??
      null;


    // =======================================================
    // New PV form
    // =======================================================

    this.pvDescription =
      '';

    this.pvDeadline =
      '';

    this.pvNotes =
      '';


    // =======================================================
    // New Decree 600 form
    // =======================================================

    this.decree600Description =
      '';

    this.decree600Deadline =
      '';

    this.decree600Notes =
      '';


    // =======================================================
    // Shelf Life
    // =======================================================

    this.shelfLifeNumberOfMonths =
      data.shelfLife?.numberOfMonths ??
      null;


    this.shelfLifeApprovalDate =
      data.shelfLife?.approvalDate ??
      '';


    this.shelfLifeAttachments =
      data.shelfLife
        ?.attachments
        ?.map(
          x => ({
            attachmentId:
              x.attachmentId,

            fileName:
              x.fileName,

            notes:
              x.notes ??
              null
          })
        ) ??
      [];
  }


  // =========================================================
  // Lookups
  // =========================================================

  loadLookups(): void {

    this.dosageFormService
      .getAll()
      .subscribe({

        next: response => {

          this.dosageForms =
            response;
        },

        error: error => {

          console.error(
            'Unable to load dosage forms',
            error
          );
        }

      });


    this.storageConditionService
      .getAll()
      .subscribe({

        next: response => {

          this.storageConditions =
            response;
        },

        error: error => {

          console.error(
            'Unable to load storage conditions',
            error
          );
        }

      });
  }


  // =========================================================
  // Create Basic Data
  // =========================================================

  createBasicData(): void {

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
        this.productId,
        this.createTradeName.trim()
          ? this.createTradeName.trim()
          : null
      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.successMessage =
            'Product Basic Data created successfully.';


          this.basicDataMissing =
            false;


          this.load();
        },


        error: (
          error:
            HttpErrorResponse
        ) => {

          console.error(
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to create Product Basic Data.';
        }

      });
  }


  // =========================================================
  // General
  // =========================================================

  saveTradeName(): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .updateTradeName(
          id,
          this.tradeName.trim() ||
          null
        )
    );
  }


  savePhysicalDescription(): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .updatePhysicalDescription(
          id,
          this.physicalDescription.trim() ||
          null
        )
    );
  }


  saveDosageForm(): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .updateDosageForm(
          id,
          this.dosageFormId
        )
    );
  }


  saveStorageCondition(): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .updateStorageCondition(
          id,
          this.storageConditionId
        )
    );
  }


  createDosageForm(
    name: string
  ): void {

    const normalizedName =
      name.trim();


    if (
      !normalizedName ||
      this.creatingDosageForm
    ) {
      return;
    }


    this.creatingDosageForm =
      true;


    this.dosageFormService
      .create(
        normalizedName
      )
      .subscribe({

        next: () => {

          this.dosageFormService
            .getAll()
            .subscribe({

              next: items => {

                this.dosageForms =
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


                if (created) {

                  this.dosageFormId =
                    created.id;


                  this.saveDosageForm();
                }


                this.creatingDosageForm =
                  false;
              },


              error: error => {

                console.error(
                  error
                );


                this.creatingDosageForm =
                  false;
              }

            });
        },


        error: error => {

          console.error(
            error
          );


          this.creatingDosageForm =
            false;


          this.errorMessage =
            `Unable to create dosage form "${normalizedName}".`;
        }

      });
  }


  createStorageCondition(
    name: string
  ): void {

    const normalizedName =
      name.trim();


    if (
      !normalizedName ||
      this.creatingStorageCondition
    ) {
      return;
    }


    this.creatingStorageCondition =
      true;


    this.storageConditionService
      .create(
        normalizedName
      )
      .subscribe({

        next: () => {

          this.storageConditionService
            .getAll()
            .subscribe({

              next: items => {

                this.storageConditions =
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


                if (created) {

                  this.storageConditionId =
                    created.id;


                  this.saveStorageCondition();
                }


                this.creatingStorageCondition =
                  false;
              },


              error: error => {

                console.error(
                  error
                );


                this.creatingStorageCondition =
                  false;
              }

            });
        },


        error: error => {

          console.error(
            error
          );


          this.creatingStorageCondition =
            false;


          this.errorMessage =
            `Unable to create storage condition "${normalizedName}".`;
        }

      });
  }


  // =========================================================
  // Product Shelf Life
  // =========================================================

  onShelfLifeFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.shelfLifeFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  addShelfLifeAttachment(): void {

    const id =
      this.basicDataId;

    const file =
      this.shelfLifeFile;


    if (
      !id ||
      !file
    ) {
      return;
    }


    this.shelfLifeUploading =
      true;


    this.attachmentService
      .upload(
        file
      )
      .pipe(

        switchMap(
          uploaded => {

            const attachment:
              AttachmentReferenceInput = {

              attachmentId:
                uploaded.attachmentId,

              fileName:
                uploaded.fileName,

              notes:
                this.shelfLifeNotes.trim()
                  ? this.shelfLifeNotes.trim()
                  : null
            };


            if (
              this.basicData?.shelfLife
            ) {

              return this.service
                .addShelfLifeAttachment(
                  id,
                  attachment
                );
            }


            this.shelfLifeAttachments
              .push(
                attachment
              );


            return of(
              void 0
            );
          }
        )

      )
      .subscribe({

        next: () => {

          this.shelfLifeUploading =
            false;


          this.shelfLifeFile =
            null;


          this.shelfLifeNotes =
            '';


          if (
            this.basicData?.shelfLife
          ) {
            this.load();
          }
        },


        error: error => {

          console.error(
            error
          );


          this.shelfLifeUploading =
            false;
        }

      });
  }


  removeProductShelfLifeAttachment(
    attachmentId: string
  ): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    if (
      this.basicData?.shelfLife
    ) {

      this.execute(
        this.service
          .removeShelfLifeAttachment(
            id,
            attachmentId
          )
      );

      return;
    }


    this.shelfLifeAttachments =
      this.shelfLifeAttachments
        .filter(
          x =>
            x.attachmentId !==
            attachmentId
        );
  }


  saveShelfLife(): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    if (
      this.shelfLifeNumberOfMonths ===
        null ||
      this.shelfLifeNumberOfMonths <=
        0
    ) {

      this.errorMessage =
        'Number of months must be greater than zero.';

      return;
    }


    if (
      !this.shelfLifeApprovalDate
    ) {

      this.errorMessage =
        'Approval date is required.';

      return;
    }


    const shelfLife:
      ShelfLifeInput = {

      numberOfMonths:
        this.shelfLifeNumberOfMonths,

      approvalDate:
        this.shelfLifeApprovalDate,

      attachments:
        this.shelfLifeAttachments
    };


    this.execute(
  this.service
    .updateShelfLife(
      id,
      shelfLife
    ),
  () => {

    this.shelfLifeHistoryLoaded =
      false;

    this.shelfLifeHistory =
      [];
  }
);
  }


  // =========================================================
  // Prices
  // =========================================================

  addPrice(): void {

    const id =
      this.basicDataId;


    if (
      !id ||
      this.newPrice === null ||
      !this.newPricePerDescription.trim() ||
      !this.newPriceIssueDate ||
      !this.newPriceExpiryDate
    ) {
      return;
    }


    this.execute(
      this.service
        .addApprovedPrice(
          id,
          this.newPrice,
          this.newPricePerDescription.trim(),
          this.newPriceIssueDate,
          this.newPriceExpiryDate
        ),
      () => {

        this.newPrice =
          null;


        this.newPricePerDescription =
          '';


        this.newPriceIssueDate =
          '';


        this.newPriceExpiryDate =
          '';
      }
    );
  }


  removePrice(
    approvedPriceId: string
  ): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .removeApprovedPrice(
          id,
          approvedPriceId
        )
    );
  }


  // =========================================================
  // Product Approved Packs
  // =========================================================

  onApprovedPackFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.newApprovedPackFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  addApprovedPack(): void {

    const id =
      this.basicDataId;

    const file =
      this.newApprovedPackFile;

    const description =
      this.newApprovedPackDescription.trim();


    if (
      !id ||
      !description ||
      !this.newApprovedPackApprovalDate ||
      !file
    ) {

      this.errorMessage =
        'Description, approval date and attachment are required.';

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
          attachment => {

            const approval:
              DatedAttachmentInput = {

              date:
                this.newApprovedPackApprovalDate,

              attachmentId:
                attachment.attachmentId,

              fileName:
                attachment.fileName,

              notes:
                this.newApprovedPackNotes.trim()
                  ? this.newApprovedPackNotes.trim()
                  : null
            };


            return this.service
              .addApprovedPack(
                id,
                description,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.newApprovedPackDescription =
            '';


          this.newApprovedPackApprovalDate =
            '';


          this.newApprovedPackNotes =
            '';


          this.newApprovedPackFile =
            null;


          this.successMessage =
            'Approved pack added successfully.';


          this.load();
        },


        error: error => {

          console.error(
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to add approved pack.';
        }

      });
  }


  // =========================================================
  // Manufacturers
  // =========================================================

  onManufacturerFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.newManufacturerFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  addApprovedManufacturer(): void {

    const id =
      this.basicDataId;

    const file =
      this.newManufacturerFile;

    const manufacturerName =
      this.newManufacturerName.trim();


    if (
      !id ||
      !manufacturerName ||
      !this.newManufacturerApprovalDate ||
      !file
    ) {

      this.errorMessage =
        'Manufacturer name, approval date and attachment are required.';

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
          attachment => {

            const approval:
              DatedAttachmentInput = {

              date:
                this.newManufacturerApprovalDate,

              attachmentId:
                attachment.attachmentId,

              fileName:
                attachment.fileName,

              notes:
                this.newManufacturerNotes.trim()
                  ? this.newManufacturerNotes.trim()
                  : null
            };


            return this.service
              .addApprovedManufacturer(
                id,
                manufacturerName,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.newManufacturerName =
            '';


          this.newManufacturerApprovalDate =
            '';


          this.newManufacturerNotes =
            '';


          this.newManufacturerFile =
            null;


          this.successMessage =
            'Approved manufacturer added successfully.';


          this.load();
        },


        error: error => {

          console.error(
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to add approved manufacturer.';
        }

      });
  }


  activateManufacturer(
    approvedManufacturerId: string
  ): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .activateApprovedManufacturer(
          id,
          approvedManufacturerId
        )
    );
  }


  deactivateManufacturer(
    approvedManufacturerId: string
  ): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .deactivateApprovedManufacturer(
          id,
          approvedManufacturerId
        )
    );
  }


  // =========================================================
  // PV HISTORY
  // =========================================================

  savePV(): void {

    const id =
      this.basicDataId;

    const description =
      this.pvDescription.trim();


    if (
      !id ||
      !description ||
      !this.pvDeadline
    ) {

      this.errorMessage =
        'PV description and deadline are required.';

      return;
    }


    const value:
      FulfillmentDeadlineInput = {

      description,

      deadline:
        this.pvDeadline,

      notes:
        this.pvNotes.trim()
          ? this.pvNotes.trim()
          : null
    };


    this.execute(
      this.service
        .addPV(
          id,
          value
        ),
      () => {

        this.pvDescription =
          '';

        this.pvDeadline =
          '';

        this.pvNotes =
          '';

        this.expandedSections
          .add('deadlines-pv');

        this.expandedSections
          .add('deadlines-pv-history');

        this.activeTab =
          'deadlines';
      }
    );
  }


  fulfillPV(): void {

    const id =
      this.basicDataId;


    if (
      !id ||
      !this.pvFulfillmentDate
    ) {
      return;
    }


    this.execute(
      this.service
        .fulfillPV(
          id,
          this.pvFulfillmentDate
        ),
      () => {

        this.pvFulfillmentDate =
          '';

        this.activeTab =
          'deadlines';
      }
    );
  }


  resetPV(): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .resetPV(
          id
        ),
      () => {

        this.pvFulfillmentDate =
          '';

        this.activeTab =
          'deadlines';
      }
    );
  }


  // =========================================================
  // DECREE 600 HISTORY
  // =========================================================

  saveDecree600(): void {

    const id =
      this.basicDataId;

    const description =
      this.decree600Description.trim();


    if (
      !id ||
      !description ||
      !this.decree600Deadline
    ) {

      this.errorMessage =
        'Decree 600 description and deadline are required.';

      return;
    }


    const value:
      FulfillmentDeadlineInput = {

      description,

      deadline:
        this.decree600Deadline,

      notes:
        this.decree600Notes.trim()
          ? this.decree600Notes.trim()
          : null
    };


    this.execute(
      this.service
        .addDecree600(
          id,
          value
        ),
      () => {

        this.decree600Description =
          '';

        this.decree600Deadline =
          '';

        this.decree600Notes =
          '';

        this.expandedSections
          .add('deadlines-decree600');

        this.expandedSections
          .add('deadlines-decree600-history');

        this.activeTab =
          'deadlines';
      }
    );
  }


  fulfillDecree600(): void {

    const id =
      this.basicDataId;


    if (
      !id ||
      !this.decree600FulfillmentDate
    ) {
      return;
    }


    this.execute(
      this.service
        .fulfillDecree600(
          id,
          this.decree600FulfillmentDate
        ),
      () => {

        this.decree600FulfillmentDate =
          '';

        this.activeTab =
          'deadlines';
      }
    );
  }


  resetDecree600(): void {

    const id =
      this.basicDataId;


    if (!id) {
      return;
    }


    this.execute(
      this.service
        .resetDecree600(
          id
        ),
      () => {

        this.decree600FulfillmentDate =
          '';

        this.activeTab =
          'deadlines';
      }
    );
  }


  // =========================================================
  // Active Ingredient Search
  // =========================================================

  searchActiveIngredients(): void {

    this.activeIngredientPageNumber =
      1;


    this.loadActiveIngredients();
  }


  loadActiveIngredients(): void {

    this.loadingActiveIngredients =
      true;


    this.activeIngredientService
      .getAll(
        this.activeIngredientSearch,
        this.activeIngredientPageNumber,
        this.activeIngredientPageSize
      )
      .subscribe({

        next: response => {

          this.activeIngredientOptions =
            response.items;


          this.activeIngredientTotalCount =
            response.totalCount;


          this.loadingActiveIngredients =
            false;
        },


        error: error => {

          console.error(
            'Unable to load active ingredients',
            error
          );


          this.activeIngredientOptions =
            [];


          this.loadingActiveIngredients =
            false;
        }

      });
  }


  isActiveIngredientAdded(
    activeIngredientId: string
  ): boolean {

    return (
      this.basicData
        ?.activeIngredients
        .some(
          x =>
            x.activeIngredient.id ===
            activeIngredientId
        )
      ?? false
    );
  }


  get filteredActiveIngredients():
    ActiveIngredientLookupResponse[] {

    let items =
      [
        ...this.activeIngredientOptions
      ];


    if (
      this.activeIngredientFilter ===
      'available'
    ) {

      items =
        items.filter(
          x =>
            !this.isActiveIngredientAdded(
              x.id
            )
        );
    }


    if (
      this.activeIngredientFilter ===
      'added'
    ) {

      items =
        items.filter(
          x =>
            this.isActiveIngredientAdded(
              x.id
            )
        );
    }


    items.sort(
      (
        a,
        b
      ) => {

        const result =
          a.name.localeCompare(
            b.name
          );


        return (
          this.activeIngredientSort ===
          'asc'
            ? result
            : -result
        );
      }
    );


    return items;
  }


  get selectedActiveIngredient():
    ActiveIngredientLookupResponse | null {

    return (
      this.activeIngredientOptions
        .find(
          x =>
            x.id ===
            this.selectedActiveIngredientId
        )
      ?? null
    );
  }


  selectActiveIngredient(
    ingredient:
      ActiveIngredientLookupResponse
  ): void {

    if (
      this.isActiveIngredientAdded(
        ingredient.id
      )
    ) {
      return;
    }


    this.selectedActiveIngredientId =
      ingredient.id;
  }


  openAddIngredientModal(): void {

    this.addIngredientModalOpen =
      true;


    this.selectedActiveIngredientId =
      null;


    this.newActiveIngredientStrength =
      '';


    this.activeIngredientSearch =
      '';


    if (
      this.activeIngredientOptions.length ===
      0
    ) {
      this.loadActiveIngredients();
    }
  }


  closeAddIngredientModal(): void {

    this.addIngredientModalOpen =
      false;


    this.selectedActiveIngredientId =
      null;


    this.newActiveIngredientStrength =
      '';


    this.activeIngredientSearch =
      '';
  }


  addActiveIngredient(): void {

    const id =
      this.basicDataId;


    if (
      !id ||
      !this.selectedActiveIngredientId ||
      !this.newActiveIngredientStrength.trim()
    ) {
      return;
    }


    this.execute(
      this.service
        .addActiveIngredient(
          id,
          this.selectedActiveIngredientId,
          this.newActiveIngredientStrength.trim()
        ),
      () => {

        this.closeAddIngredientModal();


        this.activeTab =
          'active-ingredients';
      }
    );
  }


  // =========================================================
  // Active Ingredient Details
  // =========================================================

  openIngredientDetails(
    ingredient:
      ProductActiveIngredientResponse
  ): void {

    this.selectedIngredientDetails =
      ingredient;


    this.ingredientDetailsTab =
      'general';


    this.populateIngredientDetailsForm(
      ingredient
    );


    this.ingredientDetailsModalOpen =
      true;
  }


  closeIngredientDetails(): void {

    this.ingredientDetailsModalOpen =
      false;


    this.selectedIngredientDetails =
      null;


    this.ingredientDetailsTab =
      'general';


    this.ingredientShelfLifeNumberOfMonths =
      null;


    this.ingredientShelfLifeApprovalDate =
      '';


    this.ingredientShelfLifeAttachments =
      [];


    this.ingredientShelfLifeNotes =
      '';


    this.ingredientShelfLifeFile =
      null;


    this.ingredientShelfLifeUploading =
      false;


    this.ingredientPackDescription =
      '';


    this.ingredientPackApprovalDate =
      '';


    this.ingredientPackNotes =
      '';


    this.ingredientPackFile =
      null;


    this.supplierSearch =
      '';


    this.supplierOptions =
      [];


    this.selectedSupplierId =
      null;


    this.supplierPageNumber =
      1;


    this.supplierTotalCount =
      0;
  }


  setIngredientDetailsTab(
    tab:
      'general' |
      'shelf-life' |
      'packs' |
      'suppliers'
  ): void {

    this.ingredientDetailsTab =
      tab;


    if (
      tab ===
      'suppliers'
    ) {

      this.supplierSearch =
        '';


      this.selectedSupplierId =
        null;


      this.supplierPageNumber =
        1;


      this.loadSuppliers();
    }
  }


  private populateIngredientDetailsForm(
    ingredient:
      ProductActiveIngredientResponse
  ): void {

    this.ingredientEditStrength =
      ingredient.strength;


    this.ingredientEditStorageConditionId =
      ingredient.storageCondition?.id ??
      null;


    this.ingredientEditPhysicalDescription =
      ingredient.physicalDescription ??
      '';


    this.ingredientShelfLifeNumberOfMonths =
      ingredient.shelfLife?.numberOfMonths ??
      null;


    this.ingredientShelfLifeApprovalDate =
      ingredient.shelfLife?.approvalDate ??
      '';


    this.ingredientShelfLifeAttachments =
      ingredient.shelfLife
        ?.attachments
        ?.map(
          x => ({
            attachmentId:
              x.attachmentId,

            fileName:
              x.fileName,

            notes:
              x.notes ??
              null
          })
        ) ??
      [];


    this.ingredientShelfLifeNotes =
      '';


    this.ingredientShelfLifeFile =
      null;
  }


  saveIngredientStrength(): void {

    const basicDataId =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;

    const strength =
      this.ingredientEditStrength.trim();


    if (
      !basicDataId ||
      !ingredient ||
      !strength
    ) {
      return;
    }


    this.execute(
      this.service
        .updateActiveIngredientStrength(
          basicDataId,
          ingredient.id,
          strength
        )
    );
  }


  saveIngredientStorageCondition(): void {

    const basicDataId =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;


    if (
      !basicDataId ||
      !ingredient
    ) {
      return;
    }


    this.execute(
      this.service
        .updateActiveIngredientStorageCondition(
          basicDataId,
          ingredient.id,
          this.ingredientEditStorageConditionId
        )
    );
  }


  saveIngredientPhysicalDescription(): void {

    const basicDataId =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;


    if (
      !basicDataId ||
      !ingredient
    ) {
      return;
    }


    this.execute(
      this.service
        .updateActiveIngredientPhysicalDescription(
          basicDataId,
          ingredient.id,
          this.ingredientEditPhysicalDescription.trim() ||
          null
        )
    );
  }


  createIngredientStorageCondition(
    name: string
  ): void {

    const normalizedName =
      name.trim();


    if (
      !normalizedName ||
      this.creatingIngredientStorageCondition
    ) {
      return;
    }


    this.creatingIngredientStorageCondition =
      true;


    this.storageConditionService
      .create(
        normalizedName
      )
      .subscribe({

        next: () => {

          this.storageConditionService
            .getAll()
            .subscribe({

              next: items => {

                this.storageConditions =
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


                if (created) {

                  this.ingredientEditStorageConditionId =
                    created.id;


                  this.saveIngredientStorageCondition();
                }


                this.creatingIngredientStorageCondition =
                  false;
              },


              error: error => {

                console.error(
                  error
                );


                this.creatingIngredientStorageCondition =
                  false;
              }

            });
        },


        error: error => {

          console.error(
            error
          );


          this.creatingIngredientStorageCondition =
            false;


          this.errorMessage =
            `Unable to create storage condition "${normalizedName}".`;
        }

      });
  }


  // =========================================================
  // Ingredient Shelf Life
  // =========================================================

  onIngredientShelfLifeFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.ingredientShelfLifeFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  addIngredientShelfLifeAttachment(): void {

    const id =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;

    const file =
      this.ingredientShelfLifeFile;


    if (
      !id ||
      !ingredient ||
      !file
    ) {
      return;
    }


    this.ingredientShelfLifeUploading =
      true;


    this.attachmentService
      .upload(
        file
      )
      .pipe(

        switchMap(
          uploaded => {

            const attachment:
              AttachmentReferenceInput = {

              attachmentId:
                uploaded.attachmentId,

              fileName:
                uploaded.fileName,

              notes:
                this.ingredientShelfLifeNotes.trim()
                  ? this.ingredientShelfLifeNotes.trim()
                  : null
            };


            if (
              ingredient.shelfLife
            ) {

              return this.service
                .addActiveIngredientShelfLifeAttachment(
                  id,
                  ingredient.id,
                  attachment
                );
            }


            this.ingredientShelfLifeAttachments
              .push(
                attachment
              );


            return of(
              void 0
            );
          }
        )

      )
      .subscribe({

        next: () => {

          this.ingredientShelfLifeUploading =
            false;


          this.ingredientShelfLifeFile =
            null;


          this.ingredientShelfLifeNotes =
            '';


          if (
            ingredient.shelfLife
          ) {
            this.load();
          }
        },


        error: error => {

          console.error(
            error
          );


          this.ingredientShelfLifeUploading =
            false;
        }

      });
  }


  removeIngredientShelfLifeAttachment(
    attachmentId: string
  ): void {

    const id =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;


    if (
      !id ||
      !ingredient
    ) {
      return;
    }


    if (
      ingredient.shelfLife
    ) {

      this.execute(
        this.service
          .removeActiveIngredientShelfLifeAttachment(
            id,
            ingredient.id,
            attachmentId
          )
      );

      return;
    }


    this.ingredientShelfLifeAttachments =
      this.ingredientShelfLifeAttachments
        .filter(
          x =>
            x.attachmentId !==
            attachmentId
        );
  }


  saveIngredientShelfLife(): void {

    const id =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;


    if (
      !id ||
      !ingredient
    ) {
      return;
    }


    if (
      this.ingredientShelfLifeNumberOfMonths ===
        null ||
      this.ingredientShelfLifeNumberOfMonths <=
        0
    ) {

      this.errorMessage =
        'Number of months must be greater than zero.';

      return;
    }


    if (
      !this.ingredientShelfLifeApprovalDate
    ) {

      this.errorMessage =
        'Approval date is required.';

      return;
    }


    const shelfLife:
      ShelfLifeInput = {

      numberOfMonths:
        this.ingredientShelfLifeNumberOfMonths,

      approvalDate:
        this.ingredientShelfLifeApprovalDate,

      attachments:
        this.ingredientShelfLifeAttachments
    };


    this.execute(
      this.service
        .updateActiveIngredientShelfLife(
          id,
          ingredient.id,
          shelfLife
        )
    );
  }


  // =========================================================
  // Ingredient Packs
  // =========================================================

  onIngredientPackFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.ingredientPackFile =
      input.files?.[0] ??
      null;


    input.value =
      '';
  }


  addIngredientApprovedPack(): void {

    const basicDataId =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;

    const file =
      this.ingredientPackFile;

    const description =
      this.ingredientPackDescription.trim();


    if (
      !basicDataId ||
      !ingredient ||
      !description ||
      !this.ingredientPackApprovalDate ||
      !file
    ) {
      return;
    }


    this.saving =
      true;


    this.attachmentService
      .upload(
        file
      )
      .pipe(

        switchMap(
          attachment => {

            const approval:
              DatedAttachmentInput = {

              date:
                this.ingredientPackApprovalDate,

              attachmentId:
                attachment.attachmentId,

              fileName:
                attachment.fileName,

              notes:
                this.ingredientPackNotes.trim()
                  ? this.ingredientPackNotes.trim()
                  : null
            };


            return this.service
              .addActiveIngredientApprovedPack(
                basicDataId,
                ingredient.id,
                description,
                approval
              );
          }
        )

      )
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.ingredientPackDescription =
            '';


          this.ingredientPackApprovalDate =
            '';


          this.ingredientPackNotes =
            '';


          this.ingredientPackFile =
            null;


          this.load();
        },


        error: error => {

          console.error(
            error
          );


          this.saving =
            false;
        }

      });
  }


  removeIngredientApprovedPack(
    approvedPackId: string
  ): void {

    const basicDataId =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;


    if (
      !basicDataId ||
      !ingredient
    ) {
      return;
    }


    this.execute(
      this.service
        .removeActiveIngredientApprovedPack(
          basicDataId,
          ingredient.id,
          approvedPackId
        )
    );
  }


  // =========================================================
  // Suppliers
  // =========================================================

  loadSuppliers(): void {

    this.supplierLoading =
      true;


    this.supplierService
      .getAll(
        this.supplierSearch,
        this.supplierPageNumber
      )
      .subscribe({

        next: response => {

          this.supplierOptions =
            response.items;


          this.supplierTotalCount =
            response.totalCount;


          this.supplierLoading =
            false;
        },


        error: error => {

          console.error(
            'Unable to load suppliers',
            error
          );


          this.supplierOptions =
            [];


          this.supplierLoading =
            false;
        }

      });
  }


  searchSuppliers(): void {

    this.supplierPageNumber =
      1;


    this.selectedSupplierId =
      null;


    this.loadSuppliers();
  }


  isSupplierAlreadyAdded(
    supplierId: string
  ): boolean {

    return (
      this.selectedIngredientDetails
        ?.approvedSuppliers
        .some(
          x =>
            x.supplierId ===
            supplierId
        )
      ?? false
    );
  }


  get selectedSupplier():
    SupplierLookupResponse | null {

    return (
      this.supplierOptions
        .find(
          x =>
            x.id ===
            this.selectedSupplierId
        )
      ?? null
    );
  }


  selectSupplier(
    supplier:
      SupplierLookupResponse
  ): void {

    if (
      this.isSupplierAlreadyAdded(
        supplier.id
      )
    ) {
      return;
    }


    this.selectedSupplierId =
      supplier.id;
  }


  addIngredientApprovedSupplier(): void {

    const basicDataId =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;

    const supplierId =
      this.selectedSupplierId;


    if (
      !basicDataId ||
      !ingredient ||
      !supplierId
    ) {
      return;
    }


    this.execute(
      this.service
        .addApprovedSupplier(
          basicDataId,
          ingredient.id,
          supplierId
        ),
      () => {

        this.selectedSupplierId =
          null;


        this.supplierSearch =
          '';


        this.ingredientDetailsTab =
          'suppliers';


        this.loadSuppliers();
      }
    );
  }


  toggleIngredientSupplier(
    approvedSupplierId: string,
    isActive: boolean
  ): void {

    const basicDataId =
      this.basicDataId;

    const ingredient =
      this.selectedIngredientDetails;


    if (
      !basicDataId ||
      !ingredient
    ) {
      return;
    }


    const request$ =
      isActive
        ? this.service
            .deactivateApprovedSupplier(
              basicDataId,
              ingredient.id,
              approvedSupplierId
            )
        : this.service
            .activateApprovedSupplier(
              basicDataId,
              ingredient.id,
              approvedSupplierId
            );


    this.execute(
      request$
    );
  }


  // =========================================================
  // Attachments
  // =========================================================

  isPdf(
    fileName: string
  ): boolean {

    return fileName
      .toLowerCase()
      .endsWith(
        '.pdf'
      );
  }


  previewPdf(
    attachmentId: string,
    fileName: string
  ): void {

    if (
      !this.isPdf(
        fileName
      )
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


  closePdfViewer(): void {

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
            'Attachment download failed',
            error
          );


          this.errorMessage =
            `Unable to open attachment "${fileName}".`;
        }

      });
  }

// =========================================================
// Product Shelf Life History
// =========================================================

shelfLifeHistoryModalOpen =
  false;

shelfLifeHistoryLoading =
  false;

shelfLifeHistoryLoaded =
  false;

shelfLifeHistory:
  ShelfLifeHistoryResponse[] =
    [];

shelfLifeHistoryError =
  '';

  // =========================================================
// Product Shelf Life History
// =========================================================

openShelfLifeHistory(): void {

  const id =
    this.basicDataId;


  if (!id) {
    return;
  }


  this.shelfLifeHistoryModalOpen =
    true;


  this.shelfLifeHistoryError =
    '';


  /*
   * Do not request the history again
   * every time the modal is reopened.
   */
  if (
    this.shelfLifeHistoryLoaded
  ) {
    return;
  }


  this.loadShelfLifeHistory();
}


closeShelfLifeHistory(): void {

  this.shelfLifeHistoryModalOpen =
    false;
}


loadShelfLifeHistory(
  forceReload = false
): void {

  const id =
    this.basicDataId;


  if (
    !id ||
    this.shelfLifeHistoryLoading
  ) {
    return;
  }


  if (
    this.shelfLifeHistoryLoaded &&
    !forceReload
  ) {
    return;
  }


  this.shelfLifeHistoryLoading =
    true;


  this.shelfLifeHistoryError =
    '';


  this.service
    .getShelfLifeHistory(
      id
    )
    .subscribe({

      next: response => {

        this.shelfLifeHistory =
          [...response]
            .sort(
              (
                a,
                b
              ) =>
                b.versionNumber -
                a.versionNumber
            );


        this.shelfLifeHistoryLoaded =
          true;


        this.shelfLifeHistoryLoading =
          false;
      },


      error: (
        error:
          HttpErrorResponse
      ) => {

        console.error(
          'Unable to load shelf life history',
          error
        );


        this.shelfLifeHistory =
          [];


        this.shelfLifeHistoryLoading =
          false;


        this.shelfLifeHistoryError =
          'Unable to load shelf life history.';
      }

    });
}


refreshShelfLifeHistory(): void {

  this.loadShelfLifeHistory(
    true
  );
}
  // =========================================================
  // Shared Executor
  // =========================================================

  private execute(
    request$:
      Observable<unknown>,
    afterSuccess?: () => void
  ): void {

    this.saving =
      true;


    this.errorMessage =
      '';


    this.successMessage =
      '';


    request$
      .subscribe({

        next: () => {

          this.saving =
            false;


          this.successMessage =
            'Saved successfully.';


          afterSuccess?.();


          this.load();
        },


        error: (
          error:
            unknown
        ) => {

          console.error(
            error
          );


          this.saving =
            false;


          this.errorMessage =
            'Unable to save changes.';
        }

      });
  }
}
