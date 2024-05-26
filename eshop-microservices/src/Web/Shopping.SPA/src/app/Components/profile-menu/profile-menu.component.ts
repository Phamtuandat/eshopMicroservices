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
      this.oauthService.getIdentity()?.subscribe((user: any) => {
        this.userInfor.email = user['email'];
        this.userInfor.id = user['userId'];
        this.userInfor.name = user['name'];
        this.userInfor.username = user['preferred_username'];
      });
    } else {
      this.logout();
    }
  }
}
