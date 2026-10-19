import {
  CommonModule
} from '@angular/common';

import {
  Component,
  inject
} from '@angular/core';

import {
  RouterLink
} from '@angular/router';

import {
  finalize
} from 'rxjs';

import {
  ToastrService
} from 'ngx-toastr';

import {
  EmployeeService
} from '../../services/employee.service';

import {
  EmployeeImportResponse
} from '../../models/employee.models';


@Component({
  selector:
    'app-employee-import',

  standalone:
    true,

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl:
    './employee-import.component.html',

  styleUrl:
    './employee-import.component.css'
})
export class EmployeeImportComponent {

  private readonly service =
    inject(
      EmployeeService
    );


  private readonly toastr =
    inject(
      ToastrService
    );


  selectedFile:
    File | null =
      null;


  importing =
    false;


  downloading =
    false;


  result:
    EmployeeImportResponse | null =
      null;


  // =========================================================
  // Select File
  // =========================================================

  fileChanged(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.selectedFile =
      input.files?.[0] ??
      null;


    this.result =
      null;
  }


  // =========================================================
  // Download Template
  // =========================================================

  downloadTemplate(): void {

    if (this.downloading) {

      return;
    }


    this.downloading =
      true;


    this.service
      .downloadImportTemplate()
      .pipe(
        finalize(
          () =>
            this.downloading =
              false
        )
      )
      .subscribe({

        next:
          blob => {

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
              'Employee_Import_Template.xlsx';


            anchor.click();


            URL.revokeObjectURL(
              url
            );
          },


        error:
          () => {

            this.toastr.error(
              'Unable to download template.',
              'Employees'
            );
          }
      });
  }


  // =========================================================
  // Import
  // =========================================================

  import(): void {

    if (
      !this.selectedFile ||
      this.importing
    ) {

      return;
    }


    this.importing =
      true;

    this.result =
      null;


    this.service
      .importEmployees(
        this.selectedFile
      )
      .pipe(
        finalize(
          () =>
            this.importing =
              false
        )
      )
      .subscribe({

        next:
          response => {

            this.result =
              response;


            this.toastr.success(
              `${response.inserted} inserted, ` +
              `${response.updated} updated.`,
              'Employee import completed'
            );
          }
      });
  }
}