import { Component } from '@angular/core';
import { AreaService } from '../../services/area.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Area } from '../../models/area.model';

@Component({
  selector: 'app-visualizar-area',
  templateUrl: './visualizar-area.component.html',
  styleUrl: './visualizar-area.component.scss'
})
export class VisualizarAreaComponent {
  area: Area = {} as Area;

  constructor(
    private service: AreaService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.service.buscarPorId(id).subscribe(res => {
        this.area = res;
      })
    }
  }

  onVoltar() {
    this.router.navigate([''], { relativeTo: this.route });
  }
}
