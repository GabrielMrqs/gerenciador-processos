import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-data-table',
  templateUrl: './data-table.component.html',
  styleUrl: './data-table.component.scss'
})
export class DataTableComponent {
  @Input() colunas: { [key: string]: string } = {};
  @Input() dataSource: any;
  @Input() insideComponent: boolean = false;
  @Output() edit = new EventEmitter<any>();
  @Output() view = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();
  @Output() export = new EventEmitter<any>();

  constructor(private datePipe: DatePipe) {

  }

  formatValue(column: string, value: any): string {
    if (!value) return '';

    if (column.toLowerCase().includes('data')) {
      return this.datePipe.transform(value, 'dd/MM/yyyy HH:mm')!;
    }

    return value;
  }

  editElement(element: any) {
    this.edit.emit(element);
  }

  viewElement(element: any) {
    this.view.emit(element);
  }

  deleteElement(element: any) {
    this.delete.emit(element);
  }

  exportElement(element: any) {
    this.export.emit(element);
  }

  objectKeys(obj: any): string[] {
    return Object.keys(obj);
  }
}
