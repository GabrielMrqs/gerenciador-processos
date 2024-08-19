import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Processo } from '../models/processo.model';
import { ProcessoService } from '../services/processo.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-processo',
  templateUrl: './processo.component.html',
  styleUrl: './processo.component.scss'
})
export class ProcessoComponent {
  @Output() edit = new EventEmitter();
  @Output() view = new EventEmitter();
  @Output() delete = new EventEmitter();
  @Output() export = new EventEmitter();
  @Input() insideComponent: boolean = false;
  processos$: Observable<Processo[]>;
  filtro: string = '';

  constructor(
    private service: ProcessoService,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {
    this.processos$ = this.service.buscar();
  }

  onView(processo: Processo) {
    this.router.navigate(['/processos/visualizar', processo.id]);
  }

  onExport(processo: Processo) {
    this.export.emit(processo);
  }

  onDelete(processo: Processo) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: `Tem certeza que deseja remover o processo '${processo.nome}' ?`,
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.service.excluir(processo.id)
          .subscribe(() => {
            this.refresh();
            this.onSuccess();
          });
      }
    });
  }

  onEdit(processo: Processo) {
    this.router.navigate(['processos/editar', processo.id]);
  }

  onAdd() {
    this.router.navigate(['processos/adicionar']);
  }

  onInputChange(event: KeyboardEvent) {
    const inputElement = event.target as HTMLInputElement;
    this.filtro = inputElement.value;
  }

  private refresh() {
    this.processos$ = this.service.buscar();
  }

  private onSuccess() {
    this.snackBar.open('Processo removido com sucesso.', 'x', {
      duration: 5000,
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
  }
}
