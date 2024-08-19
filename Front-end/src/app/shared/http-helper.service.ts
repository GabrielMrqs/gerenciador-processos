import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, first, catchError, of } from 'rxjs';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class HttpHelperService {
  constructor(private dialog: MatDialog) { }

  handleRequest<T>(request: Observable<T>): Observable<T> {
    return request.pipe(
      first(),
      catchError(error => {
        if (error.status === 0 && error.statusText === 'Unknown Error') {
          return of(); 
        }
        this.onError(error?.error.message || error.error);
        return of(); 
      })
    );
  }

  private onError(errorMsg: string) {
    this.dialog.open(ErrorDialogComponent, {
      data: errorMsg
    });
  }
}
