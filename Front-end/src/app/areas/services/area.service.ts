import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, } from 'rxjs';
import { HttpHelperService } from '../../shared/http-helper.service';
import { environment } from '../../../environments/environment';
import { Area, AreaAdicionarCommand } from '../models/area.model';

const url = `${environment.apiUrl}/api/Areas`;

@Injectable({
  providedIn: 'root'
})
export class AreaService {

  constructor(private http: HttpClient, private httpHelper: HttpHelperService) { }

  buscar(): Observable<Area[]> {
    return this.httpHelper.handleRequest(this.http.get<Area[]>(url));
  }

  buscarPorId(id: string): Observable<Area> {
    return this.httpHelper.handleRequest(this.http.get<Area>(`${url}/${id}`));
  }

  adicionar(area: AreaAdicionarCommand): Observable<any> {
    return this.httpHelper.handleRequest(this.http.post<Area>(url, area));
  }

  editar(area: AreaAdicionarCommand): Observable<any> {
    return this.httpHelper.handleRequest(this.http.put<Area>(`${url}/${area.id}`, area));
  }

  excluir(id: string): Observable<any> {
    return this.httpHelper.handleRequest(this.http.delete(`${url}/${id}`));
  }
}
