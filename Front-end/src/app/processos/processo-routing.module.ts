import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProcessoComponent } from './container/processo.component';
import { AdicionarProcessoComponent } from './components/adicionar-processo/adicionar-processo.component';
import { VisualizarProcessoComponent } from './components/visualizar-processo/visualizar-processo.component';

const routes: Routes = [
  { path: '', component: ProcessoComponent },
  { path: 'adicionar', component: AdicionarProcessoComponent },
  { path: 'editar/:id', component: AdicionarProcessoComponent },
  { path: 'visualizar/:id', component: VisualizarProcessoComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProcessoRoutingModule { }
