import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss',
})
export class ButtonComponent {
  @Input() type: string = 'submit';
  @Input() radius: string = '5px';
  @Input() bgcolor: string = 'white';
  @Input() height: string = '38px';
}
