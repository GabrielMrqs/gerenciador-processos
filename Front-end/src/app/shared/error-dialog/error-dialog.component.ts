import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-error-dialog',
  templateUrl: './error-dialog.component.html',
  styleUrl: './error-dialog.component.scss'
})
export class ErrorDialogComponent {
  ehArray: boolean;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {

    this.ehArray = Array.isArray(data) ? true : false;
  }
}
