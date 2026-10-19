export type AppRole =
  | 'ADMIN'
  | 'RA'
  | 'VIEWER';


export interface LoginRequest {
  userNameOrEmail: string;
  password: string;
}


export interface AuthenticationResponse {
  token: string;

  expiresAtUtc: string;

  userId: string;

  userName: string;

  email: string | null;

  shortName: string | null;

  picture: string | null;

  employeeId: string | null;

  roles: string[];

  changedHisOwnPassword: boolean;

  requiresFirstLoginSetup: boolean;
}


export interface UserResponse {
  id: string;

  userName: string;

  email: string | null;

  shortName: string | null;

  picture: string | null;

  employeeId: string | null;

  isDisabled: boolean;

  changedHisOwnPassword: boolean;

  changedHisOwnNickName: boolean;

  requiresFirstLoginSetup: boolean;

  lastLoginAtUtc: string | null;

  roles: string[];
}


export interface AuthUser {
  id: string;

  userName: string;

  email: string | null;

  shortName: string | null;

  picture: string | null;

  employeeId: string | null;

  roles: string[];

  changedHisOwnPassword: boolean;

  requiresFirstLoginSetup: boolean;
}


export interface CompleteFirstLoginRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}


export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}