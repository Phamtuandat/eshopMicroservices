import { Component } from '@angular/core';
import { FooterComponent } from '../../Components/footer/footer.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [FooterComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent {}
