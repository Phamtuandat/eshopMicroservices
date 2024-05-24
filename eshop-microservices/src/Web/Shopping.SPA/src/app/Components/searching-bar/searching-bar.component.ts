import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { ButtonComponent } from '../button/button.component';

@Component({
  selector: 'app-searching-bar',
  standalone: true,
  imports: [MatIconModule, ButtonComponent, ReactiveFormsModule],

  templateUrl: './searching-bar.component.html',
  styleUrl: './searching-bar.component.scss',
})
export class SearchingBarComponent implements OnInit {
  constructor(private fb: FormBuilder) {}
  searchForm: FormGroup = new FormGroup({
    search: new FormControl(''),
  });
  ngOnInit(): void {
    this.searchForm = this.fb.group({
      search: '',
    });
  }
}
