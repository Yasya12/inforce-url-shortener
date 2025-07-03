import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { UrlInfo } from '../../models/url-info.model';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UrlApiService } from './services/url-api.service';

@Component({
  selector: 'app-urls-table',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './urls-table.component.html',
  styleUrls: ['./urls-table.component.css']
})
export class UrlsTableComponent implements OnInit {
  //services
  private urlApiService = inject(UrlApiService);
  private fb = inject(FormBuilder);
  public authService = inject(AuthService);
  private toastr = inject(ToastrService);

  //states
  urls = signal<UrlInfo[]>([]);
  isLoading = signal<boolean>(true);
  error = signal<string | null>(null);

  // form element
  addUrlForm: FormGroup;

  constructor() {
    this.addUrlForm = this.fb.group({
      originalUrl: ['', [Validators.required, Validators.pattern(/^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$/i)]]
    });
  }

  //hooks
  ngOnInit(): void {
    this.loadUrls();
  }

  //methods
  loadUrls(): void {
    this.isLoading.set(true);
    this.urlApiService.getUrls().subscribe({
      next: (data) => {
        this.urls.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load URLs.');
        this.isLoading.set(false);
        console.error(err);
      }
    });
  }

  onAddUrl(): void {
    if (this.addUrlForm.invalid) {
      return;
    }

    const originalUrl = this.addUrlForm.value.originalUrl;
    this.urlApiService.createUrl(originalUrl).subscribe({
      next: (newUrl) => {
        this.urls.update(currentUrls => [newUrl, ...currentUrls]);
        this.addUrlForm.reset();
      },
      error: (err) => {
        if (err.status === 409) {
          this.toastr.error('This URL has already been shortened.');
        } else {
          this.toastr.error('An error occurred.');
        }
        console.error(err);
      }
    });
  }

  onDelete(id: number): void {
    if (!confirm('Are you sure you want to delete this URL?')) {
      return;
    }

    this.urlApiService.deleteUrl(id).subscribe({
      next: () => {
        this.urls.update(currentUrls => currentUrls.filter(u => u.id !== id));
      },
      error: (err) => {
        this.toastr.error('Failed to delete URL.');
        
        console.error(err);
      }
    });
  }
}