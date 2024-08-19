import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Processo } from '../../../processos/models/processo.model';
import { Area } from '../../models/area.model';

@Component({
  selector: 'app-listar-areas',
  templateUrl: './listar-areas.component.html',
  styleUrl: './listar-areas.component.scss'
})
export class ListarAreasComponent implements OnInit, OnChanges {
  @Input() areas: Area[] = [];
  @Input() filtro: string = '';
  @Output() edit = new EventEmitter();
  @Output() view = new EventEmitter();
  @Output() delete = new EventEmitter();
  colunas = {
    'nome': 'Nome',
    'descricao': 'Descrição',
  }

  dataSource: any;

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.areas);
    this.dataSource.filterPredicate = (data: Processo, filter: string) => {
      const filtro = filter.toLowerCase();
      return data.nome.toLowerCase().includes(filtro) ||
             data.descricao.toLowerCase().includes(filtro);
    };
  }

  onEdit(processo: Processo) {
    this.edit.emit(processo);
  }

  onDelete(processo: Processo) {
    this.delete.emit(processo);
  }

  onView(processo: Processo) {
    this.view.emit(processo);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.dataSource) {
      this.dataSource.filter = this.filtro.toLowerCase();
    }
  }
}
