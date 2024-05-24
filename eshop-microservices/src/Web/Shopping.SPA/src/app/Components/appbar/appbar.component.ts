import { OAuthService } from 'angular-oauth2-oidc';
import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { AuthConfigService } from '../../services/auth-config.service';
import { ProfileMenuComponent } from '../profile-menu/profile-menu.component';
import { SearchingBarComponent } from '../searching-bar/searching-bar.component';
@Component({
  selector: 'app-appbar',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatButtonModule,
    CommonModule,
    MatButtonModule,
    ProfileMenuComponent,
    SearchingBarComponent,
  ],
  templateUrl: './appbar.component.html',
  styleUrl: './appbar.component.scss',
})
export class AppbarComponent {
  /**
   *
   */
  constructor(private oauthService: AuthConfigService) {}
  isLogged = this.oauthService.hasValidAccessToken();

  login() {
    if (this.isLogged) return;
    this.oauthService.login();
  }
  logout() {
    if (this.isLogged) {
      this.oauthService.loguot();
    }
    return;
  }
}
