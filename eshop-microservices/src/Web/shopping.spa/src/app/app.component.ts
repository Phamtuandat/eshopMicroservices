import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthConfigService } from './services/auth-config.service';
import { BasketComponent } from './basket/basket.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BasketComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'Shopping.SPA';

  constructor(private authService: AuthConfigService) {}
  ngOnInit(): void {}
}
