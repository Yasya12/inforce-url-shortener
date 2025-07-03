import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { UrlInfo } from '../../../models/url-info.model';

@Injectable({
  providedIn: 'root'
})
export class UrlApiService {
  private apiUrl = `${environment.apiUrl}/urls`;

  constructor(private http: HttpClient) { }

  getUrls() {
    return this.http.get<UrlInfo[]>(this.apiUrl);
  }

  getUrl(id: number): Observable<UrlInfo> {
    return this.http.get<UrlInfo>(`${this.apiUrl}/${id}`);
  }


  createUrl(originalUrl: string) {
    return this.http.post<UrlInfo>(this.apiUrl, { originalUrl });
  }

  deleteUrl(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}