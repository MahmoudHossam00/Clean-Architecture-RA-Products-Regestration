import {
  inject,
  Injectable
} from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  API_CONFIG
} from '../../../core/config/api.config';

import {
  AppRole,
  UserResponse
} from '../../../core/auth/models/auth.models';


export interface CreateUserRequest {
  userName: string;

  email: string;

  password: string;

  shortName: string | null;

  employeeId: string | null;

  roles: AppRole[];
}


export interface UpdateUserRequest {
  userName: string;

  email: string;

  shortName: string | null;

  employeeId: string | null;

  isDisabled: boolean;
}


@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private readonly http =
    inject(
      HttpClient
    );


  private readonly baseUrl =
    `${API_CONFIG.baseUrl}/api/users`;


  getAll():
    Observable<UserResponse[]> {

    return this.http
      .get<UserResponse[]>(
        this.baseUrl
      );
  }


  getById(
    userId: string
  ): Observable<UserResponse> {

    return this.http
      .get<UserResponse>(
        `${this.baseUrl}/${userId}`
      );
  }


  create(
    request: CreateUserRequest
  ): Observable<{
    id: string;
  }> {

    return this.http
      .post<{
        id: string;
      }>(
        this.baseUrl,
        request
      );
  }


  update(
    userId: string,
    request: UpdateUserRequest
  ): Observable<void> {

    return this.http
      .put<void>(
        `${this.baseUrl}/${userId}`,
        request
      );
  }


  getRoles(
    userId: string
  ): Observable<string[]> {

    return this.http
      .get<string[]>(
        `${this.baseUrl}/${userId}/roles`
      );
  }


  setRoles(
    userId: string,
    roles: AppRole[]
  ): Observable<void> {

    return this.http
      .put<void>(
        `${this.baseUrl}/${userId}/roles`,
        {
          roles
        }
      );
  }
}