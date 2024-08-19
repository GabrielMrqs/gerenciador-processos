import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { Processo } from '../../processos/models/processo.model';
import { AreaService } from '../services/area.service';
import { Area } from '../models/area.model';

@Component({
  selector: 'app-area',
  templateUrl: './area.component.html',
  styleUrl: './area.component.scss'
})
export class AreaComponent {
  areas$: Observable<Area[]>;
  filtro: string = '';

  constructor(
    private service: AreaService,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {
    this.areas$ = this.service.buscar();
  }

  onView(area: Area) {
    this.router.navigate(['areas/visualizar', area.id]);
  }

  onDelete(area: Area) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: `Tem certeza que deseja remover a área '${area.nome}' ?`,
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.service.excluir(area.id)
          .subscribe(() => {
            this.refresh();
            this.onSuccess();
          });
      }
    });
  }

  onEdit(area: Area) {
    this.router.navigate(['areas/editar', area.id]);
  }

  onAdd() {
    this.router.navigate(['areas/adicionar']);
  }

  onInputChange(event: KeyboardEvent) {
    const inputElement = event.target as HTMLInputElement;
    this.filtro = inputElement.value;
  }

  private refresh() {
    this.areas$ = this.service.buscar();
  }

  private onSuccess() {
    this.snackBar.open('Área removida com sucesso.', 'x', {
      duration: 5000,
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
  }
}
