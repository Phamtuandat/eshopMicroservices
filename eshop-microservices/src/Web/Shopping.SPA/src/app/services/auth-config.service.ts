import { HttpClient, HttpHeaders } from '@angular/common/http';
// auth-config.service.ts
import { Injectable } from '@angular/core';
import {
  AuthConfig,
  OAuthEvent,
  OAuthService,
  UserInfo,
} from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { User } from '../../Models/Identity';

const authConfig: AuthConfig = {
  issuer: 'https://localhost:6065', // IdentityServer URL
  redirectUri: 'http://localhost:4200/callback',
  clientId: 'angular_spa',
  responseType: 'code',
  scope: 'openid profile basket.api ordering.api catalog.api read write',
  showDebugInformation: true,
  userinfoEndpoint: 'https://localhost:6065/connect/userinfo',
  sessionCheckIFrameUrl: 'https://localhost:6065/connect/checksession',
  revocationEndpoint: 'https://localhost:6065/connect/revocation',
  tokenEndpoint: 'https://localhost:6065/connect/token',
  strictDiscoveryDocumentValidation: false,
  postLogoutRedirectUri: 'http://localhost:4200',
};

@Injectable({
  providedIn: 'root',
})
export class AuthConfigService {
  constructor(private oauthService: OAuthService, private http: HttpClient) {
    this.configure();
  }

  private configure() {
    this.oauthService.configure(authConfig);
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }

  login() {
    this.oauthService.initLoginFlow();
  }

  loguot() {
    this.oauthService.logOut();
  }

  getToken() {
    return this.oauthService.getAccessToken();
  }

  hasValidAccessToken(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  getIdentity(): User {
    var user: User = {
      email: '',
      id: '',
      name: '',
      username: '',
    };
    var token = this.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    var userEnpoint = this.oauthService.userinfoEndpoint;
    if (userEnpoint) {
      this.http
        .get(userEnpoint, { headers })
        .pipe()
        .subscribe((value: any) => {
          user.email = value['email'];
          user.id = value['sub'];
          user.name = value['name'];
          user.username = value['preferred_username'];
        });
    }

    return user;
  }

  getOauthEvents(): Observable<OAuthEvent> {
    return this.oauthService.events;
  }
}
