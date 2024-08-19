import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppMaterialModule } from '../shared/app-material/app-material.module';
import { AreaRoutingModule } from './area-routing.module';
import { AreaComponent } from './container/area.component';
import { AdicionarAreaComponent } from './components/adicionar-area/adicionar-area.component';
import { ListarAreasComponent } from './components/listar-areas/listar-areas.component';
import { VisualizarAreaComponent } from './components/visualizar-area/visualizar-area.component';
import { ProcessoModule } from '../processos/processo.module';


@NgModule({
  declarations: [
    AreaComponent,
    ListarAreasComponent,
    AdicionarAreaComponent,
    VisualizarAreaComponent,
  ],
  imports: [
    CommonModule,
    AreaRoutingModule,
    SharedModule,
    AppMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    ProcessoModule,
  ]
})
export class AreaModule { }
