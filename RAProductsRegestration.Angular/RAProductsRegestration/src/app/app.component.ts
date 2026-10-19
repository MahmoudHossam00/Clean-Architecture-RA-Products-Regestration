import {
  Component,
  inject
} from '@angular/core';

import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';

import {
  AuthService
} from './core/auth/services/auth.service';

import {
  AuthSessionService
} from './core/auth/services/auth-session.service';


@Component({
  selector:
    'app-root',

  standalone:
    true,

  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],

  templateUrl:
    './app.component.html',

  styleUrl:
    './app.component.css'
})
export class AppComponent {

  private readonly router =
    inject(
      Router
    );


  readonly authService =
    inject(
      AuthService
    );


  readonly session =
    inject(
      AuthSessionService
    );


  // =========================================================
  // Shell
  // =========================================================

  get showApplicationShell(): boolean {

    const url =
      this.router.url
        .split('?')[0];


    return (
      url !== '/login' &&
      url !== '/first-login'
    );
  }


  // =========================================================
  // Current User
  // =========================================================

  get displayName(): string {

    const user =
      this.session.currentUser();


    if (!user) {

      return '';
    }


    return (
      user.shortName?.trim() ||
      user.userName
    );
  }


  // =========================================================
  // Roles
  // =========================================================

  get isAdmin(): boolean {

    return this.session
      .isAdmin();
  }


  // =========================================================
  // Logout
  // =========================================================

  logout(): void {

    this.authService
      .logout();
  }
}