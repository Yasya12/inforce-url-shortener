import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UrlInfo } from '../../../models/url-info.model';
import { UrlApiService } from '../services/url-api.service';

@Component({
  selector: 'app-url-info',
  standalone: true,
  imports: [CommonModule, DatePipe, RouterLink],
  templateUrl: './url-info.component.html',
  styleUrls: ['./url-info.component.css']
})
export class UrlInfoComponent implements OnInit {
  //services
  private route = inject(ActivatedRoute);
  private urlApiService = inject(UrlApiService);

  //states
  url = signal<UrlInfo | null>(null);
  isLoading = signal<boolean>(true);
  error = signal<string | null>(null);

  //hooks
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.urlApiService.getUrl(+id).subscribe({
        next: (data) => {
          this.url.set(data);
          this.isLoading.set(false);
        },
        error: (err) => {
          this.error.set('URL not found or you do not have permission to view it.');
          this.isLoading.set(false);
        }
      });
    } else {
      this.error.set('Invalid URL ID.');
      this.isLoading.set(false);
    }
  }
}