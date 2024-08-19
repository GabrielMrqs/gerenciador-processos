import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { DataTableComponent } from './data-table/data-table.component';
import { TreeViewComponent } from './tree-view/tree-view.component';
import { AppMaterialModule } from './app-material/app-material.module';
import { GraphViewComponent } from './graph-view/graph-view.component';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    DataTableComponent,
    TreeViewComponent,
    GraphViewComponent,
    ConfirmationDialogComponent,
    ErrorDialogComponent,
    SidebarComponent
  ],
  imports: [
    CommonModule,
    AppMaterialModule,
    RouterModule,
  ],
  exports: [
    DataTableComponent,
    TreeViewComponent,
    GraphViewComponent,
    SidebarComponent
  ],
  providers:[
    DatePipe
  ]
})
export class SharedModule { }
