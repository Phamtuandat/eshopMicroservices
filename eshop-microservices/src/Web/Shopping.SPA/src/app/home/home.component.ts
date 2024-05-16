import { Component } from '@angular/core';
import { AppbarComponent } from '../Components/appbar/appbar.component';
import { LayoutComponent } from '../Shared/layout/layout.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [AppbarComponent, LayoutComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {}
