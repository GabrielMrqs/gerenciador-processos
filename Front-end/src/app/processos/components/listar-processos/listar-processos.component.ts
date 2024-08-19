import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Processo } from '../../models/processo.model';

@Component({
  selector: 'app-listar-processos',
  templateUrl: './listar-processos.component.html',
  styleUrl: './listar-processos.component.scss'
})
export class ListarProcessosComponent implements OnInit, OnChanges {
  @Input() processos: Processo[] = [];
  @Input() filtro: string = '';
  @Input() insideComponent: boolean = false;
  @Output() edit = new EventEmitter();
  @Output() view = new EventEmitter();
  @Output() delete = new EventEmitter();
  @Output() export = new EventEmitter();
  colunas = {
    'nome': 'Nome',
    'descricao': 'Descrição',
    'dataCriacao': 'Data Criação',
  }

  dataSource: any;

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.processos);
    this.dataSource.filterPredicate = (data: Processo, filter: string) => {
      const filtro = filter.toLowerCase();
      return data.nome.toLowerCase().includes(filtro) ||
        data.descricao.toLowerCase().includes(filtro) ||
        data.areaResponsavel.toLowerCase().includes(filtro);
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

  onExport(processo: Processo) {
    this.export.emit(processo);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.dataSource) {
      this.dataSource.filter = this.filtro.toLowerCase();
    }
  }
}
