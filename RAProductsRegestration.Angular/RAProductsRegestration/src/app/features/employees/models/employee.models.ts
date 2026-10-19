export interface EmployeeResponse {

  id: string;

  zenId: number;

  employeeNumber: number;

  nameEn: string;

  nameAr: string | null;

  email: string | null;

  departmentId: string | null;
}


export interface EmployeeImportError {

  rowNumber: number;

  message: string;
}


export interface EmployeeImportResponse {

  totalRows: number;

  inserted: number;

  updated: number;

  skipped: number;

  errors: EmployeeImportError[];
}