import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, } from 'rxjs';
import { Processo, ProcessoAdicionarCommand } from '../models/processo.model';
import { HttpHelperService } from '../../shared/http-helper.service';
import { environment } from '../../../environments/environment';

const url = `${environment.apiUrl}/api/Processos`;

@Injectable({
  providedIn: 'root'
})
export class ProcessoService {

  constructor(private http: HttpClient, private httpHelper: HttpHelperService) { }

  buscar(): Observable<Processo[]> {
    return this.httpHelper.handleRequest(this.http.get<Processo[]>(url));
  }

  buscarPorId(id: string): Observable<Processo> {
    return this.httpHelper.handleRequest(this.http.get<Processo>(`${url}/${id}`));
  }

  adicionar(processo: ProcessoAdicionarCommand): Observable<any> {
    return this.httpHelper.handleRequest(this.http.post<Processo>(url, processo));
  }

  editar(processo: ProcessoAdicionarCommand): Observable<any> {
    return this.httpHelper.handleRequest(this.http.put<Processo>(`${url}/${processo.id}`, processo));
  }

  excluir(id: string): Observable<any> {
    return this.httpHelper.handleRequest(this.http.delete(`${url}/${id}`));
  }
}
