import {
  CommonModule
} from '@angular/common';

import {
  Component,
  inject,
  OnInit
} from '@angular/core';

import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

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
} from '../../../employees/services/employee.service';

import {
  EmployeeResponse
} from '../../../employees/models/employee.models';

import {
  UsersService
} from '../../services/users.service';

import {
  AppRole
} from '../../../../core/auth/models/auth.models';


@Component({
  selector:
    'app-create-user',

  standalone:
    true,

  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink
  ],

  templateUrl:
    './create-user.component.html',

  styleUrl:
    './create-user.component.css'
})
export class CreateUserComponent
  implements OnInit {

  private readonly fb =
    inject(
      FormBuilder
    );


  private readonly employeeService =
    inject(
      EmployeeService
    );


  private readonly usersService =
    inject(
      UsersService
    );


  private readonly toastr =
    inject(
      ToastrService
    );


  employees:
    EmployeeResponse[] =
      [];


  employeeSearch =
    '';


  loadingEmployees =
    false;


  saving =
    false;


  selectedRoles =
    new Set<AppRole>([
      'VIEWER'
    ]);


  readonly roleOptions:
    AppRole[] =
      [
        'ADMIN',
        'RA',
        'VIEWER'
      ];


  readonly form =
    this.fb.nonNullable.group({

      employeeId: [
        '',
        [
          Validators.required
        ]
      ],


      userName: [
        '',
        [
          Validators.required
        ]
      ],


      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],


      shortName: [
        ''
      ],


      password: [
        '',
        [
          Validators.required,

          Validators.minLength(
            8
          ),

          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/
          )
        ]
      ]

    });


  ngOnInit(): void {

    this.loadEmployees();
  }


  get filteredEmployees():
    EmployeeResponse[] {

    const search =
      this.employeeSearch
        .trim()
        .toLowerCase();


    if (!search) {

      return this.employees;
    }


    return this.employees
      .filter(
        employee =>

          employee.nameEn
            .toLowerCase()
            .includes(
              search
            ) ||

          (
            employee.nameAr ??
            ''
          )
            .toLowerCase()
            .includes(
              search
            ) ||

          String(
            employee.employeeNumber
          )
            .includes(
              search
            ) ||

          (
            employee.email ??
            ''
          )
            .toLowerCase()
            .includes(
              search
            )
      );
  }


  get selectedEmployee():
    EmployeeResponse | null {

    const id =
      this.form.controls
        .employeeId
        .value;


    return this.employees
      .find(
        x =>
          x.id === id
      )
      ?? null;
  }


  // =========================================================
  // Employees
  // =========================================================

  private loadEmployees(): void {

    this.loadingEmployees =
      true;


    this.employeeService
      .getAll()
      .pipe(
        finalize(
          () =>
            this.loadingEmployees =
              false
        )
      )
      .subscribe({

        next:
          employees => {

            this.employees =
              employees;
          }
      });
  }


  employeeChanged(): void {

    const employee =
      this.selectedEmployee;


    if (!employee) {

      return;
    }


    this.form.patchValue({

      userName:
        String(
          employee.employeeNumber
        ),

      email:
        employee.email ??
        '',

      shortName:
        employee.nameEn
    });
  }


  // =========================================================
  // Roles
  // =========================================================

  roleChanged(
    role: AppRole,
    event: Event
  ): void {

    const checked =
      (
        event.target as
        HTMLInputElement
      ).checked;


    if (checked) {

      this.selectedRoles.add(
        role
      );

    }
    else {

      this.selectedRoles.delete(
        role
      );
    }
  }


  hasRole(
    role: AppRole
  ): boolean {

    return this.selectedRoles
      .has(
        role
      );
  }


  // =========================================================
  // Create
  // =========================================================

  submit(): void {

    if (
      this.form.invalid ||
      this.saving
    ) {

      this.form
        .markAllAsTouched();

      return;
    }


    if (
      this.selectedRoles.size ===
      0
    ) {

      this.toastr.error(
        'Select at least one role.',
        'Create User'
      );

      return;
    }


    const value =
      this.form.getRawValue();


    this.saving =
      true;


    this.usersService
      .create({

        employeeId:
          value.employeeId,

        userName:
          value.userName.trim(),

        email:
          value.email.trim(),

        password:
          value.password,

        shortName:
          value.shortName.trim()
            || null,

        roles:
          Array.from(
            this.selectedRoles
          )
      })
      .pipe(
        finalize(
          () =>
            this.saving =
              false
        )
      )
      .subscribe({

        next:
          () => {

            this.toastr.success(
              'User account created successfully.',
              'Create User'
            );


            this.form.reset({
              employeeId:
                '',

              userName:
                '',

              email:
                '',

              shortName:
                '',

              password:
                ''
            });


            this.selectedRoles =
              new Set<AppRole>([
                'VIEWER'
              ]);
          }
      });
  }
}