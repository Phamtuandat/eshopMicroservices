// auth-config.service.ts
import { Injectable } from '@angular/core';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';

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
};

@Injectable({
  providedIn: 'root',
})
export class AuthConfigService {
  constructor(private oauthService: OAuthService) {
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

  getIdentity() {
    var claims = this.oauthService.getIdentityClaims();
    if (!claims) return null;
    return claims;
  }
}
