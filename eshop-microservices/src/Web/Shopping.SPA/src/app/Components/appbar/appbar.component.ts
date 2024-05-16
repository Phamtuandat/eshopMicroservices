import { OAuthService } from 'angular-oauth2-oidc';
import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { AuthConfigService } from '../../services/auth-config.service';
@Component({
  selector: 'app-appbar',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, CommonModule, MatButtonModule],
  templateUrl: './appbar.component.html',
  styleUrl: './appbar.component.scss',
})
export class AppbarComponent {
  /**
   *
   */
  constructor(private oauthService: AuthConfigService) {}
  isLogged = true;

  login() {
    this.oauthService.login();
  }
}
