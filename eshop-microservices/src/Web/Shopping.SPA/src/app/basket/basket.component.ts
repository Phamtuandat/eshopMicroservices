import { Component } from '@angular/core';
import { BasketService } from '../services/basket.service';
import { AuthConfigService } from '../services/auth-config.service';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss',
})
export class BasketComponent {
  constructor(
    private apiService: BasketService,
    private authService: AuthConfigService
  ) {}

  ngOnInit() {
    if (this.authService.hasValidAccessToken()) {
      this.fetchData(this.authService.getToken());
    }
  }

  fetchData(token: string) {
    this.apiService.getData(token).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        console.error('Error fetching data: ', error);
      }
    );
  }
}
