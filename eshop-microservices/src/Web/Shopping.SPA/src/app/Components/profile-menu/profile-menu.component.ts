import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { AuthConfigService } from '../../services/auth-config.service';
import { MatIconModule } from '@angular/material/icon';
import { User } from '../../../Models/Identity';
@Component({
  selector: 'app-profile-menu',
  standalone: true,
  imports: [MatButtonModule, MatMenuModule, MatIconModule],
  templateUrl: './profile-menu.component.html',
  styleUrl: './profile-menu.component.scss',
})
export class ProfileMenuComponent implements OnInit {
  logout() {
    this.oauthService.loguot();
  }
  constructor(private oauthService: AuthConfigService) {}
  userInfor: User = {
    email: '',
    id: '',
    name: '',
    username: '',
  };
  ngOnInit(): void {
    if (this.oauthService.hasValidAccessToken()) {
      var user = this.oauthService.getIdentity();
      this.userInfor = user;
    } else {
      this.logout();
    }
  }
}
