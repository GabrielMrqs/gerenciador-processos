import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { AdicionarProcessoComponent } from './components/adicionar-processo/adicionar-processo.component';
import { ListarProcessosComponent } from './components/listar-processos/listar-processos.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppMaterialModule } from '../shared/app-material/app-material.module';
import { VisualizarProcessoComponent } from './components/visualizar-processo/visualizar-processo.component';
import { ProcessoComponent } from './container/processo.component';
import { ProcessoRoutingModule } from './processo-routing.module';


@NgModule({
  declarations: [
    ProcessoComponent,
    ListarProcessosComponent,
    AdicionarProcessoComponent,
    VisualizarProcessoComponent,
  ],
  imports: [
    CommonModule,
    ProcessoRoutingModule,
    SharedModule,
    AppMaterialModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports:[
    ProcessoComponent,
  ]
})
export class ProcessoModule { }
