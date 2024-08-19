import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdicionarAreaComponent } from './components/adicionar-area/adicionar-area.component';
import { VisualizarAreaComponent } from './components/visualizar-area/visualizar-area.component';
import { AreaComponent } from './container/area.component';

const routes: Routes = [
  { path: '', component: AreaComponent },
  { path: 'adicionar', component: AdicionarAreaComponent },
  { path: 'editar/:id', component: AdicionarAreaComponent },
  { path: 'visualizar/:id', component: VisualizarAreaComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AreaRoutingModule { }
