import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppbarComponent } from '../Components/appbar/appbar.component';
import { AuthConfigService } from '../services/auth-config.service';
import { LayoutComponent } from '../Shared/layout/layout.component';
import { OAuthEvent, OAuthSuccessEvent } from 'angular-oauth2-oidc';
import { filter } from 'rxjs';

const subscriber = (value: any) => {
  console.log(value);
};

@Component({
  selector: 'app-callback',
  standalone: true,
  imports: [AppbarComponent, LayoutComponent],
  templateUrl: './callback.component.html',
  styleUrl: './callback.component.scss',
})
export class CallbackComponent implements OnInit {
  constructor(private router: Router, private authService: AuthConfigService) {}
  ngOnInit(): void {
    this.authService
      .getOauthEvents()
      .pipe(filter((e: OAuthEvent) => e.type === 'token_received'))
      .subscribe((e) => {
        this.router.navigate(['/']); // Redirect to the home page
      });

    if (this.authService.hasValidAccessToken()) {
      this.router.navigate(['/']); // Redirect to the home page if already authenticated
    } else {
      console.log('Invalid token');
      // Optionally redirect to a login page or show an error message
    }
  }
}
