import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'areas' },
  {
    path: 'processos',
    loadChildren: () => import('./processos/processo.module').then(m => m.ProcessoModule)
  },
  {
    path: 'areas',
    loadChildren: () => import('./areas/area.module').then(m => m.AreaModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
