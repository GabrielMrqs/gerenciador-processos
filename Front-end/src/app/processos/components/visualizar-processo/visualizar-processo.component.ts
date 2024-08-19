import { Component } from '@angular/core';
import { Processo } from '../../models/processo.model';
import { ProcessoService } from '../../services/processo.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-visualizar-processo',
  templateUrl: './visualizar-processo.component.html',
  styleUrl: './visualizar-processo.component.scss'
})
export class VisualizarProcessoComponent {
  processo: Processo = {} as Processo;

  constructor(
    private service: ProcessoService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.service.buscarPorId(id).subscribe(res => {
        this.processo = res;
      })
    }
  }

  onVoltar() {
    this.router.navigate([''], { relativeTo: this.route });
  }
}
